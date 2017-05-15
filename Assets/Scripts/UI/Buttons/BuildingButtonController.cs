using BattleCruisers.Drones;
using BattleCruisers.UI;
using BattleCruisers.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.Buildables.Buildings.Buttons
{
	public class BuildingButtonController : BuildableButtonController
	{
		private BuildingWrapper _buildingWrapper;
		private UIManager _uiManager;

		public Image buildingImage;
		public Image slotImage;
		public Text buildingName;
		public Text droneLevel;

		// FELIX  Reduce duplication with UnitButtonController
		public void Initialize(BuildingWrapper buildingWrapper, UIManager uiManager, IDroneManager droneManager, Sprite slotSprite)
		{
			base.Initialize(buildingWrapper.building, droneManager);
			
			_buildingWrapper = buildingWrapper;
			_uiManager = uiManager;

			buildingName.text = _buildingWrapper.building.buildableName;
			droneLevel.text = _buildingWrapper.building.numOfDronesRequired.ToString();
			buildingImage.sprite = _buildingWrapper.building.Sprite;
			slotImage.sprite = slotSprite;
		}

		protected override void OnClick()
		{
			_uiManager.SelectBuildingFromMenu(_buildingWrapper);
		}
	}
}
