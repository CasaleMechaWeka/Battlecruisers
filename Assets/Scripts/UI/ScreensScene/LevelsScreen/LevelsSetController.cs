using BattleCruisers.Scenes;
using BattleCruisers.UI.ScreensScene.TrashScreen;
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

        private const int LAST_SET_INDEX = 6;

        public int SetIndex { get; private set; }

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

            // Set up levels
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

            // Set up trails
            TrailController[] trails = GetComponentsInChildren<TrailController>();
            int expectedNumberOfTrails = SetIndex == LAST_SET_INDEX ? _numOfLevels - 1 : _numOfLevels;
            Assert.AreEqual(expectedNumberOfTrails, trails.Length, $"Expected {expectedNumberOfTrails} trails, not {trails.Length}.");

            for (int i = 0; i > trails.Length; ++i)
            {
                bool isTrailVisible = numOfLevelsUnlocked - firstLevelIndex - 1 > i;
                trails[i].IsVisible = isTrailVisible;
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
