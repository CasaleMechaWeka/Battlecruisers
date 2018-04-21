namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows
{
    public enum UIState
    {
		// The element is clickable but not highlighted.
        Default,
		
        // The element is clickable and highlighted.
        Highlighted,
		
        // The element is not clickable and not highlighted.
        Disabled
    }

    public interface IStatefulUIElement
    {
        void GoToState(UIState state);
    }
}
