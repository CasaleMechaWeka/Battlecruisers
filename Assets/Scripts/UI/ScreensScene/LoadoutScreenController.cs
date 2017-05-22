using BattleCruisers.Scenes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene
{
	public class LoadoutScreenController : ScreenController
	{
		public new void Initialise(IScreensSceneGod screensSceneGod)
		{
			base.Initialise(screensSceneGod);
		}

		public void GoToHomeScreen()
		{
			_screensSceneGod.GoToHomeScreen();
		}

		public void GoToLevelsScreen()
		{
			_screensSceneGod.GoToLevelsScreen();
		}
	}
}
