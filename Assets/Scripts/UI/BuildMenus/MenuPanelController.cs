using BattleCruisers.Buildings;
using BattleCruisers.Units;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.BuildMenus
{
	public class MenuPanelController : MonoBehaviour 
	{
		public void Initialize(
			IUIFactory uiFactory,
			IList<Building> buildings)
		{
			// Create building buttons
			HorizontalLayoutGroup buttonGroup = GetComponent<HorizontalLayoutGroup>();

			for (int i = 0; i < buildings.Count; ++i)
			{
				uiFactory.CreateBuildingButton(buttonGroup, buildings[i]);
			}

			uiFactory.CreateBackButton(buttonGroup);
		}

		public void Initialize(
			IUIFactory uiFactory,
			IList<Unit> units)
		{
			// Create unit buttons
			HorizontalLayoutGroup buttonGroup = GetComponent<HorizontalLayoutGroup>();

			for (int i = 0; i < units.Count; ++i)
			{
				uiFactory.CreateUnitButton(buttonGroup, units[i]);
			}

			uiFactory.CreateBackButton(buttonGroup);
		}
	}
}