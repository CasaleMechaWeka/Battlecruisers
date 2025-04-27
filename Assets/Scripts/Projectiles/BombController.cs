using BattleCruisers.Movement.Velocity;
using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using UnityEngine;

namespace BattleCruisers.Projectiles
{
    public class BombController : ProjectileWithTrail<ProjectileActivationArgs<ProjectileStats>, ProjectileStats>
    {
        protected override float TrailLifetimeInS => 3;

        public override void Activate(ProjectileActivationArgs<ProjectileStats> activationArgs)
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