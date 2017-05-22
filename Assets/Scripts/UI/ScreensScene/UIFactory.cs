using BattleCruisers.Data;
using BattleCruisers.Scenes;
using BattleCruisers.UI.ScreensScene.LevelsScreen;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene
{
	public interface IUIFactory
	{
		void CreateLevelButton(HorizontalOrVerticalLayoutGroup buttonParent, int levelNum, ILevel level, IScreensSceneGod screensSceneGod);
		void CreateHomeButton(HorizontalOrVerticalLayoutGroup buttonParent, IScreensSceneGod screensSceneGod);
	}

	public class UIFactory : MonoBehaviour, IUIFactory
	{
		public Button levelButtonPrefab;
		public Button homeButtonPrefab;

		public void CreateLevelButton(HorizontalOrVerticalLayoutGroup buttonParent, int levelNum, ILevel level, IScreensSceneGod screensSceneGod)
		{
			Button levelButton = (Button)Instantiate(levelButtonPrefab);
			levelButton.transform.SetParent(buttonParent.transform, worldPositionStays: false);
			levelButton.GetComponent<LevelButtonController>().Initialise(levelNum, level, screensSceneGod);
		}

		public void CreateHomeButton(HorizontalOrVerticalLayoutGroup buttonParent, IScreensSceneGod screensSceneGod)
		{
			Button homeButton = (Button)Instantiate(homeButtonPrefab);
			homeButton.transform.SetParent(buttonParent.transform, worldPositionStays: false);
			homeButton.GetComponent<HomeButtonController>().Initialise(screensSceneGod);
		}
	}
}
