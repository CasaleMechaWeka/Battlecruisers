using BattleCruisers.Data;
using BattleCruisers.UI.ScreensScene;
using BattleCruisers.UI.ScreensScene.LevelsScreen;
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
		private ScreenController _currentScreen;

		public UIFactory uiFactory;
		public HomeScreenController homeScreen;
		public LevelsScreenController levelsScreen;
		public PostBattleScreenController postBattleScreen;

		void Start()
		{
			// FELIX  TEMP  Force PostBattleScreen
			ApplicationModel.BattleResult = new BattleResult(1, true);


			IDataProvider dataProvider = ApplicationModel.DataProvider;
			levelsScreen.Initialise(uiFactory, this, dataProvider.Levels);
			homeScreen.Initialise(this);

			if (ApplicationModel.BattleResult != null)
			{
				postBattleScreen.Initialize(ApplicationModel.BattleResult, this);
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
			throw new NotImplementedException();
		}
		
		public void LoadLevel(int levelNum)
		{
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
