using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers.Ranking
{
    public class PvPEqualTargetRanker : IPvPTargetRanker
    {
        public int RankTarget(IPvPTarget target)
        {
            return 1;
        }
    }
}
