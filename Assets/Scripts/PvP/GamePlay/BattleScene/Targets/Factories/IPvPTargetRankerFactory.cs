using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers.Ranking;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories
{
    public interface IPvPTargetRankerFactory
    {
        IPvPTargetRanker EqualTargetRanker { get; }
        IPvPTargetRanker ShipTargetRanker { get; }
        IPvPTargetRanker OffensiveBuildableTargetRanker { get; }
        IPvPTargetRanker CreateBoostedRanker(IPvPTargetRanker baseRanker, int rankBoost);
    }
}