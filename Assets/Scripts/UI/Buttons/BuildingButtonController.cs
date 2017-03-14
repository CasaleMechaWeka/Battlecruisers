using BattleCruisers.UI.BuildMenus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.Buildings.Buttons
{
	public class BuildingButtonController : MonoBehaviour 
	{
		public Image buildingImage;
		public Image slotImage;
		public Text buildingName;
		public Text droneLevel;

		public void Initialize(Building building, IBuildMenuController buildMenuController, Sprite slotSprite)
		{
			buildingName.text = building.buildingName;
			droneLevel.text = building.numOfDronesRequired.ToString();
			buildingImage.sprite = building.BuildingSprite;
			slotImage.sprite = slotSprite;
			
			Button button = GetComponent<Button>();
			button.onClick.AddListener(() => buildMenuController.SelectBuilding(building));
		}
	}
}
