namespace BattleCruisers.UI.ScreensScene.HomeScreen.Buttons
{
    public class SkirmishButton : HomeScreenButton
    {
        protected override void OnClicked()
        {
            base.OnClicked();
            _homeScreen.GoToSkirmishScreen();
        }
    }
}