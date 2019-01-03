using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetTrackers.Ranking;
using BattleCruisers.Targets.TargetTrackers;
using UnityEngine.Assertions;

namespace BattleCruisers.Targets.TargetProcessors
{
    public class ShipTargetProcessorWrapper : ProximityTargetProcessorWrapper
    {
        private const int ATTACKING_RANK_BOOST = 200;

        public ITargetFinder InRangeTargetFinder { get; private set; }

        public override ITargetProcessor CreateTargetProcessor(ITargetProcessorArgs args)
        {
            Assert.IsNotNull(args.ParentTarget);

            // In range targets
            InRangeTargetFinder = CreateTargetFinder(args);
            ITargetRanker inRangeTargetRanker = CreateTargetRanker(args.TargetFactories.RankerFactory);
            IRankedTargetTracker inRangeTargetTracker = args.TargetFactories.TrackerFactory.CreateRankedTargetTracker(InRangeTargetFinder, inRangeTargetRanker);

            // Attacking targets
            ITargetFilter attackingTargetFilter = args.TargetFactories.FilterFactory.CreateTargetFilter(args.EnemyFaction, args.AttackCapabilities);
            ITargetFinder attackingTargetFinder = args.TargetFactories.FinderFactory.CreateAttackingTargetFinder(args.ParentTarget, attackingTargetFilter);
            ITargetRanker baseRanker = args.TargetFactories.RankerFactory.ShipTargetRanker;
            ITargetRanker attackingTargetRanker = args.TargetFactories.RankerFactory.CreateBoostedRanker(baseRanker, ATTACKING_RANK_BOOST);
            IRankedTargetTracker attackingTargetTracker = args.TargetFactories.TrackerFactory.CreateRankedTargetTracker(attackingTargetFinder, attackingTargetRanker);

            IRankedTargetTracker compositeTracker 
                = args.TargetFactories.TrackerFactory.CreateCompositeTracker(
                    inRangeTargetTracker, 
                    attackingTargetTracker, 
                    args.TargetFactories.TrackerFactory.UserChosenTargetTracker);
            return args.TargetFactories.ProcessorFactory.CreateTargetProcessor(compositeTracker);
        }
    }
}