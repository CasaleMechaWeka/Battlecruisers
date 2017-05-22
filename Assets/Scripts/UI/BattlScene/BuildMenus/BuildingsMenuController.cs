using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.UI.BattleScene.Buttons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.BuildMenus
{
	public class BuildingsMenuController : Presentable
	{
		public void Initialize(
			IUIFactory uiFactory,
			IList<BuildingWrapper> buildings)
		{
			base.Initialize();

			// Create building buttons
			HorizontalLayoutGroup buttonGroup = GetComponent<HorizontalLayoutGroup>();

			for (int i = 0; i < buildings.Count; ++i)
			{
				BuildingButtonController button = uiFactory.CreateBuildingButton(buttonGroup, buildings[i]);
				_childPresentables.Add(button);
			}

			uiFactory.CreateBackButton(buttonGroup);
		}
	}
}