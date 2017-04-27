using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets;
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

		public TurretBarrelController turretBarrel;

		void Start () 
		{
			aircraft.PatrolPoints = aircraftPatrolPoints;
			aircraft.StartPatrolling();

			turretBarrel.Initialise(Faction.Blues);
			turretBarrel.Target = aircraft.gameObject;
		}
	}
}
