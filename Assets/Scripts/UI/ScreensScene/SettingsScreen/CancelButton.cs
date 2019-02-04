using BattleCruisers.Scenes;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace BattleCruisers.UI.ScreensScene.SettingsScreen
{
    public class CancelButton : Togglable, IPointerClickHandler
    {
        private IScreensSceneGod _screensSceneGod;

        public void Initialise(IScreensSceneGod screensSceneGod)
        {
            Assert.IsNotNull(screensSceneGod);
            _screensSceneGod = screensSceneGod;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _screensSceneGod.GoToHomeScreen();
        }
    }
}