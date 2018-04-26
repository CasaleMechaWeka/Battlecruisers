using System;
using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Sorting;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.BuildMenus
{
    public class BuildMenuController : MonoBehaviour, IBuildMenu
	{
		private IUIManager _uiManager;
        private IUIFactory _uiFactory;
		private IList<IBuildingGroup> _buildingGroups;
        private Presentable _homePanel;
        private IDictionary<BuildingCategory, Presentable> _buildingGroupPanels;
        private IDictionary<UnitCategory, Presentable> _unitGroupPanels;
        private Presentable _currentPanel;

		public void Initialise(
			IUIManager uiManager,
            IUIFactory uiFactory,
            IList<IBuildingGroup> buildingGroups, 
            IDictionary<UnitCategory, IList<IBuildableWrapper<IUnit>>> units,
            IBuildableSorterFactory sorterFactory)
		{
            Helper.AssertIsNotNull(uiManager, uiFactory, buildingGroups, units, sorterFactory);

            _uiManager = uiManager;
            _uiFactory = uiFactory;
			_buildingGroups = buildingGroups;

			// Create main menu panel
			GameObject homePanelGameObject = _uiFactory.CreatePanel(isActive: true);
			_homePanel = homePanelGameObject.AddComponent<Presentable>();
			_currentPanel = _homePanel;
			_homePanel.Initialise();

			// Create building category buttons
			HorizontalLayoutGroup homeButtonGroup = _homePanel.GetComponent<HorizontalLayoutGroup>();
            IBuildableSorter<IBuilding> buildingSorter = sorterFactory.CreateBuildingSorter();

			_buildingGroupPanels = new Dictionary<BuildingCategory, Presentable>(_buildingGroups.Count);

			for (int i = 0; i < _buildingGroups.Count; ++i)
			{
				// Create category button
				IBuildingGroup buildingGroup = _buildingGroups[i];
				_uiFactory.CreateBuildingCategoryButton(homeButtonGroup, buildingGroup);

				// Create category panel
				GameObject panelGameObject = _uiFactory.CreatePanel(isActive: false);
				BuildingsMenuController buildingsMenu = panelGameObject.AddComponent<BuildingsMenuController>();
                buildingsMenu.Initialise(_uiFactory, buildingGroup.Buildings, buildingSorter);
				_buildingGroupPanels[buildingGroup.BuildingCategory] = buildingsMenu;
			}

            // Create menu UI for units
            IBuildableSorter<IUnit> unitSorter = sorterFactory.CreateUnitSorter();
			_unitGroupPanels = new Dictionary<UnitCategory, Presentable>();

			foreach (UnitCategory unitCategory in units.Keys)
			{
				GameObject panelGameObject = _uiFactory.CreatePanel(isActive: false);
				UnitsMenuController unitsMenu = panelGameObject.AddComponent<UnitsMenuController>();
                unitsMenu.Initialise(_uiManager, _uiFactory, units[unitCategory], unitSorter);
				_unitGroupPanels[unitCategory] = unitsMenu;
			}
		}

		public void HideBuildMenu()
		{
			ChangePanel(_homePanel);
			_currentPanel.gameObject.SetActive(false);
		}

		public void ShowBuildMenu()
		{
			_currentPanel.gameObject.SetActive(true);
		}

		public void ShowBuildingGroupsMenu()
		{
			ChangePanel(_homePanel);
		}

		public void ShowBuildingGroupMenu(BuildingCategory buildingCategory)
		{
			if (!_buildingGroupPanels.ContainsKey(buildingCategory))
			{
				throw new ArgumentException();
			}

			Presentable panel = _buildingGroupPanels[buildingCategory];
			ChangePanel(panel);
		}

		public void ShowUnitsMenu(IFactory factory)
		{
			if (!_unitGroupPanels.ContainsKey(factory.UnitCategory))
			{
				throw new ArgumentException();
			}

			Presentable panel = _unitGroupPanels[factory.UnitCategory];
			ChangePanel(panel, factory);
		}

		private bool ChangePanel(Presentable panel, object activationParameter = null)
		{
			if (_currentPanel != panel)
			{
				if (_currentPanel != null)
				{
					_currentPanel.OnDismissing();
					_currentPanel.gameObject.SetActive(false);
				}

				panel.OnPresenting(activationParameter);
				panel.gameObject.SetActive(true);
				_currentPanel = panel;

				return true;
			}

			return false;
		}
	}
}
