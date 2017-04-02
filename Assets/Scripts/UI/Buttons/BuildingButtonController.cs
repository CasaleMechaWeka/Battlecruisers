using BattleCruisers.Drones;
using BattleCruisers.UI;
using BattleCruisers.Utils;
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
		private IDroneManager _droneManager;
		private Button _button;

		public Image buildingImage;
		public Image slotImage;
		public Text buildingName;
		public Text droneLevel;
		public CanvasGroup canvasGroup;

		public void Initialize(Building building, UIManager uiManager, IDroneManager droneManager, Sprite slotSprite)
		{
			base.Initialize();
			
			_building = building;
			_uiManager = uiManager;
			_droneManager = droneManager;
			_button = GetComponent<Button>();

			buildingName.text = _building.buildableName;
			droneLevel.text = _building.numOfDronesRequired.ToString();
			buildingImage.sprite = _building.Sprite;
			slotImage.sprite = slotSprite;
		}

		public override void OnPresenting(object activationParameter)
		{
			base.OnPresenting(activationParameter);

			if (_droneManager.CanSupportDroneConsumer(_building.DroneConsumer))
			{
				_button.onClick.AddListener(OnClick);
			}
			else
			{
				canvasGroup.alpha = Constants.DISABLED_UI_ALPHA;
			}
		}

		public override void OnDismissing()
		{
			base.OnDismissing();

			_button.onClick.RemoveListener(OnClick);
			canvasGroup.alpha = Constants.ENABLED_UI_ALPHA;
		}

		private void OnClick()
		{
			_uiManager.SelectBuildingFromMenu(_building);
		}
	}
}
