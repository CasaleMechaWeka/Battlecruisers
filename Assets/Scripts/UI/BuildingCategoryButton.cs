using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class BuildingCategoryButton : MonoBehaviour 
{
	public void Initialize(BuildingGroup buildingGroup, IBuildMenuController buildMenuController)
	{
		Button button = GetComponent<Button>();
		button.GetComponentInChildren<Text>().text = buildingGroup.BuildingGroupName;
		button.onClick.AddListener(() => buildMenuController.ShowBuildingGroup(buildingGroup));
	}
}