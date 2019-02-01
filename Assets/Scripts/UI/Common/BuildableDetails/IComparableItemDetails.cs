using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Comparisons;

namespace BattleCruisers.UI.Common.BuildableDetails
{
    public interface IComparableItemDetails<TItem> : IDismissableEmitter where TItem : IComparableItem
	{
		void ShowItemDetails(TItem item, TItem itemToCompareTo = default(TItem));
		void Hide();
	}
}
