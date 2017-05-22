using BattleCruisers.Data;
using BattleCruisers.UI.ChooseLevelScene;
using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BattleCruisers.Scenes
{
	public interface IChooseLevelSceneGod
	{
		void LoadLevel(int levelNum);
		void Quit();
	}

	public class ChooseLevelSceneGod : MonoBehaviour, IChooseLevelSceneGod
	{
		public UIFactory uiFactory;
		public LevelsPanelController levelsPanelController;

		void Start()
		{
			IDataProvider dataProvider = ApplicationModel.DataProvider;
			levelsPanelController.Initialise(uiFactory, this, dataProvider.Levels);
		}

		public void LoadLevel(int levelNum)
		{
			ApplicationModel.SelectedLevel = levelNum;
			SceneManager.LoadScene(SceneNames.BATTLE_SCENE);
		}

		public void Quit()
		{
			throw new NotImplementedException();
		}
	}
}
