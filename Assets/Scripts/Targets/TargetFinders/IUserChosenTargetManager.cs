using BattleCruisers.Targets.TargetTrackers;

namespace BattleCruisers.Targets.TargetFinders
{
    public interface IUserChosenTargetManager : 
        IHighestPriorityTargetTracker,  // For TargetProcessor/CompositTargetFinder
        ITargetConsumer                 // For UI, so user can set their chosen target
    {
        // empty
    }
}