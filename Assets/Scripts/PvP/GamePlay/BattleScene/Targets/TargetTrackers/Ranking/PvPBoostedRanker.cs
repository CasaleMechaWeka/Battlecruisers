using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetTrackers.Ranking;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers.Ranking
{
    public class PvPBoostedRanker : ITargetRanker
    {
        private readonly ITargetRanker _baseRanker;
        private readonly int _rankBoost;

        public PvPBoostedRanker(ITargetRanker baseRanker, int rankBoost)
        {
            Assert.IsNotNull(baseRanker);

            _baseRanker = baseRanker;
            _rankBoost = rankBoost;
        }

        public int RankTarget(ITarget target)
        {
            return _baseRanker.RankTarget(target) + _rankBoost;
        }
    }
}