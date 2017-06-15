using BattleCruisers.Buildables.Buildings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
	// FELIX  Avoid duplicate code with LoadoutItem
	public class UnlockedItem : MonoBehaviour 
	{
		private ItemsRow _itemsRow;
		private Building _building;
		private bool _isBuildingInLoadout;
		private RectTransform _rectTransform;

		public Image itemImage;
		public Image isInLoadoutFeedback;

		public Vector2 Size { get { return _rectTransform.sizeDelta; } }
		public Building Building { get; private set; }

		public void Initialise(ItemsRow itemsRow, Building building, bool isBuildingInLoadout)
		{
			Assert.IsNotNull(itemsRow);
			Assert.IsNotNull(itemImage);
			Assert.IsNotNull(isInLoadoutFeedback);

			_itemsRow = itemsRow;
			_building = building;
			_isBuildingInLoadout = isBuildingInLoadout;

			itemImage.sprite = building.Sprite;

			_rectTransform = transform as RectTransform;
			Assert.IsNotNull(_rectTransform);

			isInLoadoutFeedback.gameObject.SetActive(isBuildingInLoadout);
		}

		public void ToggleItem()
		{
			if (_isBuildingInLoadout)
			{
				_itemsRow.RemoveBuildingFromLoadout(_building);
			}
			else
			{
				if (_itemsRow.CanAddBuilding())
				{
					_itemsRow.AddBuildingToLoadout(_building);
				}
				else
				{
					// FELIX  Show error to user?  BETTER => disable all buttons that would add an item :D
				}
			}

			_isBuildingInLoadout = !_isBuildingInLoadout;
			isInLoadoutFeedback.gameObject.SetActive(_isBuildingInLoadout);
		}
	}
}
