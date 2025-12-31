using System;
using BattleCruisers.Targets.Factories;
using BattleCruisers.Targets.TargetDetectors;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.Targets.TargetTrackers.Ranking;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Targets.TargetProcessors
{
    public class ProximityTargetProcessorWrapper : TargetProcessorWrapper, IManagedDisposable
    {
        private ITargetFinder _targetFinder;
        private IRankedTargetTracker _targetTracker;

        public bool considerUserChosenTarget;
        public TargetRankerType targetRankerType;

        protected override ITargetProcessor CreateTargetProcessorInternal(TargetProcessorArgs args)
        {
            _targetFinder = CreateTargetFinder(args);
            ITargetRanker targetRanker = CreateTargetRanker(args.TargetFactories.RankerFactory);
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

        protected virtual ITargetFinder CreateTargetFinder(TargetProcessorArgs args)
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

    public enum TargetRankerType
    {
        Equal = 0,
        Offensive = 1,
        Ship = 2
    }
}
