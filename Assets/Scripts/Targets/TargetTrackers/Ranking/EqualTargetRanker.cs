using BattleCruisers.Buildables;

namespace BattleCruisers.Targets.TargetTrackers.Ranking
{
	public class EqualTargetRanker : ITargetRanker
	{
		public int RankTarget(ITarget target)
		{
			return 1;
		}
	}
}
