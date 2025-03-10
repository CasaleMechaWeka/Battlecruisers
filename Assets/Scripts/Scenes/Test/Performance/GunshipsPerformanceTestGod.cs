using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.Test.Balancing.Groups;
using BattleCruisers.Scenes.Test.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Performance
{
    public class GunshipsPerformanceTestGod : TestGodBase
    {
        private Factory _navalFactory;
        private DroneStation _target;

        public List<Vector2> gunshipPatrolPoints;
        public Vector2 spawnPosition;
        public UnitWrapper shipPrefab;

        protected override List<GameObject> GetGameObjects()
        {
            _navalFactory = FindObjectOfType<Factory>();
            _target = FindObjectOfType<DroneStation>();

            return new List<GameObject>()
            {
                _navalFactory.GameObject,
                _target.GameObject
            };
        }

        protected override void Setup(Helper helper)
        {
            shipPrefab.StaticInitialise(helper.CommonStrings);

            ICruiser redCruiser = helper.CreateCruiser(Direction.Left, Faction.Reds);
            ICruiser blueCruiser = helper.CreateCruiser(Direction.Right, Faction.Blues);

            // Setup gunships
            IAircraftProvider aircraftProvider = helper.CreateAircraftProvider(gunshipPatrolPoints: gunshipPatrolPoints);

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
                gunshipsGroup.Initialise(helper, groupArgs, spawnPosition);
            }

            // Setup naval factory
            if (_navalFactory != null)
            {
                helper.InitialiseBuilding(_navalFactory, Faction.Reds, parentCruiserDirection: Direction.Left, parentCruiser: redCruiser, enemyCruiser: blueCruiser);
                _navalFactory.CompletedBuildable += Factory_CompletedBuildable;
                _navalFactory.StartConstruction();
                Helper.SetupFactoryForUnitMonitor(_navalFactory, redCruiser);
            }

            // Setup target (to keep attack boats on screen :P)
            helper.InitialiseBuilding(_target, Faction.Blues);
            _target.StartConstruction();
        }

        private void Factory_CompletedBuildable(object sender, EventArgs e)
        {
            ((Factory)sender).StartBuildingUnit(shipPrefab);
        }
    }
}