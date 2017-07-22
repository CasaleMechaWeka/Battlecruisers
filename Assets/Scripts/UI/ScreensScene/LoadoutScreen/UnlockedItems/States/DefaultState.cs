using UnityEngine;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.UnlockedItems.States
{
    public class DefaultState<TItem> : UnlockedItemState<TItem> where TItem : IComparableItem
	{
		private readonly IItemsRow<TItem> _itemsRow;

		protected override Color BackgroundColour { get { return BaseItem<TItem>.Colors.DEFAULT; } }

		public DefaultState(IItemsRow<TItem> itemsRow, UnlockedItem<TItem> item)
			: base(item)
		{
			_itemsRow = itemsRow;
		}

		public override void HandleSelection()
		{
			_item.IsItemInLoadout = _itemsRow.SelectUnlockedItem(_item);
		}
	}
}
