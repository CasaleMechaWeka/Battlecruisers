using BattleCruisers.Buildables.Units;
using BattleCruisers.UI.Common.BuildingDetails.Stats;

namespace BattleCruisers.UI.Common.BuildingDetails
{
    public class ComparableUnitDetailsController : ComparableBuildableDetailsController<IUnit>
	{
        // FELIX  Retrieve programamtically?  Then doesn't need to be public :)
		public UnitStatsController unitStatsController;
        protected override StatsController<IUnit> StatsController { get { return unitStatsController; } }
	}
}
