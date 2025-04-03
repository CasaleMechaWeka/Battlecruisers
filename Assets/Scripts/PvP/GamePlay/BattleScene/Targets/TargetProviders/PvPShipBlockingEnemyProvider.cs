using BattleCruisers.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetDetectors;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.Targets.TargetProviders;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.Targets.TargetTrackers.Ranking;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProviders
{
    /// <summary>
    /// Simply wraps a target processor that detects blocking enemy targets.
    /// 
    /// NOTE:
    /// + Assumes all blocking targets will be in front of the parent unit
    /// (should hold true for ships :) ).
    /// </summary>
    public class PvPShipBlockingEnemyProvider : BroadcastingTargetProvider, ITargetConsumer
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

        public PvPShipBlockingEnemyProvider(
            IPvPCruiserSpecificFactories cruiserSpecificFactories,
            PvPTargetFactoriesProvider targetsFactories,
            ITargetDetector enemyDetector,
            IPvPUnit parentUnit)
        {
            PvPHelper.AssertIsNotNull(cruiserSpecificFactories, targetsFactories, enemyDetector, parentUnit);

            _isInFrontFilter = targetsFactories.FilterFactory.CreateTargetInFrontFilter(parentUnit);

            IList<TargetType> blockingEnemyTypes = new List<TargetType>() { TargetType.Ships, TargetType.Cruiser, TargetType.Buildings };
            Faction enemyFaction = PvPHelper.GetOppositeFaction(parentUnit.Faction);
            ITargetFilter enemyDetectionFilter = targetsFactories.FilterFactory.CreateTargetFilter(enemyFaction, blockingEnemyTypes);
            ITargetFinder enemyFinder = new RangedTargetFinder(enemyDetector, enemyDetectionFilter);

            ITargetRanker targetRanker = targetsFactories.RankerFactory.EqualTargetRanker;
            IRankedTargetTracker targetTracker = cruiserSpecificFactories.Targets.TrackerFactory.CreateRankedTargetTracker(enemyFinder, targetRanker);
            _targetProcessor = cruiserSpecificFactories.Targets.ProcessorFactory.CreateTargetProcessor(targetTracker);

            _targetProcessor.AddTargetConsumer(this);
        }

        public override void DisposeManagedState()
        {
            _targetProcessor.DisposeManagedState();
        }
    }
}
