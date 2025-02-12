using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers.Ranking;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.Targets.TargetFinders;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories
{
    public interface IPvPTargetTrackerFactory
    {
        // Highest priority trackers
        IPvPRankedTargetTracker UserChosenTargetTracker { get; }
        IPvPRankedTargetTracker CreateUserChosenInRangeTargetTracker(ITargetTracker inRangeTargetTracker);
        IPvPRankedTargetTracker CreateRankedTargetTracker(ITargetFinder targetFinder, IPvPTargetRanker targetRanker);
        IPvPRankedTargetTracker CreateCompositeTracker(params IPvPRankedTargetTracker[] targetTrackers);

        // Trackers
        ITargetTracker CreateTargetTracker(ITargetFinder targetFinder);
    }
}