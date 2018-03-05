using BattleCruisers.Scenes;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene
{
    public abstract class ScreenController : MonoBehaviour 
	{ 
		protected IScreensSceneGod _screensSceneGod;

        public bool IsInitialised { get { return _screensSceneGod != null; } }

		protected void Initialise(IScreensSceneGod screensSceneGod)
		{
			Assert.IsNotNull(screensSceneGod);
			_screensSceneGod = screensSceneGod;
		}
	}
}
