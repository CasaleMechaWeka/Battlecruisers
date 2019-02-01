using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Comparisons;
using System;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails.States
{
    public class ReadyToCompareState<TItem> : BaseState<TItem> where TItem : IComparableItem
	{
		private readonly IItem<TItem> _itemToCompare;

		public ReadyToCompareState(
            IItemDetailsManager<TItem> itemDetailsManager, 
            IItemStateManager itemStateManager,
            IItem<TItem> itemToCompare)
            : base(itemDetailsManager, itemStateManager)
		{
			_itemToCompare = itemToCompare;
            _itemStateManager.HandleDetailsManagerReadyToCompare(_itemToCompare.Type);
		}

		public override IItemDetailsState<TItem> SelectItem(IItem<TItem> selectedItem)
		{
			_itemToCompare.ShowSelectedFeedback = false;
			_itemDetailsManager.CompareItemDetails(_itemToCompare.Item, selectedItem.Item);
			return new ComparingState<TItem>(_itemDetailsManager, _itemStateManager);
		}

        public override IItemDetailsState<TItem> Dismiss()
        {
            throw new InvalidProgramException();
        }
	}
}
