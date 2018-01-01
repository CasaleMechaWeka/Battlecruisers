using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Sorting;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.BuildMenus
{
    // FELIX  Avoid duplicate code with UnitsMenuController
    public class BuildingsMenuController : Presentable
	{
		public void Initialize(
			IUIFactory uiFactory,
			IList<IBuildableWrapper<IBuilding>> buildings,
            IBuildableSorter<IBuilding> sorter)
		{
			base.Initialize();

            Helper.AssertIsNotNull(uiFactory, buildings, sorter);

            buildings = sorter.Sort(buildings);

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