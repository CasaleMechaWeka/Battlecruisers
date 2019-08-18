using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetFinders.Filters;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Aircraft.Fighters
{
    public class PatrollingTargetTestGod : TestGodBase
	{
		private Helper _helper;

		public FighterController fighter1, fighter2, fighter3;
		public TestAircraftController targetAircraft1, targetAircraft2, targetAircraft3;
		public List<Vector2> patrolPoints1, patrolPoints2, patrolPoints3;

        protected override void Start()
        {
            base.Start();

            _helper = new Helper(updaterProvider: _updaterProvider);

            SetupPair(fighter1, targetAircraft1, patrolPoints1);
			SetupPair(fighter2, targetAircraft2, patrolPoints2);
			SetupPair(fighter3, targetAircraft3, patrolPoints3);
		}

		private void SetupPair(FighterController fighter, TestAircraftController target, IList<Vector2> patrolPoints)
		{
            ICruiser blueCruiser = _helper.CreateCruiser(Direction.Right, Faction.Blues);
            ICruiser redCruiser = _helper.CreateCruiser(Direction.Left, Faction.Reds);

            // Target
            target.PatrolPoints = patrolPoints;
            _helper.InitialiseUnit(target, Faction.Blues);
			target.StartConstruction();
            Helper.SetupUnitForUnitMonitor(target, blueCruiser);

			// Fighter
			IList<TargetType> targetTypes = new List<TargetType>() { target.TargetType };
            ITargetFilter targetFilter = new FactionAndTargetTypeFilter(target.Faction, targetTypes);
            ITargetFactories targetFactories = _helper.CreateTargetFactories(target.GameObject, redCruiser, blueCruiser, _updaterProvider, targetFilter);
            _helper.InitialiseUnit(fighter, Faction.Reds, targetFactories: targetFactories);
			fighter.StartConstruction();
		}
	}
}
