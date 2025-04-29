using BattleCruisers.Targets.Factories;
using BattleCruisers.Targets.TargetDetectors;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.Targets.TargetTrackers.Ranking;
using BattleCruisers.Targets.TargetTrackers.Ranking.Wrappers;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Targets.TargetProcessors
{
    public class ProximityTargetProcessorWrapper : TargetProcessorWrapper, IManagedDisposable
    {
        private ITargetFinder _targetFinder;
        private IRankedTargetTracker _targetTracker;

        public bool considerUserChosenTarget;

        protected override ITargetProcessor CreateTargetProcessorInternal(ITargetProcessorArgs args)
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

            return new TargetProcessor(_targetTracker);
        }

        protected ITargetRanker CreateTargetRanker(TargetRankerFactory rankerFactory)
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
            ITargetFilter enemyDetectionFilter = new FactionAndTargetTypeFilter(args.EnemyFaction, args.AttackCapabilities);
            return new RangedTargetFinder(enemyDetector, enemyDetectionFilter);
        }

        public override void DisposeManagedState()
        {
            base.DisposeManagedState();

            _targetFinder?.DisposeManagedState();
            _targetFinder = null;

            _targetTracker?.DisposeManagedState();
            _targetTracker = null;
        }
    }
}
