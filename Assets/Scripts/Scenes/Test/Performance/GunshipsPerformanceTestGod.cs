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

            Helper helper = new Helper(updaterProvider: _updaterProvider);

            ICruiser redCruiser = helper.CreateCruiser(Direction.Left, Faction.Reds);
            ICruiser blueCruiser = helper.CreateCruiser(Direction.Right, Faction.Blues);

            // Setup gunships
            IAircraftProvider aircraftProvider = helper.CreateAircraftProvider(gunshipPatrolPoints: gunshipPatrolPoints);
            IPrefabFactory prefabFactory = new PrefabFactory(new PrefabFetcher());

            BuildableGroupController gunshipsGroup = FindObjectOfType<BuildableGroupController>();
            if (gunshipsGroup != null)
            {
                BuildableInitialisationArgs groupArgs
                    = new BuildableInitialisationArgs(
                        helper,
                        Faction.Blues,
                        aircraftProvider: aircraftProvider,
                        updaterProvider: _updaterProvider,
                        enemyCruiser: redCruiser,
                        parentCruiser: blueCruiser);
                gunshipsGroup.Initialise(prefabFactory, helper, groupArgs, spawnPosition);
            }

            // Setup naval factory
            Factory navalFactory = FindObjectOfType<Factory>();
            if (navalFactory != null)
            {
                helper.InitialiseBuilding(navalFactory, Faction.Reds, parentCruiserDirection: Direction.Left, parentCruiser: redCruiser, enemyCruiser: blueCruiser);
                navalFactory.CompletedBuildable += Factory_CompletedBuildable;
                navalFactory.StartConstruction();
                Helper.SetupFactoryForUnitMonitor(navalFactory, redCruiser);
            }

            // Setup target (to keep attack boats on screen :P)
            DroneStation target = FindObjectOfType<DroneStation>();
            helper.InitialiseBuilding(target, Faction.Blues);
            target.StartConstruction();
        }

        private void Factory_CompletedBuildable(object sender, EventArgs e)
        {
            ((Factory)sender).StartBuildingUnit(shipPrefab);
        }
    }
}