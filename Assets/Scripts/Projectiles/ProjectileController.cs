using BattleCruisers.Buildables;
using BattleCruisers.Movement.Velocity;
using BattleCruisers.Projectiles.DamageAppliers;
using BattleCruisers.Projectiles.Explosions;
using BattleCruisers.Projectiles.Stats.Wrappers;
using BattleCruisers.Targets.TargetFinders.Filters;
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

		protected Rigidbody2D _rigidBody;
		protected IMovementController _movementController;

        public void Initialise(
            IProjectileStats projectileStats, 
            Vector2 velocityInMPerS, 
            ITargetFilter targetFilter, 
            IFactoryProvider factoryProvider)
		{
            Helper.AssertIsNotNull(projectileStats, targetFilter, factoryProvider);

			_rigidBody = gameObject.GetComponent<Rigidbody2D>();
			Assert.IsNotNull(_rigidBody);

			_projectileStats = projectileStats;
			_targetFilter = targetFilter;
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
                explosionFactory.CreateExplosion(gameObject.transform, _projectileStats.DamageRadiusInM) :
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
                _damageApplier.ApplyDamage(target);

                DestroyProjectile();
            }
        }

        protected virtual void DestroyProjectile()
        {
            _explosion.Show();

			// FELIX  Check this does not destroy the explosion, as this is the explosion's parent :/
			Destroy(gameObject);
		}

        private void AdjustGameObjectDirection()
        {
            transform.right = _rigidBody.velocity;
        }
	}
}
