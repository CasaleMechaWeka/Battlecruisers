using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Scenes;
using BattleCruisers.UI.Commands;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.UI.Music;
using BattleCruisers.UI.Panels;
using BattleCruisers.UI.ScreensScene.LevelsScreen;
using BattleCruisers.UI.ScreensScene.PostBattleScreen.States;
using BattleCruisers.UI.ScreensScene.TrashScreen;
using BattleCruisers.UI.Sound;
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
        Victory_NoNewLoot,  // Currently the same behaviour as Victory_LootUnlocked
        Victory_DemoCompleted,
        Victory_GameCompleted
    }

    public class PostBattleScreenController : ScreenController, IPostBattleScreen
    {
        private IApplicationModel _applicationModel;
        private IDataProvider _dataProvider;
        private ILootManager _lootManager;

		public Text title;
		public SlidingPanel unlockedItemSection;
        public GameObject completedGameMessage, defeatMessage, victoryNoLootMessage, demoCompletedScreen;
        public LevelNameController levelName;
        public LevelStatsController completedDifficultySymbol;
        public ActionButton demoHomeButton;
        public PostTutorialButtonsPanel postTutorialButtonsPanel;
        public PostBattleButtonsPanel postBattleButtonsPanel;
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
                completedGameMessage,
                defeatMessage,
                victoryNoLootMessage,
                demoCompletedScreen,
                levelName,
                completedDifficultySymbol,
                demoHomeButton,
                trashTalkList,
                postTutorialButtonsPanel,
                postBattleButtonsPanel,
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

            SetupBackground();

            if (desiredBehaviour == PostBattleScreenBehaviour.TutorialCompleted
                || _applicationModel.IsTutorial)
            {
                new TutorialCompletedState()
                    .Initialise(
                    this, 
                    _applicationModel,
                    soundPlayer, 
                    musicPlayer);
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
                    new DefeatState().Initialise(this, musicPlayer);
                }
                else
                {
                    await new VictoryState()
                        .InitialiseAsync(
                            this,
                            soundPlayer,
                            musicPlayer,
                            _dataProvider,
                            difficultySpritesProvider,
                            _lootManager,
                            levelTrashTalkData,
                            desiredBehaviour);
                }

                SetupAppraisalButtons(soundPlayer, trashTalkList);

                // Initialise AFTER loot manager potentially unlocks loot and next levels
                ICommand nextCommand = new Command(NextCommandExecute, CanNextCommandExecute);
                postBattleButtonsPanel.Initialise(this, nextCommand, soundPlayer, BattleResult.WasVictory);
            }
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
            bool wasVicotry = desiredBehaviour != PostBattleScreenBehaviour.Defeat;

            if (BattleResult == null)
            {
                _dataProvider.GameModel.LastBattleResult = new BattleResult(levelNum, wasVicotry);
            }

            BattleResult.LevelNum = levelNum;
            BattleResult.WasVictory = wasVicotry;
        }

        private void SetupBackground()
        {
            PostBattleBackgroundController background = GetComponentInChildren<PostBattleBackgroundController>(includeInactive: true);
            Assert.IsNotNull(background);
            bool isVictory = _applicationModel.IsTutorial || BattleResult.WasVictory;
            background.Initalise(isVictory);
        }

        private void SetupAppraisalButtons(ISingleSoundPlayer soundPlayer, ITrashTalkProvider trashTalkList)
        {
            if (showAppraisalButtons)
            {
                appraisalButtonsPanel.InitialiseAsync(appraisalSection, soundPlayer, trashTalkList);
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

		public void GoToHomeScreen()
		{
			_screensSceneGod.GoToHomeScreen();
		}

        public void RetryTutorial()
        {
            ApplicationModelProvider.ApplicationModel.IsTutorial = true;
            _screensSceneGod.GoToTrashScreen(levelNum: 1);
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
