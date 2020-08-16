using BattleCruisers.Scenes;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers.Sprites;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LevelsScreen
{
    public class LevelsSetController : MonoBehaviourWrapper
    {
		public async Task InitialiseAsync(
            IScreensSceneGod screensSceneGod, 
            IList<LevelInfo> levels, 
            int numOfLevelsUnlocked, 
            ISingleSoundPlayer soundPlayer,
            IDifficultySpritesProvider difficultySpritesProvider)
        {
            Helper.AssertIsNotNull(screensSceneGod, levels, soundPlayer, difficultySpritesProvider);

            LevelButtonController[] levelButtons = GetComponentsInChildren<LevelButtonController>();
            Assert.AreEqual(levels.Count, levelButtons.Length);

            for (int i = 0; i < levels.Count; ++i)
            {
                LevelButtonController button = levelButtons[i];
                LevelInfo level = levels[i];

                await button.InitialiseAsync(soundPlayer, level, screensSceneGod, difficultySpritesProvider, numOfLevelsUnlocked);
            }
		}
	}
}
