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
using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Units.Aircraft
{
	// FELIX  Extract anything common with BomberController to AircraftController?  Eg, shellSpawner?
	public class FighterController : AircraftController, ITargetConsumer
	{
		private ITargetFinder _targetFinder;
		private ITargetProcessor _targetProcessor;
		private Vector2 _velocity;
		private float _velocitySmoothTime;

		public TargetDetector enemyDetector;
		public BarrelController barrelController;

		private const float VELOCITY_EQUALITY_MARGIN = 0.1f;

		public ITarget Target 
		{ 
			get { return barrelController.Target; }
			set 
			{ 
				Logging.Log(Tags.AIRCRAFT, $"FighterController.set_Target:  {value}");

				barrelController.Target = value;

				if (value == null)
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

					Vector2 sourcePosition = transform.position;
					Vector2 targetPosition = Target.GameObject.transform.position;
					_velocitySmoothTime = Vector2.Distance(sourcePosition, targetPosition) / maxVelocityInMPerS;
				}
			}
		}

		protected override void OnBuildableCompleted()
		{
			base.OnBuildableCompleted();

			Assert.IsNotNull(enemyDetector);
			Assert.IsNotNull(barrelController);

			_attackCapabilities.Add(TargetType.Aircraft);

			barrelController.Initialise(Faction);

			// FELIX  Avoid duplicate code with DefensiveTurret.  New class and use composition?
			Faction enemyFaction = Helper.GetOppositeFaction(Faction);
			ITargetFilter targetFilter = _targetsFactory.CreateTargetFilter(enemyFaction, TargetType.Aircraft);
			enemyDetector.Initialise(targetFilter, barrelController.turretStats.rangeInM);

			_targetFinder = _targetsFactory.CreateRangedTargetFinder(enemyDetector);
			ITargetRanker targetRanker = _targetsFactory.CreateEqualTargetRanker();
			_targetProcessor = _targetsFactory.CreateTargetProcessor(_targetFinder, targetRanker);
			_targetProcessor.AddTargetConsumer(this);
		}

		protected override void OnFixedUpdate()
		{
			base.OnFixedUpdate();

			if (Target != null)
			{
				AdjustVelocity();

				// FELIX  Adjust sprite to point in direction travelling :)
			}
		}

		// FELIX  Lead target?  Copy LeadAngleCalculator :P
		private void AdjustVelocity()
		{
			Vector2 sourcePosition = transform.position;
			Vector2 targetPosition = Target.GameObject.transform.position;

			Vector2 desiredVelocity = FindDesiredVelocity(sourcePosition, targetPosition, maxVelocityInMPerS);

			// FELIX  Use, for updating sprite direction :)
			Vector2 oldVelocity = rigidBody.velocity;

			if (Math.Abs(rigidBody.velocity.x - desiredVelocity.x) <= VELOCITY_EQUALITY_MARGIN
				&& Math.Abs(rigidBody.velocity.y - desiredVelocity.y) <= VELOCITY_EQUALITY_MARGIN)
			{
				rigidBody.velocity = desiredVelocity;
			}
			else
			{
//				float velocitySmoothTime = Vector2.Distance(sourcePosition, targetPosition) / maxVelocityInMPerS;

				rigidBody.velocity = Vector2.SmoothDamp(rigidBody.velocity, desiredVelocity, ref _velocity, _velocitySmoothTime, maxVelocityInMPerS, Time.deltaTime);
			}
		}

		// FELIX  Can perhaps replace bomber velocity adjustment functionality with this?
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
				Logging.Log(Tags.AIRCRAFT, $"FighterController.FindDesiredVelocity()  angleInDegrees: {angleInDegrees}  velocityX: {velocityX}  velocityY: {velocityY}");

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

			Logging.Log(Tags.AIRCRAFT, $"FighterController.FindDesiredVelocity() {desiredVelocity}");
			return desiredVelocity;
		}

		protected override void OnDestroyed()
		{
			base.OnDestroyed();

			// FELIX  Avoid duplicate code with DefensiveTurret.  New class and use composition?
			if (BuildableState == BuildableState.Completed)
			{
				_targetProcessor.RemoveTargetConsumer(this);
				_targetProcessor.Dispose();
				_targetProcessor = null;

				_targetFinder.Dispose();
				_targetFinder = null;
			}
		}
	}
}
