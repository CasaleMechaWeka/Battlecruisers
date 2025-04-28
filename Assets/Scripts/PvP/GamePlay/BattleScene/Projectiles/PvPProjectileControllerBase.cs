using BattleCruisers.Buildables;
using BattleCruisers.Movement.Velocity;
using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.DamageAppliers;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene;
using BattleCruisers.Utils.BattleScene.Pools;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using Unity.Netcode.Components;
using Unity.Netcode;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.DamageAppliers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles
{
    public abstract class PvPProjectileControllerBase<TPvPActivationArgs, TPvPStats> : PvPPrefab,
        IRemovable,
        IPoolable<TPvPActivationArgs>
            where TPvPActivationArgs : ProjectileActivationArgs
            where TPvPStats : ProjectileStats
    {
        private ITargetFilter _targetFilter;
        private IDamageApplier _damageApplier;
        private IDamageApplier _singleDamageApplier;
        private ITarget _parent;
        private AudioClipWrapper _impactSound;
        public PvPExplosionType explosionType;

        protected bool _isActiveAndAlive;
        protected virtual bool needToTeleport { get => false; }

        // Have this to defer damaging the target until the next FixedUpdate(), because
        // there is a bug in Unity that if the target is destroyed from OnTriggerEnter2D()
        // the target collider does not trigger OnTriggerExit2D().  I filed a bug with
        // Unity so *hopefully* this is fixed one day and I can remove this deferral :)
        private ITarget _targetToDamage;

        protected Rigidbody2D _rigidBody;

        public event EventHandler Destroyed;
        public event EventHandler PositionChanged;
        public event EventHandler Deactivated;
        NetworkVariable<float> autoDetonationTimer = new NetworkVariable<float>(0f);
        public float AutoDetonationTimer = 0f;

        private IMovementController _movementController;
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
            if (!IsHost)
            {
                _rigidBody = GetComponent<Rigidbody2D>();
                Assert.IsNotNull(_rigidBody);
                _isActiveAndAlive = false;
            }
            else
            {
                Logging.LogMethod(Tags.SHELLS);

                //Debug.Log("[PvPProjectileControllerBase] Initialise() started.");
                _rigidBody = GetComponent<Rigidbody2D>();
                //Debug.Log("[PvPProjectileControllerBase] Rigidbody assigned: " + (_rigidBody != null) + ", Explosion pool set: " + (_explosionPool != null));
                Assert.IsNotNull(_rigidBody);

                _isActiveAndAlive = false;
                OnSetPosition_Visible(Position, false);
                gameObject.SetActive(false);
            }
        }

        public virtual void Activate(TPvPActivationArgs activationArgs)
        {
            Logging.Log(Tags.SHELLS, $"position: {activationArgs.Position}  initial velocity: {activationArgs.InitialVelocityInMPerS}  current velocity: {_rigidBody.velocity}");

            //Debug.Log("[PvPProjectileControllerBase] Activate() called. Position: " + activationArgs.Position + ", InitialVelocity: " + activationArgs.InitialVelocityInMPerS + ", ImpactSound: " + (activationArgs.ImpactSound != null && activationArgs.ImpactSound.AudioClip != null ? activationArgs.ImpactSound.AudioClip.name : "null"));

            gameObject.SetActive(true);
            transform.position = activationArgs.Position;

            _targetFilter = activationArgs.TargetFilter;
            _parent = activationArgs.Parent;
            _impactSound = activationArgs.ImpactSound;

            _rigidBody.velocity = activationArgs.InitialVelocityInMPerS;
            //Debug.Log("[PvPProjectileControllerBase] Setting Rigidbody velocity to: " + activationArgs.InitialVelocityInMPerS + " and gravityScale to: " + activationArgs.ProjectileStats.GravityScale);
            _rigidBody.gravityScale = activationArgs.ProjectileStats.GravityScale;
            _targetToDamage = null;

            AdjustGameObjectDirection();

            _damageApplier = CreateDamageApplier(activationArgs.ProjectileStats);
            _singleDamageApplier = new SingleDamageApplier(activationArgs.ProjectileStats.Damage);
            _isActiveAndAlive = true;

            autoDetonationTimer.Value = AutoDetonationTimer;

            if (gameObject.activeInHierarchy && autoDetonationTimer.Value > 0f)
            {
                if (IsHost)
                {
                    StartCoroutine(TimedSelfDestroy());
                }
                else
                {
                    TimedSelfDestroyClientRpc();
                }
            }

            /*            OnSetPosition_Visible(Position, true);
                        OnActiveClient(activationArgs.InitialVelocityInMPerS, activationArgs.ProjectileStats.GravityScale, _isActiveAndAlive);*/

            OnActiveClient_PositionVisible(activationArgs.InitialVelocityInMPerS, activationArgs.ProjectileStats.GravityScale, _isActiveAndAlive, Position, true);
            if (needToTeleport && GetComponent<NetworkTransform>() != null)
                GetComponent<NetworkTransform>().Teleport(activationArgs.Position, transform.rotation, transform.localScale);
        }
        public void Activate(TPvPActivationArgs activationArgs, Faction faction)
        {
        }

        private IDamageApplier CreateDamageApplier(ProjectileStats projectileStats)
        {
            return
                projectileStats.damageRadiusInM != 0 ?
                    new PvPAreaOfEffectDamageApplier(projectileStats, new DummyTargetFilter(isMatchResult: true)) :
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
                MovementController = null;
                DestroyProjectile();
                if (_damageApplier != null && _targetToDamage != null && _parent != null)
                {
                    _damageApplier.ApplyDamage(_targetToDamage, transform.position, damageSource: _parent);
                }
                else
                {
                    Debug.LogError("Cannot apply damage because _damageApplier, _targetToDamage, or _parent is null.");
                }
                _isActiveAndAlive = false;
            }
            else if (MovementController != null)
            {
                MovementController.AdjustVelocity();
            }
            AdjustGameObjectDirection();
            PositionChanged?.Invoke(this, EventArgs.Empty);

            //Debug.Log("[PvPProjectileControllerBase] FixedUpdate() running. isActiveAndAlive: " + _isActiveAndAlive + ", Rigidbody velocity: " + _rigidBody.velocity);
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (!IsHost)
                return;
            Logging.LogMethod(Tags.SHELLS);
            ITarget target = collider.gameObject.GetComponent<ITargetProxy>()?.Target;
            if (target != null
                && !target.IsDestroyed
                && _targetFilter != null
                && _targetFilter.IsMatch(target)
                && _targetToDamage == null)
            {
                //Debug.Log("[PvPProjectileControllerBase] Target found: " + target);
                _targetToDamage = target;
            }
        }

        protected virtual void DestroyProjectile()
        {
            if (IsHost)
            {
                ShowExplosion();
                RemoveFromScene();
                Debug.LogWarning("DestroyProjectile");
            }

            //Debug.Log("[PvPProjectileControllerBase] DestroyProjectile() called.");
        }

        protected void ShowExplosion()
        {
            PvPPrefabFactory.ShowExplosion(explosionType, transform.position);
            OnPlayExplosionSound(SoundType.Explosions, _impactSound.AudioClip.name, transform.position);

            //Debug.Log("[PvPProjectileControllerBase] Showing explosion at position: " + transform.position + " with sound: " + (_impactSound != null && _impactSound.AudioClip != null ? _impactSound.AudioClip.name : "null"));
        }

        private void AdjustGameObjectDirection()
        {
            Logging.Verbose(Tags.SHELLS, $"_rigidBody.velocity: {_rigidBody.velocity}");
            //Debug.Log("[PvPProjectileControllerBase] AdjustGameObjectDirection() called with Rigidbody velocity: " + _rigidBody.velocity);
            if (_rigidBody.velocity != Vector2.zero)
            {
                transform.right = _rigidBody.velocity;
                //Debug.Log("[PvPProjectileControllerBase] New transform.right set to: " + transform.right);
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
            OnSetPosition_Visible(Position, false);
            gameObject.SetActive(false);
            InvokeDestroyed();
            InvokeDeactivated();

            //Debug.Log("[PvPProjectileControllerBase] RemoveFromScene() invoked.");
            //Debug.Log("[PvPProjectileControllerBase] GameObject deactivated and removed from scene.");
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

        protected virtual void OnActiveClient(Vector3 velocity, float gravityScale, bool isAlive)
        {
        }

        protected virtual void OnActiveClient_PositionVisible(Vector3 velocity, float gravityScale, bool isAlive, Vector3 position, bool visible)
        {

        }

        protected virtual void OnPlayExplosionSound(SoundType type, string name, Vector3 position)
        {

        }

        IEnumerator TimedSelfDestroy()
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(autoDetonationTimer.Value, autoDetonationTimer.Value * 1.5f));
            DestroyProjectile();
            _isActiveAndAlive = false;
        }

        [ClientRpc]
        void TimedSelfDestroyClientRpc()
        {
            StartCoroutine(TimedSelfDestroy());
        }
    }
}
