using System;
using System.Collections.Generic;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Movement;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.Targets.TargetProcessors.Ranking;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
    // FELIX  Create interface
    public abstract class BarrelWrapper : MonoBehaviour, ITargetConsumer, IDisposable
    {
        private ITargetsFactory _targetsFactory;
        private Faction _enemyFaction;
        private ITargetFinder _targetFinder;
        private ITargetProcessor _targetProcessor;

        protected BarrelController _barrelController;

		public CircleTargetDetector enemyDetector;

		protected abstract IList<TargetType> AttackCapabilities { get; }
		public TurretStats TurretStats { get { return _barrelController.TurretStats; } }

        public ITarget Target 
        { 
            get { return _barrelController.Target; }
            set { _barrelController.Target = value; }
        }

        public void StaticInitialise()
        {
            _barrelController = gameObject.GetComponentInChildren<BarrelController>();
            Assert.IsNotNull(_barrelController);
            _barrelController.StaticInitialise();
        }

        public void Initialise(ITargetsFactory targetsFactory, IMovementControllerFactory movementControllerFactory, Faction enemyFaction)
        {
            _targetsFactory = targetsFactory;
            _enemyFaction = enemyFaction;

            _barrelController
                .Initialise(
                    CreateTargetFilter(targetsFactory, enemyFaction), 
                    CreateAngleCalculator(), 
                    CreateRotationMovementController(movementControllerFactory));
        }

        public void StartAttackingTargets()
        {
            // FELIX  Specific to defensive turrets.  Perhaps DefensiveBarrelWrapper and OffensiveBarrelWrapper classes?

            // Create target finder
            enemyDetector.Initialise(_barrelController.TurretStats.rangeInM);
			bool isDetectable = true;
			ITargetFilter enemyDetectionFilter = _targetsFactory.CreateDetectableTargetFilter(_enemyFaction, isDetectable, AttackCapabilities);
            _targetFinder = _targetsFactory.CreateRangedTargetFinder(enemyDetector, enemyDetectionFilter);

            // Start processing targets
			ITargetRanker targetRanker = _targetsFactory.CreateEqualTargetRanker();
            _targetProcessor = _targetsFactory.CreateTargetProcessor(_targetFinder, targetRanker);
			_targetProcessor.AddTargetConsumer(this);            
        }

        protected virtual ITargetFilter CreateTargetFilter(ITargetsFactory targetsFactory, Faction enemyFaction)
        {
            return targetsFactory.CreateTargetFilter(enemyFaction, AttackCapabilities);
        }

        protected abstract IAngleCalculator CreateAngleCalculator();

        protected virtual IRotationMovementController CreateRotationMovementController(IMovementControllerFactory movementControllerFactory)
        {
            return movementControllerFactory.CreateRotationMovementController(_barrelController.TurretStats.turretRotateSpeedInDegrees, _barrelController.transform);
        }

        public void Dispose()
        {
			_targetProcessor.RemoveTargetConsumer(this);
			_targetProcessor.Dispose();
			_targetProcessor = null;

			_targetFinder.Dispose();
			_targetFinder = null;
        }
    }
}
