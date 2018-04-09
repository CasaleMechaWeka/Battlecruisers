using BattleCruisers.Buildables.Units;
using BattleCruisers.UI.Common.BuildableDetails.Stats;

namespace BattleCruisers.UI.Common.BuildableDetails
{
    public class UnitDetailsController : BuildableDetailsController<IUnit>
    {
        protected override StatsController<IUnit> GetStatsController()
        {
            return GetComponentInChildren<UnitStatsController>();
        }
    }
}
