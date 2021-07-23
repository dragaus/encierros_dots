using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

/// <summary>
/// This system move every character in the scene according to their speed
/// </summary>
public class CharacterMoveSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;
        
        Entities
            .WithName("CharacterMoveSystem")
            .ForEach((ref Translation translation, ref Rotation rotation, ref CharaterData charaterData) => {
                translation.Value += charaterData.movementSpeed * math.forward(rotation.Value) * deltaTime;
        }).ScheduleParallel();
    }
}
