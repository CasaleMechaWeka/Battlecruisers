using BattleCruisers.Buildables.Buildings;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace BattleCruisers.UI.Buttons
{
	public class BuildingCategoryButton : MonoBehaviour 
	{
		public void Initialize(BuildingGroup buildingGroup, UIManager uiManager)
		{
			Button button = GetComponent<Button>();
			button.GetComponentInChildren<Text>().text = buildingGroup.BuildingGroupName;
			button.onClick.AddListener(() => uiManager.SelectBuildingGroup(buildingGroup.BuildingCategory));
		}
	}
}
