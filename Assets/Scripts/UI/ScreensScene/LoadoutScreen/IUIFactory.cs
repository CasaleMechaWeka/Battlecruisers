using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.LoadoutItems;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.UnlockedItems;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
    public interface IUIFactory
    {
        LoadoutBuildingItem CreateLoadoutBuildingItem(HorizontalOrVerticalLayoutGroup itemRow, IBuilding itemBuilding);
        UnlockedBuildingItem CreateUnlockedBuildingItem(HorizontalOrVerticalLayoutGroup itemRow, IItemsRow<IBuilding> itemsRow, IBuilding itemBuilding, bool isBuildingInLoadout);
        UnlockedHullItem CreateUnlockedHull(HorizontalOrVerticalLayoutGroup hullParent, IItemsRow<ICruiser> hullsRow, ICruiser cruiser, bool isInLoadout);
    }
}
