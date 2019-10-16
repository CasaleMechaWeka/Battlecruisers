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
		public FighterController fighter1, fighter2, fighter3;
		public TestAircraftController targetAircraft1, targetAircraft2, targetAircraft3;
		public List<Vector2> patrolPoints1, patrolPoints2, patrolPoints3;

        protected override IList<GameObject> GetGameObjects()
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
            SetupPair(helper, fighter1, targetAircraft1, patrolPoints1);
			SetupPair(helper, fighter2, targetAircraft2, patrolPoints2);
			SetupPair(helper, fighter3, targetAircraft3, patrolPoints3);
		}

		private void SetupPair(Helper helper, FighterController fighter, TestAircraftController target, IList<Vector2> patrolPoints)
		{
            ICruiser blueCruiser = helper.CreateCruiser(Direction.Right, Faction.Blues);
            ICruiser redCruiser = helper.CreateCruiser(Direction.Left, Faction.Reds);

            // Target
            target.PatrolPoints = patrolPoints;
            helper.InitialiseUnit(target, Faction.Blues);
			target.StartConstruction();
            Helper.SetupUnitForUnitMonitor(target, blueCruiser);

			// Fighter
			IList<TargetType> targetTypes = new List<TargetType>() { target.TargetType };
            ITargetFilter targetFilter = new FactionAndTargetTypeFilter(target.Faction, targetTypes);
            ITargetFactories targetFactories = helper.CreateTargetFactories(target.GameObject, redCruiser, blueCruiser, _updaterProvider, targetFilter);
            helper.InitialiseUnit(fighter, Faction.Reds, targetFactories: targetFactories);
			fighter.StartConstruction();
		}
	}
}
