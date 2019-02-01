using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Comparisons;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.ItemStates
{
    public interface IItemState<TItem> where TItem : IComparableItem
	{
		void SelectItem();
	}
}
