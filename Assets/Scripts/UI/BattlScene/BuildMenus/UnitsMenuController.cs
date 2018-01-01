using System;
using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Sorting;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.BuildMenus
{
    public class UnitsMenuController : Presentable
	{
		private IUIManager _uiManager;
		private Factory _factory;

		public void Initialize(
			IUIManager uiManager,
			IUIFactory uiFactory,
			IList<IBuildableWrapper<IUnit>> units,
            IBuildableSorter<IUnit> sorter)
		{
			base.Initialize();

            Helper.AssertIsNotNull(uiManager, uiFactory, units, sorter);

			_uiManager = uiManager;

            units = sorter.Sort(units);

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

			_factory = activationParameter.Parse<Factory>();
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
