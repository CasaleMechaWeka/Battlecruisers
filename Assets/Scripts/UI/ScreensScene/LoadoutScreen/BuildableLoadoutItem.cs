using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.BuildableDetails;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
	public class BuildableLoadoutItem : MonoBehaviour 
	{
		private IBuildableDetailsManager _buildableDetailsManager;

		public Image itemImage;
		public Image selectedFeedbackImage;

		public Building Buildable { private set; get; }

		public bool ShowSelectedFeedback
		{
			set { selectedFeedbackImage.gameObject.SetActive(value); }
		}

		public void Initialise(Building building, IBuildableDetailsManager buildableDetailsManager)
		{
			Buildable = building;
			itemImage.sprite = building.Sprite;
			_buildableDetailsManager = buildableDetailsManager;
		}

		public void SelectBuildable()
		{
			_buildableDetailsManager.SelectBuildable(this);
		}
	}
}
