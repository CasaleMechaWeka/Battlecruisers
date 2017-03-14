using BattleCruisers.UI.BuildMenus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.Buildings.Buttons
{
	public class BackButtonController : MonoBehaviour 
	{
		public void Initialize(IBuildMenuController buildMenuController)
		{
			Button button = GetComponent<Button>();
			button.onClick.AddListener(() => buildMenuController.ShowBuildingGroups());
		}
	}
}
