using BattleCruisers.Buildables;
using System;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails.States
{
	public class SelectedState<TItem> : BaseState<TItem> where TItem : IComparableItem
	{
		private IItem<TItem> _selectedItem;

		public SelectedState(IItemDetailsManager<TItem> itemDetailsManager, IItem<TItem> selectedItem)
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

