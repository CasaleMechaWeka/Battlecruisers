using BattleCruisers.Data;
using BattleCruisers.Data.PrefabKeys;
using BattleCruisers.Fetchers;
using BattleCruisers.UI.ScreensScene;
using BattleCruisers.UI.ScreensScene.LevelsScreen;
using BattleCruisers.UI.ScreensScene.LoadoutScreen;
using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace BattleCruisers.Scenes
{
	public interface IScreensSceneGod
	{
		void GoToLevelsScreen();
		void GoToHomeScreen();
		void GoToLoadoutScreen();

		void LoadLevel(int levelNum);
	}

	public class ScreensSceneGod : MonoBehaviour, IScreensSceneGod
	{
		private PrefabFactory _prefabFactory;
		private ScreenController _currentScreen;
		private IDataProvider _dataProvider;
		private IGameModel _gameModel;

		public HomeScreenController homeScreen;
		public LevelsScreenController levelsScreen;
		public PostBattleScreenController postBattleScreen;
		public LoadoutScreenController loadoutScreen;

		void Start()
		{
			_prefabFactory = new PrefabFactory(new PrefabFetcher());
			_dataProvider = ApplicationModel.DataProvider;
			_gameModel = _dataProvider.GameModel;


			// FELIX  TEMP
//			ApplicationModel.BattleResult = new BattleResult(1, false);
//			ApplicationModel.BattleResult = new BattleResult(1, true);
//			_gameModel.LastBattleResult = null;
//			ApplicationModel.ShowPostBattleScreen = false;


			levelsScreen.Initialise(this, _dataProvider.Levels, _dataProvider.NumOfLevelsUnlocked);
			homeScreen.Initialise(this, _gameModel.LastBattleResult, _dataProvider.Levels.Count);
			loadoutScreen.Initialise(this, _dataProvider, _prefabFactory);

			
			if (ApplicationModel.ShowPostBattleScreen)
			{
				ApplicationModel.ShowPostBattleScreen = false;
				postBattleScreen.Initialise(_gameModel.LastBattleResult, this, _dataProvider.NumOfLevelsUnlocked);
				GoToScreen(postBattleScreen);
			}
			else
			{
				GoToHomeScreen();
			}
		}
		
		public void GoToLevelsScreen()
		{
			GoToScreen(levelsScreen);
		}

		public void GoToHomeScreen()
		{
			GoToScreen(homeScreen);
		}

		public void GoToLoadoutScreen()
		{
			GoToScreen(loadoutScreen);
		}

		public void LoadLevel(int levelNum)
		{
			Assert.IsTrue(levelNum <= _dataProvider.NumOfLevelsUnlocked);

			ApplicationModel.SelectedLevel = levelNum;
			SceneManager.LoadScene(SceneNames.BATTLE_SCENE);
		}

		private void GoToScreen(ScreenController destinationScreen)
		{
			Assert.AreNotEqual(_currentScreen, destinationScreen);

			if (_currentScreen != null)
			{
				_currentScreen.gameObject.SetActive(false);
			}

			_currentScreen = destinationScreen;
			_currentScreen.gameObject.SetActive(true);
		}
	}
}
