using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Comparisons;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails.States
{
    public class SelectedState<TItem> : BaseState<TItem> where TItem : IComparableItem
	{
		private readonly IItem<TItem> _selectedItem;

		public SelectedState(
            IItemDetailsManager<TItem> itemDetailsManager, 
            IItemStateManager itemStateManager,
            IItem<TItem> selectedItem)
            : base(itemDetailsManager, itemStateManager)
		{
			_selectedItem = selectedItem;
		}

		public override IItemDetailsState<TItem> CompareSelectedItem()
		{
			_itemDetailsManager.HideItemDetails();
			_selectedItem.ShowSelectedFeedback = true;	
            return new ReadyToCompareState<TItem>(_itemDetailsManager, _itemStateManager, _selectedItem);
		}
	}
}
