using BattleCruisers.Data;
using BattleCruisers.Scenes;
using BattleCruisers.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LevelsScreen
{
	public class LevelButtonController : MonoBehaviour 
	{
		public Button button;
		public Text levelName;
		public CanvasGroup canvasGroup;

		public void Initialise(int levelNum, ILevel level, bool isLevelUnlocked, IScreensSceneGod screensSceneGod)
		{
			levelName.text = levelNum + ". " + level.Name;

			if (isLevelUnlocked)
			{
				button.onClick.AddListener(() => screensSceneGod.LoadLevel(levelNum));
			}
			else
			{
				canvasGroup.alpha = Constants.DISABLED_UI_ALPHA;
			}
		}
	}
}
