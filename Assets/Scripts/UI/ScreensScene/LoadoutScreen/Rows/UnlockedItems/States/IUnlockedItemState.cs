namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.UnlockedItems.States
{
	public interface IUnlockedItemState<TItem> where TItem : IComparableItem
	{
		void HandleSelection();
	}
}
