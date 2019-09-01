namespace BattleCruisers.UI.ScreensScene.HomeScreen.Buttons
{
    public class TutorialButton : HomeScreenButton
    {
        protected override void OnClicked()
        {
            base.OnClicked();
            _homeScreen.StartTutorial();
        }
    }
}