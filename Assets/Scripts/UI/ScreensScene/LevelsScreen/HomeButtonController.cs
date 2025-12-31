using BattleCruisers.Scenes;
using BattleCruisers.UI.Sound.Players;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LevelsScreen
{
    public class HomeButtonController : TextButton
    {
        private ScreensSceneGod _screensSceneGod;

        public void Initialise(SingleSoundPlayer soundPlayer, ScreensSceneGod screensSceneGod, IDismissableEmitter parent)
        {
            base.Initialise(soundPlayer, parent);

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
