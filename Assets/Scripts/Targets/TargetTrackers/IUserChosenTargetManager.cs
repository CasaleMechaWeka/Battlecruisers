namespace BattleCruisers.Targets.TargetTrackers
{
    public interface IUserChosenTargetManager : 
        IRankedTargetTracker,   // For TargetProcessor/CompositTargetFinder
        ITargetConsumer         // For UI, so user can set their chosen target
    {
        // empty
    }
}