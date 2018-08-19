using BattleCruisers.Scenes;
using BattleCruisers.UI.BattleScene;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene
{
    public abstract class ScreenController : PresentableController
	{ 
		protected IScreensSceneGod _screensSceneGod;

        public bool IsInitialised { get { return _screensSceneGod != null; } }

        protected void Initialise(IScreensSceneGod screensSceneGod)
		{
            base.Initialise();

			Assert.IsNotNull(screensSceneGod);
			_screensSceneGod = screensSceneGod;
		}
	}
}
