using BattleCruisers.Data.Models;
using BattleCruisers.Scenes;
using BattleCruisers.UI.Commands;
using BattleCruisers.UI.Common;
using BattleCruisers.UI.Common.BuildingDetails;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene
{
    public class PostBattleScreenController : ScreenController
	{
		private BattleResult _battleResult;
		private int _numOfLevelsUnlocked;

        private IItemDetailsGroup _middleDetailsGroup, _leftDetailsGroup, _rightDetailsGroup;

        // FELIX  Initialise programmatically
		public Text title;
		public GameObject unlockedItemSection;
		public ButtonController nextButton;

		private const string VICTORY_TITLE = "Congratulations!";
		private const string LOSS_TITLE = "Bad luck!";

		public void Initialise(
            BattleResult battleResult, 
            ScreensSceneGod screensSceneGod, 
            int numOfLevelsUnlocked, 
            ISpriteProvider spriteProvider)
		{
			base.Initialise(screensSceneGod);

            Helper.AssertIsNotNull(battleResult, spriteProvider);

            _battleResult = battleResult;
            _numOfLevelsUnlocked = numOfLevelsUnlocked;
			
            InitialiseDetailGroups(spriteProvider);

            ICommand nextCommand = new Command(NextCommandExecute, CanNextCommandExecute);
            nextButton.Initialise(nextCommand);

            unlockedItemSection.SetActive(_battleResult.WasVictory);

            if (_battleResult.WasVictory)
            {
                title.text = VICTORY_TITLE;

                // FELIX  Only show if first time level completed :P
                // 1. Show loot
                // 2. Add loot to unlocked items in GameModel
                // 3. Add loot to player loadout
            }
            else
            {
                title.text = LOSS_TITLE;
			}
		}

        private void InitialiseDetailGroups(ISpriteProvider spriteProvider)
        {
            _middleDetailsGroup = InitialiseGroup(spriteProvider, "UnlockedItemSection/ItemDetails/MiddleItemDetailsGroup");
            _leftDetailsGroup = InitialiseGroup(spriteProvider, "UnlockedItemSection/ItemDetails/LeftItemDetailsGroup");
            _rightDetailsGroup = InitialiseGroup(spriteProvider, "UnlockedItemSection/ItemDetails/RightItemDetailsGroup");
        }

        private IItemDetailsGroup InitialiseGroup(ISpriteProvider spriteProvider, string componentPath)
        {
            ItemDetailsGroupController detailsGroup = transform.FindNamedComponent<ItemDetailsGroupController>(componentPath);
            detailsGroup.Initialise(spriteProvider);
            return detailsGroup;
        }

		public void Retry()
		{
			_screensSceneGod.LoadLevel(_battleResult.LevelNum);
		}

		public void GoToLoadoutScreen()
		{
			_screensSceneGod.GoToLoadoutScreen();
		}

        private void NextCommandExecute()
		{
			int nextLevelNum = _battleResult.LevelNum + 1;
			Assert.IsTrue(nextLevelNum <= _numOfLevelsUnlocked);
			_screensSceneGod.LoadLevel(nextLevelNum);
		}

        private bool CanNextCommandExecute()
        {
            return _battleResult.LevelNum + 1 <= _numOfLevelsUnlocked;
        }

		public void GoToHomeScreen()
		{
			_screensSceneGod.GoToHomeScreen();
		}
	}
}
