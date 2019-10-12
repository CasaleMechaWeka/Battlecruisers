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
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;
using BattleCruisers.Targets.TargetDetectors;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.Utils.Factories;
using BattleCruisers.Buildables.Pools;
using BattleCruisers.UI.BattleScene.ProgressBars;

namespace BattleCruisers.Buildables.Units.Aircraft
{
    public class FighterController : AircraftController, ITargetConsumer, ITargetProvider
	{
		private ITargetFinder _followableTargetFinder, _shootableTargetFinder;
		private ITargetProcessor _followableTargetProcessor, _shootableTargetProcessor;
		private IExactMatchTargetFilter _exactMatchTargetFilter;
		private IMovementController _figherMovementController;
        private BarrelController _barrelController;
        private IAngleHelper _angleHelper;
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
                    ActiveMovementController = _figherMovementController;
				}
			}
		}

		protected override float MaxPatrollingVelocity => EffectiveMaxVelocityInMPerS / PATROLLING_VELOCITY_DIVISOR;
        protected override float PositionEqualityMarginInM => 2;
        protected override ISoundKey EngineSoundKey => SoundKeys.Engines.Fighter;

        public override void StaticInitialise(GameObject parent, HealthBarController healthBar)
		{
            base.StaticInitialise(parent, healthBar);

            _barrelController = gameObject.GetComponentInChildren<BarrelController>();
			Assert.IsNotNull(_barrelController);
			_barrelController.StaticInitialise();
            AddDamageStats(_barrelController.DamageCapability);
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
		}

		protected async override void OnBuildableCompleted()
		{
			base.OnBuildableCompleted();

			Faction enemyFaction = Helper.GetOppositeFaction(Faction);
            ITarget parent = this;
            IUpdater updater = _factoryProvider.UpdaterProvider.PerFrameUpdater;

            IBarrelControllerArgs args
                = new BarrelControllerArgs(
                    updater,
                    _targetFactories.FilterFactory.CreateTargetFilter(enemyFaction, AttackCapabilities),
                    _factoryProvider.TargetPositionPredictorFactory.CreateLinearPredictor(),
                    _factoryProvider.Turrets.AngleCalculatorFactory.CreateAngleCalculator(),
                    _factoryProvider.Turrets.AttackablePositionFinderFactory.DummyPositionFinder,
                    _factoryProvider.Turrets.AccuracyAdjusterFactory.CreateDummyAdjuster(),
                    _movementControllerFactory.CreateRotationMovementController(_barrelController.TurretStats.TurretRotateSpeedInDegrees, _barrelController.transform, updater),
                    _factoryProvider.Turrets.TargetPositionValidatorFactory.CreateDummyValidator(),
                    _factoryProvider.Turrets.AngleLimiterFactory.CreateFighterLimiter(),
                    _factoryProvider,
                    _cruiserSpecificFactories,
                    parent,
                    _cruiserSpecificFactories.GlobalBoostProviders.DummyBoostProviders,
                    _cruiserSpecificFactories.GlobalBoostProviders.DummyBoostProviders,
                    SoundKeys.Firing.BigCannon);

            _barrelController.InitialiseAsync(args);

			SetupTargetDetection();

            _spriteChooser = await _factoryProvider.SpriteChooserFactory.CreateFighterSpriteChooserAsync(this);
		}

		/// <summary>
		/// Enemies first come within following range, and then shootable range as the figher closes
		/// in on the enemy.
		/// 
		/// enemyDetectionRangeInM: 
		///		The range at which enemies are detected
		/// barrelController.turretStats.rangeInM:  
		///		The range at which the turret can shoot enemies
		/// enemyDetectionRangeInM > barrelController.turretStats.rangeInM
		/// </summary>
		private void SetupTargetDetection()
		{
            // Detect followable enemies
            _followableEnemyDetectorProvider 
                = _cruiserSpecificFactories.Targets.DetectorFactory.CreateEnemyAircraftTargetDetector(
                    Transform, 
                    enemyFollowDetectionRangeInM, 
                    _targetFactories.RangeCalculatorProvider.BasicCalculator);
			Faction enemyFaction = Helper.GetOppositeFaction(Faction);
            IList<TargetType> targetTypesToFollow = new List<TargetType>() { TargetType.Aircraft };
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
                    _barrelController.TurretStats.RangeInM,
                    _targetFactories.RangeCalculatorProvider.BasicCalculator);
			_shootableTargetFinder = _targetFactories.FinderFactory.CreateRangedTargetFinder(_shootableEnemeyDetectorProvider.TargetDetector, _exactMatchTargetFilter);
			
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
            if (Velocity != Vector2.zero)
            {
                float angleInDegrees = _angleHelper.FindAngle(Velocity, transform.IsMirrored());
                Quaternion rotation = rigidBody.transform.rotation;
                rotation.eulerAngles = new Vector3(rotation.eulerAngles.x, rotation.eulerAngles.y, angleInDegrees);
                rigidBody.transform.rotation = rotation;
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
    }
}
