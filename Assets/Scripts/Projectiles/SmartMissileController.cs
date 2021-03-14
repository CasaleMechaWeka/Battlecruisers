using BattleCruisers.Buildables;
using BattleCruisers.Movement.Velocity;
using BattleCruisers.Movement.Velocity.Providers;
using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetDetectors;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.Targets.TargetProviders;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.Targets.TargetTrackers.Ranking;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Factories;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Utils.PlatformAbstractions;
using BattleCruisers.Utils.Threading;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles
{
    /// <summary>
    /// By default targets the enemy cruiser.
    /// 
    /// Detects nearby targets, and switches to them.
    /// 
    /// Once a target has been detected:
    /// + Turns off target detection
    /// + Speeds up (FELIX  Need new movement controller :P)
    /// </summary>
    public class SmartMissileController :
        ProjectileWithTrail<SmartMissileActivationArgs<ISmartProjectileStats>, ISmartProjectileStats>, 
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

        private const float MISSILE_POST_TARGET_DESTROYED_LIFETIME_IN_S = 2;

        public SpriteRenderer missile;

        protected override float TrailLifetimeInS => 3;

        private ITarget _target;
        public  ITarget Target
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

        public override void Initialise(ILocTable commonStrings, IFactoryProvider factoryProvider)
        {
            base.Initialise(commonStrings, factoryProvider);
            Assert.IsNotNull(missile);

            _transform = new TransformBC(gameObject.transform);
        }

        public override void Activate(SmartMissileActivationArgs<ISmartProjectileStats> activationArgs)
        {
            base.Activate(activationArgs);

            Target = activationArgs.EnempCruiser;

            _deferrer = _factoryProvider.DeferrerProvider.Deferrer;

            IVelocityProvider maxVelocityProvider = _factoryProvider.MovementControllerFactory.CreateStaticVelocityProvider(activationArgs.ProjectileStats.MaxVelocityInMPerS);
            ITargetProvider targetProvider = this;

            MovementController
                = _factoryProvider.MovementControllerFactory.CreateMissileMovementController(
                    _rigidBody,
                    maxVelocityProvider,
                    targetProvider,
                    _factoryProvider.TargetPositionPredictorFactory);

            _dummyMovementController = _factoryProvider.MovementControllerFactory.CreateDummyMovementController();

            SetupTargetProcessor(activationArgs);

            missile.enabled = true;
        }

        private void SetupTargetProcessor(SmartMissileActivationArgs<ISmartProjectileStats> activationArgs)
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
            _targetFinder = _factoryProvider.Targets.FinderFactory.CreateRangedTargetFinder(_enemyDetectorProvider.TargetDetector, targetFilter);

            ITargetRanker targetRanker = _factoryProvider.Targets.RankerFactory.EqualTargetRanker;
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

        private void _target_Destroyed(object sender, DestroyedEventArgs e)
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