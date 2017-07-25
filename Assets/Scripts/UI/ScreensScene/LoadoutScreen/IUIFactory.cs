using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.LoadoutItems;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.UnlockedItems;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
    public interface IUIFactory
    {
        LoadoutBuildingItem CreateLoadoutItem(HorizontalOrVerticalLayoutGroup itemRow, IBuilding itemBuilding);
        UnlockedBuildingItem CreateUnlockedBuildableItem(HorizontalOrVerticalLayoutGroup itemRow, IItemsRow<IBuilding> itemsRow, IBuilding itemBuilding, bool isBuildingInLoadout);
        UnlockedHullItem CreateUnlockedHull(HorizontalOrVerticalLayoutGroup hullParent, IItemsRow<Cruiser> hullsRow, Cruiser cruiser, bool isInLoadout);
    }
}
