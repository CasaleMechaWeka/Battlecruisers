using BattleCruisers.Buildables;
using UnityEngine.Assertions;

namespace BattleCruisers.Targets.TargetProcessors.Ranking
{
    // FELIX  Test :)
    public class BoostedRanker : ITargetRanker
    {
        private readonly ITargetRanker _baseRanker;
        private readonly int _rankBoost;

        public BoostedRanker(ITargetRanker baseRanker, int rankBoost)
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