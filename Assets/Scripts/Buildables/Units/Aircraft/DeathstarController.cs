using System;
using System.Collections.Generic;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Data.Static;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Movement.Velocity;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.Targets.TargetProcessors.Ranking;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Units.Aircraft
{
    public class DeathstarController : AircraftController
	{
		private BarrelController _barrelController;
		private ITargetDetector _targetDetector;
		private ITargetProcessor _targetProcessor;

		public RotatingController leftWing, rightWing;

		private const float LEFT_WING_TARGET_ANGLE_IN_DEGREES = 270;
		private const float RIGHT_WING_TARGET_ANGLE_IN_DEGREES = 90;
		private const float WING_ROTATE_SPEED_IN_M_DEGREES_S = 45;

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

        protected override ISoundKey GetEngineSoundKey()
        {
            // TEMP  Use satellite sound once we have it :)
            return SoundKeys.Engines.Bomber;
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

            IBarrelControllerArgs args
                = new BarrelControllerArgs(
                    targetFilter,
                    _factoryProvider.TargetPositionPredictorFactory.CreateDummyPredictor(),
                    _factoryProvider.AngleCalculatorFactory.CreateAngleCalculator(),
                    _factoryProvider.AccuracyAdjusterFactory.CreateDummyAdjuster(),
                    _movementControllerFactory.CreateDummyRotationMovementController(),
                    _factoryProvider.TargetPositionValidatorFactory.CreateDummyValidator(),
                    _factoryProvider.AngleLimiterFactory.CreateDummyLimiter(),
                    _factoryProvider,
                    parent: this);

            _barrelController.Initialise(args);
			
			// Target detection
			ITargetFinder targetFinder = _targetsFactory.CreateRangedTargetFinder(_targetDetector, targetFilter);
			_targetProcessor = _targetsFactory.CreateTargetProcessor(targetFinder, new OffensiveBuildableTargetRanker());
			_targetProcessor.AddTargetConsumer(_barrelController);
            _targetProcessor.StartProcessingTargets();
		}

		protected override IList<IPatrolPoint> GetPatrolPoints()
		{
			IList<Vector2> patrolPositions = _aircraftProvider.FindDeathstarPatrolPoints(transform.position, cruisingAltitudeInM);

            IList<IPatrolPoint> patrolPoints = new List<IPatrolPoint>(patrolPositions.Count)
            {
                new PatrolPoint(patrolPositions[0], removeOnceReached: true, actionOnReached: OnClearingLaunchStation),
                new PatrolPoint(patrolPositions[1], removeOnceReached: true)
            };

            for (int i = 2; i < patrolPositions.Count; ++i)
            {
				patrolPoints.Add(new PatrolPoint(patrolPositions[i]));
            }

			return patrolPoints;
		}

		private void OnClearingLaunchStation()
		{
			// Stop moving
			SwitchMovementControllers(DummyMovementController);

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

			SwitchMovementControllers(PatrollingMovementController);
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
