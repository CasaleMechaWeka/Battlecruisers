namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows
{
    public interface IItem<TItem> where TItem : IComparableItem
	{
		TItem Item { get; }
		bool ShowSelectedFeedback { set; }
	}
}

