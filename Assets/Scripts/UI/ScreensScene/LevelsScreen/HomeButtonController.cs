using BattleCruisers.Scenes;
using BattleCruisers.UI.BattleScene.Presentables;
using BattleCruisers.UI.Sound;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LevelsScreen
{
    public class HomeButtonController : TextButton
    {
        private IScreensSceneGod _screensSceneGod;

		public void Initialise(ISingleSoundPlayer soundPlayer, IScreensSceneGod screensSceneGod, IDismissableEmitter parent)
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
