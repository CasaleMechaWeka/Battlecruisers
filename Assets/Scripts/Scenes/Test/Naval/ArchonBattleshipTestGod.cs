using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Scenes.Test.Utilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Naval
{
    public class ArchonBattleshipTestGod : TestGodBase
    {
        private Faction _leftBattleshipFaction;
        private Faction _rightBattleshipFaction;
        private ArchonBattleshipController[] _battleships;

        protected override List<GameObject> GetGameObjects()
        {
            _battleships = FindObjectsOfType<ArchonBattleshipController>();

            return
                _battleships
                    .Select(battleship => battleship.GameObject)
                    .ToList();
        }

        protected override void Setup(Helper helper)
        {
            _leftBattleshipFaction = Faction.Reds;
            _rightBattleshipFaction = Faction.Blues;

            SetupBattleships(helper);
        }

        private void SetupBattleships(Helper helper)
        {
            foreach (ArchonBattleshipController battleship in _battleships)
            {
                Vector3 position = battleship.transform.position;
                Faction faction = GetBattleshipFaction(position);
                Direction direction = GetBattleshipDirection(position);
                helper.InitialiseUnit(battleship, faction, parentCruiserDirection: direction, showDroneFeedback: true);
                battleship.StartConstruction();
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
    }
}
