using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetDetectors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers.Ranking;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers.Ranking.Wrappers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProcessors
{
    public class PvPProximityTargetProcessorWrapper : PvPTargetProcessorWrapper, IPvPManagedDisposable
    {
        private IPvPTargetFinder _targetFinder;
        private IPvPRankedTargetTracker _targetTracker;

        public bool considerUserChosenTarget;

        protected override IPvPTargetProcessor CreateTargetProcessorInternal(IPvPTargetProcessorArgs args)
        {
            _targetFinder = CreateTargetFinder(args);
            IPvPTargetRanker targetRanker = CreateTargetRanker(args.TargetFactories.RankerFactory);
            _targetTracker = args.CruiserSpecificFactories.Targets.TrackerFactory.CreateRankedTargetTracker(_targetFinder, targetRanker);

            if (considerUserChosenTarget)
            {
                IPvPTargetTracker inRangeTargetTracker = args.CruiserSpecificFactories.Targets.TrackerFactory.CreateTargetTracker(_targetFinder);
                IPvPRankedTargetTracker userChosenInRangeTargetTracker = args.CruiserSpecificFactories.Targets.TrackerFactory.CreateUserChosenInRangeTargetTracker(inRangeTargetTracker);
                IPvPRankedTargetTracker inRangeSingleTargetTracker = _targetTracker;
                _targetTracker = args.CruiserSpecificFactories.Targets.TrackerFactory.CreateCompositeTracker(inRangeSingleTargetTracker, userChosenInRangeTargetTracker);
            }

            return args.CruiserSpecificFactories.Targets.ProcessorFactory.CreateTargetProcessor(_targetTracker);
        }

        protected IPvPTargetRanker CreateTargetRanker(IPvPTargetRankerFactory rankerFactory)
        {
            IPvPTargetRankerWrapper targetRankerWrapper = GetComponent<IPvPTargetRankerWrapper>();
            Assert.IsNotNull(targetRankerWrapper);
            return targetRankerWrapper.CreateTargetRanker(rankerFactory);
        }

        protected virtual IPvPTargetFinder CreateTargetFinder(IPvPTargetProcessorArgs args)
        {
            PvPCircleTargetDetectorController enemyDetector = gameObject.GetComponentInChildren<PvPCircleTargetDetectorController>();
            Assert.IsNotNull(enemyDetector);
            enemyDetector.Initialise(args.MaxRangeInM);

            // Create target finder
            IPvPTargetFilter enemyDetectionFilter = args.TargetFactories.FilterFactory.CreateTargetFilter(args.EnemyFaction, args.AttackCapabilities);
            return args.TargetFactories.FinderFactory.CreateRangedTargetFinder(enemyDetector, enemyDetectionFilter);
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
