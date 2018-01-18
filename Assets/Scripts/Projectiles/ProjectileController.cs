using BattleCruisers.Buildables;
using BattleCruisers.Movement.Velocity;
using BattleCruisers.Projectiles.DamageAppliers;
using BattleCruisers.Projectiles.Explosions;
using BattleCruisers.Projectiles.Stats.Wrappers;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles
{
    public class ProjectileController : MonoBehaviour
	{
        private IProjectileStats _projectileStats;
		private ITargetFilter _targetFilter;
        private IDamageApplier _damageApplier;
        private IExplosion _explosion;
        private ITarget _parent;
        private ISoundManager _soundManager;

		protected Rigidbody2D _rigidBody;
		protected IMovementController _movementController;

        // By default have no impact sound
        protected virtual ISoundKey ImpactSoundKey { get { return null; } }

        public void Initialise(
            IProjectileStats projectileStats, 
            Vector2 velocityInMPerS, 
            ITargetFilter targetFilter, 
            IFactoryProvider factoryProvider,
            ITarget parent)
		{
            Helper.AssertIsNotNull(projectileStats, targetFilter, factoryProvider, parent);

			_rigidBody = gameObject.GetComponent<Rigidbody2D>();
			Assert.IsNotNull(_rigidBody);

			_projectileStats = projectileStats;
			_targetFilter = targetFilter;
            _parent = parent;
            _soundManager = factoryProvider.SoundManager;
            _rigidBody.velocity = velocityInMPerS;
			_rigidBody.gravityScale = _projectileStats.IgnoreGravity ? 0 : 1;

            AdjustGameObjectDirection();

            _damageApplier = CreateDamageApplier(factoryProvider.DamageApplierFactory);
            _explosion = CreateExplosion(factoryProvider.ExplosionFactory);
		}

        private IDamageApplier CreateDamageApplier(IDamageApplierFactory damageApplierFactory)
        {
            return
                _projectileStats.HasAreaOfEffectDamage ?
                damageApplierFactory.CreateAreaOfDamageApplier(_projectileStats) :
                damageApplierFactory.CreateSingleDamageApplier(_projectileStats);
        }

        private IExplosion CreateExplosion(IExplosionFactory explosionFactory)
        {
            return
                _projectileStats.HasAreaOfEffectDamage ?
                explosionFactory.CreateExplosion(_projectileStats.DamageRadiusInM) :
                explosionFactory.CreateDummyExplosion();
        }

		void FixedUpdate()
		{
			if (_movementController != null)
            {
                _movementController.AdjustVelocity();

                AdjustGameObjectDirection();
            }
        }

        void OnTriggerEnter2D(Collider2D collider)
		{
			Logging.Log(Tags.SHELLS, "ProjectileController.OnTriggerEnter2D()");

			ITarget target = collider.gameObject.GetComponent<ITarget>();

			if (target != null && _targetFilter.IsMatch(target))
			{
                _damageApplier.ApplyDamage(target, transform.position, damageSource: _parent);

                DestroyProjectile();
            }
        }

        protected virtual void DestroyProjectile()
        {
            _explosion.Show(transform.position);

            if (ImpactSoundKey != null)
            {
                _soundManager.PlaySound(ImpactSoundKey, transform.position);
            }

			Destroy(gameObject);
		}

        private void AdjustGameObjectDirection()
        {
            transform.right = _rigidBody.velocity;
        }
	}
}
