
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers.Ranking;
namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories
{
    public interface IPvPTargetTrackerFactory
    {
        // Highest priority trackers
        IPvPRankedTargetTracker UserChosenTargetTracker { get; }
        IPvPRankedTargetTracker CreateUserChosenInRangeTargetTracker(IPvPTargetTracker inRangeTargetTracker);
        IPvPRankedTargetTracker CreateRankedTargetTracker(IPvPTargetFinder targetFinder, IPvPTargetRanker targetRanker);
        IPvPRankedTargetTracker CreateCompositeTracker(params IPvPRankedTargetTracker[] targetTrackers);

        // Trackers
        IPvPTargetTracker CreateTargetTracker(IPvPTargetFinder targetFinder);
    }
}