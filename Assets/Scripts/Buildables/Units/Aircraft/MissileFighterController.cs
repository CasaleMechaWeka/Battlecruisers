using System.Collections.Generic;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Data.Static;
using BattleCruisers.Movement.Velocity;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.Targets.TargetTrackers.Ranking;
using BattleCruisers.Targets.TargetProviders;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;
using BattleCruisers.Targets.TargetDetectors;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.Utils.Factories;
using BattleCruisers.Buildables.Pools;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers;
using System.Linq;

namespace BattleCruisers.Buildables.Units.Aircraft
{
    public class MissileFighterController : AircraftController, ITargetConsumer, ITargetProvider
    {
        private ITargetFinder _followableTargetFinder, _shootableTargetFinder;
        private ITargetProcessor _followableTargetProcessor, _shootableTargetProcessor;
        private IExactMatchTargetFilter _exactMatchTargetFilter;
        private IMovementController _figherMovementController;
        private IAngleHelper _angleHelper;
        private ManualDetectorProvider _followableEnemyDetectorProvider, _shootableEnemeyDetectorProvider;
        private AircraftBarrelWrapper barrelWrapper;
        private BarrelController[] _barrelControllers;
        public float enemyFollowDetectionRangeInMAir;
        private IRankedTargetTracker b;

        private const float PATROLLING_VELOCITY_DIVISOR = 2;

        private ITarget _target;
        public ITarget Target
        {
            get { return _target; }
            set
            {
                Logging.Log(Tags.FIGHTER, string.Empty + value);

                _target = value;

                if (_target == null)
                {
                    ActiveMovementController = PatrollingMovementController;
                }
                else
                {
                    ActiveMovementController = _figherMovementController;
                }
            }
        }

        protected override float MaxPatrollingVelocity => EffectiveMaxVelocityInMPerS / PATROLLING_VELOCITY_DIVISOR;
        protected override float PositionEqualityMarginInM => 2;

        public override void StaticInitialise(GameObject parent, HealthBarController healthBar, ILocTable commonStrings)
        {
            base.StaticInitialise(parent, healthBar, commonStrings);

            _barrelControllers = gameObject.GetComponentsInChildren<BarrelController>();
            barrelWrapper = gameObject.GetComponentInChildren<AircraftBarrelWrapper>();
            Assert.IsNotNull(_barrelControllers);
            for (int i = 0; i < _barrelControllers.Length; i++)
            {
                _barrelControllers[i].StaticInitialise();
            }
            Assert.IsNotNull(barrelWrapper);
            barrelWrapper.StaticInitialise();
            AddDamageStats(barrelWrapper.DamageCapability);
        }

        public override void Initialise(IUIManager uiManager, IFactoryProvider factoryProvider)
        {
            base.Initialise(uiManager, factoryProvider);
            _angleHelper = _factoryProvider.Turrets.AngleCalculatorFactory.CreateAngleHelper();
        }

