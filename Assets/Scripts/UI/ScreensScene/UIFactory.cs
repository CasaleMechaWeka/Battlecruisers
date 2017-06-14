using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Data;
using BattleCruisers.Scenes;
using BattleCruisers.UI.ScreensScene.LevelsScreen;
using BattleCruisers.UI.ScreensScene.LoadoutScreen;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene
{
	public interface IUIFactory
	{
		void CreateLevelButton(HorizontalOrVerticalLayoutGroup buttonParent, int levelNum, ILevel level, bool isLevelUnlocked, IScreensSceneGod screensSceneGod);
		void CreateHomeButton(HorizontalOrVerticalLayoutGroup buttonParent, IScreensSceneGod screensSceneGod);
		void CreateLoadoutItem(HorizontalOrVerticalLayoutGroup itemRow, Building itemBuilding);
		UnlockedItem CreateUnlockedItem(HorizontalOrVerticalLayoutGroup itemRow, LoadoutScreenController loadoutScreen, Building itemBuilding, bool isBuildingInLoadout);
	}

	public class UIFactory : MonoBehaviour, IUIFactory
	{
		public Button levelButtonPrefab;
		public Button homeButtonPrefab;

		// Loadout screen
		public LoadoutItem loadoutItemPrefab;
		public UnlockedItem unlockedItemPrefab;

		public void CreateLevelButton(HorizontalOrVerticalLayoutGroup buttonParent, int levelNum, ILevel level, bool isLevelUnlocked, IScreensSceneGod screensSceneGod)
		{
			Button levelButton = Instantiate<Button>(levelButtonPrefab);
			levelButton.transform.SetParent(buttonParent.transform, worldPositionStays: false);
			levelButton.GetComponent<LevelButtonController>().Initialise(levelNum, level, isLevelUnlocked, screensSceneGod);
		}

		public void CreateHomeButton(HorizontalOrVerticalLayoutGroup buttonParent, IScreensSceneGod screensSceneGod)
		{
			Button homeButton = Instantiate<Button>(homeButtonPrefab);
			homeButton.transform.SetParent(buttonParent.transform, worldPositionStays: false);
			homeButton.GetComponent<HomeButtonController>().Initialise(screensSceneGod);
		}

		// FELIX  Avoid duplicate code?
		public void CreateLoadoutItem(HorizontalOrVerticalLayoutGroup itemRow, Building itemBuilding)
		{
			LoadoutItem loadoutItem = Instantiate<LoadoutItem>(loadoutItemPrefab);
			loadoutItem.transform.SetParent(itemRow.transform, worldPositionStays: false);
			loadoutItem.Initialise(itemBuilding);
		}

		public UnlockedItem CreateUnlockedItem(HorizontalOrVerticalLayoutGroup itemRow, LoadoutScreenController loadoutScreen, Building itemBuilding, bool isBuildingInLoadout)
		{
			UnlockedItem unlockedItem = Instantiate<UnlockedItem>(unlockedItemPrefab);
			unlockedItem.transform.SetParent(itemRow.transform, worldPositionStays: false);
			unlockedItem.Initialise(loadoutScreen, itemBuilding, isBuildingInLoadout);
			return unlockedItem;
		}
	}
}
