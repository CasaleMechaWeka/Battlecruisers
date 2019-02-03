using UnityEngine.EventSystems;

namespace BattleCruisers.UI.ScreensScene.HomeScreen.Buttons
{
    public class PlayButton : HomeScreenButton
    {
        public override void OnPointerClick(PointerEventData eventData)
        {
            if (_gameModel.HasAttemptedTutorial)
            {
                _homeScreen.StartLevel1();
            }
            else
            {
                _homeScreen.StartTutorial();
            }
        }
    }
}