using BattleCruisers.Buildables;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.LoadoutItems;
using System;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.BuildableDetails.States
{
	public class ReadyToCompareState<TItem> : BaseState<TItem> where TItem : IComparableItem
	{
		private LoadoutItem<TItem> _itemToCompare;

		public ReadyToCompareState(IItemDetailsManager<TItem> itemDetailsManager, LoadoutItem<TItem> itemToCompare)
			: base(itemDetailsManager)
		{
			_itemToCompare = itemToCompare;
		}

		public override IItemDetailsState<TItem> SelectItem(LoadoutItem<TItem> selectedItem)
		{
			_itemToCompare.ShowSelectedFeedback = false;
			_itemDetailsManager.CompareItemDetails(_itemToCompare.Item, selectedItem.Item);
			return new ComparingState<TItem>(_itemDetailsManager);
		}
	}
}

