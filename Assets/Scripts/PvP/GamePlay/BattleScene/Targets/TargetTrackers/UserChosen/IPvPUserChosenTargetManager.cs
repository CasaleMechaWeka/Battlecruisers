using BattleCruisers.Targets;
namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers.UserChosen
{
    public interface IPvPUserChosenTargetManager :
        IPvPRankedTargetTracker,   // For TargetProcessor/CompositTargetFinder
        ITargetConsumer         // For UI, so user can set their chosen target
    {
        // empty
    }
}