using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Scenes.Test.Utilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Turrets.AnitAir
{
    public class AntiAirVsPatrollingAircraftTestGod : TestGodBase 
	{
		private TurretController _turret;
		private TestAircraftController[] _aircraft;

		public Vector2[] aircraftPatrolPoints;

        protected override List<GameObject> GetGameObjects()
        {
			_aircraft = FindObjectsOfType<TestAircraftController>();
			Assert.IsTrue(_aircraft.Length > 0);

            _turret = FindObjectOfType<TurretController>();
			Assert.IsNotNull(_turret);

            List<GameObject> gameObjects
                = _aircraft
                    .Select(aircraft => aircraft.GameObject)
                    .ToList();
            gameObjects.Add(_turret.GameObject);
            return gameObjects;
        }

        protected override void Setup(Helper helper)
        {
            // Setup turret
            helper.InitialiseBuilding(_turret, faction: Faction.Reds);
			_turret.StartConstruction();

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
