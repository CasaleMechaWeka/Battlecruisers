using BattleCruisers.Buildables;

namespace BattleCruisers.Targets.TargetProcessors.Ranking
{
	public class EqualTargetRanker : ITargetRanker
	{
		public int RankTarget(ITarget target)
		{
			return 1;
		}
	}
}
