using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Scenes;
using BattleCruisers.UI.ScreensScene.TrashScreen;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LevelsScreen
{
    public class LevelsSetController : MonoBehaviourWrapper
    {
        private int _numOfLevels;
        public int firstLevelIndex;
        public NavigationFeedbackButton navigationFeedbackButton;

        public int SetIndex { get; private set; }
        public int LastLevelNum { get; private set; }

        public GameObject[] trails;

        public async Task InitialiseAsync(
            IScreensSceneGod screensSceneGod,
            LevelsScreenController levelsScreen,
            IList<LevelInfo> allLevels,
            int numOfLevelsUnlocked,
            ISingleSoundPlayer soundPlayer,
            Sprite[] difficultyIndicators,
            ITrashTalkProvider trashDataList,
            int setIndex)
        {
            Assert.IsNotNull(navigationFeedbackButton);
            Helper.AssertIsNotNull(screensSceneGod, allLevels, soundPlayer, difficultyIndicators, trashDataList);

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

                button.Initialise(soundPlayer, level, screensSceneGod, difficultyIndicators, numOfLevelsUnlocked, trashTalkData, levelsScreen);
            }

            // Set up Secret levels
            SecretLevelButtonController[] secretLevelButton = GetComponentsInChildren<SecretLevelButtonController>();
            int secretLevelNum = secretLevelButton.Length;
            if (secretLevelButton != null)
            {
                for (int i = 0; i < secretLevelNum; ++i)
                {
                    secretLevelButton[i].Initialise(screensSceneGod, soundPlayer, numOfLevelsUnlocked);
                }
            }

            SideQuestButtonController[] sideQuestButtons = GetComponentsInChildren<SideQuestButtonController>();
            for (int i = 0; i < sideQuestButtons.Count(); i++)
                sideQuestButtons[i].Initialise(screensSceneGod, soundPlayer, numOfLevelsUnlocked, true);

            // Set up trails
            int expectedNumberOfTrails = _numOfLevels - 1;
            //Assert.AreEqual(expectedNumberOfTrails, trails.Length, $"Expected {expectedNumberOfTrails} trails, not {trails.Length}.");

            for (int i = 0; i < trails.Length; ++i)
            {
                bool isTrailVisible = numOfLevelsUnlocked - firstLevelIndex - 1 > i;
                trails[i].SetActive(isTrailVisible);
            }

            // Setup navigation feedback button
            bool hasUnlockedLevels = numOfLevelsUnlocked > firstLevelIndex;
            navigationFeedbackButton.Initialise(levelsScreen, setIndex, hasUnlockedLevels);

            //Set up Side Quest levels


        }

        public void OnValidate()
        {
            if (trails.Length == 0)
            {
                Transform trailsParent = transform.GetChild(0);
                List<GameObject> children = new List<GameObject>();
                foreach (Transform t in trailsParent)
                    children.Add(t.gameObject);

                if (children.Count == 0)
                    Debug.LogError("Didn't find any trails. Please validate manually.");
                trails = children.ToArray();
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
