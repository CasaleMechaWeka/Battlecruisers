using BattleCruisers.Tutorial.Explanation;
using BattleCruisers.Tutorial.Providers;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.ClickSteps
{
    public class ExplanationDismissableStep : ExplanationClickStep
    {
        private readonly IExplanationDismissButton _dismissButton;

        public ExplanationDismissableStep(ITutorialStepArgsNEW args, IExplanationDismissButton dismissButton)
            : base(args, new StaticProvider<IClickableEmitter>(dismissButton))
        {
            Assert.IsNotNull(dismissButton);
            _dismissButton = dismissButton;
        }

        public override void Start(Action completionCallback)
        {
            base.Start(completionCallback);
            _dismissButton.Enabled = true;
        }

        private void _dismissButton_Clicked(object sender, EventArgs e)
        {
            _dismissButton.Enabled = false;
            _dismissButton.Clicked -= _dismissButton_Clicked;

            OnCompleted();
        }
    }
}