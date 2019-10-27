using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.Test.Balancing.Groups;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetFinders.Filters;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Performance
{
    public class BomberPerformanceTestGod : TestGodBase
    {
        private AirFactory _factory;

        public List<Vector2> bomberPatrolPoints;
        public Vector2 spawnPosition;

        protected override List<GameObject> GetGameObjects()
        {
            _factory = FindObjectOfType<AirFactory>();

            return new List<GameObject>()
            {
                _factory.gameObject
            };
        }

        protected override void Setup(Helper helper)
        {
            ICruiser redCruiser = helper.CreateCruiser(Direction.Left, Faction.Reds);
            ICruiser blueCruiser = helper.CreateCruiser(Direction.Right, Faction.Blues);

            // Setup target
            helper.InitialiseBuilding(_factory, Faction.Reds);

            IList<TargetType> targetTypes = new List<TargetType>() { _factory.TargetType };
            ITargetFilter targetFilter = new FactionAndTargetTypeFilter(_factory.Faction, targetTypes);
            ITargetFactories targetFactories = helper.CreateTargetFactories(_factory.GameObject, targetFilter: targetFilter);

            // Setup bombers
            IAircraftProvider aircraftProvider = helper.CreateAircraftProvider(bomberPatrolPoints: bomberPatrolPoints);

            BuildableGroupController bombersGroup = FindObjectOfType<BuildableGroupController>();
            BuildableInitialisationArgs groupArgs
                = new BuildableInitialisationArgs(
                    helper,
                    Faction.Blues,
                    aircraftProvider: aircraftProvider,
                    updaterProvider: _updaterProvider,
                    enemyCruiser: redCruiser,
                    parentCruiser: blueCruiser,
                    targetFactories: targetFactories);
            bombersGroup.Initialise(helper, groupArgs, spawnPosition);
        }
    }
}