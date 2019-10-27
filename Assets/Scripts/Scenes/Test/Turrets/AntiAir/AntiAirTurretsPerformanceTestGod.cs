using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.Threading;
using NSubstitute;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TestUtils = BattleCruisers.Scenes.Test.Utilities;

namespace BattleCruisers.Scenes.Test.Performance
{
    public class AntiAirTurretsPerformanceTestGod : MultiCameraTestGod<CameraScenario>
	{
		public UnitWrapper unitPrefab;
        public List<Vector2> patrolPoints;

        protected override void Setup(TestUtils.Helper baseHelper)
        {
            base.Setup(baseHelper);

            TimeScaleDeferrer timeScaleDeferrer = GetComponent<TimeScaleDeferrer>();
            Assert.IsNotNull(timeScaleDeferrer);

            TestUtils.Helper helper = new TestUtils.Helper(baseHelper, deferrer: timeScaleDeferrer);
   
            // Initialise prefab
			unitPrefab.StaticInitialise();

            // Initialise air factory
            IAircraftProvider aircraftProvider = Substitute.For<IAircraftProvider>();
            aircraftProvider.FighterSafeZone.Returns(new Rectangle(-50, 50, -50, 50));
            aircraftProvider.FindFighterPatrolPoints(default).ReturnsForAnyArgs(patrolPoints);

            AirFactory factory = FindObjectOfType<AirFactory>();
            helper.InitialiseBuilding(factory, Faction.Blues, parentCruiserDirection: Direction.Right, aircraftProvider: aircraftProvider);
            factory.CompletedBuildable += Factory_CompletedBuildable;
            factory.StartConstruction();

            // Initialise turrets
            TurretController[] turrets = FindObjectsOfType<TurretController>();
            foreach (TurretController turret in turrets)
            {
                helper.InitialiseBuilding(turret, Faction.Reds);
                turret.StartConstruction();
            }
		}

        private void Factory_CompletedBuildable(object sender, EventArgs e)
		{
            ((Factory)sender).StartBuildingUnit(unitPrefab);
		}

        protected override void InitialiseScenario(CameraScenario scenario)
        {
            scenario.Initialise();
        }
    }
}
