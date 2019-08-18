using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.Test.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Aircraft.Fighters
{
    public class MultipleTargetsTestGod : TestGodBase 
	{
        protected override void Start()
        {
            base.Start();

            Helper helper = new Helper(updaterProvider: _updaterProvider);

            IList<Vector2> patrolPoints = new List<Vector2>()
            {
                new Vector2(-12, 5),
                new Vector2(12, 5)
            };

            ICruiser redCruiser = helper.CreateCruiser(Direction.Left, Faction.Reds);

            // Fighter
            FighterController fighter = FindObjectOfType<FighterController>();
            helper.InitialiseUnit(fighter, Faction.Blues, enemyCruiser: redCruiser);
            fighter.StartConstruction();

            // Targets
            TestAircraftController[] targets = FindObjectsOfType<TestAircraftController>();

            foreach (TestAircraftController target in targets)
            {
                target.PatrolPoints = patrolPoints;
                helper.InitialiseUnit(target, Faction.Reds);
                target.StartConstruction();
                Helper.SetupUnitForUnitMonitor(target, redCruiser);
            }
		}
	}
}