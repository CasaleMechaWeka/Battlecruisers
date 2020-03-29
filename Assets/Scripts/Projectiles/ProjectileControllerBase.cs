using BattleCruisers.Buildables;
using BattleCruisers.Effects.Explosions;
using BattleCruisers.Effects.Explosions.Pools;
using BattleCruisers.Movement.Velocity;
using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.DamageAppliers;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene;
using BattleCruisers.Utils.BattleScene.Pools;
using BattleCruisers.Utils.Factories;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles
{
    public abstract class ProjectileControllerBase<TActivationArgs, TStats> : Projectile,
        IRemovable,
        IPoolable<TActivationArgs> 
            where TActivationArgs : ProjectileActivationArgs<TStats>
            where TStats : IProjectileStats
    {
		private ITargetFilter _targetFilter;
        private IDamageApplier _damageApplier;
        private ITarget _parent;
        private IAudioClipWrapper _impactSound;
        private IPool<IExplosion, Vector3> _explosionPool;
        private TrailRenderer[] _trailRenderers;
        private bool _isActiveAndAlive;
        protected IFactoryProvider _factoryProvider;

        // Have this to defer damaging the target until the next FixedUpdate(), because
        // there is a bug in Unity that if the target is destroyed from OnTriggerEnter2D()
        // the target collider does not trigger OnTriggerExit2D().  I filed a bug with
        // Unity so *hopefully* this is fixed one day and I can remove this deferral :)
        private ITarget _targetToDamage;

		protected Rigidbody2D _rigidBody;

        public event EventHandler Destroyed;
        public event EventHandler PositionChanged;
        public event EventHandler Deactivated;

        private IMovementController _movementController;
        protected IMovementController MovementController
        {
            get { return _movementController; }
            set
            {
                Assert.IsNotNull(value);

                if (ReferenceEquals(value, _movementController))
                {
                    return;
                }

                if (_movementController != null)
                {
                    Logging.Log(Tags.SHELLS, $"Passing along current velocity to movement controller: {_movementController.Velocity}");
                    value.Velocity = _movementController.Velocity;
                }

                _movementController = value;
            }
        }

        public Vector3 Position => transform.position;

        public virtual void Initialise(IFactoryProvider factoryProvider)
		{
            Logging.LogMethod(Tags.SHELLS);

            Assert.IsNotNull(factoryProvider);
            _factoryProvider = factoryProvider;

			_rigidBody = GetComponent<Rigidbody2D>();
			Assert.IsNotNull(_rigidBody);

            _trailRenderers = GetComponentsInChildren<TrailRenderer>();
            Assert.IsNotNull(_trailRenderers);

            IExplosionPoolChooser explosionPoolChooser = GetComponent<IExplosionPoolChooser>();
            Assert.IsNotNull(explosionPoolChooser);
            _explosionPool = explosionPoolChooser.ChoosePool(factoryProvider.PoolProviders.ExplosionPoolProvider);

            _isActiveAndAlive = false;
            gameObject.SetActive(false);
        }

        public virtual void Activate(TActivationArgs activationArgs)
        {
            Logging.Log(Tags.SHELLS, $"position: {activationArgs.Position}  initial velocity: {activationArgs.InitialVelocityInMPerS}  current velocity: {_rigidBody.velocity}");

            gameObject.SetActive(true);
            transform.position = activationArgs.Position;

            foreach (TrailRenderer trailRenderer in _trailRenderers)
            {
                trailRenderer.Clear();
            }

			_targetFilter = activationArgs.TargetFilter;
            _parent = activationArgs.Parent;
            _impactSound = activationArgs.ImpactSound;

            _rigidBody.velocity = activationArgs.InitialVelocityInMPerS;
            _rigidBody.gravityScale = activationArgs.ProjectileStats.GravityScale;
            _targetToDamage = null;

            AdjustGameObjectDirection();

            _damageApplier = CreateDamageApplier(_factoryProvider.DamageApplierFactory, activationArgs.ProjectileStats);
            _isActiveAndAlive = true;
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
            if (!_isActiveAndAlive)
            {
                return;
            }

            if (_targetToDamage != null)
            {
                DestroyProjectile();
                _damageApplier.ApplyDamage(_targetToDamage, transform.position, damageSource: _parent);
                _isActiveAndAlive = false;
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

			ITarget target = collider.gameObject.GetComponent<ITargetProxy>()?.Target;

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
            Logging.LogMethod(Tags.SHELLS);

            ShowExplosion();
			RemoveFromScene();
		}

        protected void ShowExplosion()
        {
            _explosionPool.GetItem(transform.position);
            _factoryProvider.Sound.SoundPlayer.PlaySound(_impactSound, transform.position);
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
            Logging.LogMethod(Tags.SHELLS);

            if (!gameObject.activeSelf)
            {
                return;
            }

            if (MovementController != null)
            {
                MovementController.Velocity = Vector2.zero;
            }

            gameObject.SetActive(false);
            InvokeDestroyed();
            InvokeDeactivated();
        }

        protected void InvokeDestroyed()
        {
            Destroyed?.Invoke(this, EventArgs.Empty);
        }

        protected void InvokeDeactivated()
        {
            Deactivated?.Invoke(this, EventArgs.Empty);
        }
    }
}
