using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets.Defensive;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Buildables.Units.Aircraft;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Turrets.AnitAir
{
	public class AntiAirVsPatrollingAircraftTestGod : MonoBehaviour 
	{
		private DefensiveTurret _turret;
		private TestAircraftController[] _aircraft;

		public Vector2[] aircraftPatrolPoints;

		void Start () 
		{
			Helper helper = new Helper();

			// Setup turret
			_turret = GameObject.FindObjectOfType<DefensiveTurret>();
			Assert.IsNotNull(_turret);
			helper.InitialiseBuildable(_turret, faction: Faction.Reds);
			_turret.StartConstruction();

			_aircraft = GameObject.FindObjectsOfType<TestAircraftController>();
			Assert.IsTrue(_aircraft.Length > 0);
			InitialisePlanes(helper, _aircraft, Faction.Blues);
		}

		private void InitialisePlanes(Helper helper, IList<TestAircraftController> planes, Faction faction)
		{
			foreach (TestAircraftController plane in planes)
			{
				helper.InitialiseBuildable(plane, faction);
				plane.PatrolPoints = aircraftPatrolPoints;
				plane.StartConstruction();
			}
		}
	}
}
