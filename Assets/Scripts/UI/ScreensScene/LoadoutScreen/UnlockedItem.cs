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
		private LoadoutScreenController _loadoutScreen;
		private Building _building;
		private bool _isBuildingInLoadout;
		private RectTransform _rectTransform;

		public Image itemImage;
		public Image isInLoadoutFeedback;

		public Vector2 Size { get { return _rectTransform.sizeDelta; } }
		public Building Building { get; private set; }

		public void Initialise(LoadoutScreenController loadoutScreen, Building building, bool isBuildingInLoadout)
		{
			Assert.IsNotNull(loadoutScreen);
			Assert.IsNotNull(itemImage);
			Assert.IsNotNull(isInLoadoutFeedback);

			_loadoutScreen = loadoutScreen;
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
				_loadoutScreen.RemoveBuildingFromLoadout(_building);
			}
			else
			{
				_loadoutScreen.AddBuildingToLoadout(_building);
			}

			_isBuildingInLoadout = !_isBuildingInLoadout;
			isInLoadoutFeedback.gameObject.SetActive(_isBuildingInLoadout);
		}
	}
}
