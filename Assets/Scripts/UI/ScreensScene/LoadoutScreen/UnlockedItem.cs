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
		private RectTransform _rectTransform;

		public Image itemImage;

		public Vector2 Size { get { return _rectTransform.sizeDelta; } }
		public Building Building { get; private set; }

		public void Initialise(Building building, bool isBuildingInLoadout)
		{
			itemImage.sprite = building.Sprite;

			_rectTransform = transform as RectTransform;
			Assert.IsNotNull(_rectTransform);
		}
	}
}
