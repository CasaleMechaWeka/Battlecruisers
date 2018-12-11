using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Naval
{
    public class AttackBoatUserChosenTargetTestGod : MonoBehaviour 
	{
        private IUserChosenTargetManager _userChosenTargetManager;

        public TestAircraftController inRangeLowPriorityTarget, inRangeHighPriorityTarget, outOfRangeLowPriorityTarget;

		void Start()
		{
			Helper helper = new Helper();

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
                target.SetTargetType(TargetType.Buildings);
                helper.InitialiseUnit(target, Faction.Reds);
                target.StartConstruction();
            }

            // Ship
            ShipController boat = FindObjectOfType<ShipController>();
            _userChosenTargetManager = new UserChosenTargetManager();
            helper.InitialiseUnit(boat, Faction.Blues, userChosenTargetManager: _userChosenTargetManager);
            boat.StartConstruction();

            // Imitate user choosing targets
            Invoke("ChooseInRangeLowPriorityTarget", 2);
            Invoke("ChooseOutOfRangeLowPriorityTarget", 4);
            Invoke("ClearChosenTarget", 6);
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
