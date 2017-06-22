using BattleCruisers.Buildables;
using System;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.BuildableDetails
{
	public class SelectedState<TItem> : BaseState<TItem> where TItem : IComparableItem
	{
		private LoadoutItem<TItem> _selectedItem;

		public SelectedState(ItemDetailsManager<TItem> itemDetailsManager, LoadoutItem<TItem> selectedItem)
			: base(itemDetailsManager)
		{
			_selectedItem = selectedItem;
		}

		public override IItemDetailsState<TItem> CompareSelectedBuildable()
		{
			_itemDetailsManager.HideItemDetails();
			_selectedItem.ShowSelectedFeedback = true;	
			return new ReadyToCompareState<TItem>(_itemDetailsManager, _selectedItem);
		}
	}
}

