namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.UnlockedItems.States
{
	public interface IUnlockedItemState<TItem> where TItem : IComparableItem
	{
		void HandleSelection();
	}
}
