using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Hulls
{
	public class UnlockedHullItem : MonoBehaviour 
	{
		private HullsRow _hullsRow;
		private Cruiser _cruiser;

		// FELIX Move to parent class of this and UnlockedItem?
		private RectTransform _rectTransform;

		public Image itemImage;
		public Image isInLoadoutFeedback;

		public Vector2 Size { get { return _rectTransform.sizeDelta; } }
		public Building Building { get; private set; }

		private bool _isItemInLoadout;
		private bool IsItemInLoadout
		{
			get { return _isItemInLoadout; }
			set
			{
				_isItemInLoadout = value;
				isInLoadoutFeedback.gameObject.SetActive(_isItemInLoadout);
			}
		}

		public void Initialise(HullsRow hullsRow, Cruiser cruiser, bool isInLoadout)
		{
			Assert.IsNotNull(hullsRow);
			Assert.IsNotNull(itemImage);
			Assert.IsNotNull(isInLoadoutFeedback);

			_hullsRow = hullsRow;
			_cruiser = cruiser;
			IsItemInLoadout = isInLoadout;

			itemImage.sprite = _cruiser.Sprite;

			_rectTransform = transform as RectTransform;
			Assert.IsNotNull(_rectTransform);
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
