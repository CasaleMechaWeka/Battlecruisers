using BattleCruisers.Buildings;
using BattleCruisers.Buildings.Factories;
using BattleCruisers.Cruisers;
using BattleCruisers.UI.BuildingDetails;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattleCruisers.Units;

namespace BattleCruisers.UI.BuildMenus
{
	public class BuildMenuController : MonoBehaviour
	{
		private Presentable _homePanel;
		private IDictionary<BuildingCategory, Presentable> _buildingGroupPanels;
		private IDictionary<UnitCategory, Presentable> _unitGroupPanels;
		private Presentable _currentPanel;
		private IList<BuildingGroup> _buildingGroups;

		public UIFactory uiFactory;
		public UIManager uiManager;

		public void Initialise(IList<BuildingGroup> buildingGroups, IDictionary<UnitCategory, IList<Unit>> units)
		{
			_buildingGroups = buildingGroups;

			// Create main menu panel
			GameObject homePanelGameObject = uiFactory.CreatePanel(isActive: true);
			_homePanel = homePanelGameObject.AddComponent<Presentable>();
			_currentPanel = _homePanel;
			_homePanel.Initialize();

			// Create building category buttons
			HorizontalLayoutGroup homeButtonGroup = _homePanel.GetComponent<HorizontalLayoutGroup>();

			_buildingGroupPanels = new Dictionary<BuildingCategory, Presentable>(_buildingGroups.Count);

			for (int i = 0; i < _buildingGroups.Count; ++i)
			{
				// Create category button
				BuildingGroup group = _buildingGroups[i];
				uiFactory.CreateBuildingCategoryButton(homeButtonGroup, group);

				// Create category panel
				GameObject panelGameObject = uiFactory.CreatePanel(isActive: false);
				BuildingsMenuController buildingsMenu = panelGameObject.AddComponent<BuildingsMenuController>();
				buildingsMenu.Initialize(uiFactory, group.Buildings);
				_buildingGroupPanels[group.BuildingCategory] = buildingsMenu;
			}

			// Create menu UI for units
			_unitGroupPanels = new Dictionary<UnitCategory, Presentable>();

			foreach (UnitCategory unitCategory in units.Keys)
			{
				GameObject panelGameObject = uiFactory.CreatePanel(isActive: false);
				UnitsMenuController unitsMenu = panelGameObject.AddComponent<UnitsMenuController>();
				unitsMenu.Initialize(uiManager, uiFactory, units[unitCategory]);
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

		public void ShowUnitsMenu(Factory factory)
		{
			if (!_unitGroupPanels.ContainsKey(factory.unitCategory))
			{
				throw new ArgumentException();
			}

			// FELIX  Activation arg!!!
			Presentable panel = _unitGroupPanels[factory.unitCategory];
			panel.GetComponent<UnitsMenuController>().Factory = factory;
			ChangePanel(panel);
		}

		private bool ChangePanel(Presentable panel)
		{
			if (_currentPanel != panel)
			{
				if (_currentPanel != null)
				{
					_currentPanel.OnDismissing();
					_currentPanel.gameObject.SetActive(false);
				}

				panel.OnPresenting();
				panel.gameObject.SetActive(true);
				_currentPanel = panel;

				return true;
			}

			return false;
		}
	}
}
