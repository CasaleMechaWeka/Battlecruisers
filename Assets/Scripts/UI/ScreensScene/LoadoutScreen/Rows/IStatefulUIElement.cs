namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows
{
    public interface IStatefulUIElement
    {
        /// <summary>
        /// The element is clickable but not highlighted.
        /// </summary>
        void GoToDefaultState();

        /// <summary>
        /// The element is highlighted and clickable.
        /// </summary>
        void GoToHighlightedState();

        /// <summary>
        /// The element is not clickable and not highlighted.
        /// </summary>
        void GoToDisabledState();
    }
}
