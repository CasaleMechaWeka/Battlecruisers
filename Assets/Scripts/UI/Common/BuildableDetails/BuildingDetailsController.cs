using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.UI.Common.BuildingDetails.Stats;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.Common.BuildingDetails
{
    public class BuildingDetailsController : BuildableDetailsController<IBuilding>
    {
        private ISpriteProvider _spriteProvider;
        private Image _slotImage;

        public void Initialise(ISpriteProvider spriteProvider, IDroneManager droneManager, IRepairManager repairManager)
        {
            base.Initialise(droneManager, repairManager);

            Assert.IsNotNull(spriteProvider);
            _spriteProvider = spriteProvider;

            _slotImage = transform.FindNamedComponent<Image>("SlotType");
        }

        protected override StatsController<IBuilding> GetStatsController()
        {
            return GetComponent<BuildingStatsController>();
        }

        // FELIX  Avoid dulpicate code with ComparableBuildingDetailsController.  Perhaps a SlotTypeController?
        // Handles:  SlotImage and spriteProvider
        public override void ShowBuildableDetails(IBuilding buildable, bool allowDelete)
        {
            base.ShowBuildableDetails(buildable, allowDelete);

            _slotImage.sprite = _spriteProvider.GetSlotSprite(_item.SlotType).Sprite;
        }
    }
}
