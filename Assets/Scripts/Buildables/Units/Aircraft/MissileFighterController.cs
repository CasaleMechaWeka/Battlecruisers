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
using BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Buildables.Buildings.Turrets.PositionValidators;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Targets.Factories;

namespace BattleCruisers.Buildables.Units.Aircraft
{
    public class MissileFighterController : AircraftController, ITargetConsumer, ITargetProvider
    {
        private ITargetFinder _followableTargetFinder, _shootableTargetFinder;
        private ITargetProcessor _followableTargetProcessor, _shootableTargetProcessor;
        private IExactMatchTargetFilter _exactMatchTargetFilter;
        private IMovementController _fighterMovementController;
        private BarrelController _barrelController;
        private ManualDetectorProvider _followableEnemyDetectorProvider, _shootableEnemeyDetectorProvider;

        public float enemyFollowDetectionRangeInM;

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
                    ActiveMovementController = _fighterMovementController;
                }
            }
        }

        protected override float MaxPatrollingVelocity => EffectiveMaxVelocityInMPerS / PATROLLING_VELOCITY_DIVISOR;
        protected override float PositionEqualityMarginInM => 2;

        public override void StaticInitialise(GameObject parent, HealthBarController healthBar, ILocTable commonStrings)
        {
            base.StaticInitialise(parent, healthBar, commonStrings);

            _barrelController = gameObject.GetComponentInChildren<MissileBarrelController>();
            Assert.IsNotNull(_barrelController);
            _barrelController.StaticInitialise();
            AddDamageStats(_barrelController.DamageCapability);
        }

        public override void Initialise(IUIManager uiManager, IFactoryProvider factoryProvider)
        {
            base.Initialise(uiManager, factoryProvider);
        }

        public override void Activate(BuildableActivationArgs activationArgs)
        {
            base.Activate(activationArgs);

            _fighterMovementController
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

            IBarrelControllerArgs args = new BarrelControllerArgs(
                updater,
                TargetFilterFactory.CreateTargetFilter(enemyFaction, AttackCapabilities),
                new LinearTargetPositionPredictor(),
                new AngleCalculator(),
                new AccuracyAdjuster((0, 0)),
                _movementControllerFactory.CreateRotationMovementController(_barrelController.TurretStats.TurretRotateSpeedInDegrees, _barrelController.transform, updater),
                new FacingMinRangePositionValidator(0, true),
                new AngleLimiter(-180, 180),
                _factoryProvider,
                _cruiserSpecificFactories,
                parent,
                _cruiserSpecificFactories.GlobalBoostProviders.DummyBoostProviders,
                _cruiserSpecificFactories.GlobalBoostProviders.DummyBoostProviders,
                EnemyCruiser,
                SoundKeys.Firing.Missile
            );

            await _barrelController.InitialiseAsync(args);

            SetupTargetDetection();

            _spriteChooser = await _factoryProvider.SpriteChooserFactory.CreateAircraftSpriteChooserAsync(PrefabKeyName.Unit_MissileFighter, this);
            _barrelController.ApplyVariantStats(this);
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
            _followableEnemyDetectorProvider = _cruiserSpecificFactories.Targets.DetectorFactory.CreateEnemyShipAndAircraftTargetDetector(
                Transform, enemyFollowDetectionRangeInM, _targetFactories.RangeCalculatorProvider.BasicCalculator);
            Faction enemyFaction = Helper.GetOppositeFaction(Faction);
            IList<TargetType> targetTypesToFollow = new List<TargetType>() { TargetType.Aircraft, TargetType.Ships };
            ITargetFilter targetFilter = TargetFilterFactory.CreateTargetFilter(enemyFaction, targetTypesToFollow);
            _followableTargetFinder = new RangedTargetFinder(_followableEnemyDetectorProvider.TargetDetector, targetFilter);

            ITargetRanker followableTargetRanker = _targetFactories.RankerFactory.EqualTargetRanker;
            IRankedTargetTracker followableTargetTracker = _cruiserSpecificFactories.Targets.TrackerFactory.CreateRankedTargetTracker(_followableTargetFinder, followableTargetRanker);
            _followableTargetProcessor = _cruiserSpecificFactories.Targets.ProcessorFactory.CreateTargetProcessor(followableTargetTracker);
            _followableTargetProcessor.AddTargetConsumer(this);

            // Detect shootable enemies
            _exactMatchTargetFilter = TargetFilterFactory.CreateMulitpleExactMatchTargetFilter();
            _followableTargetProcessor.AddTargetConsumer(_exactMatchTargetFilter);

            _shootableEnemeyDetectorProvider = _cruiserSpecificFactories.Targets.DetectorFactory.CreateEnemyShipAndAircraftTargetDetector(
                Transform, _barrelController.TurretStats.RangeInM, _targetFactories.RangeCalculatorProvider.BasicCalculator);
            _shootableTargetFinder = new RangedTargetFinder(_shootableEnemeyDetectorProvider.TargetDetector, _exactMatchTargetFilter);

            ITargetRanker shootableTargetRanker = _targetFactories.RankerFactory.EqualTargetRanker;
            IRankedTargetTracker shootableTargetTracker = _cruiserSpecificFactories.Targets.TrackerFactory.CreateRankedTargetTracker(_shootableTargetFinder, shootableTargetRanker);
            _shootableTargetProcessor = _cruiserSpecificFactories.Targets.ProcessorFactory.CreateTargetProcessor(shootableTargetTracker);
            _shootableTargetProcessor.AddTargetConsumer(_barrelController);
        }

        protected override IList<IPatrolPoint> GetPatrolPoints()
        {
            return Helper.ConvertVectorsToPatrolPoints(_aircraftProvider.FindFighterPatrolPoints(cruisingAltitudeInM));
        }

        protected override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            FaceVelocityDirection();
        }

        private void FaceVelocityDirection()
        {
            Logging.Verbose(Tags.FIGHTER, $"Id: {GameObject.GetInstanceID()}  Velocity: {Velocity}");

            if (Velocity != Vector2.zero)
            {
                float angle = Mathf.Atan2(Velocity.y, Velocity.x) * Mathf.Rad2Deg;
                float zRotationInDegrees = transform.IsMirrored() ? 180 - angle : (angle + 360) % 360;

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

            _shootableTargetProcessor.RemoveTargetConsumer(_barrelController);
            _shootableTargetProcessor.DisposeManagedState();
            _shootableTargetProcessor = null;

            _shootableTargetFinder.DisposeManagedState();
            _shootableTargetFinder = null;

            // Do not set to null, only created once in StaticInitialise(), so reused by unit pools.
            _barrelController.CleanUp();
        }

        public void ConsumeTargets(IEnumerable<ITarget> targets)
        {
            foreach (var target in targets)
            {
                Logging.Log(Tags.FIGHTER, $"Detected target: {target.TargetType}");
            }
        }
    }
}
