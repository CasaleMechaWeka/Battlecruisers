using UnityEngine.EventSystems;

namespace BattleCruisers.UI.ScreensScene.HomeScreen
{
    public class LevelsButton : HomeScreenButton
    {
        public override void OnPointerClick(PointerEventData eventData)
        {
            _homeScreen.GoToLevelsScreen();
        }
    }
}