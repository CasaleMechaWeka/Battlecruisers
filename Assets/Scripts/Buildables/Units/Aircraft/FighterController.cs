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

        protected override PrioritisedSoundKey ConstructionCompletedSoundKey => PrioritisedSoundKeys.Completed.Units.Fighter;

        // FELIX  Performance killers :)
        // FELIX  Remove from prefabs :)
        // Detects enemies that come within following range
        public CircleTargetDetectorController followableEnemyDetector;
		// Detects when the enemy being followed comes within shooting range
		public CircleTargetDetectorController shootableEnemyDetector;

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

        protected override void OnStaticInitialised()
		{
            base.OnStaticInitialised();

			Assert.IsNotNull(followableEnemyDetector);
			
            _barrelController = gameObject.GetComponentInChildren<BarrelController>();
			Assert.IsNotNull(_barrelController);
			_barrelController.StaticInitialise();
            AddDamageStats(_barrelController.DamageCapability);
		}

		protected override void OnInitialised()
		{
			base.OnInitialised();

			_figherMovementController 
                = _movementControllerFactory.CreateFighterMovementController(
                    rigidBody, 
                    maxVelocityProvider: this,
                    targetProvider: this, 
                    safeZone: _aircraftProvider.FighterSafeZone);

            _angleHelper = _factoryProvider.Turrets.AngleCalculatorFactory.CreateAngleHelper();
		}

		protected override void OnBuildableCompleted()
		{
			base.OnBuildableCompleted();

			Faction enemyFaction = Helper.GetOppositeFaction(Faction);

            ITarget parent = this;
            IBarrelControllerArgs args
                = new BarrelControllerArgs(
                    _targetFactories.FilterFactory.CreateTargetFilter(enemyFaction, AttackCapabilities),
                    _factoryProvider.TargetPositionPredictorFactory.CreateLinearPredictor(),
                    _factoryProvider.Turrets.AngleCalculatorFactory.CreateAngleCalculator(),
                    _factoryProvider.Turrets.AttackablePositionFinderFactory.DummyPositionFinder,
                    _factoryProvider.Turrets.AccuracyAdjusterFactory.CreateDummyAdjuster(),
                    _movementControllerFactory.CreateRotationMovementController(_barrelController.TurretStats.TurretRotateSpeedInDegrees, _barrelController.transform),
                    _factoryProvider.Turrets.TargetPositionValidatorFactory.CreateDummyValidator(),
                    _factoryProvider.Turrets.AngleLimiterFactory.CreateFighterLimiter(),
                    _factoryProvider,
                    parent,
                    _factoryProvider.GlobalBoostProviders.DummyBoostProviders,
                    _factoryProvider.GlobalBoostProviders.DummyBoostProviders,
                    SoundKeys.Firing.BigCannon);

            _barrelController.Initialise(args);

			SetupTargetDetection();

            _spriteChooser = _factoryProvider.SpriteChooserFactory.CreateFighterSpriteChooser(this);
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
			followableEnemyDetector.Initialise(enemyFollowDetectionRangeInM);
			Faction enemyFaction = Helper.GetOppositeFaction(Faction);
            IList<TargetType> targetTypesToFollow = new List<TargetType>() { TargetType.Aircraft };
            ITargetFilter targetFilter = _targetFactories.FilterFactory.CreateTargetFilter(enemyFaction, targetTypesToFollow);
			_followableTargetFinder = _targetFactories.FinderFactory.CreateRangedTargetFinder(followableEnemyDetector, targetFilter);
			
			ITargetRanker followableTargetRanker = _targetFactories.RankerFactory.EqualTargetRanker;
            IRankedTargetTracker followableTargetTracker = _targetFactories.TrackerFactory.CreateRankedTargetTracker(_followableTargetFinder, followableTargetRanker);
			_followableTargetProcessor = _targetFactories.ProcessorFactory.CreateTargetProcessor(followableTargetTracker);
			_followableTargetProcessor.AddTargetConsumer(this);


			// Detect shootable enemies
			_exactMatchTargetFilter = _targetFactories.FilterFactory.CreateMulitpleExactMatchTargetFilter();
			_followableTargetProcessor.AddTargetConsumer(_exactMatchTargetFilter);
			
			shootableEnemyDetector.Initialise(_barrelController.TurretStats.RangeInM);
			_shootableTargetFinder = _targetFactories.FinderFactory.CreateRangedTargetFinder(shootableEnemyDetector, _exactMatchTargetFilter);
			
			ITargetRanker shootableTargetRanker = _targetFactories.RankerFactory.EqualTargetRanker;
            IRankedTargetTracker shootableTargetTracker = _targetFactories.TrackerFactory.CreateRankedTargetTracker(_shootableTargetFinder, shootableTargetRanker);
			_shootableTargetProcessor = _targetFactories.ProcessorFactory.CreateTargetProcessor(shootableTargetTracker);
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

		protected override void OnDestroyed()
		{
			base.OnDestroyed();

			if (BuildableState == BuildableState.Completed
                && !IsInKamikazeMode)
            {
                CleanUp();
            }
        }

		protected override void OnKamikaze()
        {
            CleanUp();
        }

        private void CleanUp()
        {
            _followableTargetProcessor.RemoveTargetConsumer(this);
            _followableTargetProcessor.RemoveTargetConsumer(_exactMatchTargetFilter);
            _followableTargetProcessor.DisposeManagedState();
            _followableTargetProcessor = null;

            _followableTargetFinder.DisposeManagedState();
            _followableTargetFinder = null;

            _shootableTargetProcessor.RemoveTargetConsumer(_barrelController);
            _shootableTargetProcessor.DisposeManagedState();
            _shootableTargetProcessor = null;

            _shootableTargetFinder.DisposeManagedState();
            _shootableTargetFinder = null;
        }
    }
}
