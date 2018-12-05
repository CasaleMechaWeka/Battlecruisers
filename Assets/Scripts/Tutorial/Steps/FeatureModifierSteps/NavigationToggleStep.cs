using BattleCruisers.UI.Filters;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.FeatureModifierSteps
{
    // FELIX  Test :D
    public class NavigationToggleStep : TutorialStepNEW
    {
        private readonly BasicFilter _isNavigationEnabledFilter;

        public NavigationToggleStep(ITutorialStepArgsNEW args, BasicFilter isNavigationEnabledFilter)
            : base(args)
        {
            Assert.IsNotNull(isNavigationEnabledFilter);
            _isNavigationEnabledFilter = isNavigationEnabledFilter;
        }

        public override void Start(Action completionCallback)
        {
            base.Start(completionCallback);
            _isNavigationEnabledFilter.IsMatch = true;
        }

        protected override void OnCompleted()
        {
            base.OnCompleted();
            _isNavigationEnabledFilter.IsMatch = false;
        }
    }
}