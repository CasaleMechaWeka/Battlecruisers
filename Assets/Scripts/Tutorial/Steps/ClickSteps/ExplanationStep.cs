using BattleCruisers.Tutorial.Explanation;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.ClickSteps
{
    public abstract class ExplanationStep : TutorialStepNEW
    {
        private readonly IExplanationDismissButton _dismissButton;

        public ExplanationStep(ITutorialStepArgsNEW args, IExplanationDismissButton dismissButton)
            : base(args)
        {
            Assert.IsNotNull(dismissButton);
            _dismissButton = dismissButton;
        }

        public override void Start(Action completionCallback)
        {
            base.Start(completionCallback);

            _dismissButton.Enabled = true;
            _dismissButton.Clicked += _dismissButton_Clicked;
        }

        private void _dismissButton_Clicked(object sender, EventArgs e)
        {
            OnCompleted();
        }

        protected override void OnCompleted()
        {
            base.OnCompleted();

            _dismissButton.Enabled = false;
            _dismissButton.Clicked -= _dismissButton_Clicked;
        }
    }
}