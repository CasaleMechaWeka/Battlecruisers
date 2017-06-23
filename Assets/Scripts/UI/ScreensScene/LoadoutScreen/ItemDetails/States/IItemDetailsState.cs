using System;
using BattleCruisers.Buildables;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.LoadoutItems;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails.States
{
	public interface IItemDetailsState<TItem> where TItem : IComparableItem
	{
		IItemDetailsState<TItem> SelectItem(IItem<TItem> selectedItem);
		IItemDetailsState<TItem> CompareSelectedItem();
		IItemDetailsState<TItem> Dismiss();
	}
}
