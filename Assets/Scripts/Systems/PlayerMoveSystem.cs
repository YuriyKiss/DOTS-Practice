using Unity.Physics;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public partial class PlayerMoveSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float time = UnityEngine.Time.deltaTime;

        Entities.ForEach((ref MoveData moveData, ref PhysicsVelocity physVelocity, ref Translation trans) => 
        {
            float3 desiredVelocity = new float3(moveData.playerInput.x, 0f, moveData.playerInput.y) * moveData.maxSpeed;

            float maxSpeedChange = moveData.maxAcceleration * time;

            physVelocity.Linear.x = MoveTowards(physVelocity.Linear.x, desiredVelocity.x, maxSpeedChange);
            physVelocity.Linear.z = MoveTowards(physVelocity.Linear.z, desiredVelocity.z, maxSpeedChange);

            // Handmade constraint on Y axis movement
            physVelocity.Linear.y = 0;
            trans.Value.y = -0.125f;
        }).Run();
    }

    public static float MoveTowards(float current, float target, float maxDelta)
    {
        if (Abs(target - current) <= maxDelta)
        {
            return target;
        }
        return current + Sign(target - current) * maxDelta;
    }

    public static float Abs(float f)
    {
        if (f < 0)
            return -1 * f;
        else
            return f;
    }

    public static float Sign(float f)
    {
        if (f > 0)
            return 1;
        else if (f == 0)
            return 0;
        else
            return -1;
    }
}
