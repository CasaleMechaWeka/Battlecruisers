namespace BattleCruisers.Targets.TargetTrackers
{
    public interface IUserChosenTargetManager : 
        IHighestPriorityTargetTracker,  // For TargetProcessor/CompositTargetFinder
        ITargetConsumer                 // For UI, so user can set their chosen target
    {
        // empty
    }
}