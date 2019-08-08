using BattleCruisers.Buildables;
using BattleCruisers.Effects.Explosions;
using BattleCruisers.Movement.Velocity;
using BattleCruisers.Projectiles.DamageAppliers;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Projectiles.Trackers;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene;
using BattleCruisers.Utils.Factories;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles
{
    public class ProjectileController : MonoBehaviour, IRemovable, ITrackable
    {
        private IExplosionStats _explosionStats;
        private IProjectileStats _projectileStats;
		private ITargetFilter _targetFilter;
        private IDamageApplier _damageApplier;
        private IExplosionManager _explosionManager;
        private ITarget _parent;
        private ISoundPlayer _soundPlayer;

        // Have this to defer damaging the target until the next FixedUpdate(), because
        // there is a bug in Unity that if the target is destroyed from OnTriggerEnter2D()
        // the target collider does not trigger OnTriggerExit2D().  I filed a bug with
        // Unity so *hopefully* this is fixed one day and I can remove this deferral :)
        private ITarget _targetToDamage;

		protected Rigidbody2D _rigidBody;

        private IMovementController _movementController;

        public event EventHandler Destroyed;
        public event EventHandler PositionChanged;

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
        protected virtual ISoundKey ImpactSoundKey => null;

        public Vector3 Position => transform.position;

        public void Initialise(
            IProjectileStats projectileStats, 
            Vector2 velocityInMPerS, 
            ITargetFilter targetFilter, 
            IFactoryProvider factoryProvider,
            ITarget parent)
		{
            Helper.AssertIsNotNull(projectileStats, targetFilter, factoryProvider, parent);

			_rigidBody = GetComponent<Rigidbody2D>();
			Assert.IsNotNull(_rigidBody);

            _explosionStats = GetComponent<IExplosionStats>();

			_projectileStats = projectileStats;
			_targetFilter = targetFilter;
            _parent = parent;
            _soundPlayer = factoryProvider.Sound.SoundPlayer;
            _rigidBody.velocity = velocityInMPerS;
            _rigidBody.gravityScale = _projectileStats.GravityScale;
            _targetToDamage = null;

            AdjustGameObjectDirection();

            _damageApplier = CreateDamageApplier(factoryProvider.DamageApplierFactory);
            _explosionManager = factoryProvider.ExplosionManager;
		}

        private IDamageApplier CreateDamageApplier(IDamageApplierFactory damageApplierFactory)
        {
            return
                _projectileStats.HasAreaOfEffectDamage ?
                damageApplierFactory.CreateAreaOfDamageApplier(_projectileStats) :
                damageApplierFactory.CreateSingleDamageApplier(_projectileStats);
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
            }

            AdjustGameObjectDirection();

            PositionChanged?.Invoke(this, EventArgs.Empty);
        }

        void OnTriggerEnter2D(Collider2D collider)
		{
            Logging.LogMethod(Tags.SHELLS);

			ITarget target = collider.gameObject.GetComponent<ITarget>();

			if (target != null 
                && !target.IsDestroyed
                && _targetFilter.IsMatch(target)
                && _targetToDamage == null)
			{
                _targetToDamage = target;
            }
        }

        protected virtual void DestroyProjectile()
        {
            ShowExplosionIfNecessary();

            if (ImpactSoundKey != null)
            {
                _soundPlayer.PlaySound(ImpactSoundKey, transform.position);
            }

			RemoveFromScene();
		}


        private void ShowExplosionIfNecessary()
        {
            if (_explosionStats != null)
            {
                _explosionManager.ShowExplosion(_explosionStats, transform.position);
            }
        }

        private void AdjustGameObjectDirection()
        {
            Logging.Verbose(Tags.SHELLS, $"_rigidBody.velocity: {_rigidBody.velocity}");

            transform.right = _rigidBody.velocity;
        }

        public void RemoveFromScene()
        {
            Destroy(gameObject);

            Destroyed?.Invoke(this, EventArgs.Empty);
        }
    }
}
