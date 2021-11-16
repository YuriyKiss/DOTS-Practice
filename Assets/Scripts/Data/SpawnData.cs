using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct SpawnData : IComponentData
{
    public int amount;
    public float3 translate;
    public Entity entityPrefab;
}
