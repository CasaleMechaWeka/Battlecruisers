using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Scenes.Test.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Naval
{
    public class AttackBoatUserChosenTargetTestGod : MonoBehaviour 
	{
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
            // FELIX  Pass in user chosen target manager :)
            ShipController boat = FindObjectOfType<ShipController>();
			helper.InitialiseUnit(boat, Faction.Blues);
            boat.StartConstruction();
        }
	}
}
