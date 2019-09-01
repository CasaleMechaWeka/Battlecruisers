namespace BattleCruisers.UI.ScreensScene.HomeScreen.Buttons
{
    public class LevelsButton : HomeScreenButton
    {
        protected override void OnClicked()
        {
            base.OnClicked();
            _homeScreen.GoToLevelsScreen();
        }
    }
}