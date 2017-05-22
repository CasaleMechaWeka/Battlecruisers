using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Drones;
using BattleCruisers.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.Buttons
{
	public class BuildingButtonController : BuildableButtonController
	{
		private BuildingWrapper _buildingWrapper;

		public Image slotImage;

		public void Initialize(BuildingWrapper buildingWrapper, UIManager uiManager, IDroneManager droneManager, Sprite slotSprite)
		{
			base.Initialize(buildingWrapper.building, droneManager, uiManager);
			
			_buildingWrapper = buildingWrapper;
			slotImage.sprite = slotSprite;
		}

		protected override void OnClick()
		{
			_uiManager.SelectBuildingFromMenu(_buildingWrapper);
		}
	}
}
