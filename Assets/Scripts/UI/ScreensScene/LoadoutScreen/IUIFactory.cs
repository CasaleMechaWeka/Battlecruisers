using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.LoadoutItems;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.LockedItems;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.UnlockedItems;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
    // FELIX  I think all of these will be obsolete?
    public interface IUIFactory
    {
        // Unlocked items
		UnlockedHullItem CreateUnlockedHull(HorizontalOrVerticalLayoutGroup hullParent, IItemsRow<ICruiser> hullsRow, ICruiser cruiser, bool isInLoadout);

        // Loadout items
        LoadoutItem<IBuilding> CreateLoadoutBuildingItem(HorizontalOrVerticalLayoutGroup itemRow, IBuilding itemBuilding);
		LoadoutItem<IUnit> CreateLoadoutUnitItem(HorizontalOrVerticalLayoutGroup itemRow, IUnit itemUnit);

        // Locked items
		LockedItem CreateLockedHull(HorizontalOrVerticalLayoutGroup itemRow);
        LockedItem CreateLockedBuildable(HorizontalOrVerticalLayoutGroup itemRow);
    }
}
