using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.LoadoutItems;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.UnlockedItems;
using BattleCruisers.Data;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Fetchers;
using System;
using System.Collections.Generic;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
	public interface IItemsRow<TItem> where TItem : IComparableItem
	{
		/// <returns><c>true</c>, if unlocked item is now in loadout, <c>false</c> otherwise.</returns>
		bool SelectUnlockedItem(UnlockedItem<TItem> item);
	}
}
