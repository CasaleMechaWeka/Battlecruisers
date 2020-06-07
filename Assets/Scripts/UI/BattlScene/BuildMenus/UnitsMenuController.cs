using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.BattleScene.Buttons.ClickHandlers;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.BuildMenus
{
    public class UnitsMenuController : BuildablesMenuController<UnitButtonController, IUnit>
	{
        private IUnitClickHandler _unitClickHandler;
        private IFactory _factory;

		public void Initialise(
			ISingleSoundPlayer soundPlayer,
            IUIManager uiManager,
            IButtonVisibilityFilters buttonVisibilityFilters,
            IList<IBuildableWrapper<IUnit>> units,
            IUnitClickHandler clickHandler)
		{
            // Need _unitClickHandler for abstract method called by base.Initialise().  Codesmell :P
            Assert.IsNotNull(clickHandler);
            _unitClickHandler = clickHandler;

            base.Initialise(soundPlayer, uiManager, buttonVisibilityFilters, units);
		}

        protected override void InitialiseBuildableButton(UnitButtonController button, IBuildableWrapper<IUnit> buildableWrapper)
        {
            button.Initialise(_soundPlayer, buildableWrapper, _shouldBeEnabledFilter, _unitClickHandler);
        }

		public override void OnPresenting(object activationParameter)
		{
			base.OnPresenting(activationParameter);

			_factory = activationParameter.Parse<IFactory>();
			_factory.Destroyed += _factory_Destroyed;
		}

		private void _factory_Destroyed(object sender, EventArgs e)
		{
			_uiManager.HideCurrentlyShownMenu();
		}

		public override void OnDismissing()
		{
			base.OnDismissing();

			_factory.Destroyed -= _factory_Destroyed;
			_factory = null;
		}
    }
}
