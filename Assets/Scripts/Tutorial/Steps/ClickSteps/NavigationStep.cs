using System;
using BattleCruisers.Tutorial.Providers;
using BattleCruisers.UI;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.ClickSteps
{
    public class NavigationStep : ClickStep
    {
        private readonly BasicFilter _shouldNavigationBeEnabledFilter;

        public NavigationStep(ITutorialStepArgs args, BasicFilter shouldNavigationBeEnabledFilter, params IClickable[] completionClickables)
            : base(args, new StaticListProvider<IClickable>(completionClickables))
        {
            Assert.IsNotNull(shouldNavigationBeEnabledFilter);
            _shouldNavigationBeEnabledFilter = shouldNavigationBeEnabledFilter;
        }

        public override void Start(Action completionCallback)
        {
            base.Start(completionCallback);
            _shouldNavigationBeEnabledFilter.IsMatch = true;
        }

        protected override void OnCompleted()
        {
            _shouldNavigationBeEnabledFilter.IsMatch = false;
            base.OnCompleted();
        }
    }
}
