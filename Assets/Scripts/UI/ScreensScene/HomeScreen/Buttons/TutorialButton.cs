namespace BattleCruisers.UI.ScreensScene.HomeScreen.Buttons
{
    public class TutorialButton : HomeScreenButton
    {
        protected override void OnClicked()
        {
            _homeScreen.StartTutorial();
        }
    }
}