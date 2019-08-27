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
            ITargetRanker targetRanker = CreateTargetRanker(args.TargetFactories.RankerFactory);
            _targetTracker = args.CruiserSpecificFactories.Targets.TrackerFactory.CreateRankedTargetTracker(_targetFinder, targetRanker);

            if (considerUserChosenTarget)
            {
                ITargetTracker inRangeTargetTracker = args.CruiserSpecificFactories.Targets.TrackerFactory.CreateTargetTracker(_targetFinder);
                IRankedTargetTracker userChosenInRangeTargetTracker = args.CruiserSpecificFactories.Targets.TrackerFactory.CreateUserChosenInRangeTargetTracker(inRangeTargetTracker);
                IRankedTargetTracker inRangeSingleTargetTracker = _targetTracker;
                _targetTracker = args.CruiserSpecificFactories.Targets.TrackerFactory.CreateCompositeTracker(inRangeSingleTargetTracker, userChosenInRangeTargetTracker);
            }

            return args.CruiserSpecificFactories.Targets.ProcessorFactory.CreateTargetProcessor(_targetTracker);
        }

        protected ITargetRanker CreateTargetRanker(ITargetRankerFactory rankerFactory)
        {
            ITargetRankerWrapper targetRankerWrapper = GetComponent<ITargetRankerWrapper>();
            Assert.IsNotNull(targetRankerWrapper);
            return targetRankerWrapper.CreateTargetRanker(rankerFactory);
        }

        protected virtual ITargetFinder CreateTargetFinder(ITargetProcessorArgs args)
        {
			CircleTargetDetectorController enemyDetector = gameObject.GetComponentInChildren<CircleTargetDetectorController>();
			Assert.IsNotNull(enemyDetector);
			enemyDetector.Initialise(args.MaxRangeInM);

			// Create target finder
			ITargetFilter enemyDetectionFilter = args.TargetFactories.FilterFactory.CreateTargetFilter(args.EnemyFaction, args.AttackCapabilities);
			return args.TargetFactories.FinderFactory.CreateRangedTargetFinder(enemyDetector, enemyDetectionFilter);
        }
    }
}
