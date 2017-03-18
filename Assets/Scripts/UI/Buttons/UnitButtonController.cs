using BattleCruisers.UI;
using BattleCruisers.Units;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.Buildings.Buttons
{
	public class UnitButtonController : MonoBehaviour 
	{
		public Image unitImage;
		public Text unitName;
		public Text droneLevel;

		public void Initialize(Unit unit, UIManager uiManager)
		{
			unitName.text = unit.unitName;
			droneLevel.text = unit.numOfDronesRequired.ToString();
			unitImage.sprite = unit.UnitSprite;

			Button button = GetComponent<Button>();

			// FELIX
			// 1. Show unit details
			// 2. Start building unit out of factory
//			button.onClick.AddListener(() => uiManager.SelectunitFromMenu(unit));
		}
	}
}