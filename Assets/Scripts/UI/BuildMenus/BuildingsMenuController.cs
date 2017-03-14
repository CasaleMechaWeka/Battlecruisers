using BattleCruisers.Buildings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.BuildMenus
{
	public class BuildingsMenuController : MonoBehaviour 
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
	}
}