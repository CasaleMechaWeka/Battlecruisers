using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers.Ranking;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProcessors
{
    public class PvPShipTargetProcessorWrapper : PvPProximityTargetProcessorWrapper
    {
        private const int ATTACKING_RANK_BOOST = 200;

        public IPvPTargetFinder InRangeTargetFinder { get; private set; }

        protected override IPvPTargetProcessor CreateTargetProcessorInternal(IPvPTargetProcessorArgs args)
        {
            Assert.IsNotNull(args.ParentTarget);

            // In range targets
            InRangeTargetFinder = CreateTargetFinder(args);
            IPvPTargetRanker inRangeTargetRanker = CreateTargetRanker(args.TargetFactories.RankerFactory);
            IPvPRankedTargetTracker inRangeTargetTracker = args.CruiserSpecificFactories.Targets.TrackerFactory.CreateRankedTargetTracker(InRangeTargetFinder, inRangeTargetRanker);

            // Attacking targets
            IPvPTargetFilter attackingTargetFilter = args.TargetFactories.FilterFactory.CreateTargetFilter(args.EnemyFaction, args.AttackCapabilities);
            IPvPTargetFinder attackingTargetFinder = args.TargetFactories.FinderFactory.CreateAttackingTargetFinder(args.ParentTarget, attackingTargetFilter);
            IPvPTargetRanker baseRanker = args.TargetFactories.RankerFactory.ShipTargetRanker;
            IPvPTargetRanker attackingTargetRanker = args.TargetFactories.RankerFactory.CreateBoostedRanker(baseRanker, ATTACKING_RANK_BOOST);
            IPvPRankedTargetTracker attackingTargetTracker = args.CruiserSpecificFactories.Targets.TrackerFactory.CreateRankedTargetTracker(attackingTargetFinder, attackingTargetRanker);

            IPvPRankedTargetTracker compositeTracker
                = args.CruiserSpecificFactories.Targets.TrackerFactory.CreateCompositeTracker(
                    inRangeTargetTracker,
                    attackingTargetTracker,
                    args.CruiserSpecificFactories.Targets.TrackerFactory.UserChosenTargetTracker);
            return args.CruiserSpecificFactories.Targets.ProcessorFactory.CreateTargetProcessor(compositeTracker);
        }
    }
}