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
    public class StationaryTargetTestGod : TestGodBase
	{
		public FighterController fighter1, fighter2, fighter3;
		public TestAircraftController targetAircraft1, targetAircraft2, targetAircraft3;

        protected override List<GameObject> GetGameObjects()
        {
            return new List<GameObject>()
            {
                fighter1.GameObject,
                targetAircraft1.GameObject,
                fighter2.GameObject,
                targetAircraft2.GameObject,
                fighter3.GameObject,
                targetAircraft3.GameObject
            };
        }

        protected override void Setup(Helper helper)
        {
            SetupPair(helper, fighter1, targetAircraft1);
            SetupPair(helper, fighter2, targetAircraft2);
            SetupPair(helper, fighter3, targetAircraft3);
        }

        private void SetupPair(Helper helper, FighterController fighter, TestAircraftController target)
		{
            ICruiser blueCruiser = helper.CreateCruiser(Direction.Right, Faction.Blues);
            ICruiser redCruiser = helper.CreateCruiser(Direction.Left, Faction.Reds);

            // Target
            target.UseDummyMovementController = true;
            helper.InitialiseUnit(target, faction: Faction.Blues);
			target.StartConstruction();
            Helper.SetupUnitForUnitMonitor(target, blueCruiser);

            // Fighter
            IList<TargetType> targetTypes = new List<TargetType>() { target.TargetType };
            ITargetFilter targetFilter = new FactionAndTargetTypeFilter(target.Faction, targetTypes);
            ITargetFactories targetFactories = helper.CreateTargetFactories(target.GameObject, redCruiser, blueCruiser, _updaterProvider, targetFilter);
            helper.InitialiseUnit(fighter, faction: Faction.Reds, targetFactories: targetFactories);
			fighter.StartConstruction();
		}
	}
}
