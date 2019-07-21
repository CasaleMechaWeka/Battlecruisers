using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Scenes.Test.Balancing.Groups;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Utils.Fetchers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Performance
{
    public class GunshipsPerformanceTestGod : MonoBehaviour
    {
        public List<Vector2> gunshipPatrolPoints;
        public Vector2 spawnPosition;

        void Start()
        {
            Helper helper = new Helper();

            IAircraftProvider aircraftProvider = helper.CreateAircraftProvider(gunshipPatrolPoints: gunshipPatrolPoints);
            IPrefabFactory prefabFactory = new PrefabFactory(new PrefabFetcher());

            BuildableGroupController gunshipsGroup = FindObjectOfType<BuildableGroupController>();
            Assert.IsNotNull(gunshipsGroup);
            BuildableInitialisationArgs groupArgs = new BuildableInitialisationArgs(helper, aircraftProvider: aircraftProvider);
            gunshipsGroup.Initialise(prefabFactory, helper, groupArgs, spawnPosition);
        }
    }
}