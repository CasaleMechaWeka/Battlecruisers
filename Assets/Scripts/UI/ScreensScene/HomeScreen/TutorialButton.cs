using UnityEngine.EventSystems;

namespace BattleCruisers.UI.ScreensScene.HomeScreen
{
    public class TutorialButton : HomeScreenButton
    {
        public override void OnPointerClick(PointerEventData eventData)
        {
            _homeScreen.StartTutorial();
        }
    }
}