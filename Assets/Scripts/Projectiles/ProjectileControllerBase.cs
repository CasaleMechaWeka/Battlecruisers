using BattleCruisers.Buildables;
using BattleCruisers.Effects.Explosions;
using BattleCruisers.Effects.Explosions.Pools;
using BattleCruisers.Movement.Velocity;
using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.DamageAppliers;
using BattleCruisers.Projectiles.Stats;
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
        IPoolable<TActivationArgs> 
            where TActivationArgs : ProjectileActivationArgs<TStats>
            where TStats : IProjectileStats
    {
        private IProjectileStats _projectileStats;
		private ITargetFilter _targetFilter;
        private IDamageApplier _damageApplier;
        private ITarget _parent;
        private IPool<IExplosion, Vector3> _explosionPool;
        private TrailRenderer[] _trailRenderers;
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

        // By default have no impact sound
        protected virtual ISoundKey ImpactSoundKey => null;

        public Vector3 Position => transform.position;

        public void Initialise(IFactoryProvider factoryProvider)
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
            ShowExplosion();

            if (ImpactSoundKey != null)
            {
                _factoryProvider.Sound.SoundPlayer.PlaySound(ImpactSoundKey, transform.position);
            }

			RemoveFromScene();
		}


        private void ShowExplosion()
        {
            _explosionPool.GetItem(transform.position);
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
            if (!gameObject.activeSelf)
            {
                return;
            }

            if (MovementController != null)
            {
                MovementController.Velocity = Vector2.zero;
            }

            gameObject.SetActive(false);

            Destroyed?.Invoke(this, EventArgs.Empty);
            Deactivated?.Invoke(this, EventArgs.Empty);
        }
    }
}
