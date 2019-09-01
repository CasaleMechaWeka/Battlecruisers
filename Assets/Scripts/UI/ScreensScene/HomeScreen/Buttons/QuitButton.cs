namespace BattleCruisers.UI.ScreensScene.HomeScreen.Buttons
{
    public class QuitButton : HomeScreenButton
    {
        protected override void OnClicked()
        {
            base.OnClicked();
            _homeScreen.Quit();
        }
    }
}