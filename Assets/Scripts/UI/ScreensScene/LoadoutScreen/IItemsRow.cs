using BattleCruisers.UI.ScreensScene.LoadoutScreen.UnlockedItems;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
    public interface IItemsRow<TItem> where TItem : IComparableItem
	{
        // Not in constructor because setup requires protected abstract methods, which
        // should not be called from the constructor.
        void SetupUI();

		/// <returns><c>true</c>, if unlocked item is now in loadout, <c>false</c> otherwise.</returns>
		bool SelectUnlockedItem(UnlockedItem<TItem> item);
	}
}
