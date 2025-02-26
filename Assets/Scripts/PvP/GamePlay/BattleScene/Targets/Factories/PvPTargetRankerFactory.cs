using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers.Ranking;
using BattleCruisers.Targets.Factories;
using BattleCruisers.Targets.TargetTrackers.Ranking;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories
{
    public class PvPTargetRankerFactory : ITargetRankerFactory
    {
        public PvPTargetRankerFactory()
        {
            EqualTargetRanker = new PvPEqualTargetRanker();
            ShipTargetRanker = new PvPShipTargetRanker();
            OffensiveBuildableTargetRanker = new PvPOffensiveBuildableTargetRanker();
        }

        public ITargetRanker EqualTargetRanker { get; }
        public ITargetRanker ShipTargetRanker { get; }
        public ITargetRanker OffensiveBuildableTargetRanker { get; }

        public ITargetRanker CreateBoostedRanker(ITargetRanker baseRanker, int rankBoost)
        {
            return new PvPBoostedRanker(baseRanker, rankBoost);
        }
    }
}
