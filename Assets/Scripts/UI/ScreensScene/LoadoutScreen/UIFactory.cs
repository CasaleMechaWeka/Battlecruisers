using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Data;
using BattleCruisers.Scenes;
using BattleCruisers.UI.ScreensScene.LevelsScreen;
using BattleCruisers.UI.ScreensScene.LoadoutScreen;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.LoadoutItems;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.UnlockedItems;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.UnlockedItems.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
	public interface IUIFactory
	{
		LoadoutBuildingItem CreateLoadoutItem(HorizontalOrVerticalLayoutGroup itemRow, Building itemBuilding);
		UnlockedBuildingItem CreateUnlockedBuildableItem(HorizontalOrVerticalLayoutGroup itemRow, BuildingItemsRow itemsRow, Building itemBuilding, bool isBuildingInLoadout);
		UnlockedHullItem CreateUnlockedHull(HorizontalOrVerticalLayoutGroup hullParent, HullItemsRow hullsRow, Cruiser cruiser, bool isInLoadout);
	}

	public class UIFactory : MonoBehaviour, IUIFactory
	{
		private BuildingDetailsManager _buildableDetailsManager;

		public LoadoutBuildingItem loadoutBuildingItemPrefab;
		public UnlockedBuildingItem unlockedBuildableItemPrefab;
		public UnlockedHullItem unlockedHullItemPrefab;

		public void Initialise(BuildingDetailsManager buildableDetailsManager)
		{
			_buildableDetailsManager = buildableDetailsManager;
		}

		public LoadoutBuildingItem CreateLoadoutItem(HorizontalOrVerticalLayoutGroup itemRow, Building itemBuilding)
		{
			LoadoutBuildingItem loadoutItem = Instantiate<LoadoutBuildingItem>(loadoutBuildingItemPrefab);
			loadoutItem.transform.SetParent(itemRow.transform, worldPositionStays: false);
			loadoutItem.Initialise(itemBuilding, _buildableDetailsManager);
			return loadoutItem;
		}

		public UnlockedBuildingItem CreateUnlockedBuildableItem(HorizontalOrVerticalLayoutGroup itemRow, BuildingItemsRow itemsRow, Building itemBuilding, bool isInLoadout)
		{
			UnlockedBuildingItem unlockedBuilding = Instantiate<UnlockedBuildingItem>(unlockedBuildableItemPrefab);
			unlockedBuilding.transform.SetParent(itemRow.transform, worldPositionStays: false);
			IUnlockedItemState<Building> initialState = new DefaultState<Building>(itemsRow);
			unlockedBuilding.Initialise(initialState, itemBuilding, isInLoadout);
			return unlockedBuilding;
		}

		public UnlockedHullItem CreateUnlockedHull(HorizontalOrVerticalLayoutGroup hullParent, HullItemsRow hullsRow, Cruiser cruiser, bool isInLoadout)
		{
			UnlockedHullItem unlockedHull = Instantiate<UnlockedHullItem>(unlockedHullItemPrefab);
			unlockedHull.transform.SetParent(hullParent.transform, worldPositionStays: false);
			IUnlockedItemState<Cruiser> initialState = new DefaultState<Cruiser>(hullsRow);
			unlockedHull.Initialise(initialState, cruiser, isInLoadout);
			return unlockedHull;
		}
	}
}
