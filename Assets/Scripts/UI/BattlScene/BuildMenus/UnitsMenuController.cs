using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.BattleScene.Buttons.ClickHandlers;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.Utils;
using System;
using System.Collections.Generic;

namespace BattleCruisers.UI.BattleScene.BuildMenus
{
    public class UnitsMenuController : BuildablesMenuController<UnitButtonController, IUnit>
	{
        private IUnitClickHandler _unitClickHandler;
        private Factory _factory;

		public override void Initialise(
            IUIManager uiManager,
            IButtonVisibilityFilters buttonVisibilityFilters,
            IList<IBuildableWrapper<IUnit>> units)
		{
            // FELIX  Can inject... => Avoid this codesmell :)
            // Need _unitClickHandler for abstract method called by base.Initialise().  Codesmell :P
            _unitClickHandler = new UnitClickHandler(uiManager);

            base.Initialise(uiManager, buttonVisibilityFilters, units);
		}

        protected override void InitialiseBuildableButton(UnitButtonController button, IBuildableWrapper<IUnit> buildableWrapper)
        {
            button.Initialise(buildableWrapper, _uiManager, _shouldBeEnabledFilter, _unitClickHandler);
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
