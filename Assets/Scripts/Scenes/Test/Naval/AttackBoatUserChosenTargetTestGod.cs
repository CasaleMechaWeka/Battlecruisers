using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Naval
{
    public class AttackBoatUserChosenTargetTestGod : TestGodBase 
	{
        private ShipController _boat;
        private IList<TestAircraftController> _targets;
        private IUserChosenTargetManager _userChosenTargetManager;

        public TestAircraftController inRangeLowPriorityTarget, inRangeHighPriorityTarget, outOfRangeLowPriorityTarget;

        protected override List<GameObject> GetGameObjects()
        {
            _boat = FindObjectOfType<ShipController>();

            _targets = new List<TestAircraftController>()
            {
                inRangeLowPriorityTarget,
                inRangeHighPriorityTarget,
                outOfRangeLowPriorityTarget
            };

            List<GameObject> gameObjects
                = _targets
                    .Select(target => target.GameObject)
                    .ToList();
            gameObjects.Add(_boat.GameObject);
            return gameObjects;
        }

        protected override void Setup(Helper helper)
        {
            ICruiser redCruiser = helper.CreateCruiser(Direction.Left, Faction.Reds);

            // Ship
            _userChosenTargetManager = new UserChosenTargetManager();
            helper.InitialiseUnit(_boat, Faction.Blues, userChosenTargetManager: _userChosenTargetManager, enemyCruiser: redCruiser);
            _boat.StartConstruction();

            // Targets
            foreach (TestAircraftController target in _targets)
            {
                target.UseDummyMovementController = true;
                target.SetTargetType(TargetType.Ships);
                helper.InitialiseUnit(target, Faction.Reds);
                target.StartConstruction();
                Helper.SetupUnitForUnitMonitor(target, redCruiser);
            }

            // Imitate user choosing targets
            Invoke("ChooseInRangeLowPriorityTarget", 3);
            Invoke("ChooseOutOfRangeLowPriorityTarget", 6);
            Invoke("ClearChosenTarget", 9);
        }

        private void ChooseInRangeLowPriorityTarget()
        {
            Debug.Log("User chooses in range target");
            _userChosenTargetManager.Target = inRangeLowPriorityTarget;
        }

        private void ChooseOutOfRangeLowPriorityTarget()
        {
            Debug.Log("User chooses out of range target");
            _userChosenTargetManager.Target = outOfRangeLowPriorityTarget;
        }

        private void ClearChosenTarget()
        {
            Debug.Log("User clears chosen target");
            _userChosenTargetManager.Target = null;
        }
    }
}
