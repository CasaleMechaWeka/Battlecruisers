using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.Test.Balancing.Groups;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Utils.Fetchers;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Performance
{
    public class GunshipsPerformanceTestGod : TestGodBase
    {
        public List<Vector2> gunshipPatrolPoints;
        public Vector2 spawnPosition;
        public UnitWrapper shipPrefab;

        protected override void Start()
        {
            base.Start();

            shipPrefab.Initialise();
            Helper helper = new Helper();
            ICruiser redCruiser = helper.CreateCruiser(Direction.Left, Faction.Reds);

            // Setup gunships
            IAircraftProvider aircraftProvider = helper.CreateAircraftProvider(gunshipPatrolPoints: gunshipPatrolPoints);
            IPrefabFactory prefabFactory = new PrefabFactory(new PrefabFetcher());

            BuildableGroupController gunshipsGroup = FindObjectOfType<BuildableGroupController>();
            Assert.IsNotNull(gunshipsGroup);
            BuildableInitialisationArgs groupArgs 
                = new BuildableInitialisationArgs(
                    helper, 
                    Faction.Blues, 
                    aircraftProvider: aircraftProvider, 
                    updaterProvider: _updaterProvider,
                    enemyCruiser: redCruiser);
            gunshipsGroup.Initialise(prefabFactory, helper, groupArgs, spawnPosition);

            // Setup naval factory
            Factory navalFactory = FindObjectOfType<Factory>();
            Assert.IsNotNull(navalFactory);
            helper.InitialiseBuilding(navalFactory, Faction.Reds, parentCruiserDirection: Direction.Left);
            navalFactory.CompletedBuildable += Factory_CompletedBuildable;
            navalFactory.StartConstruction();
            Helper.SetupFactoryForUnitMonitor(navalFactory, redCruiser);
        }

        private void Factory_CompletedBuildable(object sender, EventArgs e)
        {
            ((Factory)sender).StartBuildingUnit(shipPrefab);
        }
    }
}