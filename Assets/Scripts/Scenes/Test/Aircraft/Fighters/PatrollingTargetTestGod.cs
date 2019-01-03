using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.Factories;
using BattleCruisers.Targets.TargetFinders.Filters;
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
			target.PatrolPoints = patrolPoints;
            _helper.InitialiseUnit(target, Faction.Blues);
			target.StartConstruction();

			// Fighter
			IList<TargetType> targetTypes = new List<TargetType>() { target.TargetType };
            ITargetFilter targetFilter = new FactionAndTargetTypeFilter(target.Faction, targetTypes);
			ITargetsFactory targetsFactory = _helper.CreateTargetsFactory(target.GameObject, targetFilter);
            _helper.InitialiseUnit(fighter, Faction.Reds, targetsFactory: targetsFactory);
			fighter.StartConstruction();
		}
	}
}
