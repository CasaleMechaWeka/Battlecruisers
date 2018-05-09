using System;
using System.Collections.Generic;
using BattleCruisers.Tutorial.Providers;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.ClickSteps
{
    /// <summary>
    /// Completed when the user clicks on a specific element.
    /// </summary>
    public class ClickStep : TutorialStep
    {
        private readonly IListProvider<IClickable> _clickablesProvider;
        private IList<IClickable> _completionClickables;

        public ClickStep(ITutorialStepArgs args, IListProvider<IClickable> clickablesProvider)
            : base(args)
        {
            Assert.IsNotNull(clickablesProvider);
            _clickablesProvider = clickablesProvider;
        }

        public override void Start(Action completionCallback)
        {
            base.Start(completionCallback);

            _completionClickables = _clickablesProvider.FindItems();

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
