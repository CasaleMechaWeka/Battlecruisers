using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.Targets.TargetTrackers.Ranking;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using UnityEngine.Assertions;

namespace BattleCruisers.Targets.Factories
{
    public class TargetTrackerFactory : ITargetTrackerFactory
    {
        public IRankedTargetTracker UserChosenTargetTracker { get; }

        public TargetTrackerFactory(IRankedTargetTracker userChosenTargetTracker)
        {
            Assert.IsNotNull(userChosenTargetTracker);
            UserChosenTargetTracker = userChosenTargetTracker;
        }

        public ITargetTracker CreateTargetTracker(ITargetFinder targetFinder)
        {
            return new TargetTracker(targetFinder);
        }

        public IRankedTargetTracker CreateUserChosenInRangeTargetTracker(ITargetTracker inRangeTargetTracker)
        {
            return new UserChosenInRangeTargetTracker(inRangeTargetTracker, UserChosenTargetTracker);
        }

        public IRankedTargetTracker CreateRankedTargetTracker(ITargetFinder targetFinder, ITargetRanker targetRanker)
        {
            return new RankedTargetTracker(targetFinder, targetRanker);
        }

        public IRankedTargetTracker CreateCompositeTracker(params IRankedTargetTracker[] targetTrackers)
        {
            return new CompositeTracker(targetTrackers);
        }
    }
}