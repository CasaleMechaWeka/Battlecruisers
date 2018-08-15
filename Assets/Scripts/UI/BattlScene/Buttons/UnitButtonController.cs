using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.BuildProgress;
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
        private IBuildProgressFeedback _buildProgressFeedback;

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

            BuildProgressFeedbackWrapper feedbackWrapper = GetComponentInChildren<BuildProgressFeedbackWrapper>();
            Assert.IsNotNull(feedbackWrapper);
            _buildProgressFeedback = feedbackWrapper.CreateFeedback();
        }

		public override void OnPresenting(object activationParameter)
		{
			_currentFactory = activationParameter.Parse<IFactory>();

			if (_currentFactory.BuildableState != BuildableState.Completed)
			{
				_currentFactory.CompletedBuildable += _factory_CompletedBuildable;
			}

            _currentFactory.StartedBuildingUnit += _currentFactory_StartedBuildingUnit;
            ShowBuildProgressIfNecessary(_currentFactory.UnitUnderConstruction);

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

        private void _currentFactory_StartedBuildingUnit(object sender, StartedUnitConstructionEventArgs e)
        {
            ShowBuildProgressIfNecessary(e.Buildable);
        }

        private void ShowBuildProgressIfNecessary(IUnit unitUnderConstruction)
        {
            if (unitUnderConstruction != null
                && unitUnderConstruction.Name == _unitWrapper.Buildable.Name)
            {
                _buildProgressFeedback.ShowBuildProgress(unitUnderConstruction);
            }
        }

		public override void OnDismissing()
		{
			base.OnDismissing();

            _buildProgressFeedback.HideBuildProgress();
			_currentFactory.CompletedBuildable -= _factory_CompletedBuildable;
            _currentFactory.StartedBuildingUnit -= _currentFactory_StartedBuildingUnit;
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