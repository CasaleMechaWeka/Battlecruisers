using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors.Ranking;
using BattleCruisers.Targets.TargetTrackers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;

namespace BattleCruisers.Targets.TargetProcessors
{
    public class ShipTargetProcessorWrapper : ProximityTargetProcessorWrapper
    {
        private const int ATTACKING_RANK_BOOST = 200;

        protected override ITargetProcessor CreateTargetProcessor(ITargetProcessorArgs args)
        {
            Assert.IsNotNull(args.ParentTarget);

            // In range targets
            ITargetFinder inRangeTargetFinder = CreateTargetFinder(args);
            ITargetRanker inRangeTargetRanker = CreateTargetRanker(args.TargetsFactory);
            IHighestPriorityTargetTracker inRangeTargetTracker = args.TargetsFactory.CreateHighestPriorityTargetTracker(inRangeTargetFinder, inRangeTargetRanker);

            // Attacking targets
            ITargetFilter attackingTargetFilter = CreateAttackingTargetFilter(args);
            ITargetFinder attackingTargetFinder = args.TargetsFactory.CreateAttackingTargetFinder(args.ParentTarget, attackingTargetFilter);
            ITargetRanker baseRanker = args.TargetsFactory.CreateShipTargetRanker();
            ITargetRanker attackingTargetRanker = args.TargetsFactory.CreateBoostedRanker(baseRanker, ATTACKING_RANK_BOOST);
            IHighestPriorityTargetTracker attackingTargetTracker = args.TargetsFactory.CreateHighestPriorityTargetTracker(attackingTargetFinder, attackingTargetRanker);

            IHighestPriorityTargetTracker compositeTracker 
                = args.TargetsFactory.CreateCompositeTracker(
                    inRangeTargetTracker, 
                    attackingTargetTracker, 
                    args.TargetsFactory.UserChosenTargetTracker);
            return args.TargetsFactory.CreateTargetProcessor(compositeTracker);
        }

        private ITargetFilter CreateAttackingTargetFilter(ITargetProcessorArgs args)
        {
            // Do not want to stop for aircraft, even if they are attacking us
            IList<TargetType> validTargetTypes = args.ParentTarget.AttackCapabilities.ToList();
            validTargetTypes.Remove(TargetType.Aircraft);
            return args.TargetsFactory.CreateTargetFilter(args.EnemyFaction, validTargetTypes);
        }
    }
}