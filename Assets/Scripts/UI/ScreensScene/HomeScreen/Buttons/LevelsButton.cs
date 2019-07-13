using UnityEngine.EventSystems;

namespace BattleCruisers.UI.ScreensScene.HomeScreen.Buttons
{
    public class LevelsButton : HomeScreenButton
    {
        protected override void OnClicked()
        {
            _homeScreen.GoToLevelsScreen();
        }
    }
}