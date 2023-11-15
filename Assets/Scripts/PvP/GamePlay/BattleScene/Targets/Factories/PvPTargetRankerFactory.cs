using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers.Ranking;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories
{
    public class PvPTargetRankerFactory : IPvPTargetRankerFactory
    {
        public PvPTargetRankerFactory()
        {
            EqualTargetRanker = new PvPEqualTargetRanker();
            ShipTargetRanker = new PvPShipTargetRanker();
            OffensiveBuildableTargetRanker = new PvPOffensiveBuildableTargetRanker();
        }

        public IPvPTargetRanker EqualTargetRanker { get; }
        public IPvPTargetRanker ShipTargetRanker { get; }
        public IPvPTargetRanker OffensiveBuildableTargetRanker { get; }

        public IPvPTargetRanker CreateBoostedRanker(IPvPTargetRanker baseRanker, int rankBoost)
        {
            return new PvPBoostedRanker(baseRanker, rankBoost);
        }
    }
}
