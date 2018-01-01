using BattleCruisers.Buildables.Buildings;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.UnlockedItems
{
    public class UnlockedBuildingItemsRow : UnlockedBuildableItemsRow<IBuilding>
	{
        protected override UnlockedItem<IBuilding> CreateUnlockedItem(IBuilding item, HorizontalOrVerticalLayoutGroup itemParent, bool isInLoadout)
        {
            return _uiFactory.CreateUnlockedBuildingItem(layoutGroup, _itemsRow, item, isInLoadout);
        }
    }
}
