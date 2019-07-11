namespace BattleCruisers.UI.ScreensScene.HomeScreen.Buttons
{
    public class SettingsButton : HomeScreenButton
    {
        protected override void OnClicked()
        {
            _homeScreen.GoToSettingsScreen();
        }
    }
}