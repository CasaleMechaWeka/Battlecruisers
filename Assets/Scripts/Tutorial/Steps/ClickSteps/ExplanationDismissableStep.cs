using BattleCruisers.Tutorial.Explanation;
using BattleCruisers.Tutorial.Providers;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.ClickSteps
{
    // FELIX  Test :D
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
            _dismissButton.IsVisible = true;
        }

        protected override void OnCompleted()
        {
            base.OnCompleted();
            _dismissButton.IsVisible = false;
        }
    }
}