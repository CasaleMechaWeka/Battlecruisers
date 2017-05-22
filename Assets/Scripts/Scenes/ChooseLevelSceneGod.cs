using BattleCruisers.Data;
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
