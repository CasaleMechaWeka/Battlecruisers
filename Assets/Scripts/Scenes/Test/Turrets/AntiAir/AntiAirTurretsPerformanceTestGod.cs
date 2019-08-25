using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Utils.Threading;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Performance
{
    public class AntiAirTurretsPerformanceTestGod : MultiCameraTestGod<CameraScenario>
	{
		public UnitWrapper unitPrefab;

        protected override void Initialise()
        {
            base.Initialise();

            TimeScaleDeferrer timeScaleDeferrer = GetComponent<TimeScaleDeferrer>();
            Assert.IsNotNull(timeScaleDeferrer);

            Helper helper = new Helper(updaterProvider: _updaterProvider, deferrer: timeScaleDeferrer);
   
            // Initialise prefab
			unitPrefab.Initialise();

            // Initialise air factory
            AirFactory factory = FindObjectOfType<AirFactory>();
            helper.InitialiseBuilding(factory, Faction.Blues, parentCruiserDirection: Direction.Right);
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
