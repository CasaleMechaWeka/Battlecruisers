using System;
using BattleCruisers.Buildables;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.LoadoutItems;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails.States
{
	public interface IItemDetailsState<TItem> where TItem : IComparableItem
	{
		IItemDetailsState<TItem> SelectItem(LoadoutItem<TItem> selectedItem);
		IItemDetailsState<TItem> CompareSelectedItem();
		IItemDetailsState<TItem> Dismiss();
	}

	public abstract class BaseState<TItem> : IItemDetailsState<TItem> where TItem : IComparableItem
	{
		protected IItemDetailsManager<TItem> _itemDetailsManager;

		public BaseState(IItemDetailsManager<TItem> itemDetailsManager)
		{
			_itemDetailsManager = itemDetailsManager;
		}

		public virtual IItemDetailsState<TItem> SelectItem(LoadoutItem<TItem> selectedItem)
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

