using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Buildables.Units.Aircraft;
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
		public TestAircraftController targetAircraft1, targetAircraft2, targetAircraft3;
		public List<Vector2> patrolPoints1, patrolPoints2, patrolPoints3;

		void Start() 
		{
			_helper = new Helper();

			SetupPair(fighter1, targetAircraft1, patrolPoints1);
			SetupPair(fighter2, targetAircraft2, patrolPoints2);
			SetupPair(fighter3, targetAircraft3, patrolPoints3);
		}

		private void SetupPair(FighterController fighter, TestAircraftController target, IList<Vector2> patrolPoints)
		{
			// Target
			_helper.InitialiseBuildable(target, faction: Faction.Blues);
			target.PatrolPoints = patrolPoints;
			target.StartConstruction();

			// Fighter
			ITargetFilter targetFilter = new FactionAndTargetTypeFilter(target.Faction, target.TargetType);
			ITargetsFactory targetsFactory = _helper.CreateTargetsFactory(target.GameObject, targetFilter);
			_helper.InitialiseBuildable(fighter, faction: Faction.Reds, targetsFactory: targetsFactory);
			fighter.StartConstruction();
		}
	}
}
