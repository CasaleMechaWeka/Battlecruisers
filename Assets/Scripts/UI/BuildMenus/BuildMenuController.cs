using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IBuildMenuController
{
	void ShowBuildingGroups();
	void ShowBuildingGroup(BuildingGroup buildingGroup);
	void SelectBuilding(Building building, bool allowDelete = false);
}

public class BuildMenuController : MonoBehaviour, IBuildMenuController
{
	private IUIFactory _uiFactory;
	private GameObject _homePanel;
	private IDictionary<BuildingCategory, GameObject> _buildingGroupPanels;
	private GameObject _currentPanel;
	private IList<BuildingGroup> _buildingGroups;
	private bool _isInitialised = false;

	public Cruiser friendlyCruiser;
	public BuildingDetailsController buildingDetails;

	private Building _selectedBuilding;
	public Building SelectedBuilding { get { return _selectedBuilding; } }

	public void Initialise(IList<BuildingGroup> buildingGroups)
	{
		_buildingGroups = buildingGroups;
		_isInitialised = true;
	}

	// Use this for initialization
	void Start () 
	{
		if (!_isInitialised)
		{
			throw new InvalidProgramException();
		}

		Debug.Log("BuildingGroups.Count: " + _buildingGroups.Count);

		_uiFactory = GetComponent<UIFactory>();

		// Create main menu panel
		_homePanel = _uiFactory.CreatePanel(isActive: true);
		_currentPanel = _homePanel;

		// Create building category buttons
		HorizontalLayoutGroup homeButtonGroup = _homePanel.GetComponent<HorizontalLayoutGroup>();

		_buildingGroupPanels = new Dictionary<BuildingCategory, GameObject>(_buildingGroups.Count);

		for (int i = 0; i < _buildingGroups.Count; ++i)
		{
			// Create category button
			BuildingGroup group = _buildingGroups[i];
			_uiFactory.CreateBuildingCategoryButton(homeButtonGroup, group);

			// Create category panel
			GameObject panel = _uiFactory.CreatePanel(isActive: false);
			_buildingGroupPanels[group.BuildingCategory] = panel;
			panel.GetComponent<BuildingsMenuController>().Initialize(_uiFactory, group.Buildings);
		}

		Debug.Log("BuildMenuController.Start()  END");
	}

	public void ShowBuildingGroups()
	{
		Debug.Log("ShowBuildingGroups");

		buildingDetails.Hide();

		friendlyCruiser.UnhighlightSlots();
		friendlyCruiser.HideAllSlots();

		ChangePanel(_homePanel);
	}

	public void ShowBuildingGroup(BuildingGroup buildingGroup)
	{
		Debug.Log("ShowBuildingGroup");

		if (!_buildingGroupPanels.ContainsKey(buildingGroup.BuildingCategory))
		{
			throw new ArgumentException();
		}

		GameObject panel = _buildingGroupPanels[buildingGroup.BuildingCategory];
		ChangePanel(panel);
		friendlyCruiser.ShowAllSlots();
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

	public void SelectBuilding(Building building, bool allowDelete = false)
	{
		Debug.Log("SelectBuilding()");
		_selectedBuilding = building;
		friendlyCruiser.HighlightAvailableSlots(building.slotType);
		buildingDetails.ShowBuildingDetails(building, allowDelete);
	}
}
