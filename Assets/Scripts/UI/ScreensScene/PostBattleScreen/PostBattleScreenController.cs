using BattleCruisers.Ads;
using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Settings;
using BattleCruisers.Data.Static;
using BattleCruisers.Data.Static.Strategies.Helper;
using BattleCruisers.Scenes;
using BattleCruisers.UI.Commands;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.UI.Music;
using BattleCruisers.UI.Panels;
using BattleCruisers.UI.ScreensScene.LevelsScreen;
using BattleCruisers.UI.ScreensScene.PostBattleScreen.States;
using BattleCruisers.UI.ScreensScene.TrashScreen;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.PostBattleScreen
{
    public enum PostBattleScreenBehaviour
    {
        Default,
        TutorialCompleted,
        Defeat,
        Victory_LootUnlocked,
        Victory_NoNewLoot,      // Currently the same behaviour as Victory_LootUnlocked
        Victory_DemoCompleted,
        Victory_GameCompleted,
        Victory_Skirmish,
        Defeat_Skirmish,
        Victory_SideQuest_LootUnlocked,
        Victory_SideQuest_NoNewLoot,
        Defeat_SideQuest
    }

    public class PostBattleScreenController : ScreenController
    {
        private LootManager _lootManager;

        public Text title;
        public SlidingPanel unlockedItemSection;
        public GameObject defeatMessage, victoryNoLootMessage, demoCompletedScreen, decreaseDifficultySection;
        public LevelNameController levelName;
        public LevelStatsController completedDifficultySymbol;
        public CanvasGroupButton demoHomeButton;
        public PostTutorialButtonsPanel postTutorialButtonsPanel;
        public PostBattleButtonsPanel postBattleButtonsPanel;
        public PostSkirmishButtonsPanel postSkirmishButtonsPanel;
        public AppraisalSectionController appraisalSection;
        public AppraisalButtonsPanel appraisalButtonsPanel;
        public DecreaseDifficultySuggestionController decreaseDifficultyController;

        [Header("Can change these for testing")]
        public PostBattleScreenBehaviour desiredBehaviour;
        [Range(1, 25)]
        public int levelNum = 2;
        public bool showAppraisalButtons = false;

        private BattleResult BattleResult => DataProvider.GameModel.LastBattleResult;
        private GameMode _gameMode;

        public async Task InitialiseAsync(
            ScreensSceneGod screensSceneGod,
            SingleSoundPlayer soundPlayer,
            MusicPlayer musicPlayer,
            Sprite[] difficultyIndicators,
            ITrashTalkProvider levelTrashTalkList,
            ITrashTalkProvider sideQuestTrashTalkList)
        {
            base.Initialise(screensSceneGod);

            Helper.AssertIsNotNull(
                title, unlockedItemSection,
                defeatMessage, victoryNoLootMessage,
                demoCompletedScreen, levelName,
                completedDifficultySymbol, demoHomeButton,
                levelTrashTalkList, sideQuestTrashTalkList,
                postTutorialButtonsPanel, postBattleButtonsPanel,
                postSkirmishButtonsPanel, appraisalSection,
                appraisalButtonsPanel,
                musicPlayer,
                difficultyIndicators);

            _lootManager = CreateLootManager();
            _gameMode = ApplicationModel.Mode;
            Debug.Log(_gameMode);

            if (desiredBehaviour != PostBattleScreenBehaviour.Default)
                SetupBattleResult();

            PostBattleState postBattleState = null;
            Debug.Log(desiredBehaviour);
            if (desiredBehaviour == PostBattleScreenBehaviour.TutorialCompleted
                || ApplicationModel.IsTutorial)
            {
                // Track tutorial completion with Firebase
                if (FirebaseAnalyticsManager.Instance != null && BattleResult != null)
                {
                    if (BattleResult.WasVictory)
                    {
                        FirebaseAnalyticsManager.Instance.LogTutorialComplete("main_tutorial");
                        FirebaseAnalyticsManager.Instance.LogLevelComplete(
                            "tutorial_1",
                            1,
                            "Tutorial",
                            0, // Time not tracked in BattleResult
                            0  // Score not tracked in BattleResult
                        );
                    }
                }

                postBattleState
                    = new TutorialCompletedState(
                        this,
                        musicPlayer,
                        soundPlayer);
            }
            else if (ApplicationModel.Mode == GameMode.Skirmish || ApplicationModel.Mode == GameMode.CoinBattle)
            {
                // Track skirmish/coin battle with Firebase
                if (FirebaseAnalyticsManager.Instance != null && BattleResult != null)
                {
                    string gameMode = ApplicationModel.Mode == GameMode.Skirmish ? "Skirmish" : "CoinBattle";
                    
                    if (ApplicationModel.UserWonSkirmish)
                    {
                        FirebaseAnalyticsManager.Instance.LogLevelComplete(
                            $"skirmish_{BattleResult.LevelNum}",
                            BattleResult.LevelNum,
                            gameMode,
                            0, // Time not tracked in BattleResult
                            0  // Score not tracked in BattleResult
                        );
                    }
                    else
                    {
                        FirebaseAnalyticsManager.Instance.LogLevelFail(
                            $"skirmish_{BattleResult.LevelNum}",
                            BattleResult.LevelNum,
                            gameMode,
                            0, // Time not tracked in BattleResult
                            "defeat"
                        );
                    }
                }

                postBattleState
                    = new PostSkirmishState(
                        this,
                        musicPlayer,
                        soundPlayer);
            }
            else
            {
                /*
                string logName = "Battle_End";
#if LOG_ANALYTICS
    Debug.Log("Analytics: " + logName);
#endif
                if (UnityServices.State != ServicesInitializationState.Uninitialized)
                {
                    try
                    {
                        AnalyticsService.Instance.CustomData("Battle",
                                                                    DataProvider.GameModel.Analytics(ApplicationModel.Mode.ToString(),
                                                                                                       logName,
                                                                                                       ApplicationModel.UserWonSkirmish));
                        AnalyticsService.Instance.Flush();
                    }
                    catch (ConsentCheckException e)
                    {
                        Debug.Log("Error reason = " + e.Reason.ToString());
                    }
                }
                */

                // User completed a level
                Assert.IsNotNull(BattleResult);
                TrashTalkData trashTalkData;

                if (ApplicationModel.Mode == GameMode.SideQuest)
                    trashTalkData = await sideQuestTrashTalkList.GetTrashTalkAsync(BattleResult.LevelNum, true);
                else
                    trashTalkData = await levelTrashTalkList.GetTrashTalkAsync(BattleResult.LevelNum);

                levelName.Initialise(BattleResult.LevelNum, trashTalkData);
                if (ApplicationModel.Mode == GameMode.SideQuest)
                    levelName.gameObject.SetActive(false);

                unlockedItemSection.Initialise();
                if (desiredBehaviour == PostBattleScreenBehaviour.Defeat || !BattleResult.WasVictory)
                {
                    // Track level failure with Firebase
                    if (FirebaseAnalyticsManager.Instance != null)
                    {
                        string gameMode = ApplicationModel.Mode.ToString();
                        string failReason = DataProvider.GameModel.TimesLostOnLastLevel >= 2 ? "multiple_attempts" : "defeat";
                        
                        FirebaseAnalyticsManager.Instance.LogLevelFail(
                            $"level_{BattleResult.LevelNum}",
                            BattleResult.LevelNum,
                            gameMode,
                            0, // Time not tracked in BattleResult
                            failReason
                        );
                    }
                    
                    postBattleState = new DefeatState(this, musicPlayer);
                    title.color = Color.white; // Set title text to white for defeat
                    if (ApplicationModel.SelectedLevel == DataProvider.GameModel.CompletedLevels.Count + 1 && DataProvider.SettingsManager.AIDifficulty != Difficulty.Normal && ApplicationModel.Mode == GameMode.Campaign)
                    {
                        DataProvider.GameModel.TimesLostOnLastLevel += 1;
                        if (DataProvider.GameModel.TimesLostOnLastLevel == 3)
                        {
                            if (defeatMessage.activeSelf)
                                defeatMessage.SetActive(false);
                            if (postBattleButtonsPanel.gameObject.activeSelf)
                                postBattleButtonsPanel.gameObject.SetActive(false);
                            Debug.LogError(DataProvider.GameModel.TimesLostOnLastLevel);
                            decreaseDifficultySection.SetActive(true);
                            DataProvider.GameModel.TimesLostOnLastLevel = 0;
                        }
                    }
                }
                else
                {
                    // Track level completion with Firebase
                    if (FirebaseAnalyticsManager.Instance != null)
                    {
                        string gameMode = ApplicationModel.Mode.ToString();
                        
                        FirebaseAnalyticsManager.Instance.LogLevelComplete(
                            $"level_{BattleResult.LevelNum}",
                            BattleResult.LevelNum,
                            gameMode,
                            0, // Time not tracked in BattleResult
                            (int)DataProvider.GameModel.LifetimeDestructionScore // Use lifetime score
                        );
                        
                        // Track player progression after victory
                        FirebaseAnalyticsManager.Instance.LogPlayerProgression(
                            DataProvider.GameModel.SelectedLevel,
                            DataProvider.GameModel.NumOfLevelsCompleted,
                            0, // TODO: Add total play time tracking
                            DataProvider.GameModel.PremiumEdition
                        );
                    }
                    
                    postBattleState
                        = new VictoryState(
                            this,
                            musicPlayer,
                            soundPlayer,
                            _lootManager,
                            trashTalkData,
                            desiredBehaviour);

                    title.color = Color.black; // Set title text to black for victory
                }

                await SetupAppraisalButtonsAsync(soundPlayer, levelTrashTalkList);

                // Initialise AFTER loot manager potentially unlocks loot and next levels
                Command nextCommand = new Command(NextCommandExecute, CanNextCommandExecute);
                Command clockedGameCommand = new Command(ClockedGameCommandExecute, CanClockedGameCommandExecute);
                postBattleButtonsPanel.Initialise(this, nextCommand, clockedGameCommand, soundPlayer, BattleResult.WasVictory);
                decreaseDifficultyController.Initialise(this, soundPlayer);
            }

            SetupBackground(postBattleState.ShowVictoryBackground);
            ShowDifficultySymbolIfNeeded(postBattleState, difficultyIndicators);
        }

        private LootManager CreateLootManager()
        {
            ItemDetailsGroupController middleDetailsGroup = InitialiseGroup("UnlockedItemSection/ItemDetails/MiddleItemDetailsGroup");
            ItemDetailsGroupController leftDetailsGroup = InitialiseGroup("UnlockedItemSection/ItemDetails/LeftItemDetailsGroup");
            ItemDetailsGroupController rightDetailsGroup = InitialiseGroup("UnlockedItemSection/ItemDetails/RightItemDetailsGroup");

            return new LootManager(middleDetailsGroup, leftDetailsGroup, rightDetailsGroup);
        }

        private ItemDetailsGroupController InitialiseGroup(string componentPath)
        {
            ItemDetailsGroupController detailsGroup = transform.FindNamedComponent<ItemDetailsGroupController>(componentPath);
            detailsGroup.Initialise();
            return detailsGroup;
        }

        private void SetupBattleResult()
        {
            if (desiredBehaviour == PostBattleScreenBehaviour.Victory_Skirmish
                || desiredBehaviour == PostBattleScreenBehaviour.Defeat_Skirmish)
            {
                DataProvider.GameModel.Skirmish = CreateSkirmish();
                ApplicationModel.Mode = GameMode.Skirmish;
                ApplicationModel.UserWonSkirmish = desiredBehaviour == PostBattleScreenBehaviour.Victory_Skirmish;
            }
            else
            {
                bool wasVicotry = desiredBehaviour != PostBattleScreenBehaviour.Defeat;

                if (BattleResult == null)
                    DataProvider.GameModel.LastBattleResult = new BattleResult(levelNum, wasVicotry);

                BattleResult.LevelNum = levelNum;
                BattleResult.WasVictory = wasVicotry;

                if (desiredBehaviour == PostBattleScreenBehaviour.Victory_GameCompleted)
                    BattleResult.LevelNum = StaticData.NUM_OF_LEVELS;
            }
        }

        private SkirmishModel CreateSkirmish()
        {
            return
                new SkirmishModel(
                    Difficulty.Harder,
                    default,
                    StaticPrefabKeys.Hulls.Trident,
                    default,
                    StaticPrefabKeys.Hulls.Eagle,
                    default,
                    StrategyType.Boom,
                    backgroundLevelNum: 1);
        }

        private void SetupBackground(bool isVictory)
        {
            PostBattleBackgroundController background = GetComponentInChildren<PostBattleBackgroundController>(includeInactive: true);
            Assert.IsNotNull(background);
            background.Initalise(isVictory);
        }

        private void ShowDifficultySymbolIfNeeded(PostBattleState postBattleState, Sprite[] difficultyIndicators)
        {
            if (postBattleState.ShowDifficultySymbol)
            {
                completedDifficultySymbol.Initialise(postBattleState.Difficulty, difficultyIndicators);
                completedDifficultySymbol.gameObject.SetActive(true);
            }
        }

        private async Task SetupAppraisalButtonsAsync(SingleSoundPlayer soundPlayer, ITrashTalkProvider trashTalkList)
        {
            if (showAppraisalButtons)
            {
                await appraisalButtonsPanel.InitialiseAsync(appraisalSection, soundPlayer, trashTalkList);
                appraisalButtonsPanel.gameObject.SetActive(true);
            }
            else
                Destroy(appraisalButtonsPanel.gameObject);
        }

        public void Retry()
        {
            ApplicationModel.Mode = _gameMode;

            if (ApplicationModel.Mode != GameMode.SideQuest)
                _screensSceneGod.GoStraightToTrashScreen(BattleResult.LevelNum);
            else
                _screensSceneGod.GoToSideQuestTrashScreen(BattleResult.LevelNum);

            /*
            string logName = "Battle_Retry_Level";
#if LOG_ANALYTICS
    Debug.Log("Analytics: " + logName);
#endif
            if (UnityServices.State != ServicesInitializationState.Uninitialized)
            {
                try
                {
                    AnalyticsService.Instance.CustomData("Battle", DataProvider.GameModel.Analytics(ApplicationModel.Mode.ToString(), logName, ApplicationModel.UserWonSkirmish));
                    AnalyticsService.Instance.Flush();
                }
                catch (ConsentCheckException ex)
                {
                    Debug.Log(ex.Message);
                }
            }
            */
        }

        public void GoToLoadoutScreen()
        {
            ApplicationModel.Mode = GameMode.SideQuest;
            _screensSceneGod.GoToLoadoutScreen();
        }

        public void GoToChooseDifficultyScreen()
        {
            _screensSceneGod.GoToChooseDifficultyScreen();
        }

        private void NextCommandExecute()
        {
            int nextLevelNum = BattleResult.LevelNum + 1;
            Assert.IsTrue(nextLevelNum <= DataProvider.LockedInfo.NumOfLevelsUnlocked);
            _screensSceneGod.GoToTrashScreen(nextLevelNum);
        }

        private bool CanNextCommandExecute()
        {
            // If this was the final campaign level, NEXT should not be displayed.
            // All subsequent levels are bonuses that users can find on their own:
            if (BattleResult.LevelNum == StaticData.NUM_OF_STANDARD_LEVELS)
                return false;
            // The rest of the time we do the normal thing:
            else
                return BattleResult.LevelNum + 1 <= DataProvider.LockedInfo.NumOfLevelsUnlocked && _gameMode == GameMode.Campaign;
        }

        private void ClockedGameCommandExecute()
        {
            _screensSceneGod.LoadCutsceneScene();
        }

        private bool CanClockedGameCommandExecute()
        {
            return
                //BattleResult.WasVictory
                //&& BattleResult.LevelNum == StaticData.NUM_OF_LEVELS
                /*||*/ BattleResult.WasVictory
                && BattleResult.LevelNum == StaticData.NUM_OF_STANDARD_LEVELS;
        }

        public void GoToHomeScreen()
        {
            //_screensSceneGod.GoToHomeScreen();
            _screensSceneGod.GotoHubScreen();
        }

        public void RetryTutorial()
        {
            ApplicationModel.Mode = GameMode.Tutorial;
            _screensSceneGod.GoToTrashScreen(levelNum: 1);
        }

        public void RetrySkirmish()
        {
            ApplicationModel.Mode = GameMode.Skirmish;
            _screensSceneGod.LoadBattleScene();
            /*
            string logName = "Battle_Retry_Skirmish";
#if LOG_ANALYTICS
    Debug.Log("Analytics: " + logName);
#endif
            if (UnityServices.State != ServicesInitializationState.Uninitialized)
            {
                try
                {
                    AnalyticsService.Instance.CustomData("Battle", DataProvider.GameModel.Analytics(ApplicationModel.Mode.ToString(), logName, ApplicationModel.UserWonSkirmish));
                    AnalyticsService.Instance.Flush();
                }
                catch (ConsentCheckException ex)
                {
                    Debug.Log(ex.Message);
                }
            }
            */
        }

        public void StartLevel1()
        {
            _screensSceneGod.GoToTrashScreen(levelNum: 1);
        }

        public override void Cancel()
        {
            GoToHomeScreen();
        }
    }
}
