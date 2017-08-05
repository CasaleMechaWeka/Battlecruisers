using BattleCruisers.Scenes;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene
{
    public abstract class ScreenController : MonoBehaviour 
	{ 
		protected IScreensSceneGod _screensSceneGod;

		protected void Initialise(IScreensSceneGod screensSceneGod)
		{
			Assert.IsNotNull(screensSceneGod);
			_screensSceneGod = screensSceneGod;
		}
	}
}
