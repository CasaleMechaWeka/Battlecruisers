using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Utils.Threading;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Turrets.AnitAir
{
    public class AntiAirVsPatrollingAircraftTestGod : TestGodBase 
	{
		private TurretController _turret;
		private TestAircraftController[] _aircraft;

		public Vector2[] aircraftPatrolPoints;

		protected override void Start () 
		{
            base.Start();

            TimeScaleDeferrer timeScaleDeferrer = GetComponent<TimeScaleDeferrer>();
            Assert.IsNotNull(timeScaleDeferrer);

            Helper helper = new Helper(deferrer: timeScaleDeferrer, updaterProvider: _updaterProvider);

            // Setup turret
            _turret = FindObjectOfType<TurretController>();
			Assert.IsNotNull(_turret);
            helper.InitialiseBuilding(_turret, faction: Faction.Reds);
			_turret.StartConstruction();

			_aircraft = FindObjectsOfType<TestAircraftController>();
			Assert.IsTrue(_aircraft.Length > 0);
			InitialisePlanes(helper, _aircraft, Faction.Blues);
		}

		private void InitialisePlanes(Helper helper, IList<TestAircraftController> planes, Faction faction)
		{
			foreach (TestAircraftController plane in planes)
			{
				plane.PatrolPoints = aircraftPatrolPoints;
                helper.InitialiseUnit(plane, faction);
				plane.StartConstruction();
			}
		}
	}
}
