using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Targets;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using NSubstitute;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Aircraft.Fighters
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

		public List<Vector2> fighterPatrolPoints, targetPatrolPoints;
		public float safeZoneMinX, safeZoneMaxX, safeZoneMinY, safeZoneMaxY;

		void Start() 
		{
			_helper = new Helper();

			FighterController fighter = GameObject.FindObjectOfType<FighterController>();
			SafeZone safeZone = new SafeZone(safeZoneMinX, safeZoneMaxX, safeZoneMinY, safeZoneMaxY);
			// FELIX
			//			IAircraftProvider aircraftProvider = _helper.CreateAircraftProvider(fighterPatrolPoints: fighterPatrolPoints, fighterSafeZone: safeZone);
			IAircraftProvider aircraftProvider = null;
			_helper.InitialiseBuildable(fighter, Faction.Reds, aircraftProvider: aircraftProvider);
			fighter.StartConstruction();

			// Target aircraft
			TestAircraftController target = GameObject.FindObjectOfType<TestAircraftController>();
			target.PatrolPoints = targetPatrolPoints;
			_helper.InitialiseBuildable(target, faction: Faction.Blues);
			target.StartConstruction();
		}
	}
}
