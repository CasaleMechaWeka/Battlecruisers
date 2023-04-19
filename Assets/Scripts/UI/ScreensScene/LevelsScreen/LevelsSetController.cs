using BattleCruisers.Scenes;
using BattleCruisers.UI.ScreensScene.TrashScreen;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers.Sprites;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.Assertions;
using UnityEngine;

namespace BattleCruisers.UI.ScreensScene.LevelsScreen
{
    public class LevelsSetController : MonoBehaviourWrapper
    {
        private int _numOfLevels;

        public int firstLevelIndex;
        public NavigationFeedbackButton navigationFeedbackButton;

        [SerializeField]
        private GameObject[] secretLevelButtons;

        public int SetIndex { get; private set; }
        public int LastLevelNum { get; private set; }

        public async Task InitialiseAsync(
            IScreensSceneGod screensSceneGod,
            LevelsScreenController levelsScreen,
            IList<LevelInfo> allLevels,
            int numOfLevelsUnlocked,
            ISingleSoundPlayer soundPlayer,
            IDifficultySpritesProvider difficultySpritesProvider,
            ITrashTalkProvider trashDataList,
            int setIndex)
        {
            Assert.IsNotNull(navigationFeedbackButton);
            Helper.AssertIsNotNull(screensSceneGod, allLevels, soundPlayer, difficultySpritesProvider, trashDataList);

            SetIndex = setIndex;

            // Set up levels
            LevelButtonController[] levelButtons = GetComponentsInChildren<LevelButtonController>();
            _numOfLevels = levelButtons.Length;

            Assert.IsTrue(firstLevelIndex >= 0);
            Assert.IsTrue(firstLevelIndex + _numOfLevels <= allLevels.Count);

            LastLevelNum = firstLevelIndex + _numOfLevels;

            for (int i = 0; i < _numOfLevels; ++i)
            {
                LevelButtonController button = levelButtons[i];
                LevelInfo level = allLevels[firstLevelIndex + i];
                ITrashTalkData trashTalkData = await trashDataList.GetTrashTalkAsync(level.Num);

                await button.InitialiseAsync(soundPlayer, level, screensSceneGod, difficultySpritesProvider, numOfLevelsUnlocked, trashTalkData, levelsScreen);
            }

            // Set up trails
            TrailController[] trails = GetComponentsInChildren<TrailController>();
            int expectedNumberOfTrails = _numOfLevels - 1;
            Assert.AreEqual(expectedNumberOfTrails, trails.Length, $"Expected {expectedNumberOfTrails} trails, not {trails.Length}.");

            for (int i = 0; i < trails.Length; ++i)
            {
                bool isTrailVisible = numOfLevelsUnlocked - firstLevelIndex - 1 > i;
                trails[i].IsVisible = isTrailVisible;
            }

            // Setup navigation feedback button
            bool hasUnlockedLevels = numOfLevelsUnlocked > firstLevelIndex;
            navigationFeedbackButton.Initialise(levelsScreen, setIndex, hasUnlockedLevels);

            // Check if level 31 has been passed and enable the corresponding secret level button
            if (numOfLevelsUnlocked >= 32 && setIndex < secretLevelButtons.Length)
            {
                GameObject secretLevelButton = secretLevelButtons[setIndex];
                secretLevelButton.SetActive(true);

                SecretLevelButtonController secretLevelButtonController = secretLevelButton.GetComponent<SecretLevelButtonController>();
                LevelInfo secretLevel = allLevels[31 + setIndex]; // Assuming secret levels are added to allLevels list
                ITrashTalkData secretTrashTalkData = await trashDataList.GetTrashTalkAsync(secretLevel.Num);
                await secretLevelButtonController.InitialiseAsync(soundPlayer, secretLevel, screensSceneGod, difficultySpritesProvider, numOfLevelsUnlocked, secretTrashTalkData, levelsScreen);
            }
            else
            {
                // If level 31 has not been passed, hide all the secret level buttons
                foreach (GameObject secretLevelButton in secretLevelButtons)
                {
                    secretLevelButton.SetActive(false);
                }
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
