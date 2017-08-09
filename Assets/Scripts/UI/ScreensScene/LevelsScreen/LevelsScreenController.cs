using System.Collections.Generic;
using BattleCruisers.Data;
using BattleCruisers.Scenes;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LevelsScreen
{
    public class LevelsScreenController : ScreenController
	{
		public UIFactory uiFactory;
		public HorizontalOrVerticalLayoutGroup buttonsWrapper;

		public void Initialise(IScreensSceneGod screensSceneGod, IList<ILevel> levels, int numOfLevelsUnlocked)
		{
			base.Initialise(screensSceneGod);

            // FELIX
			//// Create level buttons
			//for (int i = 0; i < levels.Count; ++i)
			//{
			//	int levelNum = i + 1;
			//	bool isLevelUnlocked = levelNum <= numOfLevelsUnlocked;

			//	uiFactory.CreateLevelButton(buttonsWrapper, levelNum, levels[i], isLevelUnlocked, screensSceneGod); 
			//}

			//uiFactory.CreateHomeButton(buttonsWrapper, screensSceneGod);
		}
	}
}
