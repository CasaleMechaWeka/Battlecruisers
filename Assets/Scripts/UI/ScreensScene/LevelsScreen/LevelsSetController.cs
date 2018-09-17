using BattleCruisers.Scenes;
using BattleCruisers.Utils;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LevelsScreen
{
    public class LevelsSetController : MonoBehaviourWrapper
    {
		public void Initialise(IScreensSceneGod screensSceneGod, IList<LevelInfo> levels, int numOfLevelsUnlocked)
        {
            Helper.AssertIsNotNull(screensSceneGod, levels);

            LevelButtonController[] levelButtons = GetComponentsInChildren<LevelButtonController>();
            Assert.AreEqual(levels.Count, levelButtons.Length);

            for (int i = 0; i < levels.Count; ++i)
            {
                LevelButtonController button = levelButtons[i];
                LevelInfo level = levels[i];

                button.Initialise(level, screensSceneGod, numOfLevelsUnlocked);
            }
		}
	}
}
