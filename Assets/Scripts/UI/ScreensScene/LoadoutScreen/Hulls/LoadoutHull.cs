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

		public Sprite Sprite { set { hullImage.sprite = value; } }
	}
}
