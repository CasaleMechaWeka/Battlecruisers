using System;
using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Sorting;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.BuildMenus
{
    public class UnitsMenuController : BuildablesMenuController<IUnit>
	{
		private IUIManager _uiManager;
		private Factory _factory;

		public void Initialize(
			IUIManager uiManager,
			IUIFactory uiFactory,
			IList<IBuildableWrapper<IUnit>> units,
            IBuildableSorter<IUnit> sorter)
		{
            base.Initialize(uiFactory, units, sorter);

            Assert.IsNotNull(uiManager);
			_uiManager = uiManager;
		}

        protected override IPresentable CreateBuildableButton(IUIFactory uiFactory, HorizontalLayoutGroup buttonParent, IBuildableWrapper<IUnit> buildable)
        {
            return uiFactory.CreateUnitButton(buttonParent, buildable);
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
