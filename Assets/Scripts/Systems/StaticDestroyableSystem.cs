using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateAfter(typeof(CharacterMoveSystem))]
public class StaticDestroyableSystem : SystemBase
{
    EndSimulationEntityCommandBufferSystem buffer;

    protected override void OnCreate()
    {
        buffer = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        float farestDistance = GameDataManager.instance.characterMostBehind - (GameDataManager.instance.tileSize.z * 15);
        EntityCommandBuffer.ParallelWriter ecb = buffer.CreateCommandBuffer().AsParallelWriter();

        Entities
            .WithName("StaticDestroyableSystem")
            .ForEach((Entity entity, int entityInQueryIndex, ref StaticDestroybleData staticDestroybleData, ref Translation translation) =>
            {
                if (farestDistance > translation.Value.z)
                {
                    staticDestroybleData.shouldBeDestroy = true;
                }

                if (staticDestroybleData.shouldBeDestroy)
                {
                    ecb.DestroyEntity(entityInQueryIndex, entity);
                }
            })
            .ScheduleParallel();

        buffer.AddJobHandleForProducer(this.Dependency);
    }
}
