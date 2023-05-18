using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers.Ranking
{
    /// <summary>
    /// Prioritises targets by:
    /// 1. Anti cruiser
    /// 2. By target value
    /// </summary>
    public class PvPOffensiveBuildableTargetRanker : PvPBaseTargetRanker
    {
        private const int ANTI_CRUISER_BONUS = 30;

        public PvPOffensiveBuildableTargetRanker()
        {
            _attackCapabilityToBonus[PvPTargetType.Cruiser] = ANTI_CRUISER_BONUS;
        }
    }
}
