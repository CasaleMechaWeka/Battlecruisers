using BattleCruisers.Targets.Factories;

namespace BattleCruisers.Targets.TargetTrackers.Ranking.Wrappers
{
    public interface ITargetRankerWrapper
    {
        ITargetRanker CreateTargetRanker(ITargetRankerFactory rankerFactory);
    }
}
