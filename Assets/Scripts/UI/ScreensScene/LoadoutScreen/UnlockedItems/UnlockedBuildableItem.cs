using BattleCruisers.Buildables.Buildings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using BattleCruisers.Buildables;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.UnlockedItems
{
	// FELIX  Avoid duplicate code with UnlockedHulLitem
	public class UnlockedBuildableItem : UnlockedItem<Building>
	{
		private BuildableItemsRow _itemsRow;

		public void Initialise(BuildableItemsRow itemsRow, Building building, bool isBuildingInLoadout)
		{
			base.Initialise(building, isBuildingInLoadout);

			_itemsRow = itemsRow;
			itemImage.sprite = building.Sprite;
		}

		public void SelectBuildable()
		{
			_itemsRow.SelectUnlockedItem(this);
			IsItemInLoadout = !IsItemInLoadout;
		}
	}
}
