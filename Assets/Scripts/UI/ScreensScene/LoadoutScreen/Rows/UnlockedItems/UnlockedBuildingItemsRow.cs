using System.Collections.Generic;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.UnlockedItems
{
    public class UnlockedBuildingItemsRow : UnlockedBuildableItemsRow<IBuilding>
	{
		public override void Initialise(
            IItemsRow<IBuilding> itemsRow, 
            IUIFactory uiFactory, 
            IList<IBuilding> unlockedBuildables, 
			IList<IBuilding> loadoutBuildables, 
            IItemDetailsManager<IBuilding> detailsManager)
		{
            base.Initialise(itemsRow, uiFactory, unlockedBuildables, loadoutBuildables, detailsManager);
		}

        protected override UnlockedItem<IBuilding> CreateUnlockedItem(IBuilding item, HorizontalOrVerticalLayoutGroup itemParent, bool isInLoadout)
        {
            return _uiFactory.CreateUnlockedBuildingItem(layoutGroup, _itemsRow, item, isInLoadout);
        }
    }
}
