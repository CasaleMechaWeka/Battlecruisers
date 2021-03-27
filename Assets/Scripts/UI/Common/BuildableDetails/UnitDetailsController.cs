using BattleCruisers.Buildables.Units;
using BattleCruisers.UI.Common.BuildableDetails.Stats;

namespace BattleCruisers.UI.Common.BuildableDetails
{
    public class UnitDetailsController : ItemDetails<IUnit>
    {
        protected override StatsController<IUnit> GetStatsController()
        {
            return GetComponentInChildren<UnitCompactStatsController>();
        }
    }
}
