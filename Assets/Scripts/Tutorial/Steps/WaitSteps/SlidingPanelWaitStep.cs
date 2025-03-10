using BattleCruisers.UI.Panels;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.WaitSteps
{
    /// <summary>
    /// Completed when the sliding panel reaches the desired state.  Completes
    /// instantly if the panel is already in the desired state.
    /// </summary>
    public class SlidingPanelWaitStep : TutorialStep
    {
        private readonly ISlidingPanel _slidingPanel;
        private readonly PanelState _desiredState;

        public SlidingPanelWaitStep(ITutorialStepArgs args, ISlidingPanel slidingPanel, PanelState desiredState)
            : base(args)
        {
            Assert.IsNotNull(slidingPanel);

            _slidingPanel = slidingPanel;
            _desiredState = desiredState;
        }

        public override void Start(Action completionCallback)
        {
            base.Start(completionCallback);

            if (_slidingPanel.State.Value == _desiredState)
            {
                OnCompleted();
            }
            else
            {
                _slidingPanel.State.ValueChanged += State_ValueChanged;
            }
        }

        private void State_ValueChanged(object sender, EventArgs e)
        {
            if (_slidingPanel.State.Value == _desiredState)
            {
                _slidingPanel.State.ValueChanged -= State_ValueChanged;
                OnCompleted();
            }
        }
    }
}