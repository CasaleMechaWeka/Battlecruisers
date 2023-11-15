using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AttackablePositionFinders
{
    public class PvPDummyPositionFinder : IPvPAttackablePositionFinder
    {
        public Vector2 FindClosestAttackablePosition(Vector2 sourcePosition, IPvPTarget target)
        {
            return target.Position;
        }
    }
}