using BattleCruisers.Targets.TargetTrackers.Ranking;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories
{
    public interface IPvPTargetRankerFactory
    {
        ITargetRanker EqualTargetRanker { get; }
        ITargetRanker ShipTargetRanker { get; }
        ITargetRanker OffensiveBuildableTargetRanker { get; }
        ITargetRanker CreateBoostedRanker(ITargetRanker baseRanker, int rankBoost);
    }
}