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
        public Vector2 leftSpawnPosition, rightSpawnPosition;
        public UnitWrapper shipPrefab;
        public BuildableGroupController leftFighterGroup, rightFighterGroup;

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

            InitialiseGroup(helper, redCruiser, blueCruiser, aircraftProvider, prefabFactory, leftFighterGroup, leftSpawnPosition);
            InitialiseGroup(helper, blueCruiser, redCruiser, aircraftProvider, prefabFactory, rightFighterGroup, rightSpawnPosition);
        }

        private void InitialiseGroup(
            Helper helper, 
            ICruiser enemyCruiser, 
            ICruiser parentCruiser, 
            IAircraftProvider aircraftProvider,
            IPrefabFactory prefabFactory,
            BuildableGroupController fightersGroup,
            Vector2 spawnPosition)
        {
            if (fightersGroup == null)
            {
                return;
            }

            BuildableInitialisationArgs groupArgs
                = new BuildableInitialisationArgs(
                    helper,
                    parentCruiser.Faction,
                    aircraftProvider: aircraftProvider,
                    updaterProvider: _updaterProvider,
                    enemyCruiser: enemyCruiser,
                    parentCruiser: parentCruiser);
            fightersGroup.Initialise(prefabFactory, helper, groupArgs, spawnPosition);
        }
    }
}