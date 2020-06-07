using BattleCruisers.Scenes;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LevelsScreen
{
    public class LevelsSetController : MonoBehaviourWrapper
    {
		public void Initialise(IScreensSceneGod screensSceneGod, IList<LevelInfo> levels, int numOfLevelsUnlocked, ISingleSoundPlayer soundPlayer)
        {
            Helper.AssertIsNotNull(screensSceneGod, levels, soundPlayer);

            LevelButtonController[] levelButtons = GetComponentsInChildren<LevelButtonController>();
            Assert.AreEqual(levels.Count, levelButtons.Length);

            for (int i = 0; i < levels.Count; ++i)
            {
                LevelButtonController button = levelButtons[i];
                LevelInfo level = levels[i];

                button.Initialise(soundPlayer, level, screensSceneGod, numOfLevelsUnlocked);
            }
		}
	}
}
