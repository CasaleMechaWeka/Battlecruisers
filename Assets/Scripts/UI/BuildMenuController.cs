using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IBuildMenuController
{
//	BuildingGroup[] Buildings { set; }

//	void ShowBuildingGroups();
	void ShowBuildingGroup(IBuildingGroup buildingGroup);
//	void ShowBuilding(IBuilding building);
}

public class BuildMenuController : MonoBehaviour, IBuildMenuController
{
	private Canvas _canvas;
	private GameObject _homePanel;
	private IDictionary<BuildingCategory, GameObject> _buildingGroupPanels;
	private GameObject _currentPanel;

	public GameObject menuPanelPrefab;
	public Button buttonPrefab;

	public BuildingsMenuController factoriesPanel;

//	public RectTransform panel;

//	public Canvas canvas;
//	public GameObject panelPrefab;
//	public Button buttonPrefab;

	private BuildingGroup[] _buildingGroups;
	public BuildingGroup[] BuildingGroups 
	{ 
		get { return _buildingGroups; }
		set
		{
			if (value == null
				|| value.Length < MIN_NUM_OF_BUILDING_GROUPS
			    || value.Length > MAX_NUM_OF_BUILDING_GROUPS)
			{
				throw new ArgumentException();
			}

			_buildingGroups = value;
		}
	}

//	public Building[] Factories { private get; set; }
//	public Building[] Turrets { private get; set; }

	// User needs to be able to build at least one building
	private const int MIN_NUM_OF_BUILDING_GROUPS = 1;
	// Currently only support 6 types of buildings, so the UI is optimsed for this.  Ie, there is no space for more!
	private const int MAX_NUM_OF_BUILDING_GROUPS = 6;

	// Use this for initialization
	void Start () 
	{
		_canvas = GetComponent<Canvas>();

		// Create building category menu panel
		// FELIX  Avoid duplicate code, PanelFactory?
		_currentPanel = _homePanel;
		_homePanel = Instantiate(menuPanelPrefab);
		_homePanel.transform.SetParent(_canvas.transform);
		RectTransform rectTransform = _homePanel.GetComponent<RectTransform>();
		rectTransform.anchoredPosition = new Vector2(0, 0);

		// Create building category buttons
		HorizontalLayoutGroup homeButtonGroup = _homePanel.GetComponent<HorizontalLayoutGroup>();
		_buildingGroupPanels = new Dictionary<BuildingCategory, GameObject>(BuildingGroups.Length);

		for (int i = 0; i < BuildingGroups.Length; ++i)
		{
			// FELIX:  Map category to panel
			IBuildingGroup group = BuildingGroups[i];
			Button button = (Button)Instantiate(buttonPrefab);
			button.transform.SetParent(homeButtonGroup.transform, worldPositionStays: false);
			button.GetComponent<BuildingCategoryButton>().Initialize(group, this);

			// Create panel
			GameObject panel = Instantiate(menuPanelPrefab);
			panel.SetActive(false);
			panel.transform.SetParent(_canvas.transform);
			rectTransform = panel.GetComponent<RectTransform>();
			rectTransform.anchoredPosition = new Vector2(0, 0);
			_buildingGroupPanels[group.BuildingCategory] = panel;
			panel.GetComponent<BuildingsMenuController>().Initialize(this, buttonPrefab, group.Buildings);
		}

		Debug.Log("BuildMenuController.Start()  END");
	}

	public void ShowBuildingGroups()
	{
		Debug.Log("ShowBuildingGroups");
	}

	public void ShowBuildingGroup(IBuildingGroup buildingGroup)
	{
		Debug.Log("ShowBuildingGroup");

		if (!_buildingGroupPanels.ContainsKey(buildingGroup.BuildingCategory))
		{
			throw new ArgumentException();
		}

		GameObject panel = _buildingGroupPanels[buildingGroup.BuildingCategory];
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

	public void ShowBuilding(IBuilding building)
	{
		throw new System.NotImplementedException();
	}
}
