using BattleCruisers.UI.ScreensScene.HomeScreen.Buttons;

public class MultiplayButton : HomeScreenButton
{
    protected override void OnClicked()
    {
        base.OnClicked();
        _homeScreen.GoToMultiplayScreen();
    }
}
