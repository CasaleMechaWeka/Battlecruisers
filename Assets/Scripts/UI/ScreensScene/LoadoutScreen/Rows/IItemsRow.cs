using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.UnlockedItems;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Comparisons;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows
{
    public interface IItemsRow<TItem> : IStatefulUIElement where TItem : class, IComparableItem
	{
		/// <returns><c>true</c>, if unlocked item is now in loadout, <c>false</c> otherwise.</returns>
		bool SelectUnlockedItem(UnlockedItem<TItem> item);
	}
}
