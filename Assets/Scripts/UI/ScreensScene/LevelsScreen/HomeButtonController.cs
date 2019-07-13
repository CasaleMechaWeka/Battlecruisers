using BattleCruisers.Scenes;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LevelsScreen
{
    public class HomeButtonController : TextButton
    {
        private IScreensSceneGod _screensSceneGod;

		public void Initialise(IScreensSceneGod screensSceneGod)
		{
            base.Initialise();

            Assert.IsNotNull(screensSceneGod);
            _screensSceneGod = screensSceneGod;
		}

        protected override void OnClicked()
        {
            _screensSceneGod.GoToHomeScreen();
        }
    }
}
