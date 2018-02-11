using BattleCruisers.Cruisers;
using BattleCruisers.UI.Common.BuildingDetails.Stats;

namespace BattleCruisers.UI.Common.BuildingDetails
{
    public class ComparableCruiserDetailsController : ComparableItemDetails<ICruiser>
    {
        protected override StatsController<ICruiser> GetStatsController()
        {
            return GetComponentInChildren<CruiserStatsController>();
        }
    }
}
