using BattleCruisers.Scenes;
using BattleCruisers.UI.BattleScene.Presentables;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.ScreensScene
{
    public abstract class ScreenController : PresentableController
	{ 
		protected IScreensSceneGod _screensSceneGod;
        protected ISoundPlayer _soundPlayer;

        public bool IsInitialised => _screensSceneGod != null;

        protected void Initialise(IScreensSceneGod screensSceneGod, ISoundPlayer soundPlayer)
		{
            base.Initialise();

            Helper.AssertIsNotNull(screensSceneGod, soundPlayer);

			_screensSceneGod = screensSceneGod;
            _soundPlayer = soundPlayer;
		}
	}
}
