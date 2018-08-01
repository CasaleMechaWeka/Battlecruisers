using BattleCruisers.Targets.TargetProviders;

namespace BattleCruisers.Targets.TargetFinders
{
    public interface IUserChosenTargetManager : 
        ITargetFinder,      // For TargetProcessor/CompositTargetFinder
        ITargetConsumer,    // For UI, so user can set their chosen target
        ITargetProvider     // For target rankers  (FELIX  Remove?  Once target rankers no longer use us :P)
    {
        // empty
    }
}