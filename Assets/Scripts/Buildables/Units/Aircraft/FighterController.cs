using System.Collections.Generic;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Movement.Velocity;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.Targets.TargetProcessors.Ranking;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Units.Aircraft
{
    public class FighterController : AircraftController, ITargetConsumer, ITargetProvider
	{
		private ITargetFinder _followableTargetFinder, _shootableTargetFinder;
		private ITargetProcessor _followableTargetProcessor, _shootableTargetProcessor;
		private IExactMatchTargetFilter _exactMatchTargetFilter;
		private IMovementController _figherMovementController;
        private ShellTurretBarrelController _barrelController;

		// Detects enemies that come within following range
		public CircleTargetDetector followableEnemyDetector;
		// Detects when the enemy being followed comes within shooting range
		public CircleTargetDetector shootableEnemyDetector;

		public float enemyFollowDetectionRangeInM;
		public float cruisingAltitudeInM;

		private const float PATROLLING_VELOCITY_DIVISOR = 2;

		private ITarget _target;
		public ITarget Target 
		{ 
			get { return _target; }
			set 
			{ 
				Logging.Log(Tags.AIRCRAFT, "FighterController.set_Target:  " + value);

				_target = value;

				if (_target == null)
				{
					SwitchMovementControllers(PatrollingMovementController);
				}
				else
				{
					SwitchMovementControllers(_figherMovementController);
				}
			}
		}

		protected override float MaxPatrollingVelocity { get { return EffectiveMaxVelocityInMPerS / PATROLLING_VELOCITY_DIVISOR; } }

		public override void StaticInitialise()
		{
			base.StaticInitialise();

			Assert.IsNotNull(followableEnemyDetector);
			
			_attackCapabilities.Add(TargetType.Aircraft);

            _barrelController = gameObject.GetComponentInChildren<ShellTurretBarrelController>();
			Assert.IsNotNull(_barrelController);
			_barrelController.StaticInitialise();
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
		}

		protected override void OnBuildableCompleted()
		{
			base.OnBuildableCompleted();

			Faction enemyFaction = Helper.GetOppositeFaction(Faction);
            ITargetFilter targetFilter = _targetsFactory.CreateTargetFilter(enemyFaction, _attackCapabilities);
			IAngleCalculator angleCalculator = _factoryProvider.AngleCalculatorFactory.CreateLeadingAngleCalcultor(_factoryProvider.TargetPositionPredictorFactory);
            IRotationMovementController rotationMovementController = _movementControllerFactory.CreateRotationMovementController(_barrelController.TurretStats.TurretRotateSpeedInDegrees, _barrelController.transform);
            _barrelController.Initialise(targetFilter, angleCalculator, rotationMovementController, _factoryProvider.DamageApplierFactory);

			SetupTargetDetection();
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
            ITargetFilter targetFilter = _targetsFactory.CreateTargetFilter(enemyFaction, targetTypesToFollow);
			_followableTargetFinder = _targetsFactory.CreateRangedTargetFinder(followableEnemyDetector, targetFilter);
			
			ITargetRanker followableTargetRanker = _targetsFactory.CreateEqualTargetRanker();
			_followableTargetProcessor = _targetsFactory.CreateTargetProcessor(_followableTargetFinder, followableTargetRanker);
			_followableTargetProcessor.AddTargetConsumer(this);


			// Detect shootable enemies
			_exactMatchTargetFilter = _targetsFactory.CreateExactMatchTargetFilter();
			_followableTargetProcessor.AddTargetConsumer(_exactMatchTargetFilter);
			
			shootableEnemyDetector.Initialise(_barrelController.TurretStats.RangeInM);
			_shootableTargetFinder = _targetsFactory.CreateRangedTargetFinder(shootableEnemyDetector, _exactMatchTargetFilter);
			
			ITargetRanker shootableTargetRanker = _targetsFactory.CreateEqualTargetRanker();
			_shootableTargetProcessor = _targetsFactory.CreateTargetProcessor(_shootableTargetFinder, shootableTargetRanker);
			_shootableTargetProcessor.AddTargetConsumer(_barrelController);
		}

		protected override IList<IPatrolPoint> GetPatrolPoints()
		{
			return Helper.ConvertVectorsToPatrolPoints(_aircraftProvider.FindFighterPatrolPoints(cruisingAltitudeInM));
		}

		protected override void OnFixedUpdate()
		{
			base.OnFixedUpdate();

			// Adjust game object to point in direction it's travelling
			transform.right = Velocity;
		}

		protected override void OnDirectionChange()
		{
			// Turn off parent class behaviour of mirroring accross y-axis
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
            _followableTargetProcessor.Dispose();
            _followableTargetProcessor = null;

            _followableTargetFinder.Dispose();
            _followableTargetFinder = null;

            _shootableTargetProcessor.RemoveTargetConsumer(_barrelController);
            _shootableTargetProcessor.Dispose();
            _shootableTargetProcessor = null;

            _shootableTargetFinder.Dispose();
            _shootableTargetFinder = null;
        }
    }
}
