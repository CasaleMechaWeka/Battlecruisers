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
		LoadoutBuildableItem CreateLoadoutItem(HorizontalOrVerticalLayoutGroup itemRow, Building itemBuilding);
		UnlockedBuildableItem CreateUnlockedBuildableItem(HorizontalOrVerticalLayoutGroup itemRow, BuildableItemsRow itemsRow, Building itemBuilding, bool isBuildingInLoadout);
		UnlockedHullItem CreateUnlockedHull(HorizontalOrVerticalLayoutGroup hullParent, HullItemsRow hullsRow, Cruiser cruiser, bool isInLoadout);
	}

	public class UIFactory : MonoBehaviour, IUIFactory
	{
		private BuildableDetailsManager _buildableDetailsManager;

		public LoadoutBuildableItem loadoutBuildableItemPrefab;
		public UnlockedBuildableItem unlockedBuildableItemPrefab;
		public UnlockedHullItem unlockedHullItemPrefab;

		public void Initialise(BuildableDetailsManager buildableDetailsManager)
		{
			_buildableDetailsManager = buildableDetailsManager;
		}

		public LoadoutBuildableItem CreateLoadoutItem(HorizontalOrVerticalLayoutGroup itemRow, Building itemBuilding)
		{
			LoadoutBuildableItem loadoutItem = Instantiate<LoadoutBuildableItem>(loadoutBuildableItemPrefab);
			loadoutItem.transform.SetParent(itemRow.transform, worldPositionStays: false);
			loadoutItem.Initialise(itemBuilding, _buildableDetailsManager);
			return loadoutItem;
		}

		public UnlockedBuildableItem CreateUnlockedBuildableItem(HorizontalOrVerticalLayoutGroup itemRow, BuildableItemsRow itemsRow, Building itemBuilding, bool isInLoadout)
		{
			UnlockedBuildableItem unlockedBuilding = Instantiate<UnlockedBuildableItem>(unlockedBuildableItemPrefab);
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
