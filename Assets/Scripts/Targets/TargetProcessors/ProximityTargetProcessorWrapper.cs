using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors.Ranking;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Targets.TargetProcessors
{
    public class ProximityTargetProcessorWrapper : MonoBehaviour, ITargetProcessorWrapper
	{
        private ITargetConsumer _targetConsumer;
		private ITargetFinder _targetFinder;
		private ITargetProcessor _targetProcessor;

        private bool IsInitialised { get { return _targetProcessor != null; } }

        public void StartProvidingTargets(ITargetsFactory targetsFactory, ITargetConsumer targetConsumer, 
            Faction enemyFaction, float detectionRangeInM, IList<TargetType> attackCapabilities)
		{
			CircleTargetDetector enemyDetector = gameObject.GetComponentInChildren<CircleTargetDetector>();
			Assert.IsNotNull(enemyDetector);

            _targetConsumer = targetConsumer;

            // Create target finder
            enemyDetector.Initialise(detectionRangeInM);
			ITargetFilter enemyDetectionFilter = targetsFactory.CreateTargetFilter(enemyFaction, attackCapabilities);
			_targetFinder = targetsFactory.CreateRangedTargetFinder(enemyDetector, enemyDetectionFilter);

			// Start processing targets
			ITargetRanker targetRanker = targetsFactory.CreateEqualTargetRanker();
			_targetProcessor = targetsFactory.CreateTargetProcessor(_targetFinder, targetRanker);
            _targetProcessor.AddTargetConsumer(_targetConsumer);
		}

		public void Dispose()
		{
            if (IsInitialised)
            {
	            _targetProcessor.RemoveTargetConsumer(_targetConsumer);
	            _targetProcessor.Dispose();
	            _targetProcessor = null;

	            _targetFinder.Dispose();
	            _targetFinder = null;
			}
        }
    }
}
