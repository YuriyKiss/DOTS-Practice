using Unity.Jobs;
using Unity.Burst;
using Unity.Physics;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Physics.Systems;

public class DestructibleOnTriggerSystem : JobComponentSystem
{
    // Worlds needed to schedule job
    private BuildPhysicsWorld buildPhysicsWorld;
    private StepPhysicsWorld stepPhysicsWorld;

    // Command buffer to init/destroy entities from inside jobs
    private EndSimulationEntityCommandBufferSystem commandBufferSystem;

    protected override void OnCreate()
    {
        base.OnCreate();

        buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();

        commandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new OnTriggerEnter();

        job.commandBuffer = commandBufferSystem.CreateCommandBuffer();

        job.destructGroup = GetComponentDataFromEntity<DestructibleTag>(true);
        job.playerGroup = GetComponentDataFromEntity<PlayerTag>(true);
        job.spawnGroup = GetComponentDataFromEntity<SpawnData>(true);

        JobHandle jobHandle = job.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, inputDeps);
        jobHandle.Complete();

        return jobHandle;
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
