using BattleCruisers.Buildables.Buildings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
	public class LoadoutItem : MonoBehaviour 
	{
		private Building _buildable;
		private IBuildableDetailsManager _buildableDetailsManager;

		public Image itemImage;

		public void Initialise(Building building, IBuildableDetailsManager buildableDetailsManager)
		{
			_buildable = building;
			itemImage.sprite = building.Sprite;
			_buildableDetailsManager = buildableDetailsManager;
		}

		public void SelectBuildable()
		{
			_buildableDetailsManager.SelectBuildable(_buildable);
		}
	}
}
