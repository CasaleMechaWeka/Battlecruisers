using BattleCruisers.Data;
using BattleCruisers.Scenes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LevelsScreen
{
	public class LevelsScreenController : ScreenController
	{
		private int _levelNum;

		public HorizontalOrVerticalLayoutGroup buttonsWrapper;

		public void Initialise(IUIFactory uiFactory, IScreensSceneGod screensSceneGod, IList<ILevel> levels)
		{
			base.Initialise(screensSceneGod);

			// Create level buttons
			for (int i = 0; i < levels.Count; ++i)
			{
				int levelNum = i + 1;
				uiFactory.CreateLevelButton(buttonsWrapper, levelNum, levels[i], screensSceneGod); 
			}

			uiFactory.CreateHomeButton(buttonsWrapper, screensSceneGod);
		}
	}
}
