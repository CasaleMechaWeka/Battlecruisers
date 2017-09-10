using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Naval
{
    public class ShipWithAircraftTestGod : ShipTestsGod
	{
		public List<Vector2> leftSidePatrolPoints, rightSidePatrolPoints;

		protected override void OnStart()
        {
            Helper helper = new Helper();
            TestAircraftController[] planes = FindObjectsOfType<TestAircraftController>();

            foreach (TestAircraftController plane in planes)
            {
                Faction faction;

                // Give plane opposite faction to factories, so that ships can attack them.
                if (plane.Position.x < 0)
                {
                    plane.PatrolPoints = leftSidePatrolPoints;
                    faction = FactoryFacingLeftFaction;
                }
                else
                {
                    plane.PatrolPoints = rightSidePatrolPoints;
                    faction = FactoryFacingRightFaction;
                }

                helper.InitialiseUnit(plane, faction);
                plane.StartConstruction();
            }
        }
	}
}
