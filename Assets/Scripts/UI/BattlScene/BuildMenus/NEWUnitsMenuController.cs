using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;
using System;
using System.Collections.Generic;

namespace BattleCruisers.UI.BattleScene.BuildMenus
{
    public class NEWUnitsMenuController : NEWBuildablesMenuController<UnitButtonController, IUnit>
	{
        // FELIX  Move UIManager to parent class?
        private IUIManager _uiManager;
        private IBroadcastingFilter<IBuildable> _shouldBeEnabledFilter;
        private IUnitClickHandler _unitClickHandler;
        private Factory _factory;

		public void Initialise(
            IList<IBuildableWrapper<IUnit>> units,
            IUIManager uiManager, 
            IBroadcastingFilter<IBuildable> shouldBeEnabledFilter,
            IUnitClickHandler unitClickHandler)
		{
            base.Initialise(units);

            Helper.AssertIsNotNull(uiManager, shouldBeEnabledFilter, unitClickHandler);

            _uiManager = uiManager;
            _shouldBeEnabledFilter = shouldBeEnabledFilter;
            _unitClickHandler = unitClickHandler;
		}

        protected override void InitialiseBuildableButton(UnitButtonController button, IBuildableWrapper<IUnit> buildable)
        {
            button.Initialise(buildable, _uiManager, _shouldBeEnabledFilter, _unitClickHandler);
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
