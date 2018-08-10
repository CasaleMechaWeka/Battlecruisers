using BattleCruisers.Scenes;
using BattleCruisers.UI.BattleScene;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene
{
    public abstract class ScreenController : MonoBehaviour, IPresentable
	{ 
		protected IScreensSceneGod _screensSceneGod;

        public bool IsInitialised { get { return _screensSceneGod != null; } }

        protected void Initialise(IScreensSceneGod screensSceneGod)
		{
			Assert.IsNotNull(screensSceneGod);
			_screensSceneGod = screensSceneGod;
		}

        public virtual void OnPresenting(object activationParameter)
        {
            // empty
        }

        public virtual void OnDismissing()
        {
            // empty
        }
	}
}
