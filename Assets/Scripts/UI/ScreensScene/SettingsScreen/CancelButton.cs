using BattleCruisers.Scenes;
using BattleCruisers.UI.Sound;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.SettingsScreen
{
    public class CancelButton : TextButton
    {
        private IScreensSceneGod _screensSceneGod;

        public void Initialise(ISoundPlayer soundPlayer, IScreensSceneGod screensSceneGod)
        {
            base.Initialise(soundPlayer);

            Assert.IsNotNull(screensSceneGod);
            _screensSceneGod = screensSceneGod;
        }

        protected override void OnClicked()
        {
            base.OnClicked();
            _screensSceneGod.GoToHomeScreen();
        }
    }
}