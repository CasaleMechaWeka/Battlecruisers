using BattleCruisers.Scenes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene
{
	public class HomeScreenController : ScreenController
	{
		public new void Initialise(IScreensSceneGod screensSceneGod)
		{
			base.Initialise(screensSceneGod);

			// FELIX  Hide continue button if first time
		}

		// FELIX  Start last level played OR next level if user won last level and then quit.
		public void Continue()
		{
			Debug.Log("Continue()");
		}

		public void GoToLevelsScreen()
		{
			_screensSceneGod.GoToLevelsScreen();
		}

		public void GoToLoadoutScreen()
		{
			_screensSceneGod.GoToLoadoutScreen();
		}

		public void Quit()
		{
			Application.Quit();
		}
	}
}
