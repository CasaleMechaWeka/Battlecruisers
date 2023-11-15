using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Spawners.Beams
{
    public class PvPBeamCollision : IPvPBeamCollision
    {
        public IPvPTarget Target { get; }
        public Vector2 CollisionPoint { get; }

        public PvPBeamCollision(IPvPTarget target, Vector2 collisionPoint)
        {
            Target = target;
            CollisionPoint = collisionPoint;
        }
    }
}
