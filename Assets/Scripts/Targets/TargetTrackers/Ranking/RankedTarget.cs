using BattleCruisers.Buildables;
using BattleCruisers.Utils;

namespace BattleCruisers.Targets.TargetTrackers.Ranking
{
    public class RankedTarget
    {
        public ITarget Target { get; }

        /// <summary>
        /// Bigger numbers indicate higher priority.
        /// 
        /// The lowest priority is 0.  There is no upper limit.
        /// </summary>
        public int Rank { get; }

        public RankedTarget(ITarget target, int rank)
        {
            Target = target;
            Rank = rank;
        }

        public override bool Equals(object obj)
        {
            RankedTarget other = obj as RankedTarget;
            return
                other != null
                && ReferenceEquals(Target, other.Target)
                && Rank == other.Rank;
        }

        public override int GetHashCode()
        {
            return this.GetHashCode(Target, Rank);
        }
    }
}