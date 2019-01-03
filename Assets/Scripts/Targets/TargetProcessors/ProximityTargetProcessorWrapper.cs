using BattleCruisers.Targets.Factories;
using BattleCruisers.Targets.TargetDetectors;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.Targets.TargetTrackers.Ranking;
using BattleCruisers.Targets.TargetTrackers.Ranking.Wrappers;
using UnityEngine.Assertions;

namespace BattleCruisers.Targets.TargetProcessors
{
    public class ProximityTargetProcessorWrapper : TargetProcessorWrapper
	{
		private ITargetFinder _targetFinder;
        private IRankedTargetTracker _targetTracker;

        public bool considerUserChosenTarget;

        public override ITargetProcessor CreateTargetProcessor(ITargetProcessorArgs args)
		{
            _targetFinder = CreateTargetFinder(args);
            ITargetRanker targetRanker = CreateTargetRanker(args.TargetsFactory);
            _targetTracker = args.TargetsFactory.CreateRankedTargetTracker(_targetFinder, targetRanker);

            if (considerUserChosenTarget)
            {
                ITargetTracker inRangeTargetTracker = args.TargetsFactory.CreateTargetTracker(_targetFinder);
                IRankedTargetTracker userChosenInRangeTargetTracker = args.TargetsFactory.CreateUserChosenInRangeTargetTracker(inRangeTargetTracker);
                IRankedTargetTracker inRangeSingleTargetTracker = _targetTracker;
                _targetTracker = args.TargetsFactory.CreateCompositeTracker(inRangeSingleTargetTracker, userChosenInRangeTargetTracker);
            }

            return args.TargetsFactory.CreateTargetProcessor(_targetTracker);
        }

        protected ITargetRanker CreateTargetRanker(ITargetsFactory targetsFactory)
        {
            ITargetRankerWrapper targetRankerWrapper = GetComponent<ITargetRankerWrapper>();
            Assert.IsNotNull(targetRankerWrapper);
            return targetRankerWrapper.CreateTargetRanker(targetsFactory);
        }

        protected virtual ITargetFinder CreateTargetFinder(ITargetProcessorArgs args)
        {
			CircleTargetDetectorController enemyDetector = gameObject.GetComponentInChildren<CircleTargetDetectorController>();
			Assert.IsNotNull(enemyDetector);
			
			// Create target finder
			enemyDetector.Initialise(args.MaxRangeInM);
			ITargetFilter enemyDetectionFilter = args.TargetsFactory.CreateTargetFilter(args.EnemyFaction, args.AttackCapabilities);
			return args.TargetsFactory.CreateRangedTargetFinder(enemyDetector, enemyDetectionFilter);
        }
    }
}
