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

		private const float CRUISING_HEIGHT_EQUALITY_MARGIN = 1;

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

			_attackCapabilities.Add(TargetType.Cruiser);
			_attackCapabilities.Add(TargetType.Buildings);

			_barrelController = gameObject.GetComponentInChildren<LaserBarrelController>();
			Assert.IsNotNull(_barrelController);
			_barrelController.StaticInitialise();

			_targetDetector = gameObject.GetComponentInChildren<ITargetDetector>();
			Assert.IsNotNull(_targetDetector);
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
			if (PatrolPoints.Count == _originalPatrolPointsCount)
			{
				PatrolPoints.RemoveAt(0);
				Assert.IsTrue(PatrolPoints.Count >= 2);
			}
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
