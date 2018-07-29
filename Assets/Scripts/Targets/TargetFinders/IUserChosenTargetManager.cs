using BattleCruisers.Targets.TargetProviders;

namespace BattleCruisers.Targets.TargetFinders
{
    public interface IUserChosenTargetManager : 
        ITargetFinder, 
        ITargetConsumer,
        ITargetProvider
    {
        // empty
    }
}