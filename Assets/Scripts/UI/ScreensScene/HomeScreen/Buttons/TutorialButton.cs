using UnityEngine.EventSystems;

namespace BattleCruisers.UI.ScreensScene.HomeScreen.Buttons
{
    public class TutorialButton : HomeScreenButton
    {
        public override void OnPointerClick(PointerEventData eventData)
        {
            _homeScreen.StartTutorial();
        }
    }
}