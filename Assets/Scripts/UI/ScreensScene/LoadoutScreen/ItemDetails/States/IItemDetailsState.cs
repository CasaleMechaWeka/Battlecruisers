using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails.States
{
    public interface IItemDetailsState<TItem> where TItem : IComparableItem
	{
		bool IsInReadyToCompareState { get; }

		IItemDetailsState<TItem> SelectItem(IItem<TItem> selectedItem);
		IItemDetailsState<TItem> CompareSelectedItem();
		IItemDetailsState<TItem> Dismiss();

        // FELIX  Add method here, to call right method on IItemManager?  Avoids switch in IItemManager :)
	}
}
