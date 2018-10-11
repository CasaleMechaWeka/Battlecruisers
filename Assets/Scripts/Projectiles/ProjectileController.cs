using BattleCruisers.Buildables;
using BattleCruisers.Movement.Velocity;
using BattleCruisers.Projectiles.DamageAppliers;
using BattleCruisers.Effects.Explosions;
using BattleCruisers.Projectiles.Stats.Wrappers;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene;
using BattleCruisers.Utils.Factories;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles
{
    public class ProjectileController : MonoBehaviour, IDestructable
    {
        private IProjectileStats _projectileStats;
		private ITargetFilter _targetFilter;
        private IDamageApplier _damageApplier;
        private IExplosion _explosion;
        private ITarget _parent;
        private ISoundPlayer _soundPlayer;

        // Have this to defer damaging the target until the next FixedUpdate(), because
        // there is a bug in Unity that if the target is destroyed from OnTriggerEnter2D()
        // the target collider does not trigger OnTriggerExit2D().  I filed a bug with
        // Unity so *hopefully* this is fixed one day and I can remove this deferral :)
        private ITarget _targetToDamage;

		protected Rigidbody2D _rigidBody;

        private IMovementController _movementController;
		protected IMovementController MovementController
        {
            get { return _movementController; }
            set
            {
                Assert.IsNotNull(value);
                Assert.AreNotEqual(value, _movementController);

                if (_movementController != null)
                {
                    value.Velocity = _movementController.Velocity;
                }

                _movementController = value;
            }
        }

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
            _soundPlayer = factoryProvider.Sound.SoundPlayer;
            _rigidBody.velocity = velocityInMPerS;
			_rigidBody.gravityScale = _projectileStats.IgnoreGravity ? 0 : 1;
            _targetToDamage = null;

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
            if (_targetToDamage != null)
            {
                _damageApplier.ApplyDamage(_targetToDamage, transform.position, damageSource: _parent);

                DestroyProjectile();
            }
			else if (MovementController != null)
            {
                MovementController.AdjustVelocity();

                AdjustGameObjectDirection();
            }
        }

        void OnTriggerEnter2D(Collider2D collider)
		{
			Logging.Log(Tags.SHELLS, "ProjectileController.OnTriggerEnter2D()");

			ITarget target = collider.gameObject.GetComponent<ITarget>();

			if (target != null 
                && _targetFilter.IsMatch(target)
                && _targetToDamage == null)
			{
                _targetToDamage = target;
            }
        }

        protected virtual void DestroyProjectile()
        {
            _explosion.Show(transform.position);

            if (ImpactSoundKey != null)
            {
                _soundPlayer.PlaySound(ImpactSoundKey, transform.position);
            }

			Destroy(gameObject);
		}

        private void AdjustGameObjectDirection()
        {
            transform.right = _rigidBody.velocity;
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}
