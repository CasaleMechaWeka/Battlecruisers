using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.Common.BuildingDetails
{
    public class ComparableBuildingDetailsController : ComparableBuildableDetailsController<IBuilding>
	{
        private ISpriteProvider _spriteProvider;
        private Image _slotImage;

        public void Initialise(ISpriteProvider spriteProvider)
        {
            base.Initialise();

            Assert.IsNotNull(spriteProvider);
            _spriteProvider = spriteProvider;

            _slotImage = transform.FindNamedComponent<Image>("SlotType");
        }
		
        public override void ShowItemDetails(IBuilding item, IBuilding itemToCompareTo = null)
        {
            base.ShowItemDetails(item, itemToCompareTo);

            _slotImage.sprite = _spriteProvider.GetSlotSprite(_item.SlotType).Sprite;
        }
    }
}
