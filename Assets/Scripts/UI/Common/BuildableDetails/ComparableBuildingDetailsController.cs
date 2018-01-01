using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.Common.BuildingDetails.Stats;

namespace BattleCruisers.UI.Common.BuildingDetails
{
    public class ComparableBuildingDetailsController : ComparableBuildableDetailsController<IBuilding>
	{
        protected override StatsController<IBuilding> StatsController { get { return buildingStatsController; } }

        public BuildingStatsController buildingStatsController;
	}
}
