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

        public ITargetRanker EqualTargetRanker { get; private set; }
        public ITargetRanker ShipTargetRanker { get; private set; }
        public ITargetRanker OffensiveBuildableTargetRanker { get; private set; }

        public ITargetRanker CreateBoostedRanker(ITargetRanker baseRanker, int rankBoost)
        {
            return new BoostedRanker(baseRanker, rankBoost);
        }
    }
}
