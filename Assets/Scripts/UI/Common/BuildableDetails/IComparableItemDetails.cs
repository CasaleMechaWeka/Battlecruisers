using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.UI.ScreensScene.ProfileScreen;

namespace BattleCruisers.UI.Common.BuildableDetails
{
    public interface IComparableItemDetails<TItem> : IDismissableEmitter where TItem : IComparableItem
	{
		void ShowItemDetails(TItem item, TItem itemToCompareTo = default);
		void ShowItemDetails();
		void Hide();
		void SetHullType(HullType hullType);
	}
}
