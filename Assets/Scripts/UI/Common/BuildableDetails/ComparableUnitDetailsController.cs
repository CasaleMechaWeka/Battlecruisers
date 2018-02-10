using BattleCruisers.Buildables.Units;
using BattleCruisers.UI.Common.BuildingDetails.Stats;

namespace BattleCruisers.UI.Common.BuildingDetails
{
    public class ComparableUnitDetailsController : ComparableBuildableDetailsController<IUnit>
	{
        protected override StatsController<IUnit> GetStatsController()
        {
            return GetComponent<UnitStatsController>();
        }
	}
}
