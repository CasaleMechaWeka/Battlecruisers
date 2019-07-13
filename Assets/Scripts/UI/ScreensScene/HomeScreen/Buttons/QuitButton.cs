namespace BattleCruisers.UI.ScreensScene.HomeScreen.Buttons
{
    public class QuitButton : HomeScreenButton
    {
        protected override void OnClicked()
        {
            _homeScreen.Quit();
        }
    }
}