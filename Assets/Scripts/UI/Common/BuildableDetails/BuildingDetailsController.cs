using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.Common.BuildingDetails
{
    public class BuildingDetailsController : BuildableDetailsController
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

        // FELIX  Avoid dulpicate code with ComparableBuildingDetailsController.  Perhaps a SlotTypeController?
        // Handles:  SlotImage and spriteProvider
        public override void ShowBuildableDetails(Buildables.IBuildable buildable, bool allowDelete)
        {
            base.ShowBuildableDetails(buildable, allowDelete);

            _slotImage.sprite = _spriteProvider.GetSlotSprite(_item.SlotType).Sprite;
        }
    }
}
