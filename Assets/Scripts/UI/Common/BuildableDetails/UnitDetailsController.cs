using BattleCruisers.Buildables.Units;
using BattleCruisers.UI.Common.BuildingDetails.Stats;

namespace BattleCruisers.UI.Common.BuildingDetails
{
    public class UnitDetailsController : BuildableDetailsController<IUnit>
    {
        // FELIX  Handle unit speed!

        protected override StatsController<IUnit> GetStatsController()
        {
            return GetComponent<UnitStatsController>();
        }
    }
}
