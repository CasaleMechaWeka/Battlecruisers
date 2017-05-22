using BattleCruisers.Data;
using BattleCruisers.Scenes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene
{
	public class LevelButtonController : MonoBehaviour 
	{
		public Button button;
		public Text levelName;

		public void Initialise(int levelNum, ILevel level, IScreensSceneGod screensSceneGod)
		{
			levelName.text = levelNum + ". " + level.Name;
			button.onClick.AddListener(() => screensSceneGod.LoadLevel(levelNum));
		}
	}
}
