using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using AnimationBaker.Baked;
using AnimationBaker.Components;
using AnimationBaker.Systems;
using Example.Base.Behaviours;
using Example.Base.Components;
using Example.Base.Systems;
using NavJob.Components;
using NavJob.Systems;

namespace Example.Base
{
    public class BaseBootstrap : MonoBehaviour
    {
        public UnitData[] Units = new UnitData[0];

        public static EntityArchetype Unit;
        public static List<Animated> Animations = new List<Animated>();
        public static UnitData[] UnitDatas = new UnitData[0];

        EntityManager manager;
        SpawnerSystem spawner;

        private void Start()
        {
            var world = new World("Example World");

            world.GetOrCreateManager<NavAgentSystem>();
            var query = world.GetOrCreateManager<NavMeshQuerySystem>();
            world.GetOrCreateManager<BakedArcherSystem>();
            world.GetOrCreateManager<AnimatedRendererSystem>();
            world.GetOrCreateManager<NavAgentToPositionSyncSystem>();
            world.GetOrCreateManager<RecycleDeadSystem>();
            var targetSystem = world.GetOrCreateManager<SetTargetSystem>();
            spawner = world.GetOrCreateManager<SpawnerSystem>();
            world.GetOrCreateManager<SyncNavAgentTransformSystem>();
            manager = world.GetOrCreateManager<EntityManager>();
            var allWorlds = new World[] { world };
            ScriptBehaviourUpdateOrder.UpdatePlayerLoop(allWorlds);
            World.Active = world;
            Unit = manager.CreateArchetype(
                typeof(SyncPositionFromNavAgent),
                typeof(Position),
                typeof(TransformMatrix),
                typeof(Unit),
                typeof(Animated),
                typeof(AnimatedState),
                typeof(NavAgent)
            );
            PlayerLoopManager.RegisterDomainUnload(DomainUnloadShutdown, 10000);
            query.UseCache = false;
            UnitDatas = Units;
            foreach (var unitData in Units)
            {
                Animations.Add(new Animated
                {
                    Mesh = unitData.Mesh,
                        Material = unitData.Material,
                });
            }

            var spawnerData = GetComponent<Spawner>();
            if (spawnerData)
                spawner.PendingSpawn = spawnerData.InitialSpawn;
        }

        static void DomainUnloadShutdown()
        {
            World.DisposeAllWorlds();
            ScriptBehaviourUpdateOrder.UpdatePlayerLoop();
        }
    }
}