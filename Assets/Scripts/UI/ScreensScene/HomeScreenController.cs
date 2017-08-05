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

		public Button continueButton;

		public void Initialise(IScreensSceneGod screensSceneGod, BattleResult lastBattleResult, int totalNumOfLevels)
		{
			base.Initialise(screensSceneGod);

			_lastBattleResult = lastBattleResult;
			_totalNumOfLevels = totalNumOfLevels;

			// Player has never played a battle!
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

		public void Quit()
		{
			Application.Quit();
		}
	}
}
