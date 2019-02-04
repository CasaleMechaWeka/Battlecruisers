using BattleCruisers.Scenes;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace BattleCruisers.UI.ScreensScene.LevelsScreen
{
    public class HomeButtonController : MonoBehaviour, IPointerClickHandler
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
