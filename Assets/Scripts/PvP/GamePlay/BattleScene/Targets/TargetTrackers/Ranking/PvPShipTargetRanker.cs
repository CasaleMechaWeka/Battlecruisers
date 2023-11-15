using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers.Ranking
{
    /// <summary>
    /// Prioritises targets by:
    /// 1. Anti sea
    /// 2. By target value
    /// </summary>
    public class PvPShipTargetRanker : PvPBaseTargetRanker
    {
        private const int ANTI_SHIP_BONUS = 30;

        public PvPShipTargetRanker()
        {
            _attackCapabilityToBonus[PvPTargetType.Ships] = ANTI_SHIP_BONUS;
        }
    }
}
