using BattleCruisers.Buildables;
using System;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails.States
{
	public class ReadyToCompareState<TItem> : BaseState<TItem> where TItem : IComparableItem
	{
		private IItem<TItem> _itemToCompare;

		public ReadyToCompareState(IItemDetailsManager<TItem> itemDetailsManager, IItem<TItem> itemToCompare)
			: base(itemDetailsManager)
		{
			_itemToCompare = itemToCompare;
		}

		public override IItemDetailsState<TItem> SelectItem(IItem<TItem> selectedItem)
		{
			_itemToCompare.ShowSelectedFeedback = false;
			_itemDetailsManager.CompareItemDetails(_itemToCompare.Item, selectedItem.Item);
			return new ComparingState<TItem>(_itemDetailsManager);
		}
	}
}

