using BattleCruisers.Movement.Velocity;
using UnityEngine;

namespace BattleCruisers.Projectiles
{
    public class BombController : ProjectileWithTrail
    {
        protected override float TrailLifetimeInS => 3;

        public override void Activate(ProjectileActivationArgs activationArgs)
        {
            base.Activate(activationArgs);
            MovementController = new DummyMovementController();
        }

        protected override void OnImpactCleanUp()
        {
            base.OnImpactCleanUp();
            _rigidBody.velocity = Vector2.zero;
            _rigidBody.gravityScale = 0;
        }
    }
}