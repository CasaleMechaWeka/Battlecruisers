using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.BuildMenus
{
	public class UnitsMenuController : Presentable
	{
		private UIManager _uiManager;
		private Factory _factory;

		public void Initialize(
			UIManager uiManager,
			IUIFactory uiFactory,
			IList<UnitWrapper> units)
		{
			base.Initialize();

			_uiManager = uiManager;

			// Create unit buttons
			HorizontalLayoutGroup buttonGroup = GetComponent<HorizontalLayoutGroup>();

			for (int i = 0; i < units.Count; ++i)
			{
				IPresentable presentable = uiFactory.CreateUnitButton(buttonGroup, units[i]);
				_childPresentables.Add(presentable);
			}

			uiFactory.CreateBackButton(buttonGroup);
		}

		public override void OnPresenting(object activationParameter)
		{
			base.OnPresenting(activationParameter);

			_factory = activationParameter as Factory;
			Assert.IsNotNull(_factory);
			_factory.Destroyed += _factory_Destroyed;
		}

		private void _factory_Destroyed(object sender, EventArgs e)
		{
			_uiManager.ShowBuildingGroups();
		}

		public override void OnDismissing()
		{
			base.OnDismissing();

			_factory.Destroyed -= _factory_Destroyed;
			_factory = null;
		}
	}
}
