using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Scenes;
using BattleCruisers.UI.Commands;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.UI.Music;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.PostBattleScreen
{
    public class PostBattleScreenController : ScreenController, IPostBattleScreen
    {
        private IApplicationModel _applicationModel;
        private IDataProvider _dataProvider;
        private ILootManager _lootManager;

		public Text title;
		public GameObject unlockedItemSection;
        public GameObject postTutorialMessage, completedGameMessage, defeatMessage, victoryNoLootMessage;
        public LevelNameController levelName;

        private const string VICTORY_TITLE = "Sweet as!";
		private const string LOSS_TITLE = "Bad luck!";

        private BattleResult BattleResult => _dataProvider.GameModel.LastBattleResult;

		public void Initialise(
            ISoundPlayer soundPlayer,
            ScreensSceneGod screensSceneGod, 
            IApplicationModel applicationModel,
            IPrefabFactory prefabFactory,
            IMusicPlayer musicPlayer)
		{
			base.Initialise(soundPlayer, screensSceneGod);

            Helper.AssertIsNotNull(
                title, 
                unlockedItemSection, 
                postTutorialMessage, 
                completedGameMessage, 
                defeatMessage,
                victoryNoLootMessage,
                levelName);
            Helper.AssertIsNotNull(applicationModel, prefabFactory, musicPlayer);

            _applicationModel = applicationModel;
            _dataProvider = applicationModel.DataProvider;
            _lootManager = CreateLootManager(prefabFactory);

            levelName.Initialise(applicationModel);
            SetupBackground();

            if (_applicationModel.IsTutorial)
            {
                // User completed (or rage quit) the tutorial
                _applicationModel.IsTutorial = false;
                postTutorialMessage.SetActive(true);
                musicPlayer.PlayVictoryMusic();

                PostTutorialButtonsPanel postTutorialButtonsPanel = GetComponentInChildren<PostTutorialButtonsPanel>(includeInactive: true);
                Assert.IsNotNull(postTutorialButtonsPanel);
                postTutorialButtonsPanel.Initialise(this, _soundPlayer);
                postTutorialButtonsPanel.gameObject.SetActive(true);
            }
            else
            {
                // User completed a level
                Assert.IsNotNull(BattleResult);

                if (BattleResult.WasVictory)
                {
                    title.text = VICTORY_TITLE;
                    musicPlayer.PlayVictoryMusic();

                    if (_lootManager.ShouldShowLoot(BattleResult.LevelNum))
                    {
                        unlockedItemSection.SetActive(true);
                        _lootManager.UnlockLoot(BattleResult.LevelNum);
                    }
                    else if (BattleResult.LevelNum == _dataProvider.Levels.Count
                        && BattleResult.LevelNum > _dataProvider.GameModel.NumOfLevelsCompleted)
                    {
                        // Completed last level for the frist time
                        completedGameMessage.SetActive(true);
                    }
                    else
                    {
                        victoryNoLootMessage.SetActive(true);
                    }

                    CompletedLevel level = new CompletedLevel(levelNum: BattleResult.LevelNum, hardestDifficulty: _dataProvider.SettingsManager.AIDifficulty);
                    _dataProvider.GameModel.AddCompletedLevel(level);
                    _dataProvider.SaveGame();
                }
                else
                {
                    title.text = LOSS_TITLE;
                    defeatMessage.SetActive(true);
                    musicPlayer.PlayDefeatMusic();
                }

                // Initialise AFTER loot manager potentially unlocks loot and next levels
                ICommand nextCommand = new Command(NextCommandExecute, CanNextCommandExecute);

                PostBattleButtonsPanel postBattleButtonsPanel = GetComponentInChildren<PostBattleButtonsPanel>(includeInactive: true);
                Assert.IsNotNull(postBattleButtonsPanel);
                postBattleButtonsPanel.Initialise(this, nextCommand, _soundPlayer);
                postBattleButtonsPanel.gameObject.SetActive(true);
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

        private void SetupBackground()
        {
            PostBattleBackgroundController background = GetComponentInChildren<PostBattleBackgroundController>(includeInactive: true);
            Assert.IsNotNull(background);
            bool isVictory = BattleResult == null || BattleResult.WasVictory;
            background.Initalise(isVictory);
        }

		public void Retry()
		{
			_screensSceneGod.LoadLevel(BattleResult.LevelNum);
		}

		public void GoToLoadoutScreen()
		{
			_screensSceneGod.GoToLoadoutScreen();
		}

        private void NextCommandExecute()
		{
			int nextLevelNum = BattleResult.LevelNum + 1;
            Assert.IsTrue(nextLevelNum <= _dataProvider.LockedInfo.NumOfLevelsUnlocked);
			_screensSceneGod.LoadLevel(nextLevelNum);
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
            _screensSceneGod.LoadLevel(levelNum: 1);
        }

        public void StartLevel1()
        {
            _screensSceneGod.LoadLevel(levelNum: 1);
        }
	}
}
