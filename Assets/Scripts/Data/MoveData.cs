using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct MoveData : IComponentData
{
    public float2 playerInput;
    public float3 velocity;

    public float maxSpeed;
    public float maxAcceleration;

    public float ballRadius;
}
