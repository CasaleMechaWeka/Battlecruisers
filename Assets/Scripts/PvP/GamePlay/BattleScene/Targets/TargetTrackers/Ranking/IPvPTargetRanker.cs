using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers.Ranking
{
    public interface IPvPTargetRanker
    {
        /// <returns>
        /// Ranks the target according to priority.
        /// 
        /// Bigger numbers indicate higher priority.
        /// 
        /// The lowest priority is 0.  There is no upper limit.
        /// </returns>
        int RankTarget(IPvPTarget target);
    }
}