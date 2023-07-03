using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Explosions;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Explosions.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.ActivationArgs;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.DamageAppliers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Audio;
using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEditor;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles
{
    public abstract class PvPProjectileControllerBase<TPvPActivationArgs, TPvPStats> : PvPProjectile,
        IPvPRemovable,
        IPvPPoolable<TPvPActivationArgs>
            where TPvPActivationArgs : PvPProjectileActivationArgs<TPvPStats>
            where TPvPStats : IPvPProjectileStats
    {
        private IPvPTargetFilter _targetFilter;
        private IPvPDamageApplier _damageApplier;
        private IPvPDamageApplier _singleDamageApplier;
        private IPvPTarget _parent;
        private IPvPAudioClipWrapper _impactSound;
        private IPvPPool<IPvPExplosion, Vector3> _explosionPool;

        protected bool _isActiveAndAlive;
        protected IPvPFactoryProvider _factoryProvider;

        // Have this to defer damaging the target until the next FixedUpdate(), because
        // there is a bug in Unity that if the target is destroyed from OnTriggerEnter2D()
        // the target collider does not trigger OnTriggerExit2D().  I filed a bug with
        // Unity so *hopefully* this is fixed one day and I can remove this deferral :)
        private IPvPTarget _targetToDamage;

        protected Rigidbody2D _rigidBody;

        public event EventHandler Destroyed;
        public event EventHandler PositionChanged;
        public event EventHandler Deactivated;

        private IPvPMovementController _movementController;
        protected IPvPMovementController MovementController
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


        // should be called by server
        public virtual void Initialise(ILocTable commonStrings, IPvPFactoryProvider factoryProvider)
        {
            Logging.LogMethod(Tags.SHELLS);
            Helper.AssertIsNotNull(commonStrings, factoryProvider);

            _commonStrings = commonStrings;
            _factoryProvider = factoryProvider;

            _rigidBody = GetComponent<Rigidbody2D>();
            Assert.IsNotNull(_rigidBody);

            IPvPExplosionPoolChooser explosionPoolChooser = GetComponent<IPvPExplosionPoolChooser>();
            Assert.IsNotNull(explosionPoolChooser);
            _explosionPool = explosionPoolChooser.ChoosePool(factoryProvider.PoolProviders.ExplosionPoolProvider);

            _isActiveAndAlive = false;
            gameObject.SetActive(false);
            OnSetPosition_Visible(Position, false);
        }

        // should be called by client
        public virtual void Initialise()
        {
            Logging.LogMethod(Tags.SHELLS);

            _rigidBody = GetComponent<Rigidbody2D>();
            Assert.IsNotNull(_rigidBody);
            _isActiveAndAlive = false;
        }

        public virtual void Activate(TPvPActivationArgs activationArgs)
        {
            Logging.Log(Tags.SHELLS, $"position: {activationArgs.Position}  initial velocity: {activationArgs.InitialVelocityInMPerS}  current velocity: {_rigidBody.velocity}");

            gameObject.SetActive(true);
            transform.position = activationArgs.Position;
            OnSetPosition_Visible(Position, true);

            _targetFilter = activationArgs.TargetFilter;
            _parent = activationArgs.Parent;
            _impactSound = activationArgs.ImpactSound;

            _rigidBody.velocity = activationArgs.InitialVelocityInMPerS;
            _rigidBody.gravityScale = activationArgs.ProjectileStats.GravityScale;
            _targetToDamage = null;
            OnActiveClient(_rigidBody.velocity, _rigidBody.gravityScale);

            AdjustGameObjectDirection();

            _damageApplier = CreateDamageApplier(_factoryProvider.DamageApplierFactory, activationArgs.ProjectileStats);
            _singleDamageApplier = _factoryProvider.DamageApplierFactory.CreateSingleDamageApplier(activationArgs.ProjectileStats);
            _isActiveAndAlive = true;
        }

        public void Activate(TPvPActivationArgs activationArgs, PvPFaction faction)
        {
        }

        private IPvPDamageApplier CreateDamageApplier(IPvPDamageApplierFactory damageApplierFactory, IPvPProjectileStats projectileStats)
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
            if (IsClient)
                return;
            Logging.LogMethod(Tags.SHELLS);

            IPvPTarget target = collider.gameObject.GetComponent<IPvPTargetProxy>()?.Target;

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
            /*            string filePath = AssetDatabase.GetAssetPath(_impactSound.AudioClip);
                        string fileName = System.IO.Path.GetFileName(filePath);
                        OnPlayExplosionSound(PvPSoundType.Explosions, fileName.Split(".")[0], transform.position);*/
            OnPlayExplosionSound(PvPSoundType.Explosions, _impactSound.AudioClip.name, transform.position);
            // _factoryProvider.Sound.SoundPlayer.PlaySound(_impactSound, transform.position);
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
            OnSetPosition_Visible(Position, false);
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

        protected virtual void OnSetPosition_Visible(Vector3 position, bool visible)
        {

        }

        protected virtual void OnActiveClient(Vector3 velocity, float gravityScale)
        {
        }

        protected virtual void OnPlayExplosionSound(PvPSoundType type, string name, Vector3 position)
        {

        }
    }
}
