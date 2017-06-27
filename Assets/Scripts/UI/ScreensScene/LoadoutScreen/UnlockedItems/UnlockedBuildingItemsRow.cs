using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.UnlockedItems.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.UnlockedItems
{
	public class UnlockedBuildingItemsRow : UnlockedItemsRow<Building>
	{
		private IList<Building> _loadoutBuildings;

		public void Initialise(BuildingItemsRow itemsRow, IUIFactory uiFactory, IList<Building> unlockedBuildings, 
			IList<Building> loadoutBuildings, BuildingDetailsManager detailsManager)
		{
			_uiFactory = uiFactory;
			_loadoutBuildings = loadoutBuildings;

			base.Initialise(uiFactory, unlockedBuildings, itemsRow, detailsManager);
		}

		protected override UnlockedItem<Building> CreateUnlockedItem(Building item, HorizontalOrVerticalLayoutGroup itemParent)
		{
			bool isBuildingInLoadout = _loadoutBuildings.Contains(item);
			return _uiFactory.CreateUnlockedBuildableItem(layoutGroup, _itemsRow, item, isBuildingInLoadout);
		}
	}
}
