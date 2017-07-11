using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Projectiles;
using BattleCruisers.Projectiles.Spawners;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.Targets.TargetProcessors.Ranking;
using BattleCruisers.Utils;
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
		private int _originalPatrolPointsCount;

		public float cruisingAltitudeInM;
		public DeathstarWingController leftWing, rightWing;

		private const float CRUISING_HEIGHT_EQUALITY_MARGIN = 1;
		private const float LEFT_WING_TARGET_ANGLE_IN_DEGREES = 270;
		private const float RIGHT_WING_TARGET_ANGLE_IN_DEGREES = 90;
		private const float WING_ROTATE_SPEED_IN_M_DEGREES_S = 45;
		private const float NUM_OF_PATROL_POINTS_TO_REMOVE = 2;

		private bool IsAtCruisingHeight
		{
			get
			{
				return Mathf.Abs(transform.position.y - cruisingAltitudeInM) <= CRUISING_HEIGHT_EQUALITY_MARGIN;
			}
		}

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

			IAngleCalculator angleCalculator = _angleCalculatorFactory.CreateAngleCalcultor(_targetPositionPredictorFactory);
			leftWing.Initialise(angleCalculator, WING_ROTATE_SPEED_IN_M_DEGREES_S);
			rightWing.Initialise(angleCalculator, WING_ROTATE_SPEED_IN_M_DEGREES_S);
		}

		protected override void OnBuildableCompleted()
		{
			base.OnBuildableCompleted();

			Assert.IsTrue(cruisingAltitudeInM > transform.position.y);

			// Patrolling
			PatrolPoints = _aircraftProvider.FindDeathstarPatrolPoints(transform.position, cruisingAltitudeInM);
			_originalPatrolPointsCount = PatrolPoints.Count;
			StartPatrolling();

			// Barrel controller
			IAngleCalculator angleCalculator = _angleCalculatorFactory.CreateLeadingAngleCalcultor(_targetPositionPredictorFactory);
			Faction enemyFaction = Helper.GetOppositeFaction(Faction);
			ITargetFilter targetFilter = _targetsFactory.CreateTargetFilter(enemyFaction, _attackCapabilities);
			_barrelController.Initialise(targetFilter, angleCalculator);
			
			// Target detection
			ITargetFinder targetFinder = _targetsFactory.CreateRangedTargetFinder(_targetDetector, targetFilter);
			_targetProcessor = _targetsFactory.CreateTargetProcessor(targetFinder, new OffensiveBuildableTargetRanker());
			_targetProcessor.AddTargetConsumer(_barrelController);
		}

		protected override void OnPatrolPointReached(Vector2 patrolPointReached)
 		{
			// FELIX  Refactor!  PatrolPoint class :)
			if (PatrolPoints.Count == _originalPatrolPointsCount)
			{
				PatrolPoints.Remove(patrolPointReached);
				UnfoldWings();
			}
			else if (PatrolPoints.Count == _originalPatrolPointsCount - 1)
			{
				PatrolPoints.Remove(patrolPointReached);
			}

			Assert.IsTrue(PatrolPoints.Count >= 2);
		}
		
		private void UnfoldWings()
		{
			leftWing.StartRotatingWing(LEFT_WING_TARGET_ANGLE_IN_DEGREES);
			rightWing.StartRotatingWing(RIGHT_WING_TARGET_ANGLE_IN_DEGREES);
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
