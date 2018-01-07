using BattleCruisers.Buildables;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Utils.Fetchers;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.Common.BuildingDetails
{
    public abstract class BaseBuildableDetails<TItem> : ItemDetails<TItem> where TItem : class, IBuildable
	{
        private ISpriteProvider _spriteProvider;

        public Image slotImage;
		
        public void Initialise(ISpriteProvider spriteProvider)
		{
            base.Initialise();

            Assert.IsNotNull(spriteProvider);
            _spriteProvider = spriteProvider;
		}

		public override void ShowItemDetails(TItem item, TItem itemToCompareTo = null)
		{
            base.ShowItemDetails(item, itemToCompareTo);

			bool hasSlot = _item.SlotType != SlotType.None;
			if (hasSlot)
			{
                slotImage.sprite = _spriteProvider.GetSlotSprite(_item.SlotType).Sprite;
			}
			slotImage.gameObject.SetActive(hasSlot);
		}
	}
}
