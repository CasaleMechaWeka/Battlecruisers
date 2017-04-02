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
		private UIManager _uiManager;
		private Factory _factory;

		public void Initialize(
			UIManager uiManager,
			IUIFactory uiFactory,
			IList<Unit> units)
		{
			base.Initialize();

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
			Assert.IsNotNull(_factory);
			_factory.Unit = unit;
			_uiManager.ShowUnitDetails(unit);
		}

		public override void OnPresenting(object activationParameter)
		{
			base.OnPresenting(activationParameter);

			_factory = activationParameter as Factory;
			Assert.IsNotNull(_factory);
		}
	}
}
