using BattleCruisers.Buildables;
using BattleCruisers.Scenes.Test.Utilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Naval
{
    public class ShipWithAircraftTestGod : ShipTestsGod
	{
        private TestAircraftController[] _planes;

        public List<Vector2> leftSidePatrolPoints, rightSidePatrolPoints;

        protected override List<GameObject> GetGameObjects()
        {
            List<GameObject> gameObjects = base.GetGameObjects();

            _planes = FindObjectsOfType<TestAircraftController>();
            IList<GameObject> planeGameObjects
                = _planes
                    .Select(plane => plane.GameObject)
                    .ToList();

            gameObjects.AddRange(planeGameObjects);
            return gameObjects;
        }

        protected override void Setup(Helper helper)
        {
            base.Setup(helper);

            foreach (TestAircraftController plane in _planes)
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
