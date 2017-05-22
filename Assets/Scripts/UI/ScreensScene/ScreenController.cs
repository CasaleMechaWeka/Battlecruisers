using BattleCruisers.Data;
using BattleCruisers.Scenes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

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
