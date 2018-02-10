using BattleCruisers.Cruisers;
using BattleCruisers.UI.Common.BuildingDetails.Stats;

namespace BattleCruisers.UI.Common.BuildingDetails
{
    public class CruiserDetailsController : ItemDetails<ICruiser>
    {
        protected override StatsController<ICruiser> GetStatsController()
        {
            return GetComponent<CruiserStatsController>();
        }
    }
}
