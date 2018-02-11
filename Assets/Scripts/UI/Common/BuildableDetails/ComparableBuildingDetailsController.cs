using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.Common.BuildingDetails.Buttons;
using BattleCruisers.UI.Common.BuildingDetails.Stats;
using BattleCruisers.Utils.Fetchers;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Common.BuildingDetails
{
    public class ComparableBuildingDetailsController : ComparableItemDetails<IBuilding>
	{
        private SlotTypeController _slotType;

        public void Initialise(ISpriteProvider spriteProvider)
        {
            base.Initialise();

            _slotType = GetComponentInChildren<SlotTypeController>();
            Assert.IsNotNull(_slotType);
            _slotType.Initialise(spriteProvider);
        }

        protected override StatsController<IBuilding> GetStatsController()
        {
            return GetComponentInChildren<BuildingStatsController>();
        }
		
        public override void ShowItemDetails(IBuilding item, IBuilding itemToCompareTo = null)
        {
            base.ShowItemDetails(item, itemToCompareTo);
            _slotType.SlotType = item.SlotType;
        }
    }
}
