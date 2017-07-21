using BattleCruisers.Cruisers;
using BattleCruisers.UI.Common.BuildingDetails.Stats;

namespace BattleCruisers.UI.Common.BuildingDetails
{
    public class CruiserDetailsController : ItemDetails<Cruiser>
	{
        protected override StatsController<Cruiser> StatsController { get { return cruiserStatsController; } }
		
        public CruiserStatsController cruiserStatsController;
    }
}
