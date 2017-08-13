using BattleCruisers.Buildables;

namespace BattleCruisers.Targets.TargetProcessors.Ranking
{
    public interface ITargetRanker
	{
		/// <returns>
		/// Ranks the target according to priority.
		/// 
		/// Bigger numbers indicate higher priority.
		/// 
		/// The lowest priority is 0.  There is no upper limit.
		/// </returns>
		int RankTarget(ITarget target);
	}
}