        public override void Activate(BuildableActivationArgs activationArgs)
        {
            base.Activate(activationArgs);

            _figherMovementController
                = _movementControllerFactory.CreateFighterMovementController(
                    rigidBody,
                    maxVelocityProvider: this,
                    targetProvider: this,
                    safeZone: _aircraftProvider.FighterSafeZone);

            // Reset rotation
            Quaternion baseRotation = Quaternion.Euler(0, 0, 0);
            transform.rotation = baseRotation;
            rigidBody.rotation = 0;
            Logging.Verbose(Tags.FIGHTER, $"Id: {GameObject.GetInstanceID()}  After reset rotation: {rigidBody.rotation}");
        }
        protected async override void OnBuildableCompleted()
        {
            base.OnBuildableCompleted();

            Faction enemyFaction = Helper.GetOppositeFaction(Faction);
            ITarget parent = this;
            IUpdater updater = _factoryProvider.UpdaterProvider.PerFrameUpdater;
            for (int i = 0; i < _barrelControllers.Length; i++)
            {
                await _barrelControllers[i].InitialiseAsync(new BarrelControllerArgs(
                        updater,
                        _targetFactories.FilterFactory.CreateTargetFilter(enemyFaction, AttackCapabilities),
                        _factoryProvider.TargetPositionPredictorFactory.CreateLinearPredictor(),
                        _factoryProvider.Turrets.AngleCalculatorFactory.CreateAngleCalculator(),
                        _factoryProvider.Turrets.AttackablePositionFinderFactory.DummyPositionFinder,
                        _factoryProvider.Turrets.AccuracyAdjusterFactory.CreateDummyAdjuster(),
                        _movementControllerFactory.CreateRotationMovementController(_barrelControllers[0].TurretStats.TurretRotateSpeedInDegrees, _barrelControllers[0].transform, updater),
                        _factoryProvider.Turrets.TargetPositionValidatorFactory.CreateDummyValidator(),
                        _factoryProvider.Turrets.AngleLimiterFactory.CreateMissileFighterLimiter(),
                        _factoryProvider,
                        _cruiserSpecificFactories,
                        parent,
                        _cruiserSpecificFactories.GlobalBoostProviders.DummyBoostProviders,
                        _cruiserSpecificFactories.GlobalBoostProviders.DummyBoostProviders,
                        EnemyCruiser,
                        SoundKeys.Firing.BigCannon),
                        true);
            }

            SetupTargetDetection();

            _spriteChooser = await _factoryProvider.SpriteChooserFactory.CreateMissileFighterSpriteChooserAsync(this);
            for (int i = 0; i < _barrelControllers.Length; i++)
            {
                _barrelControllers[i].ApplyVariantStats(this);
            }

        }

        /// <summary>
        /// Enemies first come within following range, and then shootable range as the figher closes
        /// in on the enemy.
        /// 
        /// enemyFollowDetectionRangeInM: 
        ///		The range at which enemies are detected
        /// barrelController.turretStats.rangeInM:  
        ///		The range at which the turret can shoot enemies
        /// enemyFollowDetectionRangeInM > barrelController.turretStats.rangeInM
        /// </summary>
        private void SetupTargetDetection()
        {
            // Detect followable enemies
            _followableEnemyDetectorProvider
                = _cruiserSpecificFactories.Targets.DetectorFactory.CreateEnemyShipAndAircraftTargetDetector(
                    Transform,
                    enemyFollowDetectionRangeInMAir,
                    _targetFactories.RangeCalculatorProvider.BasicCalculator);
            Faction enemyFaction = Helper.GetOppositeFaction(Faction);
            IList<TargetType> targetTypesToFollow = new List<TargetType>() { TargetType.Aircraft, TargetType.Ships };
            ITargetFilter targetFilter = _targetFactories.FilterFactory.CreateTargetFilter(enemyFaction, targetTypesToFollow);
            _followableTargetFinder = _targetFactories.FinderFactory.CreateRangedTargetFinder(_followableEnemyDetectorProvider.TargetDetector, targetFilter);

            ITargetRanker followableTargetRanker = _targetFactories.RankerFactory.EqualTargetRanker;
            IRankedTargetTracker followableTargetTracker = _cruiserSpecificFactories.Targets.TrackerFactory.CreateRankedTargetTracker(_followableTargetFinder, followableTargetRanker);
            _followableTargetProcessor = _cruiserSpecificFactories.Targets.ProcessorFactory.CreateTargetProcessor(followableTargetTracker);
            _followableTargetProcessor.AddTargetConsumer(this);

            // Detect shootable enemies
            _exactMatchTargetFilter = _targetFactories.FilterFactory.CreateMulitpleExactMatchTargetFilter();
            _followableTargetProcessor.AddTargetConsumer(_exactMatchTargetFilter);
            _shootableEnemeyDetectorProvider
                = _cruiserSpecificFactories.Targets.DetectorFactory.CreateEnemyAircraftTargetDetector(
                    Transform,
                    Mathf.Max(_barrelControllers.Select(barrel => barrel.TurretStats.RangeInM).ToArray()),
                    _targetFactories.RangeCalculatorProvider.BasicCalculator);
            _shootableTargetFinder = _targetFactories.FinderFactory.CreateRangedTargetFinder(_shootableEnemeyDetectorProvider.TargetDetector, _exactMatchTargetFilter);

            ITargetRanker shootableTargetRanker = _targetFactories.RankerFactory.EqualTargetRanker;
            IRankedTargetTracker shootableTargetTracker = _cruiserSpecificFactories.Targets.TrackerFactory.CreateRankedTargetTracker(_shootableTargetFinder, shootableTargetRanker);
            _shootableTargetProcessor = _cruiserSpecificFactories.Targets.ProcessorFactory.CreateTargetProcessor(shootableTargetTracker);
            for (int i = 0; i < _barrelControllers.Length; i++)
                _shootableTargetProcessor.AddTargetConsumer(_barrelControllers[i]);
            if (barrelWrapper != null)
            {
                ManualDetectorProvider shootableDetectorProvider = _cruiserSpecificFactories.Targets.DetectorFactory.CreateEnemyShipAndAircraftTargetDetector(
                    Transform,
                    Mathf.Max(_barrelControllers.Select(barrel => barrel.TurretStats.RangeInM).ToArray()),
                    _targetFactories.RangeCalculatorProvider.BasicCalculator);
                ITargetFinder shootableTargetFinder = _targetFactories.FinderFactory.CreateRangedTargetFinder(shootableDetectorProvider.TargetDetector, _exactMatchTargetFilter);

                IRankedTargetTracker wrapperTargetTracker = _cruiserSpecificFactories.Targets.TrackerFactory.CreateRankedTargetTracker(shootableTargetFinder, shootableTargetRanker);
                b = wrapperTargetTracker;
                ITargetProcessor wrapperTargetProcessor = _cruiserSpecificFactories.Targets.ProcessorFactory.CreateTargetProcessor(wrapperTargetTracker);

                wrapperTargetProcessor.AddTargetConsumer(barrelWrapper);
            }
        }

