using UnityEngine;
using Unity.Physics;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public class PlayerMoveSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float time = UnityEngine.Time.deltaTime;

        Entities.ForEach((ref MoveData moveData, ref PhysicsVelocity physVelocity, ref Rotation rotation, ref Translation trans) => 
        {
            float3 desiredVelocity = new float3(moveData.playerInput.x, 0f, moveData.playerInput.y) * moveData.maxSpeed;

            float maxSpeedChange = moveData.maxAcceleration * time;

            moveData.velocity = physVelocity.Linear;
            moveData.velocity.x = Mathf.MoveTowards(moveData.velocity.x, desiredVelocity.x, maxSpeedChange);
            moveData.velocity.z = Mathf.MoveTowards(moveData.velocity.z, desiredVelocity.z, maxSpeedChange);
            
            float3 movement = moveData.velocity * time;

            float distance = movement.x * movement.x + movement.y * movement.y + movement.z * movement.z;
            if (distance > 0.001f)
            {
                float angle = distance * (180f / Mathf.PI) / moveData.ballRadius;
                
                Vector3 rotationAxis = Vector3.Cross(Vector3.up, movement).normalized;
                rotation.Value = Quaternion.Euler(rotationAxis * angle) * rotation.Value;
            }
            else
            {
                physVelocity.Angular = float3.zero;
            }
            
            physVelocity.Linear = moveData.velocity;
            trans.Value = new float3(trans.Value.x, -0.125f, trans.Value.z);
        }).Run();
    }
}
