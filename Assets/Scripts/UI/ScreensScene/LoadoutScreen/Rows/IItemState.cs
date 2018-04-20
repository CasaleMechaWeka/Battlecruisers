namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows
{
	public interface IItemState<TItem> where TItem : IComparableItem
	{
		void HandleSelection();
	}
}
