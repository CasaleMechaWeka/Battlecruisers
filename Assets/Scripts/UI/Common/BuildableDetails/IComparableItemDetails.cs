using BattleCruisers.Buildables;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.LoadoutItems;
using System;

namespace BattleCruisers.UI.Common.BuildingDetails
{
	public interface IComparableItemDetails<TItem> where TItem : IComparableItem
	{
		void ShowItemDetails(TItem item, TItem itemToCompareTo = default(TItem));
		void Hide();
	}
}
