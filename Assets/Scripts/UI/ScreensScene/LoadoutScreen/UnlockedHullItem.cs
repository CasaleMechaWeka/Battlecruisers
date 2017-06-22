using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
	public class UnlockedHullItem : UnlockedItem 
	{
		private HullsRow _hullsRow;
		private Cruiser _cruiser;

		public void Initialise(HullsRow hullsRow, Cruiser cruiser, bool isInLoadout)
		{
			base.Initialise(isInLoadout);

			_hullsRow = hullsRow;
			_cruiser = cruiser;

			itemImage.sprite = _cruiser.Sprite;
		}

		public void SelectHull()
		{
			if (!IsItemInLoadout)
			{
				_hullsRow.SelectHull(_cruiser);
			}
		}

		public void OnNewHullSelected(Cruiser selectedCruiser)
		{
			IsItemInLoadout = object.ReferenceEquals(selectedCruiser, _cruiser);
		}
	}
}
