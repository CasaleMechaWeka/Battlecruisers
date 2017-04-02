using BattleCruisers.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.Buildings.Buttons
{
	public class BuildingButtonController : Presentable
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
			base.Initialize();

			buildingName.text = building.buildableName;
			droneLevel.text = building.numOfDronesRequired.ToString();
			buildingImage.sprite = building.Sprite;
			slotImage.sprite = slotSprite;
			
			_button = GetComponent<Button>();
			_building = building;
			_uiManager = uiManager;
		}

		public override void OnPresenting(object activationParameter)
		{
			base.OnPresenting(activationParameter);

			_button.onClick.AddListener(OnClick);
		}

		public override void OnDismissing()
		{
			base.OnDismissing();

			_button.onClick.RemoveListener(OnClick);
		}

		private void OnClick()
		{
			_uiManager.SelectBuildingFromMenu(_building);
		}
	}
}
