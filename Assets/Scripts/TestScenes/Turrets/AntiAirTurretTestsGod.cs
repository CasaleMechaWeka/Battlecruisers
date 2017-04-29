using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.TargetFinders;
using BattleCruisers.TestScenes.Utilities;
using BattleCruisers.Units.Aircraft;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.TestScenes.Turrets
{
	public class AntiAirTurretTestsGod : MonoBehaviour 
	{
		public Vector2[] aircraftPatrolPoints;
		public AircraftController aircraft;

		public DefensiveTurret turret;

		void Start () 
		{
			Helper helper = new Helper();

			helper.InitialiseBuildable(aircraft, faction: Faction.Blues);
			aircraft.PatrolPoints = aircraftPatrolPoints;
			aircraft.StartConstruction();
			aircraft.StartPatrolling();

			helper.InitialiseBuildable(turret, faction: Faction.Reds);
			turret.StartConstruction();
		}
	}
}
