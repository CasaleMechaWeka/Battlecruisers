using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Scenes;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.HomeScreen
{
    public class HomeScreenController : ScreenController, IHomeScreen
	{
		private BattleResult _lastBattleResult;
		private int _totalNumOfLevels;

        private Button _firstTimePlayButton, _continueButton, _selectLevelButton, _loadoutButton, _settingsButton, _tutorialButton, _quitButton;

        public void Initialise(IScreensSceneGod screensSceneGod, ISoundPlayer soundPlayer, IGameModel gameModel, int totalNumOfLevels)
		{
			base.Initialise(screensSceneGod, soundPlayer);

            Assert.IsNotNull(gameModel);

            _lastBattleResult = gameModel.LastBattleResult;
            _totalNumOfLevels = totalNumOfLevels;

            HomeScreenLayout layout = GetLayout(gameModel);
            layout.Initialise(this, gameModel, soundPlayer);
            layout.IsVisible = true;
		}

        private HomeScreenLayout GetLayout(IGameModel gameModel)
        {
            // TEMP  Force layouyts :)
            //return transform.FindNamedComponent<HomeScreenLayout>("FirstTimeLayout");
            //return transform.FindNamedComponent<HomeScreenLayout>("FirstTimeNonTutorial");
            //return transform.FindNamedComponent<HomeScreenLayout>("DefaultLayout");

            if (!gameModel.HasAttemptedTutorial)
            {
                // First time play
                return transform.FindNamedComponent<HomeScreenLayout>("FirstTimeLayout");
            }
            else if (_lastBattleResult == null
                && gameModel.NumOfLevelsCompleted == 0)
            {
                // First time playing non-tutorial
                return transform.FindNamedComponent<HomeScreenLayout>("FirstTimeNonTutorial");
            }
            else
            {
                // Normal play
                return transform.FindNamedComponent<HomeScreenLayout>("DefaultLayout");
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
            ApplicationModelProvider.ApplicationModel.IsTutorial = true;
            _screensSceneGod.LoadLevel(levelNum: 1);
        }

        public void Quit()
		{
			Application.Quit();
		}

        public void StartLevel1()
        {
            _screensSceneGod.LoadLevel(levelNum: 1);
        }
    }
}
