using BattleCruisers.Data;
using BattleCruisers.UI.ScreensScene;
using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BattleCruisers.Scenes
{
	public interface IScreensSceneGod
	{
		// Home Menu
		void Continue();
		void GoToLevelsMenu();
		void Quit();

		// levels Menu
		void LoadLevel(int levelNum);
		void GoToHomeMenu();
	}

	public class ScreensSceneGod : MonoBehaviour, IScreensSceneGod
	{
		public UIFactory uiFactory;
		public GameObject homePanel;
		public LevelsPanelController levelsPanelController;

		void Start()
		{
			IDataProvider dataProvider = ApplicationModel.DataProvider;
			levelsPanelController.Initialise(uiFactory, this, dataProvider.Levels);
		}
		
		#region HomeMenu
		// FELIX  Hide if first time
		// FELIX  Start last level played OR next level if user won last level and then quit.
		public void Continue()
		{
			Debug.Log("Continue()");
		}

		public void GoToLevelsMenu()
		{
			homePanel.SetActive(false);
			levelsPanelController.gameObject.SetActive(true);
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

		public void GoToHomeMenu()
		{
			levelsPanelController.gameObject.SetActive(false);
			homePanel.SetActive(true);
		}
		#endregion LevelsMenu
	}
}
