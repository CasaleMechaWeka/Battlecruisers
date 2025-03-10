using BattleCruisers.Buildables.Units;
using BattleCruisers.UI.Common.BuildableDetails.Stats;

namespace BattleCruisers.UI.Common.BuildableDetails
{
    public class ComparableUnitDetailsController : ComparableItemDetails<IUnit>
	{
        protected override StatsController<IUnit> GetStatsController()
        {
            return GetComponentInChildren<UnitStatsController>();
        }
	}
}
