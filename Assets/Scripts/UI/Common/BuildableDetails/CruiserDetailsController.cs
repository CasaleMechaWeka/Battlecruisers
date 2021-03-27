using BattleCruisers.Cruisers;
using BattleCruisers.UI.Common.BuildableDetails.Stats;

namespace BattleCruisers.UI.Common.BuildableDetails
{
    public class CruiserDetailsController : ItemDetails<ICruiser>
    {
        protected override StatsController<ICruiser> GetStatsController()
        {
            return GetComponentInChildren<CruiserCompactStatsController>();
        }
    }
}