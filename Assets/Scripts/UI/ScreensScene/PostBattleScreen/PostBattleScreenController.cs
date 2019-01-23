using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Scenes;
using BattleCruisers.UI.Commands;
using BattleCruisers.UI.Common;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.UI.Music;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.PostBattleScreen
{
    public class PostBattleScreenController : ScreenController
	{
        private IDataProvider _dataProvider;
        private ILootManager _lootManager;

		public Text title;
		public GameObject unlockedItemSection;
		public ButtonController nextButton;
        public GameObject postBattleButtonsPanel, postTutorialButtonsPanel;
        public GameObject postTutorialMessage, completedGameMessage;

		private const string VICTORY_TITLE = "Congratulations!";
		private const string LOSS_TITLE = "Bad luck!";

        private BattleResult BattleResult { get { return _dataProvider.GameModel.LastBattleResult; } }

		public void Initialise(
            ScreensSceneGod screensSceneGod, 
            IDataProvider dataProvider,
            IPrefabFactory prefabFactory,
            IMusicPlayer musicPlayer)
		{
			base.Initialise(screensSceneGod);

            Helper.AssertIsNotNull(title, unlockedItemSection, nextButton, postBattleButtonsPanel, postTutorialButtonsPanel, postTutorialMessage, completedGameMessage);
            Helper.AssertIsNotNull(dataProvider, prefabFactory, musicPlayer);

            _dataProvider = dataProvider;
            _lootManager = CreateLootManager(prefabFactory);

            SetupBackground();

            if (BattleResult == null)
            {
                // User completed (or rage quit) the tutorial
                postTutorialMessage.SetActive(true);
                postTutorialButtonsPanel.SetActive(true);
                musicPlayer.PlayVictoryMusic();
            }
            else
            {
                // User completed a level
                postBattleButtonsPanel.SetActive(true);

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

                    CompletedLevel level = new CompletedLevel(levelNum: BattleResult.LevelNum, hardestDifficulty: _dataProvider.SettingsManager.AIDifficulty);
                    _dataProvider.GameModel.AddCompletedLevel(level);
                    _dataProvider.SaveGame();
                }
                else
                {
                    title.text = LOSS_TITLE;
                }

                // Initialise AFTER loot manager potentially unlocks loot and next levels
                ICommand nextCommand = new Command(NextCommandExecute, CanNextCommandExecute);
                nextButton.Initialise(nextCommand);
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
