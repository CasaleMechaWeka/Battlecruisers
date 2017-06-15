using BattleCruisers.Cruisers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Hulls
{
	public class LoadoutHull : MonoBehaviour 
	{
		public Image hullImage;

		public void Initialise(ICruiser cruiser)
		{
			hullImage.sprite = cruiser.Sprite;
		}
	}
}
