using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps
{
    /// <summary>
    /// Completed when the user clicks on a specific element.
    /// </summary>
    // FELIX  Test :D
    public class ClickStep : TutorialStep
    {
        private readonly IClickable _completionClickable;

        public ClickStep(ITutorialStepArgs args, IClickable completionClickable)
            : base(args)
        {
            Assert.IsNotNull(completionClickable);
            _completionClickable = completionClickable;
        }

        public override void Start(Action completionCallback)
        {
            base.Start(completionCallback);
            _completionClickable.Clicked += _completionClickable_Clicked;
        }

        private void _completionClickable_Clicked(object sender, EventArgs e)
        {
			_completionClickable.Clicked -= _completionClickable_Clicked;
            OnCompleted();
        }
	}
}
