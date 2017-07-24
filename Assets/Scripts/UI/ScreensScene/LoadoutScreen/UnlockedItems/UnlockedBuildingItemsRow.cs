using System.Collections.Generic;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.UnlockedItems
{
    public class UnlockedBuildingItemsRow : UnlockedItemsRow<IBuilding>
	{
		private IList<IBuilding> _loadoutBuildings;

		public void Initialise(BuildingItemsRow itemsRow, IUIFactory uiFactory, IList<IBuilding> unlockedBuildings, 
			IList<IBuilding> loadoutBuildings, BuildingDetailsManager detailsManager)
		{
			_uiFactory = uiFactory;
			_loadoutBuildings = loadoutBuildings;

			base.Initialise(uiFactory, unlockedBuildings, itemsRow, detailsManager);
		}

		protected override UnlockedItem<IBuilding> CreateUnlockedItem(IBuilding item, HorizontalOrVerticalLayoutGroup itemParent)
		{
			bool isBuildingInLoadout = _loadoutBuildings.Contains(item);
			return _uiFactory.CreateUnlockedBuildableItem(layoutGroup, _itemsRow, item, isBuildingInLoadout);
		}
	}
}
