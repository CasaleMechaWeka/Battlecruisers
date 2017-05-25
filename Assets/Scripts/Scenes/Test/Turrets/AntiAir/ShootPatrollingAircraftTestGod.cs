using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Units.Aircraft;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Turrets.AnitAir
{
	public class ShootPatrollingAircraftTestGod : MonoBehaviour 
	{
		public Vector2[] aircraftPatrolPoints;
		public List<AircraftController> aircraft;

		public DefensiveTurret turret;

		void Start () 
		{
			Helper helper = new Helper();

			InitialisePlanes(helper, aircraft, Faction.Blues);

			helper.InitialiseBuildable(turret, faction: Faction.Reds);
			turret.StartConstruction();
		}

		private void InitialisePlanes(Helper helper, IList<AircraftController> planes, Faction faction)
		{
			foreach (AircraftController plane in planes)
			{
				helper.InitialiseBuildable(plane, faction);
				plane.PatrolPoints = aircraftPatrolPoints;
				plane.StartConstruction();
				plane.StartPatrolling();
			}
		}
	}
}
