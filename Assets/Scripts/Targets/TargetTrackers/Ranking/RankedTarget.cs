using BattleCruisers.Buildables;
using BattleCruisers.Utils;

namespace BattleCruisers.Targets.TargetTrackers.Ranking
{
    public class RankedTarget
    {
        public ITarget Target { get; private set; }

        /// <summary>
        /// Bigger numbers indicate higher priority.
        /// 
        /// The lowest priority is 0.  There is no upper limit.
        /// </summary>
        public int Rank { get; private set; }

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