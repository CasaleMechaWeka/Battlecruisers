using BattleCruisers.UI;
using BattleCruisers.UI.BuildMenus;
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

		public void Initialize(Unit unit, UnitsMenuController unitsMenu)
		{
			unitName.text = unit.buildableName;
			droneLevel.text = unit.numOfDronesRequired.ToString();
			unitImage.sprite = unit.UnitSprite;

			Button button = GetComponent<Button>();
			button.onClick.AddListener(() => unitsMenu.SelectUnit(unit));
		}
	}
}