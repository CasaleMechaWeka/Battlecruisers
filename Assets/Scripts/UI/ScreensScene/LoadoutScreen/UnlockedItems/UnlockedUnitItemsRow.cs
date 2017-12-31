using System.Collections.Generic;
using BattleCruisers.Buildables.Units;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.UnlockedItems
{
    public class UnlockeUnitItemsRow : UnlockedBuildableItemsRow<IUnit>
	{
        // FELIX  Create arg wrapper class?  Reduce constructor size :P
		public override void Initialise(
            IItemsRow<IUnit> itemsRow, 
            IUIFactory uiFactory, 
            IList<IUnit> unlockedBuildables, 
            IList<IUnit> loadoutBuildables, 
            IItemDetailsManager<IUnit> detailsManager)
		{
            base.Initialise(itemsRow, uiFactory, unlockedBuildables, loadoutBuildables, detailsManager);
		}

        protected override UnlockedItem<IUnit> CreateUnlockedItem(IUnit item, HorizontalOrVerticalLayoutGroup itemParent, bool isInLoadout)
        {
            return _uiFactory.CreateUnlockedUnitItem(layoutGroup, _itemsRow, item, isInLoadout);
        }
    }
}
