namespace BattleCruisers.UI.ScreensScene.HomeScreen.Buttons
{
    public class ContinueButton : HomeScreenButton
    {
        protected override void OnClicked()
        {
            base.OnClicked();
            _homeScreen.Continue();
        }
    }
}