namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows
{
    // FELIX  Create Items namespace?
	public interface IItemState<TItem> where TItem : IComparableItem
	{
		void HandleSelection();
	}
}
