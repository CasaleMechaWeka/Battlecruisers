using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows;

namespace BattleCruisers.UI.Common.BuildingDetails
{
    public interface IComparableItemDetails<TItem> where TItem : IComparableItem
	{
		void ShowItemDetails(TItem item, TItem itemToCompareTo = default(TItem));
		void Hide();
	}
}
