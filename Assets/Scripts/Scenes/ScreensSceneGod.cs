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
		// Home Menu
		void Continue();
		void GoToLevelsScreen();
		void Quit();

		// levels Menu
		void LoadLevel(int levelNum);
		void GoToHomeScreen();
	}

	public class ScreensSceneGod : MonoBehaviour, IScreensSceneGod
	{
		public ScreenController _currentScreen;

		public UIFactory uiFactory;
		public HomeScreenController homeScreen;
		public LevelsScreenController levelsScreen;


		void Start()
		{
			IDataProvider dataProvider = ApplicationModel.DataProvider;
			levelsScreen.Initialise(uiFactory, this, dataProvider.Levels);

			_currentScreen = homeScreen;
		}
		
		#region HomeMenu
		// FELIX  Hide if first time
		// FELIX  Start last level played OR next level if user won last level and then quit.
		public void Continue()
		{
			Debug.Log("Continue()");
		}

		public void GoToLevelsScreen()
		{
			GoToScreen(levelsScreen);
		}

		public void Quit()
		{
			Application.Quit();
		}
		#endregion HomeMenu

		#region LevelsMenu
		public void LoadLevel(int levelNum)
		{
			ApplicationModel.SelectedLevel = levelNum;
			SceneManager.LoadScene(SceneNames.BATTLE_SCENE);
		}

		public void GoToHomeScreen()
		{
			GoToScreen(homeScreen);
		}
		#endregion LevelsMenu

		private void GoToScreen(ScreenController destinationScreen)
		{
			Assert.AreNotEqual(_currentScreen, destinationScreen);

			_currentScreen.gameObject.SetActive(false);

			_currentScreen = destinationScreen;
			_currentScreen.gameObject.SetActive(true);
		}
	}
}
