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
	public class UnitsMenuController : Presentable
	{
		public void Initialize(
			IUIFactory uiFactory,
			IList<Unit> units)
		{
			base.Initialize();

			// Create unit buttons
			HorizontalLayoutGroup buttonGroup = GetComponent<HorizontalLayoutGroup>();

			for (int i = 0; i < units.Count; ++i)
			{
				IPresentable presentable = uiFactory.CreateUnitButton(buttonGroup, units[i]);
				_childPresentables.Add(presentable);
			}

			uiFactory.CreateBackButton(buttonGroup);
		}
	}
}
