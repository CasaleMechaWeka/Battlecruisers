using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.UnlockedItems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
	public class UnlockedBuildableItemsRow : UnlockedItemsRow<Building>
	{
		private BuildableItemsRow _itemsRow;
		private IList<Building> _loadoutBuildings;

		public void Initialise(BuildableItemsRow itemsRow, IUIFactory uiFactory, IList<Building> unlockedBuildings, IList<Building> loadoutBuildings)
		{
			_itemsRow = itemsRow;
			_uiFactory = uiFactory;
			_loadoutBuildings = loadoutBuildings;

			base.Initialise(uiFactory, unlockedBuildings);
		}

		protected override UnlockedItem CreateUnlockedItem(Building item, HorizontalOrVerticalLayoutGroup itemParent)
		{
			bool isBuildingInLoadout = _loadoutBuildings.Contains(item);
			return _uiFactory.CreateUnlockedItem(layoutGroup, _itemsRow, item, isBuildingInLoadout);
		}
	}
}
