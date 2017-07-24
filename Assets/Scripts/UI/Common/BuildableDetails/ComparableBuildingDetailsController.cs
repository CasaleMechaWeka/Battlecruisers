using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.Common.BuildingDetails.Stats;
using UnityEngine.EventSystems;

namespace BattleCruisers.UI.Common.BuildingDetails
{
    public class ComparableBuildingDetailsController : BaseBuildableDetails<IBuilding>, IPointerClickHandler
	{
        protected override StatsController<IBuilding> StatsController { get { return buildingStatsController; } }

        public BuildingStatsController buildingStatsController;
        
        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
		{
			// Empty.  Simply here to eat event so parent does not receive event.
		}
	}
}
