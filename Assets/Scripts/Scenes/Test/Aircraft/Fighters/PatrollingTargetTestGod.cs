using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Targets;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Units.Aircraft;
using NSubstitute;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Aircraft.Fighters
{
	public class PatrollingTargetTestGod : MonoBehaviour 
	{
		private Helper _helper;

		public FighterController fighter1, fighter2, fighter3;
		public AircraftController targetAircraft1, targetAircraft2, targetAircraft3;
		public List<Vector2> patrolPoints1, patrolPoints2, patrolPoints3;

		void Start() 
		{
			_helper = new Helper();

			SetupPair(fighter1, targetAircraft1, patrolPoints1);
			SetupPair(fighter2, targetAircraft2, patrolPoints2);
			SetupPair(fighter3, targetAircraft3, patrolPoints3);
		}

		private void SetupPair(FighterController fighter, AircraftController target, IList<Vector2> patrolPoints)
		{
			ITargetsFactory targetsFactory = _helper.CreateTargetsFactory(target.GameObject);

			_helper.InitialiseBuildable(fighter, faction: Faction.Reds, targetsFactory: targetsFactory);
			fighter.StartConstruction();
			
			_helper.InitialiseBuildable(target, faction: Faction.Blues);
			target.CompletedBuildable += (sender, e) => SetPatrolPoints(sender, patrolPoints);
			target.StartConstruction();
		}

		private void SetPatrolPoints(object target, IList<Vector2> patrolPoints)
		{
			AircraftController aircraft = target as AircraftController;
			aircraft.PatrolPoints = patrolPoints;
			aircraft.StartPatrolling();
		}			
	}
}
