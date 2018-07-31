using BattleCruisers.Buildables;

namespace BattleCruisers.Targets.TargetProcessors
{
    public class RankedTarget
    {
        public ITarget Target { get; private set; }
        public int Rank { get; private set; }

        public RankedTarget(ITarget target, int rank)
        {
            Target = target;
            Rank = rank;
        }
    }
}