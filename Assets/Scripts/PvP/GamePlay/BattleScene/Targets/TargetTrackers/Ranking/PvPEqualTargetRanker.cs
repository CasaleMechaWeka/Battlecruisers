using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetTrackers.Ranking;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers.Ranking
{
    public class PvPEqualTargetRanker : ITargetRanker
    {
        public int RankTarget(ITarget target)
        {
            return 1;
        }
    }
}
