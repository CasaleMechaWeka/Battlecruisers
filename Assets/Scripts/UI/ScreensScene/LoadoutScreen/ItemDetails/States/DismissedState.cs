using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Comparisons;
using System;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails.States
{
    public class DismissedState<TItem> : BaseState<TItem> where TItem : IComparableItem
	{
        public DismissedState(IItemDetailsManager<TItem> itemsDetailsManager, IItemStateManager itemStateManager)
            : base(itemsDetailsManager, itemStateManager) 
        {
            _itemStateManager.HandleDetailsManagerDismissed();
        }

		public override IItemDetailsState<TItem> SelectItem(IItem<TItem> selectedItem)
		{
			_itemDetailsManager.ShowItemDetails(selectedItem.Item);
			return new SelectedState<TItem>(_itemDetailsManager, _itemStateManager, selectedItem);
		}

		public override IItemDetailsState<TItem> Dismiss()
		{
			throw new InvalidProgramException();
		}
	}
}
