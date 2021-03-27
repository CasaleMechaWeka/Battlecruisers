using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.Common.BuildableDetails.Stats;

namespace BattleCruisers.UI.Common.BuildableDetails
{
    public class BuildingDetailsController : ItemDetails<IBuilding>
    {
        protected override StatsController<IBuilding> GetStatsController()
        {
            return GetComponentInChildren<BuildingCompactStatsController>();
        }
    }
}
