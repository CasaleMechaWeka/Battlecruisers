using BattleCruisers.Buildables;
using BattleCruisers.Movement.Velocity;
using BattleCruisers.Movement.Velocity.Homing;
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
    /// Once a target has been detected turns off target detection.
    /// </summary>
    public class SmartMissileController :
        ProjectileWithTrail<ProjectileActivationArgs, ProjectileStats>,
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
        private RocketTarget _rocketTarget;

        private const float MISSILE_POST_TARGET_DESTROYED_LIFETIME_IN_S = 0.5f;

        public SpriteRenderer missile;

        protected override float TrailLifetimeInS => 3;

        private ITarget _target;

        private ProjectileActivationArgs _activationArgs;
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

        public override void Initialise()
        {
            base.Initialise();

            _rocketTarget = GetComponentInChildren<RocketTarget>();
            Assert.IsNotNull(_rocketTarget);

            Assert.IsNotNull(missile);

            _transform = new TransformBC(gameObject.transform);
        }

        public override void Activate(ProjectileActivationArgs activationArgs)
        {
            base.Activate(activationArgs);
            _activationArgs = activationArgs;

            Target = activationArgs.EnemyCruiser;

            _deferrer = FactoryProvider.DeferrerProvider.Deferrer;

            IVelocityProvider maxVelocityProvider = new StaticVelocityProvider(activationArgs.ProjectileStats.MaxVelocityInMPerS);
            ITargetProvider targetProvider = this;

            MovementController
                = new MissileMovementController(
                    _rigidBody,
                    maxVelocityProvider,
                    targetProvider);

            _dummyMovementController = new DummyMovementController();

            SetupTargetProcessor(activationArgs);

            _rocketTarget.GameObject.SetActive(true);
            _rocketTarget.Initialise(activationArgs.Parent.Faction, _rigidBody, this);

            missile.enabled = true;

            Logging.Log(Tags.SMART_MISSILE, $"Rotation: {transform.rotation.eulerAngles}  _rigidBody.velocity: {_rigidBody.velocity}  MovementController.Velocity: {MovementController.Velocity}  activationArgs.InitialVelocityInMPerS: {activationArgs.InitialVelocityInMPerS}");
        }

        private void SetupTargetProcessor(ProjectileActivationArgs activationArgs)
        {
            ITargetFilter targetFilter
                = new FactionAndTargetTypeFilter(
                    activationArgs.EnemyCruiser.Faction,
                    activationArgs.ProjectileStats.AttackCapabilities);
            _enemyDetectorProvider
                = activationArgs.TargetFactories.DetectorFactory.CreateEnemyShipAndAircraftTargetDetector(
                    _transform,
                    activationArgs.ProjectileStats.DetectionRangeM,
                    FactoryProvider.Targets.RangeCalculatorProvider.BasicCalculator);
            _targetFinder = new RangedTargetFinder(_enemyDetectorProvider.TargetDetector, targetFilter);

            ITargetRanker targetRanker = FactoryProvider.Targets.RankerFactory.EqualTargetRanker;
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

        private void Retarget()
        {
            Target = _activationArgs.EnemyCruiser;

            SetupTargetProcessor(_activationArgs);
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
            _rocketTarget.GameObject.SetActive(false);
            _target.Destroyed -= _target_Destroyed;
            CleanUpTargetProcessor();
            base.DestroyProjectile();
        }

        private void _target_Destroyed(object sender, DestroyedEventArgs e)
        {
            e.DestroyedTarget.Destroyed -= _target_Destroyed;
            if (_activationArgs.EnemyCruiser != null)
                Retarget();
            else
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