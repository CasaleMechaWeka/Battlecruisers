using BattleCruisers.Targets.Factories;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.Targets.TargetTrackers.Ranking;
using UnityEngine.Assertions;

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
            ITargetRanker inRangeTargetRanker = CreateTargetRanker(args.TargetFactories.RankerFactory);
            IRankedTargetTracker inRangeTargetTracker = args.CruiserSpecificFactories.Targets.TrackerFactory.CreateRankedTargetTracker(InRangeTargetFinder, inRangeTargetRanker);

            // Attacking targets
            ITargetFilter attackingTargetFilter = args.TargetFactories.FilterFactory.CreateTargetFilter(args.EnemyFaction, args.AttackCapabilities);
            ITargetFinder attackingTargetFinder = new AttackingTargetFinder(args.ParentTarget, attackingTargetFilter);
            ITargetRanker baseRanker = args.TargetFactories.RankerFactory.ShipTargetRanker;
            ITargetRanker attackingTargetRanker = args.TargetFactories.RankerFactory.CreateBoostedRanker(baseRanker, ATTACKING_RANK_BOOST);
            IRankedTargetTracker attackingTargetTracker = args.CruiserSpecificFactories.Targets.TrackerFactory.CreateRankedTargetTracker(attackingTargetFinder, attackingTargetRanker);

            IRankedTargetTracker compositeTracker
                = args.CruiserSpecificFactories.Targets.TrackerFactory.CreateCompositeTracker(
                    inRangeTargetTracker,
                    attackingTargetTracker,
                    args.CruiserSpecificFactories.Targets.TrackerFactory.UserChosenTargetTracker);
            return args.CruiserSpecificFactories.Targets.ProcessorFactory.CreateTargetProcessor(compositeTracker);
        }
    }
}