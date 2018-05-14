using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Scenes;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene
{
    public class HomeScreenController : ScreenController
	{
		private BattleResult _lastBattleResult;
		private int _totalNumOfLevels;

        public Button firstTimePlayButton;
		public Button continueButton;
        public Button selectLevelButton;
        public Button tutorialButton;

        public void Initialise(IScreensSceneGod screensSceneGod, IGameModel gameModel, int totalNumOfLevels)
		{
			base.Initialise(screensSceneGod);

            Assert.IsNotNull(gameModel);

            _lastBattleResult = gameModel.LastBattleResult;
			_totalNumOfLevels = totalNumOfLevels;

            if (gameModel.HasAttemptedTutorial)
            {
                firstTimePlayButton.gameObject.SetActive(false);
            }
            else
            {
                selectLevelButton.gameObject.SetActive(false);
                tutorialButton.gameObject.SetActive(false);
            }

            // Player has never played a (non-tutorial) battle!
			if (_lastBattleResult == null)
			{
				continueButton.gameObject.SetActive(false);
			}
		}

		public void Continue()
		{
			Assert.IsNotNull(_lastBattleResult);

			int nextLevelToPlay = _lastBattleResult.LevelNum;

			if (_lastBattleResult.WasVictory && nextLevelToPlay < _totalNumOfLevels)
			{
				nextLevelToPlay++;
			}

			_screensSceneGod.LoadLevel(nextLevelToPlay);
		}

		public void GoToLevelsScreen()
		{
			_screensSceneGod.GoToLevelsScreen();
		}

		public void GoToLoadoutScreen()
		{
			_screensSceneGod.GoToLoadoutScreen();
		}

        public void GoToSettingsScreen()
        {
            _screensSceneGod.GoToSettingsScreen();
        }

        public void StartTutorial()
        {
            ApplicationModel.IsTutorial = true;
            _screensSceneGod.LoadLevel(levelNum: 1);
        }

		public void Quit()
		{
			Application.Quit();
		}
	}
}
