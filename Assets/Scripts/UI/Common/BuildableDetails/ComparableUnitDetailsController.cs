using BattleCruisers.Buildables.Units;
using BattleCruisers.UI.Common.BuildingDetails.Stats;

namespace BattleCruisers.UI.Common.BuildingDetails
{
    public class ComparableUnitDetailsController : ComparableBuildableDetailsController<IUnit>
	{
        protected override StatsController<IUnit> StatsController { get { return unitStatsController; } }

        // FELIX  Can be assigned programmatically?
        public UnitStatsController unitStatsController;
	}
}
