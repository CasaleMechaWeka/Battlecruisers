using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;

namespace BattleCruisers.UI.Common.BuildableDetails
{
    public interface IComparableItemDetails<TItem> : IDismissableEmitter where TItem : IComparableItem
	{
		void ShowItemDetails(TItem item, TItem itemToCompareTo = default);
		void Hide();
	}
}
