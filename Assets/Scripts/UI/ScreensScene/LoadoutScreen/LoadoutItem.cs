using BattleCruisers.Buildables.Buildings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
	public class LoadoutItem : MonoBehaviour 
	{
		public Image itemImage;

		public void Initialise(Building building)
		{
			itemImage.sprite = building.Sprite;
		}
	}
}
