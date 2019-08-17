using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Targets.Factories;
using BattleCruisers.Targets.TargetDetectors;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.Targets.TargetTrackers.Ranking;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Factories;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.Targets.TargetProviders
{
    /// <summary>
    /// Simply wraps a target processor that detects blocking enemy targets.
    /// 
    /// NOTE:
    /// + Assumes all blocking targets will be in front of the parent unit
    /// (should hold true for ships :) ).
    /// </summary>
    public class ShipBlockingEnemyProvider : BroadcastingTargetProvider, ITargetConsumer
    {
        private readonly ITargetProcessor _targetProcessor;
        private readonly ITargetFilter _isInFrontFilter;

        ITarget ITargetConsumer.Target
        {
            set
            {
                if (value != null)
                {
                    Assert.IsTrue(_isInFrontFilter.IsMatch(value));
                }

                Target = value;
            }
        }

        public ShipBlockingEnemyProvider(
            ICruiserSpecificFactories cruiserSpecificFactories,
            ITargetFactoriesProvider targetsFactories, 
            ITargetDetector enemyDetector, 
            IUnit parentUnit)
        {
            Helper.AssertIsNotNull(cruiserSpecificFactories, targetsFactories, enemyDetector, parentUnit);

            _isInFrontFilter = targetsFactories.FilterFactory.CreateTargetInFrontFilter(parentUnit);

            IList<TargetType> blockingEnemyTypes = new List<TargetType>() { TargetType.Ships, TargetType.Cruiser, TargetType.Buildings };
            Faction enemyFaction = Helper.GetOppositeFaction(parentUnit.Faction);
            ITargetFilter enemyDetectionFilter = targetsFactories.FilterFactory.CreateTargetFilter(enemyFaction, blockingEnemyTypes);
            ITargetFinder enemyFinder = targetsFactories.FinderFactory.CreateRangedTargetFinder(enemyDetector, enemyDetectionFilter);

            ITargetRanker targetRanker = targetsFactories.RankerFactory.EqualTargetRanker;
            IRankedTargetTracker targetTracker = cruiserSpecificFactories.TrackerFactory.CreateRankedTargetTracker(enemyFinder, targetRanker);
            _targetProcessor = cruiserSpecificFactories.ProcessorFactory.CreateTargetProcessor(targetTracker);

            _targetProcessor.AddTargetConsumer(this);
        }

        public override void DisposeManagedState()
        {
            _targetProcessor.DisposeManagedState();
        }
    }
}
