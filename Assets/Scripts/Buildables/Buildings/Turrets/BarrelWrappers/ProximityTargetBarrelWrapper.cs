using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.Targets.TargetProcessors.Ranking;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
    public abstract class ProximityTargetBarrelWrapper : BarrelWrapper
    {
		private CircleTargetDetector _enemyDetector;
		private ITargetFinder _targetFinder;
		private ITargetProcessor _targetProcessor;

        public override void StaticInitialise()
        {
            base.StaticInitialise();

			_enemyDetector = gameObject.GetComponentInChildren<CircleTargetDetector>();
			Assert.IsNotNull(_enemyDetector);
        }

		protected override ITargetProcessor GetTargetProcessor()
        {
			// Create target finder
			_enemyDetector.Initialise(_barrelController.TurretStats.rangeInM);
			bool isDetectable = true;
			ITargetFilter enemyDetectionFilter = _factoryProvider.TargetsFactory.CreateDetectableTargetFilter(_enemyFaction, isDetectable, _attackCapabilities);
			_targetFinder = _factoryProvider.TargetsFactory.CreateRangedTargetFinder(_enemyDetector, enemyDetectionFilter);

			// Start processing targets
			ITargetRanker targetRanker = _factoryProvider.TargetsFactory.CreateEqualTargetRanker();
			_targetProcessor = _factoryProvider.TargetsFactory.CreateTargetProcessor(_targetFinder, targetRanker);
            return _targetProcessor;
        }

		public override void Dispose()
        {
            _targetProcessor.RemoveTargetConsumer(this);
		    _targetProcessor.Dispose();
            _targetProcessor = null;

            _targetFinder.Dispose();
            _targetFinder = null;
        }
    }
}
