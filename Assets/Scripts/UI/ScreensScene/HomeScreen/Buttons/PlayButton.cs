namespace BattleCruisers.UI.ScreensScene.HomeScreen.Buttons
{
    public class PlayButton : HomeScreenButton
    {
        protected override void OnClicked()
        {
            base.OnClicked();

            if (_gameModel.HasAttemptedTutorial)
            {
                _homeScreen.GoToChooseDifficultyScreen();
            }
            else
            {
                _homeScreen.StartTutorial();
            }
        }
    }
}