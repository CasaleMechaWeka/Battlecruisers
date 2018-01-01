using BattleCruisers.Buildables.Units;
using BattleCruisers.UI.Common.BuildingDetails.Stats;

namespace BattleCruisers.UI.Common.BuildingDetails
{
    public class ComparableUnitDetailsController : ComparableBuildableDetailsController<IUnit>
	{
		public UnitStatsController unitStatsController;
        protected override StatsController<IUnit> StatsController { get { return unitStatsController; } }
	}
}
