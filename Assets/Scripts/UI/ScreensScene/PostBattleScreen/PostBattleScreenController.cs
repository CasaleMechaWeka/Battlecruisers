using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Scenes;
using BattleCruisers.UI.Commands;
using BattleCruisers.UI.Common;
using BattleCruisers.UI.Common.BuildableDetails;
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

		private const string VICTORY_TITLE = "Congratulations!";
		private const string LOSS_TITLE = "Bad luck!";

        private BattleResult BattleResult { get { return _dataProvider.GameModel.LastBattleResult; } }

		public void Initialise(
            ScreensSceneGod screensSceneGod, 
            IDataProvider dataProvider,
            IPrefabFactory prefabFactory,
            ISpriteProvider spriteProvider)
		{
			base.Initialise(screensSceneGod);

            Helper.AssertIsNotNull(dataProvider, dataProvider.GameModel.LastBattleResult, prefabFactory, spriteProvider);

            _dataProvider = dataProvider;
			
            _lootManager = CreateLootManager(prefabFactory, spriteProvider);

            if (BattleResult.WasVictory)
            {
                title.text = VICTORY_TITLE;

                if (_lootManager.ShouldShowLoot(BattleResult.LevelNum))
                {
					unlockedItemSection.SetActive(true);
                    _lootManager.UnlockLoot(BattleResult.LevelNum);
                }
            }
            else
            {
                title.text = LOSS_TITLE;
            }
			
            // Initialise AFTER loot manager potentially unlocks loot and next levels
			ICommand nextCommand = new Command(NextCommandExecute, CanNextCommandExecute);
			nextButton.Initialise(nextCommand);
		}

        private ILootManager CreateLootManager(IPrefabFactory prefabFactory, ISpriteProvider spriteProvider)
        {
            IItemDetailsGroup middleDetailsGroup = InitialiseGroup(spriteProvider, "UnlockedItemSection/ItemDetails/MiddleItemDetailsGroup");
            IItemDetailsGroup leftDetailsGroup = InitialiseGroup(spriteProvider, "UnlockedItemSection/ItemDetails/LeftItemDetailsGroup");
            IItemDetailsGroup rightDetailsGroup = InitialiseGroup(spriteProvider, "UnlockedItemSection/ItemDetails/RightItemDetailsGroup");

            return new LootManager(_dataProvider, prefabFactory, middleDetailsGroup, leftDetailsGroup, rightDetailsGroup);
        }

        private IItemDetailsGroup InitialiseGroup(ISpriteProvider spriteProvider, string componentPath)
        {
            ItemDetailsGroupController detailsGroup = transform.FindNamedComponent<ItemDetailsGroupController>(componentPath);
            detailsGroup.Initialise(spriteProvider);
            return detailsGroup;
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
            Assert.IsTrue(nextLevelNum <= _dataProvider.NumOfLevelsUnlocked);
			_screensSceneGod.LoadLevel(nextLevelNum);
		}

        private bool CanNextCommandExecute()
        {
            return BattleResult.LevelNum + 1 <= _dataProvider.NumOfLevelsUnlocked;
        }

		public void GoToHomeScreen()
		{
			_screensSceneGod.GoToHomeScreen();
		}
	}
}
