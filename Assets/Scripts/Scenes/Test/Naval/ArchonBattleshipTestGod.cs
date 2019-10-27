using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Scenes.Test.Utilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Naval
{
    public class ArchonBattleshipTestGod : TestGodBase
    {
        private Faction _leftBattleshipFaction;
        private Faction _rightBattleshipFaction;
        private ArchonBattleshipController[] _battleships;
        private TestAircraftController[] _planes;

        public List<Vector2> leftSidePatrolPoints, rightSidePatrolPoints;

        protected override IList<GameObject> GetGameObjects()
        {
            _battleships = FindObjectsOfType<ArchonBattleshipController>();
            _planes = FindObjectsOfType<TestAircraftController>();

            List<GameObject> gameObjects
                = _battleships
                    .Select(battleship => battleship.GameObject)
                    .ToList();
            List<GameObject> planeGameObjects 
                = _planes
                    .Select(plane => plane.GameObject)
                    .ToList();
            gameObjects.AddRange(planeGameObjects);

            return gameObjects;
        }

        protected override void Setup(Helper helper)
        {
            _leftBattleshipFaction = Faction.Reds;
            _rightBattleshipFaction = Faction.Blues;

            SetupBattleships(helper);
            SetupPlanes(helper);
        }

        private void SetupBattleships(Helper helper)
        {
            foreach (ArchonBattleshipController battleship in _battleships)
            {
                Vector3 position = battleship.transform.position;
                Faction faction = GetBattleshipFaction(position);
                Direction direction = GetBattleshipDirection(position);
                helper.InitialiseUnit(battleship, faction, parentCruiserDirection: direction);
                battleship.StartConstruction();
			}
        }

        private void SetupPlanes(Helper helper)
        {

            foreach (TestAircraftController plane in _planes)
            {
                Vector3 position = plane.transform.position;
                Faction faction = BCUtils.Helper.GetOppositeFaction(GetBattleshipFaction(position));
                plane.PatrolPoints = GetPatrolPoints(position);

                helper.InitialiseUnit(plane, faction);
                plane.StartConstruction();
            }
        }

        private Faction GetBattleshipFaction(Vector3 unitPosition)
        {
            return unitPosition.x > 0 ? _rightBattleshipFaction : _leftBattleshipFaction;
        }

        private Direction GetBattleshipDirection(Vector3 unitPosition)
        {
            return unitPosition.x > 0 ? Direction.Left : Direction.Right;
        }

        private List<Vector2> GetPatrolPoints(Vector3 aircraftPosition)
        {
            return aircraftPosition.x > 0 ? rightSidePatrolPoints : leftSidePatrolPoints;
        }
    }
}
