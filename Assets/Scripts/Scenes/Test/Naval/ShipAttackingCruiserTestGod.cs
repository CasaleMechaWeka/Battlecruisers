using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Scenes.Test.Utilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Naval
{
    public class ShipAttackingCruiserTestGod : TestGodBase
    {
        private TestAircraftController[] _planes;
        private ShipController _ship;

        public List<Vector2> aircraftPatorlPoints;

        protected override List<GameObject> GetGameObjects()
        {
            _planes = FindObjectsOfType<TestAircraftController>();
            List<GameObject> gameObjects
                = _planes
                    .Select(plane => plane.GameObject)
                    .ToList();

            _ship = FindObjectOfType<ShipController>();
            gameObjects.Add(_ship.GameObject);

            return gameObjects;
        }

        protected override void Setup(Helper helper)
        {
            // Setup ship
            helper.InitialiseUnit(_ship, Faction.Blues);
            _ship.StartConstruction();

            // Setup fake cruiser
            TestTarget fakeCruiser = FindObjectOfType<TestTarget>();
            fakeCruiser.Initialise(Faction.Reds);

            // Setup planes
            foreach (TestAircraftController plane in _planes)
            {
                plane.PatrolPoints = aircraftPatorlPoints;
                helper.InitialiseUnit(plane, Faction.Reds);
                plane.StartConstruction();
            }
        }
    }
}