using UnityEngine.EventSystems;

namespace BattleCruisers.UI.ScreensScene.HomeScreen.Buttons
{
    public class SettingsButton : HomeScreenButton
    {
        public override void OnPointerClick(PointerEventData eventData)
        {
            _homeScreen.GoToSettingsScreen();
        }
    }
}