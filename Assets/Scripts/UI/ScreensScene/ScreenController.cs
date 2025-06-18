using BattleCruisers.Network.Multiplay.Scenes;
using BattleCruisers.Scenes;
using BattleCruisers.UI.BattleScene.Presentables;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene
{
    public class ScreenController : PresentableController
    {
        protected ScreensSceneGod _screensSceneGod;
        protected IMultiplayScreensSceneGod _multiplayScreensSceneGod;
        public bool IsInitialised => _screensSceneGod != null;

        public void Initialise(ScreensSceneGod screensSceneGod)
        {
            base.Initialise();

            Assert.IsNotNull(screensSceneGod);
            _screensSceneGod = screensSceneGod;
        }

        public void Initialise(IMultiplayScreensSceneGod multiplayScreensSceneGod)
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
