using UnityEngine.EventSystems;

namespace BattleCruisers.UI.ScreensScene.HomeScreen
{
    public class LoadoutButton : HomeScreenButton
    {
        public override void OnPointerClick(PointerEventData eventData)
        {
            _homeScreen.GoToLoadoutScreen();
        }
    }
}