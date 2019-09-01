namespace BattleCruisers.UI.ScreensScene.HomeScreen.Buttons
{
    public class SettingsButton : HomeScreenButton
    {
        protected override void OnClicked()
        {
            base.OnClicked();
            _homeScreen.GoToSettingsScreen();
        }
    }
}