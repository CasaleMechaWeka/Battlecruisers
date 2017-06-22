using BattleCruisers.Buildables;
using System;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.BuildableDetails
{
	public class DismissedState<TItem> : BaseState<TItem> where TItem : IComparableItem
	{
		public DismissedState(ItemDetailsManager<TItem> itemsDetailsManager)
			: base(itemsDetailsManager) { }

		public override IItemDetailsState<TItem> SelectBuildable(LoadoutItem<TItem> selectedItem)
		{
			_itemDetailsManager.ShowItemDetails(selectedItem.Item);
			return new SelectedState(_itemDetailsManager, selectedItem);
		}

		public override IItemDetailsState<TItem> Dismiss()
		{
			throw new InvalidProgramException();
		}
	}
}

