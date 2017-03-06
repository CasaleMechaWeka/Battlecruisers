using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingButtonController : MonoBehaviour 
{
	public void Initialize(Building building, IBuildMenuController buildMenuController)
	{
		Button button = GetComponent<Button>();
		button.GetComponentInChildren<Text>().text = building.buildingName;
		button.onClick.AddListener(() => buildMenuController.ShowBuilding(building));
	}
}
