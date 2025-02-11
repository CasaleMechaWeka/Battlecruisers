using BattleCruisers.Buildables;
using BattleCruisers.Projectiles.Spawners.Beams;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Spawners.Beams
{
    public class PvPBeamCollision : IBeamCollision
    {
        public ITarget Target { get; }
        public Vector2 CollisionPoint { get; }

        public PvPBeamCollision(ITarget target, Vector2 collisionPoint)
        {
            Target = target;
            CollisionPoint = collisionPoint;
        }
    }
}
