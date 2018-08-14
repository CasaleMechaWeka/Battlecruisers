using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    public class UnitButtonController : BuildableButtonController
	{
        // The unit wrapper is always the same for this button.  The factory
        // can change :)
		private IBuildableWrapper<IUnit> _unitWrapper;
		private IFactory _currentFactory;
        private IUnitClickHandler _unitClickHandler;

        public override bool IsMatch
		{
			get
			{
				return 
                    base.IsMatch
                    && _currentFactory != null
                    && _currentFactory.BuildableState == BuildableState.Completed;
			}
		}

		public void Initialise(
            IBuildableWrapper<IUnit> unitWrapper, 
            IUIManager uiManager, 
            IBroadcastingFilter<IBuildable> shouldBeEnabledFilter,
            IUnitClickHandler unitClickHandler)
		{
            Helper.AssertIsNotNull(unitWrapper, unitClickHandler);

            base.Initialise(unitWrapper.Buildable, uiManager, shouldBeEnabledFilter);

			_unitWrapper = unitWrapper;
            _unitClickHandler = unitClickHandler;
		}

		public override void OnPresenting(object activationParameter)
		{
			_currentFactory = activationParameter.Parse<IFactory>();

			if (_currentFactory.BuildableState != BuildableState.Completed)
			{
				_currentFactory.CompletedBuildable += _factory_CompletedBuildable;
			}

            TriggerPotentialMatchChange();

			// Usually have this at the start of the overriding method, but 
			// do not want parent to call ShouldBeEnabled() until we have
			// set our _factory field.
			base.OnPresenting(activationParameter);
		}

		private void _factory_CompletedBuildable(object sender, System.EventArgs e)
		{
            TriggerPotentialMatchChange();
		}

		public override void OnDismissing()
		{
			base.OnDismissing();

			_currentFactory.CompletedBuildable -= _factory_CompletedBuildable;
			_currentFactory = null;
		}

		protected override void HandleClick()
		{
            base.HandleClick();

            Assert.IsNotNull(_currentFactory);

            _unitClickHandler.HandleUnitClick(_unitWrapper, _currentFactory);
			_uiManager.ShowUnitDetails(_unitWrapper.Buildable);
		}
	}
}