using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.Test.Balancing.Groups;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils.Fetchers;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Performance
{
    public class BomberPerformanceTestGod : TestGodBase
    {
        public List<Vector2> bomberPatrolPoints;
        public Vector2 spawnPosition;

        // FELIX  Try test scene :)
        protected async override void Start()
        {
            base.Start();

            Helper helper = new Helper(updaterProvider: _updaterProvider);

            ICruiser redCruiser = helper.CreateCruiser(Direction.Left, Faction.Reds);
            ICruiser blueCruiser = helper.CreateCruiser(Direction.Right, Faction.Blues);

            // Setup target
            AirFactory factory = FindObjectOfType<AirFactory>();
            helper.InitialiseBuilding(factory, Faction.Reds);

            IList<TargetType> targetTypes = new List<TargetType>() { factory.TargetType };
            ITargetFilter targetFilter = new FactionAndTargetTypeFilter(factory.Faction, targetTypes);
            ITargetFactories targetFactories = helper.CreateTargetFactories(factory.GameObject, targetFilter: targetFilter);

            // Setup bombers
            IAircraftProvider aircraftProvider = helper.CreateAircraftProvider(bomberPatrolPoints: bomberPatrolPoints);
            IPrefabFactory prefabFactory = await Helper.CreatePrefabFactoryAsync();

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
            bombersGroup.Initialise(prefabFactory, helper, groupArgs, spawnPosition);
        }
    }
}