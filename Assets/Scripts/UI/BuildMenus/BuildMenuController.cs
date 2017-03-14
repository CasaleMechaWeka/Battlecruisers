using BattleCruisers.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.UI.BuildingDetails;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.BuildMenus
{
	public class BuildMenuController : MonoBehaviour
	{
		private GameObject _homePanel;
		private IDictionary<BuildingCategory, GameObject> _buildingGroupPanels;
		private GameObject _currentPanel;
		private IList<BuildingGroup> _buildingGroups;

		public UIFactory uiFactory;
		public UIManager uiManager;

		public void Initialise(IList<BuildingGroup> buildingGroups)
		{
			_buildingGroups = buildingGroups;

			// Create main menu panel
			_homePanel = uiFactory.CreatePanel(isActive: true);
			_currentPanel = _homePanel;

			// Create building category buttons
			HorizontalLayoutGroup homeButtonGroup = _homePanel.GetComponent<HorizontalLayoutGroup>();

			_buildingGroupPanels = new Dictionary<BuildingCategory, GameObject>(_buildingGroups.Count);

			for (int i = 0; i < _buildingGroups.Count; ++i)
			{
				// Create category button
				BuildingGroup group = _buildingGroups[i];
				uiFactory.CreateBuildingCategoryButton(homeButtonGroup, group);

				// Create category panel
				GameObject panel = uiFactory.CreatePanel(isActive: false);
				_buildingGroupPanels[group.BuildingCategory] = panel;
				panel.GetComponent<BuildingsMenuController>().Initialize(uiFactory, group.Buildings);
			}
		}

		public void HideBuildMenu()
		{
			ChangePanel(_homePanel);
			_currentPanel.SetActive(false);
		}

		public void ShowBuildMenu()
		{
			_currentPanel.SetActive(true);
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

			GameObject panel = _buildingGroupPanels[buildingCategory];
			ChangePanel(panel);
		}

		private bool ChangePanel(GameObject panel)
		{
			if (_currentPanel != panel)
			{
				if (_currentPanel != null)
				{
					_currentPanel.SetActive(false);
				}

				panel.SetActive(true);
				_currentPanel = panel;

				return true;
			}

			return false;
		}
	}
}
