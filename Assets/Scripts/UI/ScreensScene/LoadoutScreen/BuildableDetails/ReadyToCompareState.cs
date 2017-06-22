using BattleCruisers.Buildables;
using System;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.BuildableDetails
{
	public class ReadyToCompareState<TItem> : BaseState<TItem> where TItem : IComparableItem
	{
		private LoadoutItem<TItem> _itemToCompare;

		public ReadyToCompareState(ItemDetailsManager<TItem> itemDetailsManager, LoadoutItem<TItem> itemToCompare)
			: base(itemDetailsManager)
		{
			_itemToCompare = itemToCompare;
		}

		public override IItemDetailsState<TItem> SelectBuildable(LoadoutItem<TItem> selectedItem)
		{
			_itemToCompare.ShowSelectedFeedback = false;
			_itemDetailsManager.CompareItemDetails(_itemToCompare.Item, selectedItem.Item);
			return new ComparingState<TItem>(_itemDetailsManager);
		}
	}
}

