using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IBuildMenuController
{
	void ShowBuildingGroups();
	void ShowBuildingGroup(BuildingGroup buildingGroup);
	void SelectBuilding(Building building);
}

public class BuildMenuController : MonoBehaviour, IBuildMenuController
{
	private IUIFactory _uiFactory;
	private GameObject _homePanel;
	private IDictionary<BuildingCategory, GameObject> _buildingGroupPanels;
	private GameObject _currentPanel;

	public Cruiser friendlyCruiser;

	private Building _selectedBuilding;
	public Building SelectedBuilding { get { return _selectedBuilding; } }

	// Use this for initialization
	void Start () 
	{
		_uiFactory = GetComponent<UIFactory>();

		// Create main menu panel
		_currentPanel = _homePanel;
		_homePanel = _uiFactory.CreatePanel(isActive: true);

		// Create building category buttons
		BuildingGroup[] buildingGroups = friendlyCruiser.loadout.BuildingGroups;
		HorizontalLayoutGroup homeButtonGroup = _homePanel.GetComponent<HorizontalLayoutGroup>();
		_buildingGroupPanels = new Dictionary<BuildingCategory, GameObject>(buildingGroups.Length);

		for (int i = 0; i < buildingGroups.Length; ++i)
		{
			// Create category button
			BuildingGroup group = buildingGroups[i];
			_uiFactory.CreateBuildingCategoryButton(homeButtonGroup, group);

			// Create category panel
			GameObject panel = _uiFactory.CreatePanel(isActive: false);
			_buildingGroupPanels[group.buildingCategory] = panel;
			panel.GetComponent<BuildingsMenuController>().Initialize(_uiFactory, group.Buildings);
		}

		Debug.Log("BuildMenuController.Start()  END");
	}

	public void ShowBuildingGroups()
	{
		Debug.Log("ShowBuildingGroups");
		friendlyCruiser.UnhighlightSlots();
		ChangePanel(_homePanel);
	}

	public void ShowBuildingGroup(BuildingGroup buildingGroup)
	{
		Debug.Log("ShowBuildingGroup");

		if (!_buildingGroupPanels.ContainsKey(buildingGroup.buildingCategory))
		{
			throw new ArgumentException();
		}

		GameObject panel = _buildingGroupPanels[buildingGroup.buildingCategory];
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

	public void SelectBuilding(Building building)
	{
		Debug.Log("ShowBuilding()");
		_selectedBuilding = building;
		friendlyCruiser.HighlightAvailableSlots(building.slotType);

		// FELIX  Show building details
	}
}
