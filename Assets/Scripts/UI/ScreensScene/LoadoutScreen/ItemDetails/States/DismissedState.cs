using BattleCruisers.Buildables;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.LoadoutItems;
using System;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails.States
{
	public class DismissedState<TItem> : BaseState<TItem> where TItem : IComparableItem
	{
		public DismissedState(IItemDetailsManager<TItem> itemsDetailsManager)
			: base(itemsDetailsManager) { }

		public override IItemDetailsState<TItem> SelectItem(LoadoutItem<TItem> selectedItem)
		{
			_itemDetailsManager.ShowItemDetails(selectedItem.Item);
			return new SelectedState<TItem>(_itemDetailsManager, selectedItem);
		}

		public override IItemDetailsState<TItem> Dismiss()
		{
			throw new InvalidProgramException();
		}
	}
}

