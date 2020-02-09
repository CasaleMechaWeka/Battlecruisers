using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.BuildProgress;
using BattleCruisers.Buildables.Units;
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
        private IUnitClickHandler _unitClickHandler;
        private IUnitBuildProgress _unitBuildProgress;

		private IFactory _currentFactory;
        private IFactory CurrentFactory
        {
            get => _currentFactory;
            set
            {
                if (_currentFactory != null)
                {
                    _currentFactory.CompletedBuildable -= _currentFactory_CompletedBuildable;
                    
                    if (_currentFactory.BuildableState != BuildableState.Completed)
                    {
                        _currentFactory.CompletedBuildable += _currentFactory_CompletedBuildable;
                    }
                }

                _currentFactory = value;
                _unitBuildProgress.Factory = value;

                if (_currentFactory != null)
                {
                    _currentFactory.CompletedBuildable -= _currentFactory_CompletedBuildable;
                }
            }
        }

        public override bool IsMatch
		{
			get
			{
				return 
                    base.IsMatch
                    && CurrentFactory != null
                    && CurrentFactory.BuildableState == BuildableState.Completed;
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
            IBuildProgressFeedback buildProgressFeedback = feedbackWrapper.CreateFeedback();

            _unitBuildProgress = new UnitBuildProgress(unitWrapper.Buildable.Name, buildProgressFeedback);
        }

		public override void OnPresenting(object activationParameter)
		{
			CurrentFactory = activationParameter.Parse<IFactory>();
            TriggerPotentialMatchChange();

			// Usually have this at the start of the overriding method, but 
			// do not want parent to call ShouldBeEnabled() until we have
			// set our factory field.
			base.OnPresenting(activationParameter);
		}

        private void _currentFactory_CompletedBuildable(object sender, EventArgs e)
		{
            TriggerPotentialMatchChange();
		}

		public override void OnDismissing()
		{
			base.OnDismissing();
            CurrentFactory = null;
        }

        protected override void HandleClick(bool isButtonEnabled)
        {
            Assert.IsNotNull(CurrentFactory);
            _unitClickHandler.HandleClick(isButtonEnabled, _unitWrapper, CurrentFactory);
		}
	}
}