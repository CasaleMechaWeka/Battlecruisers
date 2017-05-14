using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Targets;
using BattleCruisers.TestScenes.Utilities;
using BattleCruisers.Units.Aircraft;
using NSubstitute;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.TestScenes.Aircraft.Fighters
{
	/// <summary>
	/// 1. Fighter is patrolling
	/// 2. Target is patrolling
	/// 3. Target enters fighter's "safe" zone
	/// 4. Fighter "sees" target, start attacking
	/// 5. Target moves out of fighter's "safe" zone
	/// 6. Fighter abandons chase
	/// 7. Repeat
	/// </summary>
	public class SafeZoneTestsGod : MonoBehaviour 
	{
		private Helper _helper;

		public FighterController fighter;
		public AircraftController targetAircraft;
		public List<Vector2> fighterPatrolPoints, targetPatrolPoints;
		public float safeZoneMinX, safeZoneMaxX, safeZoneMinY, safeZoneMaxY;

		void Start() 
		{
			_helper = new Helper();

			SetupPair(fighter, fighterPatrolPoints, targetAircraft, targetPatrolPoints);
		}

		private void SetupPair(FighterController fighter, IList<Vector2> fighterPatrolPoints, AircraftController target, IList<Vector2> targetPatrolPoints)
		{
			// Use real targetting logic
			ICruiser enemyCruiser = Substitute.For<ICruiser>();
			ITargetsFactory targetsFactory = new TargetsFactory(enemyCruiser);

			fighter.SetSafeZone(safeZoneMinX, safeZoneMaxX, safeZoneMinY, safeZoneMaxY);
			_helper.InitialiseBuildable(fighter, faction: Faction.Reds, targetsFactory: targetsFactory);
			fighter.CompletedBuildable += (sender, e) => SetPatrolPoints(sender, fighterPatrolPoints);
			fighter.StartConstruction();

			_helper.InitialiseBuildable(target, faction: Faction.Blues);
			target.CompletedBuildable += (sender, e) => SetPatrolPoints(sender, targetPatrolPoints);
			target.StartConstruction();
		}

		private void SetPatrolPoints(object aircraftAsObj, IList<Vector2> patrolPoints)
		{
			AircraftController aircraft = aircraftAsObj as AircraftController;
			aircraft.PatrolPoints = patrolPoints;
			aircraft.StartPatrolling();
		}			
	}
}
