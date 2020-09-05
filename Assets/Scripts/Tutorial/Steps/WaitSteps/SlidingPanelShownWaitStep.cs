using BattleCruisers.UI.Panels;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.WaitSteps
{
    /// <summary>
    /// Completed when the sliding panel reaches the shown state.  Completes
    /// instantly if the panel is already in the shown state.
    /// </summary>
    public class SlidingPanelShownWaitStep : TutorialStep
    {
        private readonly ISlidingPanel _slidingPanel;

        public SlidingPanelShownWaitStep(ITutorialStepArgs args, ISlidingPanel slidingPanel)
            : base(args)
        {
            Assert.IsNotNull(slidingPanel);
            _slidingPanel = slidingPanel;
        }

        public override void Start(Action completionCallback)
        {
            base.Start(completionCallback);

            if (_slidingPanel.State.Value == PanelState.Shown)
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
            if (_slidingPanel.State.Value == PanelState.Shown)
            {
                _slidingPanel.State.ValueChanged -= State_ValueChanged;
                OnCompleted();
            }
        }
    }
}