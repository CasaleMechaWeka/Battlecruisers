using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.BuildProgress;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Construction;
using BattleCruisers.UI.BattleScene.Buttons.ClickHandlers;
using BattleCruisers.UI.Filters;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    public class UnitButtonController : BuildableButtonController
	{
        // The unit wrapper is always the same for this button.  In contrast 
        // the factory can change
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
            ISoundPlayer soundPlayer,
            IBuildableWrapper<IUnit> unitWrapper, 
            IBroadcastingFilter<IBuildable> shouldBeEnabledFilter,
            IUnitClickHandler unitClickHandler)
		{
            Helper.AssertIsNotNull(unitWrapper, unitClickHandler);

            base.Initialise(soundPlayer, unitWrapper.Buildable, shouldBeEnabledFilter);

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

            _currentFactory.UnitStarted += _currentFactory_StartedBuildingUnit;
            _currentFactory.NewUnitChosen += _currentFactory_NewUnitChosen;

            ShowBuildProgressIfNecessary(_currentFactory.UnitUnderConstruction);

            TriggerPotentialMatchChange();

			// Usually have this at the start of the overriding method, but 
			// do not want parent to call ShouldBeEnabled() until we have
			// set our _factory field.
			base.OnPresenting(activationParameter);
		}

        private void _factory_CompletedBuildable(object sender, EventArgs e)
		{
            TriggerPotentialMatchChange();
		}

        private void _currentFactory_StartedBuildingUnit(object sender, UnitStartedEventArgs e)
        {
            ShowBuildProgressIfNecessary(e.StartedUnit);
        }

        private void _currentFactory_NewUnitChosen(object sender, EventArgs e)
        {
            _buildProgressFeedback.HideBuildProgress();
        }

        private void ShowBuildProgressIfNecessary(IUnit unitUnderConstruction)
        {
            if (unitUnderConstruction != null
                && unitUnderConstruction.Name == _unitWrapper.Buildable.Name)
            {
                _buildProgressFeedback.ShowBuildProgress(unitUnderConstruction, _currentFactory);
            }
            else
            {
                _buildProgressFeedback.HideBuildProgress();
            }
        }

		public override void OnDismissing()
		{
			base.OnDismissing();

            _buildProgressFeedback.HideBuildProgress();
			_currentFactory.CompletedBuildable -= _factory_CompletedBuildable;
            _currentFactory.UnitStarted -= _currentFactory_StartedBuildingUnit;
            _currentFactory.NewUnitChosen -= _currentFactory_NewUnitChosen;
			_currentFactory = null;
		}

        protected override void HandleClick(bool isButtonEnabled)
        {
            Assert.IsNotNull(_currentFactory);
            _unitClickHandler.HandleClick(isButtonEnabled, _unitWrapper, _currentFactory);
		}
	}
}