using BattleCruisers.Targets.TargetTrackers.Ranking;

namespace BattleCruisers.Targets.Factories
{
    public class TargetRankerFactory : ITargetRankerFactory
    {
        public TargetRankerFactory()
		{
            EqualTargetRanker = new EqualTargetRanker();
            ShipTargetRanker = new ShipTargetRanker();
            OffensiveBuildableTargetRanker = new OffensiveBuildableTargetRanker();
		}

        public ITargetRanker EqualTargetRanker { get; }
        public ITargetRanker ShipTargetRanker { get; }
        public ITargetRanker OffensiveBuildableTargetRanker { get; }

        public ITargetRanker CreateBoostedRanker(ITargetRanker baseRanker, int rankBoost)
        {
            return new BoostedRanker(baseRanker, rankBoost);
        }
    }
}
