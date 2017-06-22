using BattleCruisers.Buildables;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.LoadoutItems;
using System;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.BuildableDetails.States
{
	public class SelectedState<TItem> : BaseState<TItem> where TItem : IComparableItem
	{
		private LoadoutItem<TItem> _selectedItem;

		public SelectedState(IItemDetailsManager<TItem> itemDetailsManager, LoadoutItem<TItem> selectedItem)
			: base(itemDetailsManager)
		{
			_selectedItem = selectedItem;
		}

		public override IItemDetailsState<TItem> CompareSelectedItem()
		{
			_itemDetailsManager.HideItemDetails();
			_selectedItem.ShowSelectedFeedback = true;	
			return new ReadyToCompareState<TItem>(_itemDetailsManager, _selectedItem);
		}
	}
}

