using BattleCruisers.Network.Multiplay.Scenes;
using BattleCruisers.Scenes;
using BattleCruisers.UI.BattleScene.Presentables;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene
{
    public abstract class ScreenController : PresentableController
	{ 
		protected IScreensSceneGod _screensSceneGod;
        protected IMultiplayScreensSceneGod _multiplayScreensSceneGod;
        public bool IsInitialised => _screensSceneGod != null;

        protected void Initialise(IScreensSceneGod screensSceneGod)
		{
            base.Initialise();

            Assert.IsNotNull(screensSceneGod);
			_screensSceneGod = screensSceneGod;
		}

        protected void Initialise(IMultiplayScreensSceneGod multiplayScreensSceneGod)
        {
            base.Initialise();

            Assert.IsNotNull(multiplayScreensSceneGod);
            _multiplayScreensSceneGod = multiplayScreensSceneGod;
        }

        public virtual void Cancel() { }

        public void WishlistGame()
        {
            Application.OpenURL("https://store.steampowered.com/app/955870/Battlecruisers/");
        }
    }
}
