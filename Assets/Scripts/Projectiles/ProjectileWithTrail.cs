using BattleCruisers.Effects.Trails;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene;
using BattleCruisers.Utils.BattleScene.Pools;
using BattleCruisers.Utils.Factories;
using BattleCruisers.Utils.Threading;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles
{
    /// <summary>
    /// Projectiles with trails (eg: rockets) should not have the trail disappear on impact.
    /// Instead, the projectile should be inert, but set the trail hang around and dissipate
    /// before deactivating completely and being recycled.
    /// </summary>
    public abstract class ProjectileWithTrail : ProjectileControllerBase,
        IRemovable,
        IPoolable<ProjectileActivationArgs>
    {
        private Collider2D _collider;
        private IDeferrer _deferrer;
        private IProjectileTrail _trail;

        protected virtual float TrailLifetimeInS { get => 10; }

        public override void Initialise()
        {
            base.Initialise();

            _deferrer = FactoryProvider.DeferrerProvider.Deferrer;

            _collider = GetComponent<Collider2D>();
            Assert.IsNotNull(_collider);

            _trail = GetComponentInChildren<IProjectileTrail>();
            Assert.IsNotNull(_trail);
            _trail.Initialise();
        }

        public override void Activate(ProjectileActivationArgs activationArgs)
        {
            base.Activate(activationArgs);

            _collider.enabled = true;
            _trail.ShowAllEffects();
        }

        protected override void DestroyProjectile()
        {
            Logging.LogMethod(Tags.SHELLS);

            ShowExplosion();
            OnImpactCleanUp();
            InvokeDestroyed();
            _deferrer.Defer(OnTrailsDoneCleanup, TrailLifetimeInS);
        }

        protected virtual void OnImpactCleanUp()
        {
            Logging.LogMethod(Tags.SHELLS);

            // Dummy movement controller doesn't set velocity to 0, so need to set rigid body velocity manually.
            _rigidBody.velocity = Vector2.zero;
            MovementController = null;
            _collider.enabled = false;
            _trail.HideEffects();
        }

        private void OnTrailsDoneCleanup()
        {
            Logging.LogMethod(Tags.SHELLS);

            gameObject.SetActive(false);
            InvokeDeactivated();
        }
    }
}