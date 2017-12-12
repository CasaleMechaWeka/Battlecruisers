using BattleCruisers.Buildables;
using UnityEngine;

namespace BattleCruisers.Projectiles.Spawners
{
    public class LaserCollision : ILaserCollision
    {
        public ITarget Target { get; private set; }
        public Vector2 CollisionPoint { get; private set; }

        public LaserCollision(ITarget target, Vector2 collisionPoint)
        {
            Target = target;
            CollisionPoint = collisionPoint;
        }
    }
}
