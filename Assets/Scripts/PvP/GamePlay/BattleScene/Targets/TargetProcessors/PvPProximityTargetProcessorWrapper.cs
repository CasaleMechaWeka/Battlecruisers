using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetDetectors;
using BattleCruisers.Targets.Factories;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.Targets.TargetTrackers.Ranking;
using BattleCruisers.Targets.TargetTrackers.Ranking.Wrappers;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProcessors
{
    public class PvPProximityTargetProcessorWrapper : PvPTargetProcessorWrapper, IManagedDisposable
    {
        private ITargetFinder _targetFinder;
        private IRankedTargetTracker _targetTracker;

        public bool considerUserChosenTarget;

        protected override ITargetProcessor CreateTargetProcessorInternal(IPvPTargetProcessorArgs args)
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

        protected ITargetRanker CreateTargetRanker(TargetRankerFactory rankerFactory)
        {
            ITargetRankerWrapper targetRankerWrapper = GetComponent<ITargetRankerWrapper>();
            Assert.IsNotNull(targetRankerWrapper);
            return targetRankerWrapper.CreateTargetRanker(rankerFactory);
        }

        protected virtual ITargetFinder CreateTargetFinder(IPvPTargetProcessorArgs args)
        {
            PvPCircleTargetDetectorController enemyDetector = gameObject.GetComponentInChildren<PvPCircleTargetDetectorController>();
            Assert.IsNotNull(enemyDetector);
            enemyDetector.Initialise(args.MaxRangeInM);

            // Create target finder
            ITargetFilter enemyDetectionFilter = args.TargetFactories.FilterFactory.CreateTargetFilter(args.EnemyFaction, args.AttackCapabilities);
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
