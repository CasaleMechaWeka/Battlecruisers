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
        private int _numOfLevels;
        public int firstLevelIndex;

        public int SetIndex { get; private set; }

		public async Task InitialiseAsync(
            IScreensSceneGod screensSceneGod, 
            IList<LevelInfo> allLevels, 
            int numOfLevelsUnlocked, 
            ISingleSoundPlayer soundPlayer,
            IDifficultySpritesProvider difficultySpritesProvider,
            int setIndex)
        {
            Helper.AssertIsNotNull(screensSceneGod, allLevels, soundPlayer, difficultySpritesProvider);

            SetIndex = setIndex;

            LevelButtonController[] levelButtons = GetComponentsInChildren<LevelButtonController>();
            _numOfLevels = levelButtons.Length;

            Assert.IsTrue(firstLevelIndex >= 0);
            Assert.IsTrue(firstLevelIndex + _numOfLevels <= allLevels.Count);

            for (int i = 0; i < _numOfLevels; ++i)
            {
                LevelButtonController button = levelButtons[i];
                LevelInfo level = allLevels[firstLevelIndex + i];

                await button.InitialiseAsync(soundPlayer, level, screensSceneGod, difficultySpritesProvider, numOfLevelsUnlocked);
            }
		}

        public bool ContainsLevel(int levelNum)
        {
            return
                levelNum > firstLevelIndex
                && levelNum <= firstLevelIndex + _numOfLevels;
        }
	}
}
