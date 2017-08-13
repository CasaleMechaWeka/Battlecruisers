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
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
    // FELIX  Use in Turret class to avoid duplicate code!
    public abstract class BarrelWrapper : MonoBehaviour, IBarrelWrapper
    {
		private CircleTargetDetector _enemyDetector;
        protected BarrelController _barrelController;
        protected IFactoryProvider _factoryProvider;
        private Faction _enemyFaction;
        private IList<TargetType> _attackCapabilities;
        private ITargetFinder _targetFinder;
        private ITargetProcessor _targetProcessor;

		public TurretStats TurretStats { get { return _barrelController.TurretStats; } }

        public ITarget Target 
        { 
            get { return _barrelController.Target; }
            set { _barrelController.Target = value; }
        }

        public void StaticInitialise()
        {
            _enemyDetector = gameObject.GetComponentInChildren<CircleTargetDetector>();
            Assert.IsNotNull(_enemyDetector);

            _barrelController = gameObject.GetComponentInChildren<BarrelController>();
            Assert.IsNotNull(_barrelController);
            _barrelController.StaticInitialise();
        }

        public void Initialise(IFactoryProvider factoryProvider, Faction enemyFaction, IList<TargetType> attackCapabilities)
        {
            Helper.AssertIsNotNull(factoryProvider, enemyFaction, attackCapabilities);

            _factoryProvider = factoryProvider;
            _enemyFaction = enemyFaction;
            _attackCapabilities = attackCapabilities;

            _barrelController
                .Initialise(
                    CreateTargetFilter(_factoryProvider.TargetsFactory, enemyFaction), 
                    CreateAngleCalculator(), 
                    CreateRotationMovementController(_factoryProvider.MovementControllerFactory));
        }

        public void StartAttackingTargets()
        {
            // FELIX  Specific to defensive turrets.  Perhaps DefensiveBarrelWrapper and OffensiveBarrelWrapper classes?
            // Ranged detection vs global detection

            // Create target finder
            _enemyDetector.Initialise(_barrelController.TurretStats.rangeInM);
			bool isDetectable = true;
			ITargetFilter enemyDetectionFilter = _factoryProvider.TargetsFactory.CreateDetectableTargetFilter(_enemyFaction, isDetectable, _attackCapabilities);
            _targetFinder = _factoryProvider.TargetsFactory.CreateRangedTargetFinder(_enemyDetector, enemyDetectionFilter);

            // Start processing targets
			ITargetRanker targetRanker = _factoryProvider.TargetsFactory.CreateEqualTargetRanker();
            _targetProcessor = _factoryProvider.TargetsFactory.CreateTargetProcessor(_targetFinder, targetRanker);
			_targetProcessor.AddTargetConsumer(this);            
        }

        protected ITargetFilter CreateTargetFilter(ITargetsFactory targetsFactory, Faction enemyFaction)
        {
            return targetsFactory.CreateTargetFilter(enemyFaction, _attackCapabilities);
        }

        protected abstract IAngleCalculator CreateAngleCalculator();

        protected IRotationMovementController CreateRotationMovementController(IMovementControllerFactory movementControllerFactory)
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
