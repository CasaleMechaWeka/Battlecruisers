using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers.Ranking
{
    public class PvPRankedTarget
    {
        public IPvPTarget Target { get; }

        /// <summary>
        /// Bigger numbers indicate higher priority.
        /// 
        /// The lowest priority is 0.  There is no upper limit.
        /// </summary>
        public int Rank { get; }

        public PvPRankedTarget(IPvPTarget target, int rank)
        {
            Target = target;
            Rank = rank;
        }

        public override bool Equals(object obj)
        {
            PvPRankedTarget other = obj as PvPRankedTarget;
            return
                other != null
                && ReferenceEquals(Target, other.Target)
                && Rank == other.Rank;
        }

        public override int GetHashCode()
        {
            return this.GetHashCode(Target, Rank);
        }

        public override string ToString()
        {
            return base.ToString() + $"Target: {Target}";
        }
    }
}