using System.Linq;
using System.Collections.Generic;
using BattleCruisers.Data;
using BattleCruisers.Scenes;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LevelsScreen
{
    public class LevelsScreenController : ScreenController
	{
        private const int SET_SIZE = 7;

		public void Initialise(IScreensSceneGod screensSceneGod, IList<ILevel> levels, int numOfLevelsUnlocked)
		{
			base.Initialise(screensSceneGod);

            UIFactory uiFactory = GetComponent<UIFactory>();
            Assert.IsNotNull(uiFactory);

            // FELIX  Create set panels from prefab instead of relying on existing panel
            LevelsSetController setController = GetComponentInChildren<LevelsSetController>(includeInactive: true);
            Assert.IsNotNull(setController);

            Assert.IsTrue(levels.Count % SET_SIZE == 0);
            IList<ILevel> setLevels = new List<ILevel>(SET_SIZE);
            for (int i = 0; i < SET_SIZE; ++i)
            {
                setLevels.Add(levels[i]);
            }

            setController.Initialise(screensSceneGod, uiFactory, setLevels, numOfLevelsUnlocked);
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
