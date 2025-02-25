using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers.Ranking;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.Targets.TargetFinders;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories
{
    public interface IPvPTargetTrackerFactory
    {
        // Highest priority trackers
        IRankedTargetTracker UserChosenTargetTracker { get; }
        IRankedTargetTracker CreateUserChosenInRangeTargetTracker(ITargetTracker inRangeTargetTracker);
        IRankedTargetTracker CreateRankedTargetTracker(ITargetFinder targetFinder, IPvPTargetRanker targetRanker);
        IRankedTargetTracker CreateCompositeTracker(params IRankedTargetTracker[] targetTrackers);

        // Trackers
        ITargetTracker CreateTargetTracker(ITargetFinder targetFinder);
    }
}