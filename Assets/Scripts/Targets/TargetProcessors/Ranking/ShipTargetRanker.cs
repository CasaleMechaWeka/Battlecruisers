using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetProviders;

namespace BattleCruisers.Targets.TargetProcessors.Ranking
{
    /// <summary>
    /// Prioritises targets by:
    /// 1. Anti sea
    /// 2. By target value
    /// </summary>
    public class ShipTargetRanker : BaseTargetRanker
    {
        private const int ANTI_SHIP_BONUS = 30;

        public ShipTargetRanker(ITargetProvider userChosenTargetProvider)
            : base(userChosenTargetProvider)
        {
            _attackCapabilityToBonus[TargetType.Ships] = ANTI_SHIP_BONUS;
        }
    }
}
