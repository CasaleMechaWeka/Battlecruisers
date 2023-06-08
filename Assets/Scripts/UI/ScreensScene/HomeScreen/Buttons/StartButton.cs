namespace BattleCruisers.UI.ScreensScene.HomeScreen.Buttons
{
    public class StartButton : HomeScreenButton
    {
        protected override void OnClicked()
        {
            base.OnClicked();
            _homeScreen.StartBattleHub();
        }
    }
}