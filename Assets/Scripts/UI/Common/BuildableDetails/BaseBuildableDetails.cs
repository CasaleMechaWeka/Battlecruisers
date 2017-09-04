using BattleCruisers.Buildables;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Fetchers;
using UnityEngine.UI;

namespace BattleCruisers.UI.Common.BuildingDetails
{
    public abstract class BaseBuildableDetails<TItem> : ItemDetails<TItem> where TItem : class, IBuildable
	{
		private ISpriteFetcher _spriteFetcher;

        public Image slotImage;
		
		public void Initialise(ISpriteFetcher spriteFetcher)
		{
            base.Initialise();

			_spriteFetcher = spriteFetcher;
			Hide();
		}

		public override void ShowItemDetails(TItem item, TItem itemToCompareTo = null)
		{
            base.ShowItemDetails(item, itemToCompareTo);

			bool hasSlot = _item.SlotType != SlotType.None;
			if (hasSlot)
			{
				slotImage.sprite = _spriteFetcher.GetSlotSprite(_item.SlotType);
			}
			slotImage.gameObject.SetActive(hasSlot);
		}
	}
}
