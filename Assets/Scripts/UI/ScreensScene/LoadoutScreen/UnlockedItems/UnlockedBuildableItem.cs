using BattleCruisers.Buildables.Buildings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.UnlockedItems
{
	public class UnlockedBuildableItem : UnlockedItem
	{
		private ItemsRow _itemsRow;
		private Building _building;

		public void Initialise(ItemsRow itemsRow, Building building, bool isBuildingInLoadout)
		{
			base.Initialise(isBuildingInLoadout);

			_itemsRow = itemsRow;
			_building = building;

			itemImage.sprite = building.Sprite;
		}

		public void ToggleItem()
		{
			if (IsItemInLoadout)
			{
				_itemsRow.RemoveBuildingFromLoadout(_building);
				IsItemInLoadout = !IsItemInLoadout;
			}
			else
			{
				if (_itemsRow.CanAddBuilding())
				{
					_itemsRow.AddBuildingToLoadout(_building);
					IsItemInLoadout = !IsItemInLoadout;
				}
				else
				{
					// FELIX  Show error to user?  BETTER => disable all buttons that would add an item :D
				}
			}

		}
	}
}
