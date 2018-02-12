using BattleCruisers.Buildables.Units;
using BattleCruisers.UI.Common.BuildingDetails.Stats;

namespace BattleCruisers.UI.Common.BuildingDetails
{
    public class UnitDetailsController : BuildableDetailsController<IUnit>
    {
        protected override StatsController<IUnit> GetStatsController()
        {
            return GetComponentInChildren<UnitStatsController>();
        }
    }
}
