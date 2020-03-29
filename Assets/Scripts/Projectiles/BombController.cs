using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using UnityEngine;

namespace BattleCruisers.Projectiles
{
    public class BombController : ProjectileWithTrail<ProjectileActivationArgs<IProjectileStats>, IProjectileStats>
    {
        protected override float TrailLifetimeInS => 3;

        public override void Activate(ProjectileActivationArgs<IProjectileStats> activationArgs)
        {
            base.Activate(activationArgs);
            MovementController = _factoryProvider.MovementControllerFactory.CreateDummyMovementController();
        }

        protected override void OnImpactCleanUp()
        {
            base.OnImpactCleanUp();
            _rigidBody.velocity = Vector2.zero;
            _rigidBody.gravityScale = 0;
        }
    }
}