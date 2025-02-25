using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers.Ranking;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors;
using UnityEngine.Assertions;
using BattleCruisers.Targets.TargetFinders;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProcessors
{
    public class PvPShipTargetProcessorWrapper : PvPProximityTargetProcessorWrapper
    {
        private const int ATTACKING_RANK_BOOST = 200;

        public ITargetFinder InRangeTargetFinder { get; private set; }

        protected override ITargetProcessor CreateTargetProcessorInternal(IPvPTargetProcessorArgs args)
        {
            Assert.IsNotNull(args.ParentTarget);

            // In range targets
            InRangeTargetFinder = CreateTargetFinder(args);
            IPvPTargetRanker inRangeTargetRanker = CreateTargetRanker(args.TargetFactories.RankerFactory);
            IPvPRankedTargetTracker inRangeTargetTracker = args.CruiserSpecificFactories.Targets.TrackerFactory.CreateRankedTargetTracker(InRangeTargetFinder, inRangeTargetRanker);

            // Attacking targets
            ITargetFilter attackingTargetFilter = args.TargetFactories.FilterFactory.CreateTargetFilter(args.EnemyFaction, args.AttackCapabilities);
            ITargetFinder attackingTargetFinder = args.TargetFactories.FinderFactory.CreateAttackingTargetFinder(args.ParentTarget, attackingTargetFilter);
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