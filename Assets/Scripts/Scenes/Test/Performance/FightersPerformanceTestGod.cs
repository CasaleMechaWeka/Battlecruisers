using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.Test.Balancing.Groups;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Utils.Fetchers;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Performance
{
    public class FightersPerformanceTestGod : TestGodBase
    {
        public List<Vector2> patrolPoints;
        public Vector2 spawnPosition;
        public UnitWrapper shipPrefab;

        protected override void Start()
        {
            base.Start();

            shipPrefab.Initialise();

            Helper helper = new Helper(updaterProvider: _updaterProvider);

            ICruiser redCruiser = helper.CreateCruiser(Direction.Left, Faction.Reds);
            ICruiser blueCruiser = helper.CreateCruiser(Direction.Right, Faction.Blues);

            // Setup fighters
            IAircraftProvider aircraftProvider = helper.CreateAircraftProvider(fighterPatrolPoints: patrolPoints);
            IPrefabFactory prefabFactory = new PrefabFactory(new PrefabFetcher());

            BuildableGroupController fightersGroup = FindObjectOfType<BuildableGroupController>();
            if (fightersGroup != null)
            {
                BuildableInitialisationArgs groupArgs
                    = new BuildableInitialisationArgs(
                        helper,
                        Faction.Blues,
                        aircraftProvider: aircraftProvider,
                        updaterProvider: _updaterProvider,
                        enemyCruiser: redCruiser,
                        parentCruiser: blueCruiser);
                fightersGroup.Initialise(prefabFactory, helper, groupArgs, spawnPosition);
            }
        }
    }
}