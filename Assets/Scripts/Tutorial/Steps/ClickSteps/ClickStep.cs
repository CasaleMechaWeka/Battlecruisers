using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.ClickSteps
{
    /// <summary>
    /// Completed when the user clicks on a specific element.
    /// </summary>
    // FELIX  Test :D
    public class ClickStep : TutorialStep
    {
        private readonly IClickable[] _completionClickables;

        public ClickStep(ITutorialStepArgs args, params IClickable[] completionClickables)
            : base(args)
        {
            Assert.IsNotNull(completionClickables);
            Assert.IsTrue(completionClickables.Length > 0);

            _completionClickables = completionClickables;
        }

        public override void Start(Action completionCallback)
        {
            base.Start(completionCallback);

            foreach (IClickable completionClickable in _completionClickables)
            {
                completionClickable.Clicked += completionClickable_Clicked;
			}
        }

        private void completionClickable_Clicked(object sender, EventArgs e)
        {
            foreach (IClickable completionClickable in _completionClickables)
            {
                completionClickable.Clicked -= completionClickable_Clicked;
            }

            OnCompleted();
        }
	}
}
