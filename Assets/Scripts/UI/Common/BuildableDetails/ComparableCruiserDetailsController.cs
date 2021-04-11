using BattleCruisers.Cruisers;
using BattleCruisers.UI.Common.BuildableDetails.Stats;

namespace BattleCruisers.UI.Common.BuildableDetails
{
    public class ComparableCruiserDetailsController : ComparableItemDetails<ICruiser>
    {
        protected override StatsController<ICruiser> GetStatsController()
        {
            return GetComponentInChildren<CruiserCompactStatsController>();
        }
    }
}
