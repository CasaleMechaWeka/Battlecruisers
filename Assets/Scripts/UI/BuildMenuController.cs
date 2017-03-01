using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//public interface IBuildMenuController
//{
//	BuildingGroup[] Buildings { set; }
//
//	void ShowBuildingGroups();
//	void ShowBuildingGroup(IBuildingGroup buildingGroup);
//	void ShowBuilding(IBuilding building);
//}

public class BuildMenuController : MonoBehaviour
{
	private Canvas _canvas;

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

		// Create building category buttons
		GameObject buildingGroupsPanel = Instantiate(menuPanelPrefab);
		buildingGroupsPanel.transform.SetParent(_canvas.transform);
		RectTransform rectTransform = buildingGroupsPanel.GetComponent<RectTransform>();
		rectTransform.anchoredPosition = new Vector2(0, 0);
//		buildingGroupsPanel.transform.

		// Create building category menu panels
		foreach (BuildingGroup group in BuildingGroups)
		{
//			Button button = (Button)Instantiate(buttonPrefab);
//			button.transform.SetParent(buttonGroup.transform, worldPositionStays: false);
		}

//		factoriesPanel.Initialize(this, buttonPrefab, Factories);

//		_factoriesPanel = (BuildMenuPanelController)GetComponent("

//		BuildMenuPanelController panelController = Instantiate(panelPrefab).GetComponent<BuildMenuPanelController>();
//		panelController.transform.parent = canvas.transform;
//		HorizontalLayoutGroup buttonGroup = GetComponent<HorizontalLayoutGroup>();
//
//		// Instantiate group buttons
//		for (int i = 0; i < 5; ++i)
//		{
//		}

		// Instantiate building buttons

		Debug.Log("BuildMenuController.Start()  END");
	}

	public void ShowBuildingGroups()
	{
		throw new System.NotImplementedException();
	}

	public void ShowBuildingGroup(IBuildingGroup buildingGroup)
	{
		throw new System.NotImplementedException();
	}

	public void ShowBuilding(IBuilding building)
	{
		throw new System.NotImplementedException();
	}
}
