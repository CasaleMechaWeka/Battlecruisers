using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.LoadoutItems;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.LockedItems;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.UnlockedItems;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.UnlockedItems.States;
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
        public UnlockedHullItem unlockedHullItemPrefab;
        public LockedItem lockedHullItemPrefab, lockedBuildableItemPrefab;

        public void Initialise(
            IItemDetailsManager<IBuilding> buildingDetailsManager, 
            IItemDetailsManager<IUnit> unitDetailsManager)
		{
            Helper.AssertIsNotNull(
                buildingDetailsManager, 
                unitDetailsManager, 
                loadoutBuildingItemPrefab, 
                loadoutUnitItemPrefab, 
                unlockedHullItemPrefab,
                lockedHullItemPrefab,
                lockedBuildableItemPrefab);

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

        public UnlockedItem<TBuildable> CreateUnlockedBuildableItem<TBuildable>(
            UnlockedItem<TBuildable> itemPrefab,
            HorizontalOrVerticalLayoutGroup itemRow, 
            IItemsRow<TBuildable> itemsRow, 
            TBuildable itemBuildable, 
            bool isInLoadout)
                where TBuildable : IBuildable
        {
            UnlockedItem<TBuildable> unlockedBuildable = Instantiate(itemPrefab);
            unlockedBuildable.transform.SetParent(itemRow.transform, worldPositionStays: false);
            IUnlockedItemState<TBuildable> initialState = new DefaultState<TBuildable>(itemsRow, unlockedBuildable);
            unlockedBuildable.Initialise(initialState, itemBuildable, isInLoadout);
            return unlockedBuildable;
        }

        public UnlockedHullItem CreateUnlockedHull(HorizontalOrVerticalLayoutGroup hullParent, IItemsRow<ICruiser> hullsRow, ICruiser cruiser, bool isInLoadout)
        {
            UnlockedHullItem unlockedHull = Instantiate(unlockedHullItemPrefab);
            unlockedHull.transform.SetParent(hullParent.transform, worldPositionStays: false);
            IUnlockedItemState<ICruiser> initialState = new DefaultState<ICruiser>(hullsRow, unlockedHull);
            unlockedHull.Initialise(initialState, cruiser, isInLoadout);
            return unlockedHull;
        }

        public LockedItem CreateLockedHull(HorizontalOrVerticalLayoutGroup itemRow)
        {
            return CreateLockedItem(itemRow, lockedHullItemPrefab);
        }

        public LockedItem CreateLockedBuildable(HorizontalOrVerticalLayoutGroup itemRow)
        {
            return CreateLockedItem(itemRow, lockedBuildableItemPrefab);
        }
		
        private LockedItem CreateLockedItem(HorizontalOrVerticalLayoutGroup itemRow, LockedItem lockedItemPrefab)
        {
            LockedItem lockedItem = Instantiate(lockedItemPrefab);
            lockedItem.transform.SetParent(itemRow.transform, worldPositionStays: false);
            lockedItem.Initialise();
            return lockedItem;
        }
    }
}
