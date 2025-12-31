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
            TargetFactoriesProvider targetsFactories,
            ITargetDetector enemyDetector,
            IUnit parentUnit)
        {
            Helper.AssertIsNotNull(targetsFactories, enemyDetector, parentUnit);

            _isInFrontFilter = new TargetInFrontFilter(parentUnit);

            IList<TargetType> blockingEnemyTypes = new List<TargetType>() { TargetType.Ships, TargetType.Cruiser, TargetType.Buildings };
            Faction enemyFaction = Helper.GetOppositeFaction(parentUnit.Faction);
            ITargetFilter enemyDetectionFilter = new FactionAndTargetTypeFilter(enemyFaction, blockingEnemyTypes);
            ITargetFinder enemyFinder = new RangedTargetFinder(enemyDetector, enemyDetectionFilter);

            ITargetRanker targetRanker = targetsFactories.RankerFactory.EqualTargetRanker;
            IRankedTargetTracker targetTracker = new RankedTargetTracker(enemyFinder, targetRanker);
            _targetProcessor = new TargetProcessor(targetTracker);

            _targetProcessor.AddTargetConsumer(this);
        }

        public override void DisposeManagedState()
        {
            _targetProcessor.DisposeManagedState();
        }
    }
}
