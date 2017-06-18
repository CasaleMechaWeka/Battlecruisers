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

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
	public interface IUIFactory
	{
		LoadoutItem CreateLoadoutItem(HorizontalOrVerticalLayoutGroup itemRow, Building itemBuilding);
		UnlockedBuildableItem CreateUnlockedItem(HorizontalOrVerticalLayoutGroup itemRow, ItemsRow itemsRow, Building itemBuilding, bool isBuildingInLoadout);
		UnlockedHullItem CreateUnlockedHull(HorizontalOrVerticalLayoutGroup hullParent, HullsRow hullsRow, Cruiser cruiser, bool isInLoadout);
	}

	public class UIFactory : MonoBehaviour, IUIFactory
	{
		public LoadoutItem loadoutBuildableItemPrefab;
		public UnlockedBuildableItem unlockedBuildableItemPrefab;
		public UnlockedHullItem unlockedHullItemPrefab;

		// FELIX  Avoid duplicate code?
		public LoadoutItem CreateLoadoutItem(HorizontalOrVerticalLayoutGroup itemRow, Building itemBuilding)
		{
			LoadoutItem loadoutItem = Instantiate<LoadoutItem>(loadoutBuildableItemPrefab);
			loadoutItem.transform.SetParent(itemRow.transform, worldPositionStays: false);
			// FELIX
			loadoutItem.Initialise(itemBuilding, null);
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
