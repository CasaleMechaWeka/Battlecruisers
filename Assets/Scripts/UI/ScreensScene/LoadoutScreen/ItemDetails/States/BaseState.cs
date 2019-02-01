using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Comparisons;
using BattleCruisers.Utils;
using System;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails.States
{
    public abstract class BaseState<TItem> : IItemDetailsState<TItem> where TItem : IComparableItem
	{
		protected readonly IItemDetailsManager<TItem> _itemDetailsManager;
        protected readonly IItemStateManager _itemStateManager;

        protected BaseState(
            IItemDetailsManager<TItem> itemDetailsManager,
            IItemStateManager itemStateManager)
		{
            Helper.AssertIsNotNull(itemDetailsManager, itemStateManager);

			_itemDetailsManager = itemDetailsManager;
            _itemStateManager = itemStateManager;
		}

		public virtual IItemDetailsState<TItem> SelectItem(IItem<TItem> selectedItem)
		{
			throw new InvalidProgramException();
		}

		public virtual IItemDetailsState<TItem> CompareSelectedItem()
		{
			throw new InvalidProgramException();
		}

		public virtual IItemDetailsState<TItem> Dismiss()
		{
			_itemDetailsManager.HideItemDetails();
			return new DismissedState<TItem>(_itemDetailsManager, _itemStateManager);
		}
	}
}
