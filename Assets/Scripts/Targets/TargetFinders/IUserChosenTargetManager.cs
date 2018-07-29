using BattleCruisers.Targets.TargetProviders;

namespace BattleCruisers.Targets.TargetFinders
{
    public interface IUserChosenTargetManager : 
        ITargetFinder,      // For TargetProcessor/CompositTargetFinder
        ITargetConsumer,    // For target rankers
        ITargetProvider     // For UI, so user can set their chosen target
    {
        // empty
    }
}