using BattleCruisers.Buildables;

namespace BattleCruisers.Targets.TargetFinders
{
    public interface IUserChosenTargetTracker
    {
        ITarget UserChosenTarget { set; }
    }
}