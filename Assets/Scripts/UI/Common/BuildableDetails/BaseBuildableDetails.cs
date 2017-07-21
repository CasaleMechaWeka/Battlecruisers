using BattleCruisers.Buildables;
using BattleCruisers.Cruisers;
using BattleCruisers.Fetchers;
using UnityEngine.UI;

namespace BattleCruisers.UI.Common.BuildingDetails
{
    public abstract class BaseBuildableDetails<TItem> : ItemDetails<TItem> where TItem : Buildable
	{
		private ISpriteFetcher _spriteFetcher;

        public Image slotImage;
		
		public void Initialise(ISpriteFetcher spriteFetcher)
		{
			_spriteFetcher = spriteFetcher;
			Hide();
		}

		public override void ShowItemDetails(TItem item, TItem itemToCompareTo = null)
		{
            base.ShowItemDetails(item, itemToCompareTo);

			bool hasSlot = _item.slotType != SlotType.None;
			if (hasSlot)
			{
				slotImage.sprite = _spriteFetcher.GetSlotSprite((SlotType)_item.slotType);
			}
			slotImage.gameObject.SetActive(hasSlot);
		}
	}
}
