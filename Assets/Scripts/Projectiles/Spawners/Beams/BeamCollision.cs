using BattleCruisers.Buildables;
using UnityEngine;

namespace BattleCruisers.Projectiles.Spawners.Beams
{
    public class BeamCollision : IBeamCollision
    {
        public ITarget Target { get; }
        public Vector2 CollisionPoint { get; }

        public BeamCollision(ITarget target, Vector2 collisionPoint)
        {
            Target = target;
            CollisionPoint = collisionPoint;
        }
    }
}
