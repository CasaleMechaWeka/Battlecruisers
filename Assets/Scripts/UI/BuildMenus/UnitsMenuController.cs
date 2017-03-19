using BattleCruisers.Buildings;
using BattleCruisers.Buildings.Factories;
using BattleCruisers.Units;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.BuildMenus
{
	public class UnitsMenuController : MonoBehaviour 
	{
		private UIManager _uiManager;

		public Factory Factory { private get; set; }

		public void Initialize(
			UIManager uiManager,
			IUIFactory uiFactory,
			IList<Unit> units)
		{
			_uiManager = uiManager;

			// Create unit buttons
			HorizontalLayoutGroup buttonGroup = GetComponent<HorizontalLayoutGroup>();

			for (int i = 0; i < units.Count; ++i)
			{
				uiFactory.CreateUnitButton(buttonGroup, units[i], this);
			}

			uiFactory.CreateBackButton(buttonGroup);
		}

		public void SelectUnit(Unit unit)
		{
			Assert.IsNotNull(Factory);
			Factory.Unit = unit;

			// FELIX
			// Show building details
		}
	}
}
