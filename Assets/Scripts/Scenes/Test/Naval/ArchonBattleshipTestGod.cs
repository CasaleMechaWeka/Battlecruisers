using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Naval
{
    public class ArchonBattleshipTestGod : MonoBehaviour
    {
        private Faction _leftBattleshipFaction;
        private Faction _rightBattleshipFaction;

        public List<Vector2> leftSidePatrolPoints, rightSidePatrolPoints;

        void Start()
        {
            _leftBattleshipFaction = Faction.Reds;
            _rightBattleshipFaction = Faction.Blues;

            Helper helper = new Helper();

            SetupBattleships(helper);
            SetupPlanes(helper);
        }

        private void SetupBattleships(Helper helper)
        {
            ArchonBattleshipController[] battleships = FindObjectsOfType<ArchonBattleshipController>();

            foreach (ArchonBattleshipController battleship in battleships)
            {
                Faction faction = GetBattleshipFaction(battleship.transform.position);
                helper.InitialiseUnit(battleship, faction);
                battleship.StartConstruction();
			}
        }

        private void SetupPlanes(Helper helper)
        {
            TestAircraftController[] planes = FindObjectsOfType<TestAircraftController>();

            foreach (TestAircraftController plane in planes)
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

        private List<Vector2> GetPatrolPoints(Vector3 aircraftPosition)
        {
            return aircraftPosition.x > 0 ? rightSidePatrolPoints : leftSidePatrolPoints;
        }
    }
}
