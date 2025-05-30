using System;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetDetectors;
using BattleCruisers.Targets.Factories;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.Targets.TargetTrackers.Ranking;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProcessors
{
    public class PvPProximityTargetProcessorWrapper : PvPTargetProcessorWrapper, IManagedDisposable
    {
        private ITargetFinder _targetFinder;
        private IRankedTargetTracker _targetTracker;

        public bool considerUserChosenTarget;
        public TargetRankerType targetRankerType;

        protected override ITargetProcessor CreateTargetProcessorInternal(IPvPTargetProcessorArgs args)
        {
            _targetFinder = CreateTargetFinder(args);
            ITargetRanker targetRanker = CreateTargetRanker(PvPTargetFactoriesProvider.RankerFactory);
            _targetTracker = args.CruiserSpecificFactories.Targets.TrackerFactory.CreateRankedTargetTracker(_targetFinder, targetRanker);

            if (considerUserChosenTarget)
            {
                TargetTracker inRangeTargetTracker = args.CruiserSpecificFactories.Targets.TrackerFactory.CreateTargetTracker(_targetFinder);
                IRankedTargetTracker userChosenInRangeTargetTracker = args.CruiserSpecificFactories.Targets.TrackerFactory.CreateUserChosenInRangeTargetTracker(inRangeTargetTracker);
                IRankedTargetTracker inRangeSingleTargetTracker = _targetTracker;
                _targetTracker = args.CruiserSpecificFactories.Targets.TrackerFactory.CreateCompositeTracker(inRangeSingleTargetTracker, userChosenInRangeTargetTracker);
            }

            return new TargetProcessor(_targetTracker);
        }

        protected ITargetRanker CreateTargetRanker(TargetRankerFactory rankerFactory)
        {
            return targetRankerType switch
            {
                TargetRankerType.Equal => rankerFactory.EqualTargetRanker,
                TargetRankerType.Offensive => rankerFactory.OffensiveBuildableTargetRanker,
                TargetRankerType.Ship => rankerFactory.ShipTargetRanker,
                _ => throw new ArgumentException(),
            };
        }

        protected virtual ITargetFinder CreateTargetFinder(IPvPTargetProcessorArgs args)
        {
            PvPCircleTargetDetectorController enemyDetector = gameObject.GetComponentInChildren<PvPCircleTargetDetectorController>();
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
