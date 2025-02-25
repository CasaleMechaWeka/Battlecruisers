using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetTrackers;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers.UserChosen
{
    public interface IPvPUserChosenTargetManager :
        IRankedTargetTracker,   // For TargetProcessor/CompositTargetFinder
        ITargetConsumer         // For UI, so user can set their chosen target
    {
        // empty
    }
}