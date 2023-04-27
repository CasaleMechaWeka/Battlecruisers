using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity.Providers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.ActivationArgs;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetDetectors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProcessors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProviders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers.Ranking;
using BattleCruisers.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Threading;
using UnityEngine;
using UnityEngine.Assertions;

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
        PvPProjectileWithTrail<PvPSmartMissileActivationArgs<IPvPSmartProjectileStats>, IPvPSmartProjectileStats>,
        IPvPTargetProvider,
        IPvPTargetConsumer
    {
        private IPvPTransform _transform;
        private IPvPDeferrer _deferrer;
        private IPvPMovementController _dummyMovementController;
        private PvPManualDetectorProvider _enemyDetectorProvider;
        private IPvPTargetFinder _targetFinder;
        private IPvPRankedTargetTracker _targetTracker;
        private IPvPTargetProcessor _targetProcessor;

        private const float MISSILE_POST_TARGET_DESTROYED_LIFETIME_IN_S = 0.5f;

        public SpriteRenderer missile;

        protected override float TrailLifetimeInS => 3;

        private IPvPTarget _target;
        public IPvPTarget Target
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

        public override void Initialise(ILocTable commonStrings, IPvPFactoryProvider factoryProvider)
        {
            base.Initialise(commonStrings, factoryProvider);
            Assert.IsNotNull(missile);

            _transform = new PvPTransformBC(gameObject.transform);
        }

        public override void Activate(PvPSmartMissileActivationArgs<IPvPSmartProjectileStats> activationArgs)
        {
            base.Activate(activationArgs);

            Target = activationArgs.EnempCruiser;

            _deferrer = _factoryProvider.DeferrerProvider.Deferrer;

            IPvPVelocityProvider maxVelocityProvider = _factoryProvider.MovementControllerFactory.CreateStaticVelocityProvider(activationArgs.ProjectileStats.MaxVelocityInMPerS);
            IPvPTargetProvider targetProvider = this;

            MovementController
                = _factoryProvider.MovementControllerFactory.CreateMissileMovementController(
                    _rigidBody,
                    maxVelocityProvider,
                    targetProvider,
                    _factoryProvider.TargetPositionPredictorFactory);

            _dummyMovementController = _factoryProvider.MovementControllerFactory.CreateDummyMovementController();

            SetupTargetProcessor(activationArgs);

            missile.enabled = true;

            Logging.Log(Tags.SMART_MISSILE, $"Rotation: {transform.rotation.eulerAngles}  _rigidBody.velocity: {_rigidBody.velocity}  MovementController.Velocity: {MovementController.Velocity}  activationArgs.InitialVelocityInMPerS: {activationArgs.InitialVelocityInMPerS}");
        }

        private void SetupTargetProcessor(PvPSmartMissileActivationArgs<IPvPSmartProjectileStats> activationArgs)
        {
            IPvPTargetFilter targetFilter
                = _factoryProvider.Targets.FilterFactory.CreateTargetFilter(
                    activationArgs.EnempCruiser.Faction,
                    activationArgs.ProjectileStats.AttackCapabilities);
            _enemyDetectorProvider
                = activationArgs.TargetFactories.DetectorFactory.CreateEnemyShipAndAircraftTargetDetector(
                    _transform,
                    activationArgs.ProjectileStats.DetectionRangeM,
                    _factoryProvider.Targets.RangeCalculatorProvider.BasicCalculator);
            _targetFinder = _factoryProvider.Targets.FinderFactory.CreateRangedTargetFinder(_enemyDetectorProvider.TargetDetector, targetFilter);

            IPvPTargetRanker targetRanker = _factoryProvider.Targets.RankerFactory.EqualTargetRanker;
            _targetTracker = activationArgs.TargetFactories.TrackerFactory.CreateRankedTargetTracker(_targetFinder, targetRanker);
            _targetProcessor = activationArgs.TargetFactories.ProcessorFactory.CreateTargetProcessor(_targetTracker);
            _targetProcessor.AddTargetConsumer(this);
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
            _target.Destroyed -= _target_Destroyed;
            CleanUpTargetProcessor();
            base.DestroyProjectile();
        }

        private void _target_Destroyed(object sender, PvPDestroyedEventArgs e)
        {
            e.DestroyedTarget.Destroyed -= _target_Destroyed;
            ReleaseMissile();
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
    }
}