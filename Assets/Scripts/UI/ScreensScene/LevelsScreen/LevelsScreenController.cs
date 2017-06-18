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

		public UIFactory uiFactory;
		public HorizontalOrVerticalLayoutGroup buttonsWrapper;

		public void Initialise(IScreensSceneGod screensSceneGod, IList<ILevel> levels, int numOfLevelsUnlocked)
		{
			base.Initialise(screensSceneGod);

			// Create level buttons
			for (int i = 0; i < levels.Count; ++i)
			{
				int levelNum = i + 1;
				bool isLevelUnlocked = levelNum <= numOfLevelsUnlocked;

				uiFactory.CreateLevelButton(buttonsWrapper, levelNum, levels[i], isLevelUnlocked, screensSceneGod); 
			}

			uiFactory.CreateHomeButton(buttonsWrapper, screensSceneGod);
		}
	}
}