        protected override IList<IPatrolPoint> GetPatrolPoints()
        {
            return Helper.ConvertVectorsToPatrolPoints(_aircraftProvider.FindMissileFighterPatrolPoints(cruisingAltitudeInM));
        }

        protected override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            FaceVelocityDirection();
            //Debug.LogError(b.HighestPriorityTarget.Target.TargetType);
            //Debug.LogError(b.HighestPriorityTarget.Target.GameObject.name);
        }

        private void FaceVelocityDirection()
        {
            Logging.Verbose(Tags.FIGHTER, $"Id: {GameObject.GetInstanceID()}  Velocity: {Velocity}");

            if (Velocity != Vector2.zero)
            {
                float zRotationInDegrees = _angleHelper.FindAngle(Velocity, transform.IsMirrored());
                Quaternion rotation = rigidBody.transform.rotation;
                rotation.eulerAngles = new Vector3(rotation.eulerAngles.x, rotation.eulerAngles.y, zRotationInDegrees);
                transform.rotation = rotation;
            }
        }

        protected override void CleanUp()
        {
            base.CleanUp();

            _followableEnemyDetectorProvider.DisposeManagedState();
            _followableEnemyDetectorProvider = null;

            _followableTargetProcessor.RemoveTargetConsumer(this);
            _followableTargetProcessor.RemoveTargetConsumer(_exactMatchTargetFilter);
            _followableTargetProcessor.DisposeManagedState();
            _followableTargetProcessor = null;

            _followableTargetFinder.DisposeManagedState();
            _followableTargetFinder = null;

            _shootableEnemeyDetectorProvider.DisposeManagedState();
            _shootableEnemeyDetectorProvider = null;
            for (int i = 0; i < _barrelControllers.Length; i++)
                _shootableTargetProcessor.RemoveTargetConsumer(_barrelControllers[i]);
            _shootableTargetProcessor.DisposeManagedState();
            _shootableTargetProcessor = null;

            _shootableTargetFinder.DisposeManagedState();
            _shootableTargetFinder = null;

            // Do not set to null, only created once in StaticInitialise(), so reused by unit pools.
            for (int i = 0; i < _barrelControllers.Length; i++)
                _barrelControllers[i].CleanUp();
        }
    }
}
