using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Data;
using BattleCruisers.Scenes;
using BattleCruisers.UI.ScreensScene.LevelsScreen;
using BattleCruisers.UI.ScreensScene.LoadoutScreen;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Hulls;
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

		// Loadout screen
		LoadoutItem CreateLoadoutItem(HorizontalOrVerticalLayoutGroup itemRow, Building itemBuilding);
		UnlockedBuildableItem CreateUnlockedItem(HorizontalOrVerticalLayoutGroup itemRow, ItemsRow itemsRow, Building itemBuilding, bool isBuildingInLoadout);
		UnlockedHullItem CreateUnlockedHull(HorizontalOrVerticalLayoutGroup hullParent, HullsRow hullsRow, Cruiser cruiser, bool isInLoadout);
	}

	public class UIFactory : MonoBehaviour, IUIFactory
	{
		public Button levelButtonPrefab;
		public Button homeButtonPrefab;

		// Loadout screen
		public LoadoutItem loadoutBuildableItemPrefab;
		public UnlockedBuildableItem unlockedBuildableItemPrefab;
		public UnlockedHullItem unlockedHullItemPrefab;

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
		public LoadoutItem CreateLoadoutItem(HorizontalOrVerticalLayoutGroup itemRow, Building itemBuilding)
		{
			LoadoutItem loadoutItem = Instantiate<LoadoutItem>(loadoutBuildableItemPrefab);
			loadoutItem.transform.SetParent(itemRow.transform, worldPositionStays: false);
			loadoutItem.Initialise(itemBuilding);
			return loadoutItem;
		}

		public UnlockedBuildableItem CreateUnlockedItem(HorizontalOrVerticalLayoutGroup itemRow, ItemsRow itemsRow, Building itemBuilding, bool isInLoadout)
		{
			UnlockedBuildableItem unlockedItem = Instantiate<UnlockedBuildableItem>(unlockedBuildableItemPrefab);
			unlockedItem.transform.SetParent(itemRow.transform, worldPositionStays: false);
			unlockedItem.Initialise(itemsRow, itemBuilding, isInLoadout);
			return unlockedItem;
		}

		public UnlockedHullItem CreateUnlockedHull(HorizontalOrVerticalLayoutGroup hullParent, HullsRow hullsRow, Cruiser cruiser, bool isInLoadout)
		{
			UnlockedHullItem unlockedHull = Instantiate<UnlockedHullItem>(unlockedHullItemPrefab);
			unlockedHull.transform.SetParent(hullParent.transform, worldPositionStays: false);
			unlockedHull.Initialise(hullsRow, cruiser, isInLoadout);
			return unlockedHull;
		}
	}
}
