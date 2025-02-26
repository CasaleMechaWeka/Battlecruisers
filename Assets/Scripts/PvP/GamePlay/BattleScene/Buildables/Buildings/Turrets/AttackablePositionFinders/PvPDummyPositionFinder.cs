using BattleCruisers.Buildables.Buildings.Turrets.AttackablePositionFinders;
using BattleCruisers.Buildables;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AttackablePositionFinders
{
    public class PvPDummyPositionFinder : IAttackablePositionFinder
    {
        public Vector2 FindClosestAttackablePosition(Vector2 sourcePosition, ITarget target)
        {
            return target.Position;
        }
    }
}