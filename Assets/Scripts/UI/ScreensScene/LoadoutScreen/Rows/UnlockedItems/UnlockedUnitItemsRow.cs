using System.Collections.Generic;
using BattleCruisers.Buildables.Units;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.UnlockedItems
{
    public class UnlockedUnitItemsRow : UnlockedBuildableItemsRow<IUnit>
	{
        // FELIX  Remove method?
        public override void Initialise(IUnlockedItemsRowArgs<IUnit> args, IList<IUnit> loadoutBuildables)
		{
            base.Initialise(args, loadoutBuildables);
		}

        protected override UnlockedItem<IUnit> CreateUnlockedItem(IUnit item, HorizontalOrVerticalLayoutGroup itemParent, bool isInLoadout)
        {
            return _uiFactory.CreateUnlockedUnitItem(layoutGroup, _itemsRow, item, isInLoadout);
        }
    }
}
