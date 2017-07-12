using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Movement.Velocity;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.Targets.TargetProcessors.Ranking;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Units.Aircraft
{
	public class FighterController : AircraftController, ITargetConsumer
	{
		private ITargetFinder _followableTargetFinder, _shootableTargetFinder;
		private ITargetProcessor _followableTargetProcessor, _shootableTargetProcessor;
		private IExactMatchTargetFilter _exactMatchTargetFilter;
		private IHomingMovementController _movementController;

		// Detects enemies that come within following range
		public CircleTargetDetector followableEnemyDetector;
		// Detects when the enemy being followed comes within shooting range
		public CircleTargetDetector shootableEnemyDetector;

		public BarrelController barrelController;
		public float enemyFollowDetectionRangeInM;
		public float cruisingAltitudeInM;

		private const float PATROLLING_VELOCITY_DIVISOR = 2;

		// Even setting the rigidBody.velocity in FixedUpdate() instead of in
		// this setter did not fix my double OnTriggerEnter2D() problem.  This
		// would happen when both aircraft are patrolling.
		private ITarget _target;
		public ITarget Target 
		{ 
			get { return _target; }
			set 
			{ 
				Logging.Log(Tags.AIRCRAFT, "FighterController.set_Target:  " + value);

				_target = value;
				_movementController.Target = _target;

				if (_target == null)
				{
					_patrollingVelocity = rigidBody.velocity;
					rigidBody.velocity = new Vector2(0, 0);

					StartPatrolling();
				}
				else
				{
					if (_isPatrolling)
					{
						rigidBody.velocity = _patrollingVelocity;
					}

					StopPatrolling();
				}
			}
		}

		protected override float PatrollingVelocity	{ get {	return maxVelocityInMPerS / PATROLLING_VELOCITY_DIVISOR; } }

		public override void StaticInitialise()
		{
			base.StaticInitialise();

			Assert.IsNotNull(followableEnemyDetector);
			Assert.IsNotNull(barrelController);
			
			_attackCapabilities.Add(TargetType.Aircraft);
			
			barrelController.StaticInitialise();
		}

		protected override void OnBuildableCompleted()
		{
			base.OnBuildableCompleted();

			Faction enemyFaction = Helper.GetOppositeFaction(Faction);
			ITargetFilter targetFilter = _targetsFactory.CreateTargetFilter(enemyFaction, _attackCapabilities);
			IAngleCalculator angleCalculator = _angleCalculatorFactory.CreateLeadingAngleCalcultor(_targetPositionPredictorFactory);
			IRotationMovementController rotationMovementController = _movementControllerFactory.CreateDummyRotationMovementController();
			barrelController.Initialise(targetFilter, angleCalculator, rotationMovementController);

			PatrolPoints = _aircraftProvider.FindFighterPatrolPoints(cruisingAltitudeInM);

			_movementController = _movementControllerFactory.CreateFighterMovementController(rigidBody, maxVelocityInMPerS, _aircraftProvider.FighterSafeZone);

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
			ITargetFilter targetFilter = _targetsFactory.CreateTargetFilter(enemyFaction, TargetType.Aircraft);
			_followableTargetFinder = _targetsFactory.CreateRangedTargetFinder(followableEnemyDetector, targetFilter);
			
			ITargetRanker followableTargetRanker = _targetsFactory.CreateEqualTargetRanker();
			_followableTargetProcessor = _targetsFactory.CreateTargetProcessor(_followableTargetFinder, followableTargetRanker);
			_followableTargetProcessor.AddTargetConsumer(this);


			// Detect shootable enemies
			_exactMatchTargetFilter = _targetsFactory.CreateExactMatchTargetFiler();
			_followableTargetProcessor.AddTargetConsumer(_exactMatchTargetFilter);
			
			shootableEnemyDetector.Initialise(barrelController.TurretStats.rangeInM);
			_shootableTargetFinder = _targetsFactory.CreateRangedTargetFinder(shootableEnemyDetector, _exactMatchTargetFilter);
			
			ITargetRanker shootableTargetRanker = _targetsFactory.CreateEqualTargetRanker();
			_shootableTargetProcessor = _targetsFactory.CreateTargetProcessor(_shootableTargetFinder, shootableTargetRanker);
			_shootableTargetProcessor.AddTargetConsumer(barrelController);
		}

		protected override void OnFixedUpdate()
		{
			base.OnFixedUpdate();

			if (Target != null)
			{
				_movementController.AdjustVelocity();
			}

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

			if (BuildableState == BuildableState.Completed)
			{
				_followableTargetProcessor.RemoveTargetConsumer(this);
				_followableTargetProcessor.RemoveTargetConsumer(_exactMatchTargetFilter);
				_followableTargetProcessor.Dispose();
				_followableTargetProcessor = null;

				_followableTargetFinder.Dispose();
				_followableTargetFinder = null;

				_shootableTargetProcessor.RemoveTargetConsumer(barrelController);
				_shootableTargetProcessor.Dispose();
				_shootableTargetProcessor = null;

				_shootableTargetFinder.Dispose();
				_shootableTargetFinder = null;
			}
		}
	}
}
