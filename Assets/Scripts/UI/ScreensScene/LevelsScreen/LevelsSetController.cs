using BattleCruisers.Scenes;
using BattleCruisers.UI.ScreensScene.TrashScreen;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers.Sprites;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LevelsScreen
{
    public class LevelsSetController : MonoBehaviourWrapper
    {
        private int _numOfLevels;
        public int firstLevelIndex;

        public int SetIndex { get; private set; }
        public bool HasUnlockedLevel { get; private set; }

		public async Task InitialiseAsync(
            IScreensSceneGod screensSceneGod, 
            IList<LevelInfo> allLevels, 
            int numOfLevelsUnlocked, 
            ISingleSoundPlayer soundPlayer,
            IDifficultySpritesProvider difficultySpritesProvider,
            ITrashTalkDataList trashDataList,
            int setIndex)
        {
            Helper.AssertIsNotNull(screensSceneGod, allLevels, soundPlayer, difficultySpritesProvider, trashDataList);

            SetIndex = setIndex;
            HasUnlockedLevel = numOfLevelsUnlocked > firstLevelIndex;

            LevelButtonController[] levelButtons = GetComponentsInChildren<LevelButtonController>();
            _numOfLevels = levelButtons.Length;

            Assert.IsTrue(firstLevelIndex >= 0);
            Assert.IsTrue(firstLevelIndex + _numOfLevels <= allLevels.Count);

            for (int i = 0; i < _numOfLevels; ++i)
            {
                LevelButtonController button = levelButtons[i];
                LevelInfo level = allLevels[firstLevelIndex + i];
                ITrashTalkData trashTalkData = trashDataList.GetTrashTalk(level.Num);

                await button.InitialiseAsync(soundPlayer, level, screensSceneGod, difficultySpritesProvider, numOfLevelsUnlocked, trashTalkData);
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
