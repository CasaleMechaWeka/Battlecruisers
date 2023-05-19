using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetDetectors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProcessors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers.Ranking;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
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
    public class PvPShipBlockingEnemyProvider : PvPBroadcastingTargetProvider, IPvPTargetConsumer
    {
        private readonly IPvPTargetProcessor _targetProcessor;
        private readonly IPvPTargetFilter _isInFrontFilter;

        IPvPTarget IPvPTargetConsumer.Target
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
            IPvPTargetFactoriesProvider targetsFactories,
            IPvPTargetDetector enemyDetector,
            IPvPUnit parentUnit)
        {
            PvPHelper.AssertIsNotNull(cruiserSpecificFactories, targetsFactories, enemyDetector, parentUnit);

            _isInFrontFilter = targetsFactories.FilterFactory.CreateTargetInFrontFilter(parentUnit);

            IList<PvPTargetType> blockingEnemyTypes = new List<PvPTargetType>() { PvPTargetType.Ships, PvPTargetType.Cruiser, PvPTargetType.Buildings };
            PvPFaction enemyFaction = PvPHelper.GetOppositeFaction(parentUnit.Faction);
            IPvPTargetFilter enemyDetectionFilter = targetsFactories.FilterFactory.CreateTargetFilter(enemyFaction, blockingEnemyTypes);
            IPvPTargetFinder enemyFinder = targetsFactories.FinderFactory.CreateRangedTargetFinder(enemyDetector, enemyDetectionFilter);

            IPvPTargetRanker targetRanker = targetsFactories.RankerFactory.EqualTargetRanker;
            IPvPRankedTargetTracker targetTracker = cruiserSpecificFactories.Targets.TrackerFactory.CreateRankedTargetTracker(enemyFinder, targetRanker);
            _targetProcessor = cruiserSpecificFactories.Targets.ProcessorFactory.CreateTargetProcessor(targetTracker);

            _targetProcessor.AddTargetConsumer(this);
        }

        public override void DisposeManagedState()
        {
            _targetProcessor.DisposeManagedState();
        }
    }
}
