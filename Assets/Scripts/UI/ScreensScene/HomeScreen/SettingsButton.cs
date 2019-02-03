using UnityEngine.EventSystems;

namespace BattleCruisers.UI.ScreensScene.HomeScreen
{
    public class SettingsButton : HomeScreenButton
    {
        public override void OnPointerClick(PointerEventData eventData)
        {
            _homeScreen.GoToSettingsScreen();
        }
    }
}