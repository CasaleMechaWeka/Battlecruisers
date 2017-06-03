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
	/// 2. Target is patrolling very quickly
	/// 3. Fighter "sees" targets, start attacking
	/// 4. Target quickly moves out of range, fighter stops pursuing target and continues patrolling
	/// 5. Repeat
	/// </summary>
	public class TargetingTestsGod : MonoBehaviour 
	{
		private Helper _helper;

		public FighterController fighter;
		public AircraftController targetAircraft;
		public List<Vector2> fighterPatrolPoints, targetPatrolPoints;

		void Start() 
		{
			_helper = new Helper();

			SetupPair(fighter, fighterPatrolPoints, targetAircraft, targetPatrolPoints);
		}

		private void SetupPair(FighterController fighter, IList<Vector2> fighterPatrolPoints, AircraftController target, IList<Vector2> targetPatrolPoints)
		{
			IAircraftProvider aircraftProvider = _helper.CreateAircraftProvider(fighterPatrolPoints: fighterPatrolPoints);
			_helper.InitialiseBuildable(fighter, Faction.Reds, aircraftProvider: aircraftProvider);
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
