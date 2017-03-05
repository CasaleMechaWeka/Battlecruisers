using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackButtonController : MonoBehaviour 
{
	public void Initialize(IBuildMenuController buildMenuController)
	{
		Button button = GetComponent<Button>();
		button.onClick.AddListener(() => buildMenuController.ShowBuildingGroups());
	}
}
