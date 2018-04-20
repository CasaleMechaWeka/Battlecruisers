using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using UnityEngine;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.UnlockedItems.States
{
    public class HighlightedState<TItem> : UnlockedItemState<TItem> where TItem : IComparableItem
	{
		private readonly IItemDetailsManager<TItem> _itemDetailsManager;

        protected override Color BackgroundColour { get { return BaseItem<TItem>.Colors.HIGHLIGHTED; } }

		public HighlightedState(IItemDetailsManager<TItem> itemDetailsManager, UnlockedItem<TItem> item)
			: base(item)
		{
			_itemDetailsManager = itemDetailsManager;
		}

		public override void HandleSelection()
		{
			_itemDetailsManager.SelectItem(_item);
		}
	}
}
