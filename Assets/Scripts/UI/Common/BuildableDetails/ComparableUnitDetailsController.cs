using BattleCruisers.Buildables.Units;
using BattleCruisers.UI.Common.BuildingDetails.Stats;

namespace BattleCruisers.UI.Common.BuildingDetails
{
    public class ComparableUnitDetailsController : ComparableItemDetails<IUnit>
	{
        protected override StatsController<IUnit> GetStatsController()
        {
            return GetComponentInChildren<UnitStatsController>();
        }
	}
}
