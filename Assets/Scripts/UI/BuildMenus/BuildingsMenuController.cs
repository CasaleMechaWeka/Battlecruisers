using BattleCruisers.Buildings;
using BattleCruisers.Buildings.Buttons;
using BattleCruisers.Units;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.BuildMenus
{
	public class BuildingsMenuController : Presentable
	{
		private IList<BuildingButtonController> _buttons;

		public void Initialize(
			IUIFactory uiFactory,
			IList<Building> buildings)
		{
			_buttons = new List<BuildingButtonController>();

			// Create building buttons
			HorizontalLayoutGroup buttonGroup = GetComponent<HorizontalLayoutGroup>();

			for (int i = 0; i < buildings.Count; ++i)
			{
				BuildingButtonController button = uiFactory.CreateBuildingButton(buttonGroup, buildings[i]);
				_buttons.Add(button);
			}

			uiFactory.CreateBackButton(buttonGroup);
		}

		// FELIX  Extract forwarding Present/Dismiss to base class
		public override void OnPresenting()
		{
			foreach (BuildingButtonController button in _buttons)
			{
				button.OnPresenting();
			}
		}

		public override void OnDismissing()
		{
			foreach (BuildingButtonController button in _buttons)
			{
				button.OnDismissing();
			}
		}
	}
}