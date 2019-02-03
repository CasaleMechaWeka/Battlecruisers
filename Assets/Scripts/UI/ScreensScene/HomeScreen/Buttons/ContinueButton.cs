using UnityEngine.EventSystems;

namespace BattleCruisers.UI.ScreensScene.HomeScreen.Buttons
{
    public class ContinueButton : HomeScreenButton
    {
        public override void OnPointerClick(PointerEventData eventData)
        {
            _homeScreen.Continue();
        }
    }
}