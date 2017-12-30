using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.LoadoutItems;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.UnlockedItems;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.UnlockedItems.States;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
	public class UIFactory : MonoBehaviour, IUIFactory
	{
        private IItemDetailsManager<IBuilding> _buildingDetailsManager;
        private IItemDetailsManager<IUnit> _unitDetailsManager;

		public LoadoutBuildingItem loadoutBuildingItemPrefab;
        public LoadoutUnitItem loadoutUnitItemPrefab;
		public UnlockedBuildingItem unlockedBuildableItemPrefab;
		public UnlockedHullItem unlockedHullItemPrefab;

        public void Initialise(
            IItemDetailsManager<IBuilding> buildingDetailsManager, 
            IItemDetailsManager<IUnit> unitDetailsManager)
		{
            Helper.AssertIsNotNull(
                buildingDetailsManager, 
                unitDetailsManager, 
                loadoutBuildingItemPrefab, 
                loadoutUnitItemPrefab, 
                unlockedBuildableItemPrefab, 
                unlockedHullItemPrefab);

			_buildingDetailsManager = buildingDetailsManager;
            _unitDetailsManager = unitDetailsManager;
		}

        public LoadoutItem<IBuilding> CreateLoadoutBuildingItem(HorizontalOrVerticalLayoutGroup itemRow, IBuilding itemBuilding)
		{
            return CreateLoadoutBuildableItem(loadoutBuildingItemPrefab, itemRow, itemBuilding, _buildingDetailsManager);
		}

        public LoadoutItem<IUnit> CreateLoadoutUnitItem(HorizontalOrVerticalLayoutGroup itemRow, IUnit itemUnit)
        {
            return CreateLoadoutBuildableItem(loadoutUnitItemPrefab, itemRow, itemUnit, _unitDetailsManager);
        }

        private LoadoutItem<TBuildable> CreateLoadoutBuildableItem<TBuildable>(
            LoadoutItem<TBuildable> itemPrefab, 
            HorizontalOrVerticalLayoutGroup itemRow, 
            TBuildable itemBuildable,
            IItemDetailsManager<TBuildable> detailsManager) 
                where TBuildable : IBuildable
        {
            LoadoutItem<TBuildable> loadoutItem = Instantiate(itemPrefab);
            loadoutItem.transform.SetParent(itemRow.transform, worldPositionStays: false);
            loadoutItem.Initialise(itemBuildable, detailsManager);
            return loadoutItem;
        }

		public UnlockedBuildingItem CreateUnlockedBuildingItem(HorizontalOrVerticalLayoutGroup itemRow, IItemsRow<IBuilding> itemsRow, IBuilding itemBuilding, bool isInLoadout)
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
