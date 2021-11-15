using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct MoveData : IComponentData
{
    public float2 playerInput;

    public float maxSpeed;
    public float maxAcceleration;

    public float ballRadius;
}
