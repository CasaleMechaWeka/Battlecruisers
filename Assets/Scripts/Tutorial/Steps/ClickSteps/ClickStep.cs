using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.ClickSteps
{
    /// <summary>
    /// Completed when the user clicks on a specific element.
    /// </summary>
    public class ClickStep : TutorialStep
    {
        private readonly IClickablesProvider _clickablesProvider;
        private IList<IClickable> _completionClickables;

        public ClickStep(ITutorialStepArgs args, IClickablesProvider clickablesProvider)
            : base(args)
        {
            Assert.IsNotNull(clickablesProvider);
            _clickablesProvider = clickablesProvider;
        }

        public override void Start(Action completionCallback)
        {
            base.Start(completionCallback);

            _completionClickables = _clickablesProvider.FindClickables();

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
