using Unity.Entities;

[GenerateAuthoringComponent]
public struct StaticDestroybleData : IComponentData
{
    public bool shouldBeDestroy;
}
