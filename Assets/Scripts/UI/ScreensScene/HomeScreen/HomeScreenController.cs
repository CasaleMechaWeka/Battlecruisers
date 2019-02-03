using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Scenes;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.HomeScreen
{
    public class HomeScreenController : ScreenController
	{
		private BattleResult _lastBattleResult;
		private int _totalNumOfLevels;

        private Button _firstTimePlayButton, _continueButton, _selectLevelButton, _loadoutButton, _settingsButton, _tutorialButton, _quitButton;

        public void Initialise(IScreensSceneGod screensSceneGod, IGameModel gameModel, int totalNumOfLevels)
		{
			base.Initialise(screensSceneGod);

            Assert.IsNotNull(gameModel);

            _firstTimePlayButton = transform.FindNamedComponent<Button>("FirstTimePlayButton");
            _firstTimePlayButton.onClick.AddListener(StartTutorial);

            _continueButton = transform.FindNamedComponent<Button>("ContinueButton");
            _continueButton.onClick.AddListener(Continue);

            _selectLevelButton = transform.FindNamedComponent<Button>("SelectLevelButton");
            _selectLevelButton.onClick.AddListener(GoToLevelsScreen);

            _loadoutButton = transform.FindNamedComponent<Button>("LoadoutButton");
            _loadoutButton.onClick.AddListener(GoToLoadoutScreen);

            _settingsButton = transform.FindNamedComponent<Button>("SettingsButton");
            _settingsButton.onClick.AddListener(GoToSettingsScreen);

            _tutorialButton = transform.FindNamedComponent<Button>("TutorialButton");
            _tutorialButton.onClick.AddListener(StartTutorial);

            _quitButton = transform.FindNamedComponent<Button>("QuitButton");
            _quitButton.onClick.AddListener(Quit);

            _lastBattleResult = gameModel.LastBattleResult;
			_totalNumOfLevels = totalNumOfLevels;

            if (gameModel.HasAttemptedTutorial)
            {
                _firstTimePlayButton.gameObject.SetActive(false);
            }
            else
            {
                _selectLevelButton.gameObject.SetActive(false);
                _tutorialButton.gameObject.SetActive(false);
                _loadoutButton.gameObject.SetActive(false);
                _settingsButton.gameObject.SetActive(false);
            }

            // Player has never played a (non-tutorial) battle!
			if (_lastBattleResult == null)
			{
				_continueButton.gameObject.SetActive(false);
			}
		}

		private void Continue()
		{
			Assert.IsNotNull(_lastBattleResult);

			int nextLevelToPlay = _lastBattleResult.LevelNum;

			if (_lastBattleResult.WasVictory && nextLevelToPlay < _totalNumOfLevels)
			{
				nextLevelToPlay++;
			}

			_screensSceneGod.LoadLevel(nextLevelToPlay);
		}

		private void GoToLevelsScreen()
		{
			_screensSceneGod.GoToLevelsScreen();
		}

		private void GoToLoadoutScreen()
		{
			_screensSceneGod.GoToLoadoutScreen();
		}

        private void GoToSettingsScreen()
        {
            _screensSceneGod.GoToSettingsScreen();
        }

        private void StartTutorial()
        {
            ApplicationModelProvider.ApplicationModel.IsTutorial = true;
            _screensSceneGod.LoadLevel(levelNum: 1);
        }

		private void Quit()
		{
			Application.Quit();
		}
	}
}
