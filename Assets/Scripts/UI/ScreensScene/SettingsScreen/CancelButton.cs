using BattleCruisers.Scenes;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.SettingsScreen
{
    public class CancelButton : TextButton
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