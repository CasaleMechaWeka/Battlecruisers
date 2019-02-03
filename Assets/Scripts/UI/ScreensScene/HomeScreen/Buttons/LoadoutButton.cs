using UnityEngine.EventSystems;

namespace BattleCruisers.UI.ScreensScene.HomeScreen.Buttons
{
    public class LoadoutButton : HomeScreenButton
    {
        public override void OnPointerClick(PointerEventData eventData)
        {
            _homeScreen.GoToLoadoutScreen();
        }
    }
}