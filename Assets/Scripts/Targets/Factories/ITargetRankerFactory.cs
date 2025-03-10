using BattleCruisers.Targets.TargetTrackers.Ranking;

namespace BattleCruisers.Targets.Factories
{
    public interface ITargetRankerFactory
    {
        ITargetRanker EqualTargetRanker { get; }
        ITargetRanker ShipTargetRanker { get; }
        ITargetRanker OffensiveBuildableTargetRanker { get; }
        ITargetRanker CreateBoostedRanker(ITargetRanker baseRanker, int rankBoost);
    }
}