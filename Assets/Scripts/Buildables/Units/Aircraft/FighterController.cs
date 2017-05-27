using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.Targets.TargetProcessors.Ranking;
using BattleCruisers.Units.Aircraft.Providers;
using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Units.Aircraft
{
	public class FighterController : AircraftController, ITargetConsumer
	{
		private ITargetFinder _followableTargetFinder, _shootableTargetFinder;
		private ITargetProcessor _followableTargetProcessor, _shootableTargetProcessor;
		private IExactMatchTargetFilter _exactMatchTargetFilter;
		private Vector2 _velocity;
		private float _velocitySmoothTime;

		// Zone in which fighter will pursue enemies.  If those enemies move outside this
		// safe zone the fighter will abandon pursuit.
		private SafeZone _safeZone;

		// Detects enemies that come within following range
		public TargetDetector followableEnemyDetector;
		// Detects when the enemy being followed comes within shooting range
		public TargetDetector shootableEnemyDetector;

		public BarrelController barrelController;
		public float enemyFollowDetectionRangeInM;
		public float cruisingAltitudeInM;

		private const float VELOCITY_EQUALITY_MARGIN = 0.1f;
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

		protected override void OnInitialised()
		{
			base.OnInitialised();
			_velocitySmoothTime = FindSmoothTime(maxVelocityInMPerS);
		}

		protected override void OnBuildableCompleted()
		{
			base.OnBuildableCompleted();

			Assert.IsNotNull(followableEnemyDetector);
			Assert.IsNotNull(barrelController);

			_attackCapabilities.Add(TargetType.Aircraft);

			barrelController.Initialise(Faction);

			_safeZone = _aircraftProvider.FighterSafeZone;			
			PatrolPoints = _aircraftProvider.FindFighterPatrolPoints(cruisingAltitudeInM);

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
			
			shootableEnemyDetector.Initialise(barrelController.turretStats.rangeInM);
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
				AdjustVelocity();
			}

			// Adjust game object to point in direction it's travelling
			transform.right = Velocity;
		}

		private void AdjustVelocity()
		{
			Vector2 sourcePosition = transform.position;
			Vector2 targetPosition = Target.GameObject.transform.position;

			targetPosition = CapTargetPositionInSafeZone(targetPosition);

			Vector2 desiredVelocity = FindDesiredVelocity(sourcePosition, targetPosition, maxVelocityInMPerS);

			if (Math.Abs(rigidBody.velocity.x - desiredVelocity.x) <= VELOCITY_EQUALITY_MARGIN
				&& Math.Abs(rigidBody.velocity.y - desiredVelocity.y) <= VELOCITY_EQUALITY_MARGIN)
			{
				rigidBody.velocity = desiredVelocity;
			}
			else
			{
				Logging.Log(Tags.AIRCRAFT, string.Format("AdjustVelocity():  rigidBody.velocity: {0}  desiredVelocity: {1}  _velocitySmoothTime: {2}  maxVelocityInMPerS: {3}", 
					rigidBody.velocity, desiredVelocity, _velocitySmoothTime, maxVelocityInMPerS));

				rigidBody.velocity = Vector2.SmoothDamp(rigidBody.velocity, desiredVelocity, ref _velocity, _velocitySmoothTime, maxVelocityInMPerS, Time.deltaTime);
			}
		}

		protected override void OnDirectionChange()
		{
			// Turn off parent class behaviour of mirroring accross y-axis
		}

		private Vector2 CapTargetPositionInSafeZone(Vector2 targetPosition)
		{
			if (targetPosition.x < _safeZone.MinX)
			{
				targetPosition.x = _safeZone.MinX;
			}
			if (targetPosition.x > _safeZone.MaxX)
			{
				targetPosition.x = _safeZone.MaxX;
			}
			if (targetPosition.y < _safeZone.MinY)
			{
				targetPosition.y = _safeZone.MinY;
			}
			if (targetPosition.y > _safeZone.MaxY)
			{
				targetPosition.y = _safeZone.MaxY;
			}

			return targetPosition;
		}

		private Vector2 FindDesiredVelocity(Vector2 sourcePosition, Vector2 targetPosition, float maxVelocityInMPerS)
		{
			Vector2 desiredVelocity = new Vector2(0, 0);

			if (sourcePosition == targetPosition)
			{
				return desiredVelocity;
			}

			if (sourcePosition.x == targetPosition.x)
			{
				// On same x-axis
				desiredVelocity.y = sourcePosition.y < targetPosition.y ? maxVelocityInMPerS : -maxVelocityInMPerS;
			}
			else if (sourcePosition.y == targetPosition.y)
			{
				// On same y-axis
				desiredVelocity.x = sourcePosition.x < targetPosition.x ? maxVelocityInMPerS : -maxVelocityInMPerS;
			}
			else
			{
				// Different x and y axes, so need to calculate the angle
				float xDiff = Math.Abs(sourcePosition.x - targetPosition.x);
				float yDiff = Math.Abs(sourcePosition.y - targetPosition.y);
				float angleInRadians = Mathf.Atan(yDiff / xDiff);
				float angleInDegrees = angleInRadians * Mathf.Rad2Deg;

				float velocityX = Mathf.Cos(angleInRadians) * maxVelocityInMPerS;
				float velocityY = Mathf.Sin(angleInRadians) * maxVelocityInMPerS;
				Logging.Log(Tags.AIRCRAFT, string.Format("FighterController.FindDesiredVelocity()  angleInDegrees: {0}  velocityX: {1}  velocityY: {2}",
					angleInDegrees, velocityX, velocityY));

				if (sourcePosition.x > targetPosition.x)
				{
					// Source is to right of target
					velocityX *= -1;
				}

				if (sourcePosition.y > targetPosition.y)
				{
					// Source is above target
					velocityY *= -1;
				}

				desiredVelocity.x = velocityX;
				desiredVelocity.y = velocityY;
			}

			Logging.Log(Tags.AIRCRAFT, "FighterController.FindDesiredVelocity() " + desiredVelocity);
			return desiredVelocity;
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
