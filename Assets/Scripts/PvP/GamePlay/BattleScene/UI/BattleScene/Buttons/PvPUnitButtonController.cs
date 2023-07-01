using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Factories;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.BuildProgress;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.ClickHandlers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System;
using System.Diagnostics;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons
{
    public class PvPUnitButtonController : PvPBuildableButtonController
    {
        // The unit wrapper is always the same for this button.  In contrast 
        // the factory can change
        private IPvPBuildableWrapper<IPvPUnit> _unitWrapper;
        private IPvPUnitClickHandler _unitClickHandler;
        private IPvPUnitBuildProgressTrigger _unitBuildProgress;

        private IPvPFactory _currentFactory;
        private IPvPFactory CurrentFactory
        {
            get => _currentFactory;
            set
            {
                if (_currentFactory != null)
                {
                    _currentFactory.CompletedBuildable -= _currentFactory_CompletedBuildable;
                }

                _currentFactory = value;
                _unitBuildProgress.Factory = value;

                if (_currentFactory != null
                    && _currentFactory.BuildableState != PvPBuildableState.Completed)
                {
                    _currentFactory.CompletedBuildable += _currentFactory_CompletedBuildable;
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
                    && CurrentFactory.BuildableState == PvPBuildableState.Completed;
            }
        }

        public void Initialise(
            IPvPSingleSoundPlayer soundPlayer,
            IPvPBuildableWrapper<IPvPUnit> unitWrapper,
            IPvPBroadcastingFilter<IPvPBuildable> shouldBeEnabledFilter,
            IPvPUnitClickHandler unitClickHandler)
        {
            PvPHelper.AssertIsNotNull(unitWrapper, unitClickHandler);

            base.Initialise(soundPlayer, unitWrapper.Buildable, shouldBeEnabledFilter);

            _unitWrapper = unitWrapper;
            _unitClickHandler = unitClickHandler;

            PvPBuildProgressFeedbackWrapper feedbackWrapper = GetComponentInChildren<PvPBuildProgressFeedbackWrapper>();
            Assert.IsNotNull(feedbackWrapper);
            IPvPBuildProgressFeedback buildProgressFeedback = feedbackWrapper.CreateFeedback();

            _unitBuildProgress = new PvPUnitBuildProgressTrigger(new PvPUnitBuildProgress(unitWrapper.Buildable.Name, buildProgressFeedback));
        }

        public override void OnPresenting(object activationParameter)
        {
            
            CurrentFactory = activationParameter.Parse<IPvPFactory>();           
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

        public override void HandleHover()
        {
            _unitClickHandler.HandleHover(_unitWrapper);
        }

        public override void HandleHoverExit()
        {
            _unitClickHandler.HandleHoverExit();
        }
    }
}