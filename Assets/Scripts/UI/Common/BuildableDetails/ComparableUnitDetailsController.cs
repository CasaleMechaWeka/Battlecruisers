using BattleCruisers.Buildables.Units;
using BattleCruisers.UI.Common.BuildingDetails.Stats;
using UnityEngine.EventSystems;

namespace BattleCruisers.UI.Common.BuildingDetails
{
    public class ComparableUnitDetailsController : BaseBuildableDetails<IUnit>, IPointerClickHandler
	{
        protected override StatsController<IUnit> StatsController { get { return unitStatsController; } }

        // FELIX  Can be assigned programmatically?
        public UnitStatsController unitStatsController;

        // FELIX  Avoid duplicate code with ComparableBuildingDetailsController?
        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
		{
			// Empty.  Simply here to eat event so parent does not receive event.
		}
	}
}
