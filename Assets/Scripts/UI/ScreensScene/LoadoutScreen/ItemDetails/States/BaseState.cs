using System;
using BattleCruisers.Buildables;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails.States
{
	public abstract class BaseState<TItem> : IItemDetailsState<TItem> where TItem : IComparableItem
	{
		protected IItemDetailsManager<TItem> _itemDetailsManager;

		public virtual bool IsInReadyToCompareState { get { return false; } }

		public BaseState(IItemDetailsManager<TItem> itemDetailsManager)
		{
			_itemDetailsManager = itemDetailsManager;
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
			return new DismissedState<TItem>(_itemDetailsManager);
		}
	}
}
