using BattleCruisers.Buildables;

namespace BattleCruisers.Targets.TargetProcessors.Ranking
{
    public class RankedTarget : IRankedTarget
    {
        public int Rank { get; private set; }
        public ITarget Target { get; private set; }

        public RankedTarget(int rank, ITarget target)
        {
            Rank = rank;
            Target = target;
        }
    }
}
