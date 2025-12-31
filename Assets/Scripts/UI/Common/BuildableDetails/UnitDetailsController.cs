using BattleCruisers.Buildables.Units;
using BattleCruisers.UI.Common.BuildableDetails.Stats;

namespace BattleCruisers.UI.Common.BuildableDetails
{
    public class UnitDetailsController : ItemDetails<IUnit>
    {
        protected override StatsController<IUnit> GetStatsController()
        {
            return GetComponentInChildren<UnitStatsController>();
        }
        public override UnitVariantDetailController GetUnitVariantDetailController() { return GetComponent<UnitVariantDetailController>(); }
    }
}
