using BattleCruisers.Drones;
using BattleCruisers.UI;
using BattleCruisers.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.Buildings.Buttons
{
	public class BuildingButtonController : BuildableButtonController
	{
		private Building _building;
		private UIManager _uiManager;

		public Image buildingImage;
		public Image slotImage;
		public Text buildingName;
		public Text droneLevel;

		public void Initialize(Building building, UIManager uiManager, IDroneManager droneManager, Sprite slotSprite)
		{
			base.Initialize(building, droneManager);
			
			_building = building;
			_uiManager = uiManager;

			buildingName.text = _building.buildableName;
			droneLevel.text = _building.numOfDronesRequired.ToString();
			buildingImage.sprite = _building.Sprite;
			slotImage.sprite = slotSprite;
		}

		protected override void OnClick()
		{
			_uiManager.SelectBuildingFromMenu(_building);
		}
	}
}
