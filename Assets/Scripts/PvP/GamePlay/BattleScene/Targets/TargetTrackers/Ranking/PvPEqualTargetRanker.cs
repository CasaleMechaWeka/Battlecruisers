using BattleCruisers.Buildables;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers.Ranking
{
    public class PvPEqualTargetRanker : IPvPTargetRanker
    {
        public int RankTarget(ITarget target)
        {
            return 1;
        }
    }
}
