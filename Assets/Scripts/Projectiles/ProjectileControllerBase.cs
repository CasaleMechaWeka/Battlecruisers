using BattleCruisers.Buildables;
using BattleCruisers.Effects.Explosions;
using BattleCruisers.Movement.Velocity;
using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.DamageAppliers;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Projectiles.Trackers;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene;
using BattleCruisers.Utils.BattleScene.Pools;
using BattleCruisers.Utils.Factories;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles
{
    public abstract class ProjectileControllerBase<TActivationArgs, TStats> : MonoBehaviour,
        IRemovable,
        ITrackable,
        IPoolable<TActivationArgs> 
            where TActivationArgs : ProjectileActivationArgs<TStats>
            where TStats : IProjectileStats
    {
        private IProjectileStats _projectileStats;
		private ITargetFilter _targetFilter;
        private IDamageApplier _damageApplier;
        private ITarget _parent;
        private IPool<Vector3> _explosionPool;
        protected IFactoryProvider _factoryProvider;

        // Have this to defer damaging the target until the next FixedUpdate(), because
        // there is a bug in Unity that if the target is destroyed from OnTriggerEnter2D()
        // the target collider does not trigger OnTriggerExit2D().  I filed a bug with
        // Unity so *hopefully* this is fixed one day and I can remove this deferral :)
        private ITarget _targetToDamage;

		protected Rigidbody2D _rigidBody;

        private IMovementController _movementController;

        public event EventHandler Destroyed;
        public event EventHandler PositionChanged;
        public event EventHandler Deactivated;

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

        public void Initialise(IFactoryProvider factoryProvider)
		{
            Assert.IsNotNull(factoryProvider);
            _factoryProvider = factoryProvider;

			_rigidBody = GetComponent<Rigidbody2D>();
			Assert.IsNotNull(_rigidBody);

            _explosionPool = GetComponent<IExplosionPoolChooser>()?.ChoosePool(factoryProvider.PoolProviders.ExplosionPoolProvider);
		}

        public virtual void Activate(TActivationArgs activationArgs)
        {
            gameObject.SetActive(true);
            transform.position = activationArgs.Position;

			_projectileStats = activationArgs.ProjectileStats;
			_targetFilter = activationArgs.TargetFilter;
            _parent = activationArgs.Parent;

            _rigidBody.velocity = activationArgs.InitialVelocityInMPerS;
            _rigidBody.gravityScale = activationArgs.ProjectileStats.GravityScale;
            _targetToDamage = null;

            AdjustGameObjectDirection();

            _damageApplier = CreateDamageApplier(_factoryProvider.DamageApplierFactory, activationArgs.ProjectileStats);
        }

        private IDamageApplier CreateDamageApplier(IDamageApplierFactory damageApplierFactory, IProjectileStats projectileStats)
        {
            return
                projectileStats.HasAreaOfEffectDamage ?
                damageApplierFactory.CreateAreaOfDamageApplier(projectileStats) :
                damageApplierFactory.CreateSingleDamageApplier(projectileStats);
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
                _factoryProvider.Sound.SoundPlayer.PlaySound(ImpactSoundKey, transform.position);
            }

			RemoveFromScene();
		}


        private void ShowExplosionIfNecessary()
        {
            _explosionPool?.GetItem(transform.position);
        }

        private void AdjustGameObjectDirection()
        {
            Logging.Verbose(Tags.SHELLS, $"_rigidBody.velocity: {_rigidBody.velocity}");

            if (_rigidBody.velocity != Vector2.zero)
            {
                transform.right = _rigidBody.velocity;
            }
        }

        public void RemoveFromScene()
        {
            gameObject.SetActive(false);

            Destroyed?.Invoke(this, EventArgs.Empty);
            Deactivated?.Invoke(this, EventArgs.Empty);
        }
    }
}
