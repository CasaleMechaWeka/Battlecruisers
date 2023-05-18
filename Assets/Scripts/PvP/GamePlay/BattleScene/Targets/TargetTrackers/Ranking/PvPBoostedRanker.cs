using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers.Ranking
{
    public class PvPBoostedRanker : IPvPTargetRanker
    {
        private readonly IPvPTargetRanker _baseRanker;
        private readonly int _rankBoost;

        public PvPBoostedRanker(IPvPTargetRanker baseRanker, int rankBoost)
        {
            Assert.IsNotNull(baseRanker);

            _baseRanker = baseRanker;
            _rankBoost = rankBoost;
        }

        public int RankTarget(IPvPTarget target)
        {
            return _baseRanker.RankTarget(target) + _rankBoost;
        }
    }
}