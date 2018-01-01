using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.LoadoutItems;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.UnlockedItems;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
    public interface IUIFactory
    {
        LoadoutItem<IBuilding> CreateLoadoutBuildingItem(HorizontalOrVerticalLayoutGroup itemRow, IBuilding itemBuilding);
        LoadoutItem<IUnit> CreateLoadoutUnitItem(HorizontalOrVerticalLayoutGroup itemRow, IUnit itemUnit);
        UnlockedItem<IBuilding> CreateUnlockedBuildingItem(HorizontalOrVerticalLayoutGroup itemRow, IItemsRow<IBuilding> itemsRow, IBuilding itemBuilding, bool isBuildingInLoadout);
        UnlockedItem<IUnit> CreateUnlockedUnitItem(HorizontalOrVerticalLayoutGroup itemRow, IItemsRow<IUnit> itemsRow, IUnit itemUnit, bool isUnitInLoadout);
        UnlockedHullItem CreateUnlockedHull(HorizontalOrVerticalLayoutGroup hullParent, IItemsRow<ICruiser> hullsRow, ICruiser cruiser, bool isInLoadout);
    }
}
