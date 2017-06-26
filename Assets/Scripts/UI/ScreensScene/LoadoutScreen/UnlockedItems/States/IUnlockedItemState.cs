namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.UnlockedItems.States
{
	public interface IUnlockedItemState<TItem> where TItem : IComparableItem
	{
		// FELIX  Item background colour!

		void HandleSelection(UnlockedItem<TItem> item);
	}
}
