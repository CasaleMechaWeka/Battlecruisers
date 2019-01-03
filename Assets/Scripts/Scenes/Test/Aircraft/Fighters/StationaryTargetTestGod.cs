using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.Factories;
using BattleCruisers.Targets.TargetFinders.Filters;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Aircraft.Fighters
{
    public class StationaryTargetTestGod : MonoBehaviour 
	{
		private Helper _helper;

		public FighterController fighter1, fighter2, fighter3;
		public TestAircraftController targetAircraft1, targetAircraft2, targetAircraft3;

		void Start() 
		{
			_helper = new Helper();

			SetupPair(fighter1, targetAircraft1);
			SetupPair(fighter2, targetAircraft2);
			SetupPair(fighter3, targetAircraft3);
		}

		private void SetupPair(FighterController fighter, TestAircraftController target)
		{
			// Target
			target.UseDummyMovementController = true;
            _helper.InitialiseUnit(target, faction: Faction.Blues);
			target.StartConstruction();

			// Fighter
			IList<TargetType> targetTypes = new List<TargetType>() { target.TargetType };
            ITargetFilter targetFilter = new FactionAndTargetTypeFilter(target.Faction, targetTypes);
			ITargetsFactory targetsFactory = _helper.CreateTargetsFactory(target.GameObject, targetFilter);
            _helper.InitialiseUnit(fighter, faction: Faction.Reds, targetsFactory: targetsFactory);
			fighter.StartConstruction();
		}
	}
}
