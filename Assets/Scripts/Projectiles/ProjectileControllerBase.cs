using BattleCruisers.Buildables;
using BattleCruisers.Data.Static;
using BattleCruisers.Movement.Velocity;
using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.DamageAppliers;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene;
using BattleCruisers.Utils.BattleScene.Pools;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles
{
    public abstract class ProjectileControllerBase<TActivationArgs, TStats> : Prefab,
        IRemovable,
        IPoolable<TActivationArgs>
            where TActivationArgs : ProjectileActivationArgs<TStats>
            where TStats : IProjectileStats
    {
        private ITargetFilter _targetFilter;
        private IDamageApplier _damageApplier;
        private IDamageApplier _singleDamageApplier;
        private ITarget _parent;
        private AudioClipWrapper _impactSound;
        public ExplosionType explosionType;

        private bool _isActiveAndAlive;

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
        public float autoDetonationTimer = 0f;
        protected IMovementController MovementController
        {
            get { return _movementController; }
            set
            {
                if (ReferenceEquals(value, _movementController))
                {
                    return;
                }

                if (_movementController != null
                    && value != null)
                {
                    Logging.Log(Tags.SHELLS, $"Passing along current velocity to movement controller: {_movementController.Velocity}  {_movementController} > {value}");
                    value.Velocity = _movementController.Velocity;
                }

                _movementController = value;
            }
        }

        public Vector3 Position => transform.position;

        public virtual void Initialise()
        {
            Logging.LogMethod(Tags.SHELLS);

            _rigidBody = GetComponent<Rigidbody2D>();
            Assert.IsNotNull(_rigidBody);

            _isActiveAndAlive = false;
            gameObject.SetActive(false);
        }

        public virtual void Activate(TActivationArgs activationArgs)
        {
            Logging.Log(Tags.SHELLS, $"position: {activationArgs.Position}  initial velocity: {activationArgs.InitialVelocityInMPerS}  current velocity: {_rigidBody.velocity}");

            gameObject.SetActive(true);
            transform.position = activationArgs.Position;

            _targetFilter = activationArgs.TargetFilter;
            _parent = activationArgs.Parent;
            _impactSound = activationArgs.ImpactSound;

            _rigidBody.velocity = activationArgs.InitialVelocityInMPerS;
            _rigidBody.gravityScale = activationArgs.ProjectileStats.GravityScale;
            _targetToDamage = null;

            AdjustGameObjectDirection();

            _damageApplier = CreateDamageApplier(activationArgs.ProjectileStats);
            _singleDamageApplier = new SingleDamageApplier(activationArgs.ProjectileStats.Damage);
            _isActiveAndAlive = true;

            if (gameObject.activeInHierarchy && autoDetonationTimer > 0f)
            {
                IEnumerator timedSelfDestroy = TimedSelfDestroy();
                StartCoroutine(timedSelfDestroy);
            }
        }

        public void Activate(TActivationArgs activationArgs, Faction faction)
        {

        }

        private IDamageApplier CreateDamageApplier(IProjectileStats projectileStats)
        {
            return
                projectileStats.HasAreaOfEffectDamage ?
                    new AreaOfEffectDamageApplier(projectileStats, new DummyTargetFilter(isMatchResult: true)) :
                    new SingleDamageApplier(projectileStats.Damage);
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
                //if (_targetToDamage.IsShield())
                //{
                //    _singleDamageApplier.ApplyDamage(_targetToDamage, transform.position, damageSource: _parent);
                //}
                //else{
                _damageApplier.ApplyDamage(_targetToDamage, transform.position, damageSource: _parent);
                //}
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
            PrefabFactory.ShowExplosion(explosionType, transform.position);
            SoundPlayer.PlaySound(_impactSound.AudioClip, transform.position);
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

            MovementController = null;
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

        IEnumerator TimedSelfDestroy()
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(autoDetonationTimer, autoDetonationTimer * 1.5f));
            DestroyProjectile();
            _isActiveAndAlive = false;
        }
    }
}
