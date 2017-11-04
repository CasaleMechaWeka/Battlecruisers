using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.LoadoutItems;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.UnlockedItems;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.UnlockedItems.States;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
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

		public LoadoutBuildingItem CreateLoadoutItem(HorizontalOrVerticalLayoutGroup itemRow, IBuilding itemBuilding)
		{
			LoadoutBuildingItem loadoutItem = Instantiate(loadoutBuildingItemPrefab);
			loadoutItem.transform.SetParent(itemRow.transform, worldPositionStays: false);
			loadoutItem.Initialise(itemBuilding, _buildableDetailsManager);
			return loadoutItem;
		}

		public UnlockedBuildingItem CreateUnlockedBuildableItem(HorizontalOrVerticalLayoutGroup itemRow, IItemsRow<IBuilding> itemsRow, IBuilding itemBuilding, bool isInLoadout)
		{
			UnlockedBuildingItem unlockedBuilding = Instantiate(unlockedBuildableItemPrefab);
			unlockedBuilding.transform.SetParent(itemRow.transform, worldPositionStays: false);
			IUnlockedItemState<IBuilding> initialState = new DefaultState<IBuilding>(itemsRow, unlockedBuilding);
			unlockedBuilding.Initialise(initialState, itemBuilding, isInLoadout);
			return unlockedBuilding;
		}

		public UnlockedHullItem CreateUnlockedHull(HorizontalOrVerticalLayoutGroup hullParent, IItemsRow<ICruiser> hullsRow, ICruiser cruiser, bool isInLoadout)
		{
			UnlockedHullItem unlockedHull = Instantiate<UnlockedHullItem>(unlockedHullItemPrefab);
			unlockedHull.transform.SetParent(hullParent.transform, worldPositionStays: false);
			IUnlockedItemState<ICruiser> initialState = new DefaultState<ICruiser>(hullsRow, unlockedHull);
			unlockedHull.Initialise(initialState, cruiser, isInLoadout);
			return unlockedHull;
		}
	}
}
