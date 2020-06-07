using BattleCruisers.Scenes;
using BattleCruisers.UI.BattleScene.Presentables;
using BattleCruisers.UI.Sound;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene
{
    public abstract class ScreenController : PresentableController
	{ 
		protected IScreensSceneGod _screensSceneGod;

        public bool IsInitialised => _screensSceneGod != null;

        protected void Initialise(ISingleSoundPlayer soundPlayer, IScreensSceneGod screensSceneGod)
		{
            base.Initialise(soundPlayer);

            Assert.IsNotNull(screensSceneGod);
			_screensSceneGod = screensSceneGod;
		}
	}
}
