using BattleCruisers.Buildables;
using UnityEngine;

namespace BattleCruisers.Projectiles.Spawners.Beams
{
    public class LaserCollision : ILaserCollision
    {
        public ITarget Target { get; }
        public Vector2 CollisionPoint { get; }

        public LaserCollision(ITarget target, Vector2 collisionPoint)
        {
            Target = target;
            CollisionPoint = collisionPoint;
        }
    }
}
