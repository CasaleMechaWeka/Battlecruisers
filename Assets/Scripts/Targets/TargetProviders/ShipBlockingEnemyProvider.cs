using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
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

        public ShipBlockingEnemyProvider(ITargetsFactory targetsFactory, ITargetDetector enemyDetector, IUnit parentUnit)
        {
            Helper.AssertIsNotNull(targetsFactory, enemyDetector, parentUnit);

            _isInFrontFilter = targetsFactory.CreateTargetInFrontFilter(parentUnit);

            IList<TargetType> blockingEnemyTypes = new List<TargetType>() { TargetType.Ships, TargetType.Cruiser, TargetType.Buildings };
            Faction enemyFaction = Helper.GetOppositeFaction(parentUnit.Faction);
            ITargetFilter enemyDetectionFilter = targetsFactory.CreateTargetFilter(enemyFaction, blockingEnemyTypes);
            ITargetFinder enemyFinder = targetsFactory.CreateRangedTargetFinder(enemyDetector, enemyDetectionFilter);

            ITargetRanker targetRanker = targetsFactory.EqualTargetRanker;
            IRankedTargetTracker targetTracker = targetsFactory.CreateRankedTargetTracker(enemyFinder, targetRanker);
            _targetProcessor = targetsFactory.CreateTargetProcessor(targetTracker);

            _targetProcessor.AddTargetConsumer(this);
        }

        public override void DisposeManagedState()
        {
            _targetProcessor.DisposeManagedState();
        }
    }
}
