using BattleCruisers.Buildables;
using BattleCruisers.Movement.Velocity;
using BattleCruisers.Movement.Velocity.Providers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.ActivationArgs;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetDetectors;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.Targets.TargetProviders;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.Targets.TargetTrackers.Ranking;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using BattleCruisers.Utils.Threading;
using UnityEngine;
using UnityEngine.Assertions;
using Unity.Netcode;
using BattleCruisers.Movement.Velocity.Homing;


namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles
{
    /// <summary>
    /// By default targets the enemy cruiser.
    /// 
    /// Detects nearby targets, and switches to them.
    /// 
    /// Once a target has been detected turns off target detection.
    /// </summary>
    public class PvPSmartMissileController :
        PvPProjectileWithTrail<PvPSmartMissileActivationArgs<ISmartProjectileStats>, ISmartProjectileStats>,
        ITargetProvider,
        ITargetConsumer
    {
        private ITransform _transform;
        private IDeferrer _deferrer;
        private IMovementController _dummyMovementController;
        private ManualDetectorProvider _enemyDetectorProvider;
        private ITargetFinder _targetFinder;
        private IRankedTargetTracker _targetTracker;
        private ITargetProcessor _targetProcessor;
        //---> CODE BY ANUJ
        private PvPRocketTarget _rocketTarget;
        private PvPSmartMissileActivationArgs<ISmartProjectileStats> _activationArgs;
        //<---

        private const float MISSILE_POST_TARGET_DESTROYED_LIFETIME_IN_S = 0.5f;

        public SpriteRenderer missile;

        protected override float TrailLifetimeInS => 3;

        private ITarget _target;
        public ITarget Target
        {
            get => _target;
            set
            {
                Logging.Log(Tags.SMART_MISSILE, $"{_target} > {value}");

                bool isInitialTarget = _target == null;

                if (_target != null)
                {
                    _target.Destroyed -= _target_Destroyed;
                }

                if (value == null)
                {
                    // Keep initial non null target
                    return;
                }

                _target = value;

                _target.Destroyed += _target_Destroyed;

                if (!isInitialTarget)
                {
                    // Only care about first target found. Hence can clean up target processor once a target is found.
                    CleanUpTargetProcessor();
                }
            }
        }

        public override void Initialise(IPvPFactoryProvider factoryProvider)
        {
            base.Initialise(factoryProvider);

            //---> CODE BY ANUJ
            _rocketTarget = GetComponentInChildren<PvPRocketTarget>();
            Assert.IsNotNull(_rocketTarget);
            //<---
            Assert.IsNotNull(missile);

            _transform = new TransformBC(gameObject.transform);
        }

        public override void Activate(PvPSmartMissileActivationArgs<ISmartProjectileStats> activationArgs)
        {
            base.Activate(activationArgs);

            _activationArgs = activationArgs;

            Target = activationArgs.EnempCruiser;

            _deferrer = _factoryProvider.DeferrerProvider.Deferrer;

            IVelocityProvider maxVelocityProvider = new StaticVelocityProvider(activationArgs.ProjectileStats.MaxVelocityInMPerS);
            ITargetProvider targetProvider = this;

            MovementController
                = new MissileMovementController(
                    _rigidBody,
                    maxVelocityProvider,
                    targetProvider);

            _dummyMovementController = new DummyMovementController();

            SetupTargetProcessor(activationArgs);

            //---> CODE BY ANUJ
            _rocketTarget.GameObject.SetActive(true);
            _rocketTarget.Initialise(activationArgs.Parent.Faction, _rigidBody, this);
            //<---

            missile.enabled = true;
            SetMissileVisibleClientRpc(true);
            Logging.Log(Tags.SMART_MISSILE, $"Rotation: {transform.rotation.eulerAngles}  _rigidBody.velocity: {_rigidBody.velocity}  MovementController.Velocity: {MovementController.Velocity}  activationArgs.InitialVelocityInMPerS: {activationArgs.InitialVelocityInMPerS}");
        }

        private void SetupTargetProcessor(PvPSmartMissileActivationArgs<ISmartProjectileStats> activationArgs)
        {
            ITargetFilter targetFilter
                = _factoryProvider.Targets.FilterFactory.CreateTargetFilter(
                    activationArgs.EnempCruiser.Faction,
                    activationArgs.ProjectileStats.AttackCapabilities);
            _enemyDetectorProvider
                = activationArgs.TargetFactories.DetectorFactory.CreateEnemyShipAndAircraftTargetDetector(
                    _transform,
                    activationArgs.ProjectileStats.DetectionRangeM,
                    _factoryProvider.Targets.RangeCalculatorProvider.BasicCalculator);
            _targetFinder = new RangedTargetFinder(_enemyDetectorProvider.TargetDetector, targetFilter);

            ITargetRanker targetRanker = _factoryProvider.Targets.RankerFactory.EqualTargetRanker;
            _targetTracker = activationArgs.TargetFactories.TrackerFactory.CreateRankedTargetTracker(_targetFinder, targetRanker);
            _targetProcessor = activationArgs.TargetFactories.ProcessorFactory.CreateTargetProcessor(_targetTracker);
            _targetProcessor.AddTargetConsumer(this);
        }

        private void Retarget()
        {
            Target = _activationArgs.EnempCruiser;

            SetupTargetProcessor(_activationArgs);
        }

        private void ReleaseMissile()
        {
            Logging.LogMethod(Tags.SMART_MISSILE);

            // Let missile keep current velocity
            MovementController = _dummyMovementController;

            // Destroy missile eventually (in case it does not hit a matching target)
            _deferrer.Defer(ConditionalDestroy, MISSILE_POST_TARGET_DESTROYED_LIFETIME_IN_S);
        }

        private void ConditionalDestroy()
        {
            if (gameObject.activeSelf)
            {
                DestroyProjectile();
            }
        }

        protected override void DestroyProjectile()
        {
            missile.enabled = false;
            //---> CODE BY ANUJ
            _rocketTarget.GameObject.SetActive(false);
            //<---
            SetMissileVisibleClientRpc(false);
            _target.Destroyed -= _target_Destroyed;
            CleanUpTargetProcessor();
            base.DestroyProjectile();
        }

        private void _target_Destroyed(object sender, DestroyedEventArgs e)
        {
            e.DestroyedTarget.Destroyed -= _target_Destroyed;
            Retarget();
            //ReleaseMissile();
        }

        private void CleanUpTargetProcessor()
        {
            Logging.LogMethod(Tags.SMART_MISSILE);

            if (_targetProcessor != null)
            {
                _enemyDetectorProvider.DisposeManagedState();
                _enemyDetectorProvider = null;

                _targetFinder.DisposeManagedState();
                _targetFinder = null;

                _targetTracker.DisposeManagedState();
                _targetTracker = null;

                _targetProcessor.RemoveTargetConsumer(this);
                _targetProcessor.DisposeManagedState();
                _targetProcessor = null;
            }
        }

        // Sava added these fields and methods

        protected override float timeToActiveTrail => 0.1f;
        protected override bool needToTeleport => true;
        private SoundType _type;
        private string _name;
        private Vector3 _pos;


        public override void OnNetworkSpawn()
        {
            if (IsClient)
                PvPBattleSceneGodClient.Instance.AddNetworkObject(GetComponent<NetworkObject>());
        }
        public override void OnNetworkDespawn()
        {
            if (IsClient)
                PvPBattleSceneGodClient.Instance.RemoveNetworkObject(GetComponent<NetworkObject>());
        }

        // Set Position
        protected override void OnSetPosition_Visible(Vector3 position, bool visible)
        {
            OnSetPosition_VisibleClientRpc(position, visible);
        }
        protected override void OnActiveClient_PositionVisible(Vector3 velocity, float gravityScale, bool isAlive, Vector3 position, bool visible)
        {
            OnActiveClient_PositionVisibleClientRpc(velocity, gravityScale, isAlive, position, visible);
        }
        // PlayExplosionSound
        protected override void OnPlayExplosionSound(SoundType type, string name, Vector3 position)
        {
            OnPlayExplosionSoundClientRpc(type, name, position);
        }

        private async void PlayExplosionSound()
        {
            await PvPBattleSceneGodClient.Instance.factoryProvider.Sound.SoundPlayer.PlaySoundAsync(new SoundKey(_type, _name), _pos);
        }

        // should be called by client

        private void Awake()
        {
            InitialiseTril();
        }

        protected override void HideEffectsOfClient()
        {
            if (IsServer)
                HideEffectsOfClientRpc();
            else
                base.HideEffectsOfClient();
        }

        protected override void ShowAllEffectsOfClient()
        {
            if (IsServer)
                ShowAllEffectsOfClientRpc();
            else
                base.ShowAllEffectsOfClient();
        }
        private void iSetActive_Rigidbody()
        {
            gameObject.SetActive(true);
            /*            _rigidBody.velocity = temp_velocity;
                        _rigidBody.gravityScale = temp_gravityScale;
                        _isActiveAndAlive = temp_isAlive;

                        if (_rigidBody.velocity != Vector2.zero)
                        {
                            transform.right = _rigidBody.velocity;
                        }*/
        }
        //----------------------------- Rpcs -----------------------------

        Vector3 temp_velocity;
        float temp_gravityScale;
        bool temp_isAlive;
        [ClientRpc]
        private void OnActiveClient_PositionVisibleClientRpc(Vector3 velocity, float gravityScale, bool isAlive, Vector3 position, bool visible)
        {
            if (!IsHost)
            {
                transform.position = position;
                if (!visible)
                    gameObject.SetActive(false);
                else
                {
                    temp_velocity = velocity;
                    temp_gravityScale = gravityScale;
                    temp_isAlive = isAlive;
                    Invoke("iSetActive_Rigidbody", timeToActiveTrail);
                }
            }
        }
        [ClientRpc]
        private void OnSetPosition_VisibleClientRpc(Vector3 position, bool visible)
        {
            if (!visible)
                gameObject.SetActive(false);
            else
                Invoke("iSetActive", timeToActiveTrail);
        }
        private void iSetActive()
        {
            gameObject.SetActive(true);
        }
        [ClientRpc]
        private void OnPlayExplosionSoundClientRpc(SoundType type, string name, Vector3 position)
        {
            _type = type;
            _name = name;
            _pos = position;
            Invoke("PlayExplosionSound", 0.05f);
        }

        // missile
        [ClientRpc]
        private void SetMissileVisibleClientRpc(bool visible)
        {
            missile.enabled = visible;
        }

        [ClientRpc]
        protected void HideEffectsOfClientRpc()
        {
            if (!IsHost)
                HideEffectsOfClient();
        }

        [ClientRpc]
        protected void ShowAllEffectsOfClientRpc()
        {
            if (!IsHost)
                ShowAllEffectsOfClient();
        }
    }
}