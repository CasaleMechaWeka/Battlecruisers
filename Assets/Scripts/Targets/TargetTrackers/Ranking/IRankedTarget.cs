using BattleCruisers.Buildables;

namespace BattleCruisers.Targets.TargetTrackers.Ranking
{
    public interface IRankedTarget
    {
        /// <summary>
        /// Bigger numbers indicate higher priority.
        /// 
        /// The lowest priority is 0.  There is no upper limit.
        /// </summary>
        int Rank { get; }

        ITarget Target { get; }
    }
}
