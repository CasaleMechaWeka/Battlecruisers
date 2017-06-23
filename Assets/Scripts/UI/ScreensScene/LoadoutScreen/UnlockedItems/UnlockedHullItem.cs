using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.UnlockedItems
{
	public class UnlockedHullItem : UnlockedItem<Cruiser>
	{
		private HullItemsRow _hullsRow;

		public void Initialise(HullItemsRow hullsRow, Cruiser cruiser, bool isInLoadout)
		{
			base.Initialise(cruiser, isInLoadout);

			_hullsRow = hullsRow;
			itemImage.sprite = _item.Sprite;
		}

		public void SelectHull()
		{
			if (!IsItemInLoadout)
			{
				_hullsRow.SelectHull(_item);
			}
		}

		public void OnNewHullSelected(Cruiser selectedCruiser)
		{
			IsItemInLoadout = object.ReferenceEquals(selectedCruiser, _item);
		}
	}
}
