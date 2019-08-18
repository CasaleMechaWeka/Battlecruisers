using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.Factories;
using BattleCruisers.Targets.TargetFinders.Filters;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Aircraft.Fighters
{
    public class StationaryTargetTestGod : TestGodBase
	{
		private Helper _helper;

		public FighterController fighter1, fighter2, fighter3;
		public TestAircraftController targetAircraft1, targetAircraft2, targetAircraft3;

        protected override void Start()
        {
            base.Start();

            _helper = new Helper(updaterProvider: _updaterProvider);

            SetupPair(fighter1, targetAircraft1);
			SetupPair(fighter2, targetAircraft2);
			SetupPair(fighter3, targetAircraft3);
		}

		private void SetupPair(FighterController fighter, TestAircraftController target)
		{
            ICruiser blueCruiser = _helper.CreateCruiser(Direction.Right, Faction.Blues);
            ICruiser redCruiser = _helper.CreateCruiser(Direction.Left, Faction.Reds);

            // Target
            target.UseDummyMovementController = true;
            _helper.InitialiseUnit(target, faction: Faction.Blues);
			target.StartConstruction();
            Helper.SetupUnitForUnitMonitor(target, blueCruiser);

            // Fighter
            IList<TargetType> targetTypes = new List<TargetType>() { target.TargetType };
            ITargetFilter targetFilter = new FactionAndTargetTypeFilter(target.Faction, targetTypes);
            ITargetFactories targetFactories = _helper.CreateTargetFactories(target.GameObject, redCruiser, blueCruiser, _updaterProvider, targetFilter);
            _helper.InitialiseUnit(fighter, faction: Faction.Reds, targetFactories: targetFactories);
			fighter.StartConstruction();
		}
	}
}
