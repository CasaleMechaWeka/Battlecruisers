using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetProviders;

namespace BattleCruisers.Targets.TargetProcessors.Ranking
{
    /// <summary>
    /// Prioritises targets by:
    /// 1. Anti air
    /// 2. Anti cruiser
    /// 3. By target value
    /// </summary>
    public class BomberTargetRanker : BaseTargetRanker
	{
		private const int ANTI_AIR_BONUS = 60;
		private const int ANTI_CRUISER_BONUS = 30;

		public BomberTargetRanker(ITargetProvider userChosenTargetProvider)
            : base(userChosenTargetProvider)
		{
			_attackCapabilityToBonus[TargetType.Aircraft] = ANTI_AIR_BONUS;
			_attackCapabilityToBonus[TargetType.Cruiser] = ANTI_CRUISER_BONUS;
		}
	}
}
