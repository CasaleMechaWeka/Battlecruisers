using BattleCruisers.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.Buildables.Buildings.Buttons
{
	public class BackButtonController : MonoBehaviour 
	{
		public void Initialize(UIManager uiManager)
		{
			Button button = GetComponent<Button>();
			button.onClick.AddListener(() => uiManager.ShowBuildingGroups());
		}
	}
}
