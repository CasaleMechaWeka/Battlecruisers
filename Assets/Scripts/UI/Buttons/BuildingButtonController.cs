using BattleCruisers.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.Buildings.Buttons
{
	// FELIX  Create base Presentable class?
	public class BuildingButtonController : MonoBehaviour, IPresentable
	{
		private Building _building;
		private UIManager _uiManager;
		private Button _button;

		public Image buildingImage;
		public Image slotImage;
		public Text buildingName;
		public Text droneLevel;

		public void Initialize(Building building, UIManager uiManager, Sprite slotSprite)
		{
			buildingName.text = building.buildableName;
			droneLevel.text = building.numOfDronesRequired.ToString();
			buildingImage.sprite = building.Sprite;
			slotImage.sprite = slotSprite;
			
			_button = GetComponent<Button>();
			_building = building;
			_uiManager = uiManager;
		}

		public void OnPresenting(object activationParameter)
		{
			_button.onClick.AddListener(OnClick);
		}

		public void OnDismissing()
		{
			_button.onClick.RemoveListener(OnClick);
		}

		private void OnClick()
		{
			_uiManager.SelectBuildingFromMenu(_building);
		}
	}
}
