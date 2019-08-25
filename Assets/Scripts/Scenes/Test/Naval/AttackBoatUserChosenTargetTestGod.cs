using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Naval
{
    public class AttackBoatUserChosenTargetTestGod : TestGodBase 
	{
        private IUserChosenTargetManager _userChosenTargetManager;

        public TestAircraftController inRangeLowPriorityTarget, inRangeHighPriorityTarget, outOfRangeLowPriorityTarget;

        protected override void Start()
        {
            base.Start();

            Helper helper = new Helper(updaterProvider: _updaterProvider);

            ICruiser redCruiser = helper.CreateCruiser(Direction.Left, Faction.Reds);

            // Ship
            ShipController boat = FindObjectOfType<ShipController>();
            _userChosenTargetManager = new UserChosenTargetManager();
            helper.InitialiseUnit(boat, Faction.Blues, userChosenTargetManager: _userChosenTargetManager, enemyCruiser: redCruiser);
            boat.StartConstruction();

            // Targets
            IList<TestAircraftController> targets = new List<TestAircraftController>()
            {
                inRangeLowPriorityTarget,
                inRangeHighPriorityTarget,
                outOfRangeLowPriorityTarget
            };
            foreach (TestAircraftController target in targets)
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
