using BattleCruisers.Data.Static;
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

        public GameObject[] trails; // Optional - can be empty if no trails wanted

        public async Task InitialiseAsync(
            ScreensSceneGod screensSceneGod,
            LevelsScreenController levelsScreen,
            IList<LevelInfo> allLevels,
            int numOfLevelsUnlocked,
            SingleSoundPlayer soundPlayer,
            Sprite[] difficultyIndicators,
            int setIndex)
        {
            Assert.IsNotNull(navigationFeedbackButton);
            Helper.AssertIsNotNull(screensSceneGod, allLevels, soundPlayer, difficultyIndicators);

            SetIndex = setIndex;

            // Set up levels
            LevelButtonController[] levelButtons = GetComponentsInChildren<LevelButtonController>();
            _numOfLevels = levelButtons.Length;

            Assert.IsTrue(firstLevelIndex >= 0);
            Assert.IsTrue(firstLevelIndex + _numOfLevels <= allLevels.Count);

            LastLevelNum = firstLevelIndex + _numOfLevels;

            List<Task> levelButtonInitialisations = new List<Task>();

            for (int i = 0; i < _numOfLevels; ++i)
            {
                LevelInfo level = allLevels[firstLevelIndex + i];
                TrashTalkData trashTalkData = StaticData.LevelTrashTalk[level.Num];

                levelButtonInitialisations.Add(levelButtons[i].Initialise(soundPlayer, level, screensSceneGod, difficultyIndicators, numOfLevelsUnlocked, trashTalkData, levelsScreen));
            }

            await Task.WhenAll(levelButtonInitialisations);

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

            // Set up trails (optional)
            if (trails != null && trails.Length > 0)
            {
                int expectedNumberOfTrails = _numOfLevels - 1;
                // Only warn if trails are provided but wrong count - don't enforce
                if (trails.Length != expectedNumberOfTrails)
                {
                    Debug.LogWarning($"Level set {SetIndex} has {trails.Length} trails but expected {expectedNumberOfTrails} (_numOfLevels={_numOfLevels}). This is optional.");
                }

                for (int i = 0; i < trails.Length; ++i)
                {
                    if (trails[i] != null)
                    {
                        bool isTrailVisible = numOfLevelsUnlocked - firstLevelIndex - 1 > i;
                        trails[i].SetActive(isTrailVisible);
                    }
                }
            }

            // Setup navigation feedback button
            bool hasUnlockedLevels = numOfLevelsUnlocked > firstLevelIndex;
            navigationFeedbackButton.Initialise(levelsScreen, setIndex, hasUnlockedLevels);

            //Set up Side Quest levels


        }

        public void OnValidate()
        {
            // Only auto-populate trails if the array is empty and there are child transforms
            // This allows level sets to have no trails if desired
            if (trails == null || trails.Length == 0)
            {
                if (transform.childCount > 0)
                {
                    Transform trailsParent = transform.GetChild(0);
                    List<GameObject> children = new List<GameObject>();
                    foreach (Transform t in trailsParent)
                    {
                        if (t.gameObject != null)
                            children.Add(t.gameObject);
                    }

                    if (children.Count > 0)
                    {
                        trails = children.ToArray();
                        Debug.Log($"Auto-populated {trails.Length} trails for level set {name}");
                    }
                    // If no children found, trails remains empty - this is now allowed
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
