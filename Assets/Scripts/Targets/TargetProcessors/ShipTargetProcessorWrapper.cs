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

        protected override ITargetProcessor CreateTargetProcessor(ITargetProcessorArgs args)
        {
            Assert.IsNotNull(args.ParentTarget);

            // In range targets
            InRangeTargetFinder = CreateTargetFinder(args);
            ITargetRanker inRangeTargetRanker = CreateTargetRanker(args.TargetsFactory);
            IHighestPriorityTargetTracker inRangeTargetTracker = args.TargetsFactory.CreateHighestPriorityTargetTracker(InRangeTargetFinder, inRangeTargetRanker);

            // Attacking targets
            ITargetFilter attackingTargetFilter = args.TargetsFactory.CreateTargetFilter(args.EnemyFaction, args.AttackCapabilities);
            ITargetFinder attackingTargetFinder = args.TargetsFactory.CreateAttackingTargetFinder(args.ParentTarget, attackingTargetFilter);
            ITargetRanker baseRanker = args.TargetsFactory.ShipTargetRanker;
            ITargetRanker attackingTargetRanker = args.TargetsFactory.CreateBoostedRanker(baseRanker, ATTACKING_RANK_BOOST);
            IHighestPriorityTargetTracker attackingTargetTracker = args.TargetsFactory.CreateHighestPriorityTargetTracker(attackingTargetFinder, attackingTargetRanker);

            IHighestPriorityTargetTracker compositeTracker 
                = args.TargetsFactory.CreateCompositeTracker(
                    inRangeTargetTracker, 
                    attackingTargetTracker, 
                    args.TargetsFactory.UserChosenTargetTracker);
            return args.TargetsFactory.CreateTargetProcessor(compositeTracker);
        }
    }
}