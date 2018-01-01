using BattleCruisers.Buildables.Units;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.UnlockedItems
{
    public class UnlockedUnitItemsRow : UnlockedBuildableItemsRow<IUnit>
	{
        protected override UnlockedItem<IUnit> CreateUnlockedItem(IUnit item, HorizontalOrVerticalLayoutGroup itemParent, bool isInLoadout)
        {
            return _uiFactory.CreateUnlockedUnitItem(layoutGroup, _itemsRow, item, isInLoadout);
        }
    }
}
