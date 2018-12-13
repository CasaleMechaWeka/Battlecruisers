using BattleCruisers.Tutorial.Explanation;
using BattleCruisers.Tutorial.Providers;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.ClickSteps
{
    public class ExplanationDismissableStep : ExplanationClickStep
    {
        private readonly IExplanationDismissButton _dismissButton;

        public ExplanationDismissableStep(ITutorialStepArgs args, IExplanationDismissButton dismissButton)
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
            // This needs to happen BEFORE base.OnCompleted(), because that will trigger the start
            // the next step which may enable the _dismissButton.  If this was after base.OnCompleted()
            // the _dimissButton.IsVisible would be overridden.
            _dismissButton.IsVisible = false;

            base.OnCompleted();
        }
    }
}