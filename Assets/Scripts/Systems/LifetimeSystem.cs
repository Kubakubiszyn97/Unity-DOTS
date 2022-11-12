using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public partial class LifetimeSystem : SystemBase
{
    public EndSimulationEntityCommandBufferSystem endSimulationEcbSystem;

    protected override void OnCreate()
    {
        endSimulationEcbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;
        var ecb = endSimulationEcbSystem.CreateCommandBuffer().AsParallelWriter();

        Entities.ForEach((Entity entity, int entityInQueryIndex, ref Lifetime lifetime) => 
        {
            lifetime.Value -= deltaTime;

            if (lifetime.Value <= 0)
            {
                ecb.DestroyEntity(entityInQueryIndex, entity);
            }
        }).ScheduleParallel();

        endSimulationEcbSystem.AddJobHandleForProducer(Dependency);
    }
}
