using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Movement.Velocity;
using BattleCruisers.Projectiles;
using BattleCruisers.Projectiles.Spawners;
using BattleCruisers.Projectiles.Stats;
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

namespace BattleCruisers.Buildables.Units.Aircraft
{
	public class DeathstarController : AircraftController
	{
		private LaserBarrelController _barrelController;
		private ITargetDetector _targetDetector;
		private ITargetProcessor _targetProcessor;

		public float cruisingAltitudeInM;
		public RotatingController leftWing, rightWing;

		private const float LEFT_WING_TARGET_ANGLE_IN_DEGREES = 270;
		private const float RIGHT_WING_TARGET_ANGLE_IN_DEGREES = 90;
		private const float WING_ROTATE_SPEED_IN_M_DEGREES_S = 45;
		private const int NUM_OF_PATROL_POINTS = 4;

		public override void StaticInitialise()
		{
			base.StaticInitialise();

			Assert.IsNotNull(leftWing);
			Assert.IsNotNull(rightWing);

			_attackCapabilities.Add(TargetType.Cruiser);
			_attackCapabilities.Add(TargetType.Buildings);

			_barrelController = gameObject.GetComponentInChildren<LaserBarrelController>();
			Assert.IsNotNull(_barrelController);
			_barrelController.StaticInitialise();

			_targetDetector = gameObject.GetComponentInChildren<ITargetDetector>();
			Assert.IsNotNull(_targetDetector);
		}

		protected override void OnInitialised()
		{
			base.OnInitialised();

			leftWing.Initialise(_movementControllerFactory, WING_ROTATE_SPEED_IN_M_DEGREES_S, LEFT_WING_TARGET_ANGLE_IN_DEGREES);
			rightWing.Initialise(_movementControllerFactory, WING_ROTATE_SPEED_IN_M_DEGREES_S, RIGHT_WING_TARGET_ANGLE_IN_DEGREES);
		}

		protected override void OnBuildableCompleted()
		{
			base.OnBuildableCompleted();

			Assert.IsTrue(cruisingAltitudeInM > transform.position.y);

			// Barrel controller
			Faction enemyFaction = Helper.GetOppositeFaction(Faction);
			ITargetFilter targetFilter = _targetsFactory.CreateTargetFilter(enemyFaction, _attackCapabilities);
			IAngleCalculator angleCalculator = _angleCalculatorFactory.CreateLeadingAngleCalcultor(_targetPositionPredictorFactory);
			IRotationMovementController rotationMovementController = _movementControllerFactory.CreateDummyRotationMovementController();
			_barrelController.Initialise(targetFilter, angleCalculator, rotationMovementController);
			
			// Target detection
			ITargetFinder targetFinder = _targetsFactory.CreateRangedTargetFinder(_targetDetector, targetFilter);
			_targetProcessor = _targetsFactory.CreateTargetProcessor(targetFinder, new OffensiveBuildableTargetRanker());
			_targetProcessor.AddTargetConsumer(_barrelController);
		}

		protected override IList<IPatrolPoint> GetPatrolPoints()
		{
			IList<Vector2> patrolPositions = _aircraftProvider.FindDeathstarPatrolPoints(transform.position, cruisingAltitudeInM);
			Assert.IsTrue(patrolPositions.Count == NUM_OF_PATROL_POINTS);

			IList<IPatrolPoint> patrolPoints = new List<IPatrolPoint>(patrolPositions.Count);
			patrolPoints.Add(new PatrolPoint(patrolPositions[0], removeOnceReached: true, actionOnReached: OnClearingLaunchStation));
			patrolPoints.Add(new PatrolPoint(patrolPositions[1], removeOnceReached: true));
			patrolPoints.Add(new PatrolPoint(patrolPositions[2]));
			patrolPoints.Add(new PatrolPoint(patrolPositions[3]));

			return patrolPoints;
		}

		private void OnClearingLaunchStation()
		{
			// Stop moving
			SwitchMovementControllers(_dummyMovementController);

			UnfoldWings();
		}

		private void UnfoldWings()
		{
			leftWing.ReachedDesiredAngle += Wing_ReachedDesiredAngle;

			leftWing.StartRotating();
			rightWing.StartRotating();
		}

		private void Wing_ReachedDesiredAngle(object sender, EventArgs e)
		{
			leftWing.ReachedDesiredAngle -= Wing_ReachedDesiredAngle;

			SwitchMovementControllers(_patrollingMovementController);
		}

		protected override void OnDestroyed()
		{
			base.OnDestroyed();

			if (BuildableState == BuildableState.Completed)
			{
				_targetProcessor.RemoveTargetConsumer(_barrelController);
				_targetProcessor = null;
			}
		}
	}
}
