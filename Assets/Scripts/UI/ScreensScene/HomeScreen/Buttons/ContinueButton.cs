namespace BattleCruisers.UI.ScreensScene.HomeScreen.Buttons
{
    public class ContinueButton : HomeScreenButton
    {
        protected override void OnClicked()
        {
            _homeScreen.Continue();
        }
    }
}