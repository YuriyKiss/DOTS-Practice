using Unity.Jobs;
using Unity.Burst;
using Unity.Physics;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Physics.Systems;

public partial class DestructibleOnTriggerSystem : SystemBase
{
    private StepPhysicsWorld stepPhysicsWorld;
    private EndSimulationEntityCommandBufferSystem commandBufferSystem;

    protected override void OnCreate()
    {
        stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
        commandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        var job = new OnTriggerEnter
        {
            destructGroup = GetComponentDataFromEntity<DestructibleTag>(true),
            playerGroup = GetComponentDataFromEntity<PlayerTag>(true),
            spawnGroup = GetComponentDataFromEntity<SpawnData>(true),
            commandBuffer = commandBufferSystem.CreateCommandBuffer()
        };

        Dependency = job.Schedule(stepPhysicsWorld.Simulation, Dependency);
        commandBufferSystem.AddJobHandleForProducer(Dependency);
    }

    [BurstCompile]
    struct OnTriggerEnter : ITriggerEventsJob
    {
        public EntityCommandBuffer commandBuffer;

        [ReadOnly] public ComponentDataFromEntity<DestructibleTag> destructGroup;
        [ReadOnly] public ComponentDataFromEntity<PlayerTag> playerGroup;

        [ReadOnly] public ComponentDataFromEntity<SpawnData> spawnGroup;

        public void Execute(TriggerEvent triggerEvent)
        {
            Entity left = triggerEvent.EntityA;
            Entity right = triggerEvent.EntityB;

            if (destructGroup.HasComponent(left) && playerGroup.HasComponent(right))
            {
                SpawnData spawnData = spawnGroup[left];

                Destruction(left, spawnData);
            }
            else if (destructGroup.HasComponent(right) && playerGroup.HasComponent(left))
            {
                SpawnData spawnData = spawnGroup[right];

                Destruction(right, spawnData);
            }
        }

        private void Destruction(Entity toDestroy, SpawnData spawnData)
        {
            Random random = new Random(1);

            for (int i = 0; i < spawnData.amount; ++i)
            {
                Entity copy = commandBuffer.Instantiate(spawnData.entityPrefab);

                commandBuffer.SetComponent(copy, new Translation()
                {
                    Value = new float3(
                        spawnData.translate.x + (float)random.NextDouble(-2.5f, 2.5f),
                        spawnData.translate.y + (float)random.NextDouble(-2.3f, 2.5f),
                        spawnData.translate.z + (float)random.NextDouble(-2.5f, 2.5f))
                });
            }

            commandBuffer.DestroyEntity(toDestroy);
        }
    }
}
