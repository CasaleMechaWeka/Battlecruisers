using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Data;
using BattleCruisers.Scenes;
using BattleCruisers.UI.ScreensScene.LevelsScreen;
using BattleCruisers.UI.ScreensScene.LoadoutScreen;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LevelsScreen
{
	public interface IUIFactory
	{
		void CreateLevelButton(HorizontalOrVerticalLayoutGroup buttonParent, ILevel level, bool isLevelUnlocked, IScreensSceneGod screensSceneGod);
		void CreateHomeButton(HorizontalOrVerticalLayoutGroup buttonParent, IScreensSceneGod screensSceneGod);
	}

	public class UIFactory : MonoBehaviour, IUIFactory
	{
		public Button levelButtonPrefab;
		public Button homeButtonPrefab;

		public void CreateLevelButton(HorizontalOrVerticalLayoutGroup buttonParent, ILevel level, bool isLevelUnlocked, IScreensSceneGod screensSceneGod)
		{
			Button levelButton = Instantiate<Button>(levelButtonPrefab);
			levelButton.transform.SetParent(buttonParent.transform, worldPositionStays: false);
			levelButton.GetComponent<LevelButtonController>().Initialise(level, isLevelUnlocked, screensSceneGod);
		}

		public void CreateHomeButton(HorizontalOrVerticalLayoutGroup buttonParent, IScreensSceneGod screensSceneGod)
		{
			Button homeButton = Instantiate<Button>(homeButtonPrefab);
			homeButton.transform.SetParent(buttonParent.transform, worldPositionStays: false);
			homeButton.GetComponent<HomeButtonController>().Initialise(screensSceneGod);
		}
	}
}
