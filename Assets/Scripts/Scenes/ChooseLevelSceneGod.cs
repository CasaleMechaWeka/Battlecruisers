using BattleCruisers.Data;
using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BattleCruisers.Scenes
{
	public class ChooseLevelSceneGod : MonoBehaviour 
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
