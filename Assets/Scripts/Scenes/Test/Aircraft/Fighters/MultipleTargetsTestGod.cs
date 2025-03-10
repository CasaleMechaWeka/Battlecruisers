using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.Test.Utilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Aircraft.Fighters
{
    public class MultipleTargetsTestGod : TestGodBase 
	{
        private FighterController _fighter;
        private TestAircraftController[] _targets;

        protected override List<GameObject> GetGameObjects()
        {
            _targets = FindObjectsOfType<TestAircraftController>();
            List<GameObject> gameObjects = _targets.Select(target => target.GameObject).ToList();

            _fighter = FindObjectOfType<FighterController>();
            gameObjects.Add(_fighter.GameObject);

            return gameObjects;
        }

        protected override void Setup(Helper helper)
        {
            IList<Vector2> patrolPoints = new List<Vector2>()
            {
                new Vector2(-12, 5),
                new Vector2(12, 5)
            };

            ICruiser redCruiser = helper.CreateCruiser(Direction.Left, Faction.Reds);

            // Fighter
            helper.InitialiseUnit(_fighter, Faction.Blues, enemyCruiser: redCruiser);
            _fighter.StartConstruction();

            // Targets
            foreach (TestAircraftController target in _targets)
            {
                target.PatrolPoints = patrolPoints;
                helper.InitialiseUnit(target, Faction.Reds);
                target.StartConstruction();
                Helper.SetupUnitForUnitMonitor(target, redCruiser);
            }
		}
	}
}