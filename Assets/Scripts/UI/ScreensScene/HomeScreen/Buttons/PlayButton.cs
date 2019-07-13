namespace BattleCruisers.UI.ScreensScene.HomeScreen.Buttons
{
    public class PlayButton : HomeScreenButton
    {
        protected override void OnClicked()
        {
            if (_gameModel.HasAttemptedTutorial)
            {
                _homeScreen.StartLevel1();
            }
            else
            {
                _homeScreen.StartTutorial();
            }
        }
    }
}