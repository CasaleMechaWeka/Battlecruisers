using BattleCruisers.Effects.Trails;
using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene;
using BattleCruisers.Utils.BattleScene.Pools;
using BattleCruisers.Utils.Factories;
using BattleCruisers.Utils.Localisation;
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
    public abstract class ProjectileWithTrail<TActivationArgs, TStats> : ProjectileControllerBase<TActivationArgs, TStats>,
        IRemovable,
        IPoolable<TActivationArgs>
            where TActivationArgs : ProjectileActivationArgs<TStats>
            where TStats : IProjectileStats
    {
        private Collider2D _collider;
        private IDeferrer _deferrer;
        private IProjectileTrail _trail;

        protected virtual float TrailLifetimeInS { get => 10; }

        public override void Initialise(ILocTable commonStrings, IFactoryProvider factoryProvider)
        {
            base.Initialise(commonStrings, factoryProvider);

            _deferrer = factoryProvider.DeferrerProvider.Deferrer;

            _collider = GetComponent<Collider2D>();
            Assert.IsNotNull(_collider);

            _trail = GetComponentInChildren<IProjectileTrail>();
            Assert.IsNotNull(_trail);
            _trail.Initialise();
        }

        public override void Activate(TActivationArgs activationArgs)
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