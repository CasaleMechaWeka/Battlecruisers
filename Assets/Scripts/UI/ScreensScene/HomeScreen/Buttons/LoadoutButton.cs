namespace BattleCruisers.UI.ScreensScene.HomeScreen.Buttons
{
    public class LoadoutButton : HomeScreenButton
    {
        protected override void OnClicked()
        {
            base.OnClicked();
            _homeScreen.GoToLoadoutScreen();
        }
    }
}