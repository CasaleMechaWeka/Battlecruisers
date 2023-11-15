using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers.Ranking
{
    /// <summary>
    /// Prioritises targets by:
    /// 1. Anti air
    /// 2. Anti cruiser
    /// 3. By target value
    /// </summary>
    public class PvPBomberTargetRanker : PvPBaseTargetRanker
    {
        private const int ANTI_AIR_BONUS = 60;
        private const int ANTI_CRUISER_BONUS = 30;

        public PvPBomberTargetRanker()
        {
            _attackCapabilityToBonus[PvPTargetType.Aircraft] = ANTI_AIR_BONUS;
            _attackCapabilityToBonus[PvPTargetType.Cruiser] = ANTI_CRUISER_BONUS;
        }
    }
}
