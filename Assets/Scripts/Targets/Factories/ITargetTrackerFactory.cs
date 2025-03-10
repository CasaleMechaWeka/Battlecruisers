using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.Targets.TargetTrackers.Ranking;

namespace BattleCruisers.Targets.Factories
{
    public interface ITargetTrackerFactory
    {
        // Highest priority trackers
        IRankedTargetTracker UserChosenTargetTracker { get; }
        IRankedTargetTracker CreateUserChosenInRangeTargetTracker(ITargetTracker inRangeTargetTracker);
        IRankedTargetTracker CreateRankedTargetTracker(ITargetFinder targetFinder, ITargetRanker targetRanker);
        IRankedTargetTracker CreateCompositeTracker(params IRankedTargetTracker[] targetTrackers);

        // Trackers
        ITargetTracker CreateTargetTracker(ITargetFinder targetFinder);
    }
}