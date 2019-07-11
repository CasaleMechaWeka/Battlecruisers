namespace BattleCruisers.UI.ScreensScene.HomeScreen.Buttons
{
    public class LoadoutButton : HomeScreenButton
    {
        protected override void OnClicked()
        {
            _homeScreen.GoToLoadoutScreen();
        }
    }
}