using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Settings;
using BattleCruisers.Data.Models;
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
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Fetchers.Sprites;
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
        Defeat_Skirmish
    }

    public class PostBattleScreenController : ScreenController, IPostBattleScreen
    {
        private IApplicationModel _applicationModel;
        private IDataProvider _dataProvider;
        private ILootManager _lootManager;

		public Text title;
		public SlidingPanel unlockedItemSection;
        public GameObject defeatMessage, victoryNoLootMessage, demoCompletedScreen;
        public LevelNameController levelName;
        public LevelStatsController completedDifficultySymbol;
        public CanvasGroupButton demoHomeButton;
        public PostTutorialButtonsPanel postTutorialButtonsPanel;
        public PostBattleButtonsPanel postBattleButtonsPanel;
        public PostSkirmishButtonsPanel postSkirmishButtonsPanel;
        public AppraisalSectionController appraisalSection;
        public AppraisalButtonsPanel appraisalButtonsPanel;

        [Header("Can change these for testing")]
        public PostBattleScreenBehaviour desiredBehaviour;
        [Range(1, 25)]
        public int levelNum = 2;
        public bool showAppraisalButtons = false;

        private BattleResult BattleResult => _dataProvider.GameModel.LastBattleResult;

		public async Task InitialiseAsync(
            IScreensSceneGod screensSceneGod,
            ISingleSoundPlayer soundPlayer,
            IApplicationModel applicationModel,
            IPrefabFactory prefabFactory,
            IMusicPlayer musicPlayer,
            IDifficultySpritesProvider difficultySpritesProvider,
            ITrashTalkProvider trashTalkList)
		{
			base.Initialise(screensSceneGod);

            Helper.AssertIsNotNull(
                title,
                unlockedItemSection,
                defeatMessage,
                victoryNoLootMessage,
                demoCompletedScreen,
                levelName,
                completedDifficultySymbol,
                demoHomeButton,
                trashTalkList,
                postTutorialButtonsPanel,
                postBattleButtonsPanel,
                postSkirmishButtonsPanel,
                appraisalSection,
                appraisalButtonsPanel);
            Helper.AssertIsNotNull(applicationModel, prefabFactory, musicPlayer, difficultySpritesProvider);

            _applicationModel = applicationModel;
            _dataProvider = applicationModel.DataProvider;
            _lootManager = CreateLootManager(prefabFactory);

            if (desiredBehaviour != PostBattleScreenBehaviour.Default)
            {
                SetupBattleResult();
            }

            IPostBattleState postBattleState = null;

            if (desiredBehaviour == PostBattleScreenBehaviour.TutorialCompleted
                || _applicationModel.IsTutorial)
            {
                postBattleState
                    = new TutorialCompletedState(
                        this,
                        _applicationModel,
                        musicPlayer,
                        soundPlayer); 
            }
            else if (_applicationModel.Mode == GameMode.Skirmish)
            {
                postBattleState
                    = new PostSkirmishState(
                        this,
                        _applicationModel,
                        musicPlayer,
                        soundPlayer);
            }
            else
            {
                // User completed a level
                Assert.IsNotNull(BattleResult);
                ITrashTalkData levelTrashTalkData = await trashTalkList.GetTrashTalkAsync(BattleResult.LevelNum);
                levelName.Initialise(BattleResult.LevelNum, levelTrashTalkData);
                unlockedItemSection.Initialise();

                if (desiredBehaviour == PostBattleScreenBehaviour.Defeat
                    || !BattleResult.WasVictory)
                {
                    postBattleState = new DefeatState(this, _applicationModel, musicPlayer);
                }
                else
                {
                    postBattleState 
                        = new VictoryState(
                            this, 
                            _applicationModel, 
                            musicPlayer,
                            soundPlayer,
                            _lootManager,
                            levelTrashTalkData,
                            desiredBehaviour);
                }

                await SetupAppraisalButtonsAsync(soundPlayer, trashTalkList);

                // Initialise AFTER loot manager potentially unlocks loot and next levels
                ICommand nextCommand = new Command(NextCommandExecute, CanNextCommandExecute);
                ICommand clockedGameCommand = new Command(ClockedGameCommandExecute, CanClockedGameCommandExecute);
                postBattleButtonsPanel.Initialise(this, nextCommand, clockedGameCommand, soundPlayer, BattleResult.WasVictory);
            }

            SetupBackground(postBattleState.ShowVictoryBackground);
            await ShowDifficultySymbolIfNeeded(postBattleState, difficultySpritesProvider);
        }

        private ILootManager CreateLootManager(IPrefabFactory prefabFactory)
        {
            IItemDetailsGroup middleDetailsGroup = InitialiseGroup("UnlockedItemSection/ItemDetails/MiddleItemDetailsGroup");
            IItemDetailsGroup leftDetailsGroup = InitialiseGroup("UnlockedItemSection/ItemDetails/LeftItemDetailsGroup");
            IItemDetailsGroup rightDetailsGroup = InitialiseGroup("UnlockedItemSection/ItemDetails/RightItemDetailsGroup");

            return new LootManager(_dataProvider, prefabFactory, middleDetailsGroup, leftDetailsGroup, rightDetailsGroup);
        }

        private IItemDetailsGroup InitialiseGroup(string componentPath)
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
                _applicationModel.DataProvider.GameModel.Skirmish = CreateSkirmish();
                _applicationModel.Mode = GameMode.Skirmish;
                _applicationModel.UserWonSkirmish = desiredBehaviour == PostBattleScreenBehaviour.Victory_Skirmish;
            }
            else
            {
                bool wasVicotry = desiredBehaviour != PostBattleScreenBehaviour.Defeat;

                if (BattleResult == null)
                {
                    _dataProvider.GameModel.LastBattleResult = new BattleResult(levelNum, wasVicotry);
                }

                BattleResult.LevelNum = levelNum;
                BattleResult.WasVictory = wasVicotry;

                if (desiredBehaviour == PostBattleScreenBehaviour.Victory_GameCompleted)
                {
                    BattleResult.LevelNum = StaticData.NUM_OF_LEVELS;
                }
            }
        }

        private SkirmishModel CreateSkirmish()
        {
            return
                new SkirmishModel(
                    Difficulty.Harder,
                    StaticPrefabKeys.Hulls.Eagle,
                    StrategyType.Boom);
        }

        private void SetupBackground(bool isVictory)
        {
            PostBattleBackgroundController background = GetComponentInChildren<PostBattleBackgroundController>(includeInactive: true);
            Assert.IsNotNull(background);
            background.Initalise(isVictory);
        }

        private async Task ShowDifficultySymbolIfNeeded(IPostBattleState postBattleState, IDifficultySpritesProvider difficultySpritesProvider)
        {
            if (postBattleState.ShowDifficultySymbol)
            {
                await completedDifficultySymbol.InitialiseAsync(postBattleState.Difficulty, difficultySpritesProvider);
                completedDifficultySymbol.gameObject.SetActive(true);
            }
        }

        private async Task SetupAppraisalButtonsAsync(ISingleSoundPlayer soundPlayer, ITrashTalkProvider trashTalkList)
        {
            if (showAppraisalButtons)
            {
                await appraisalButtonsPanel.InitialiseAsync(appraisalSection, soundPlayer, trashTalkList);
                appraisalButtonsPanel.gameObject.SetActive(true);
            }
            else
            {
                Destroy(appraisalButtonsPanel.gameObject);
            }
        }

        public void Retry()
		{
			_screensSceneGod.GoToTrashScreen(BattleResult.LevelNum);
		}

		public void GoToLoadoutScreen()
		{
			_screensSceneGod.GoToLoadoutScreen();
		}

        public void GoToChooseDifficultyScreen()
        {
            _screensSceneGod.GoToChooseDifficultyScreen();
        }

        private void NextCommandExecute()
		{
			int nextLevelNum = BattleResult.LevelNum + 1;
            Assert.IsTrue(nextLevelNum <= _dataProvider.LockedInfo.NumOfLevelsUnlocked);
			_screensSceneGod.GoToTrashScreen(nextLevelNum);
		}

        private bool CanNextCommandExecute()
        {
            return BattleResult.LevelNum + 1 <= _dataProvider.LockedInfo.NumOfLevelsUnlocked;
        }

        private void ClockedGameCommandExecute()
        {
            _screensSceneGod.LoadCreditsScene();
        }

        private bool CanClockedGameCommandExecute()
        {
            return
                BattleResult.WasVictory
                && BattleResult.LevelNum == StaticData.NUM_OF_LEVELS;
        }

        public void GoToHomeScreen()
		{
			_screensSceneGod.GoToHomeScreen();
		}

        public void RetryTutorial()
        {
            _applicationModel.Mode = GameMode.Tutorial;
            _screensSceneGod.GoToTrashScreen(levelNum: 1);
        }

        public void RetrySkirmish()
        {
            _applicationModel.Mode = GameMode.Skirmish;
            _screensSceneGod.LoadBattleScene();
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
