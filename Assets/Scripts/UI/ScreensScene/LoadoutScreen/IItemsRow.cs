using BattleCruisers.UI.ScreensScene.LoadoutScreen.UnlockedItems;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
    public interface IItemsRow<TItem> where TItem : IComparableItem
	{
		/// <returns><c>true</c>, if unlocked item is now in loadout, <c>false</c> otherwise.</returns>
		bool SelectUnlockedItem(UnlockedItem<TItem> item);
	}
}
