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

		// FELIX  Do not updat IsInItemLoadout here, instead have update method and follow UnlockedHullItem class
		public void ToggleItem()
		{
			if (IsItemInLoadout)
			{
				_itemsRow.RemoveBuildingFromLoadout(Item);
				IsItemInLoadout = !IsItemInLoadout;
			}
			else
			{
				if (_itemsRow.CanAddBuilding())
				{
					_itemsRow.AddBuildingToLoadout(Item);
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
