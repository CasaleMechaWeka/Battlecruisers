using BattleCruisers.UI.Filters;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.FeatureModifierSteps
{
    // FELIX  Test :D
    public class NavigationToggleStep : TutorialStepNEW
    {
        private readonly BasicFilter _isNavigationEnabledFilter;
        private readonly bool _enableNavigation;

        public NavigationToggleStep(ITutorialStepArgsNEW args, BasicFilter isNavigationEnabledFilter, bool enableNavigation)
            : base(args)
        {
            Assert.IsNotNull(isNavigationEnabledFilter);

            _isNavigationEnabledFilter = isNavigationEnabledFilter;
            _enableNavigation = enableNavigation;
        }

        public override void Start(Action completionCallback)
        {
            base.Start(completionCallback);
            _isNavigationEnabledFilter.IsMatch = _enableNavigation;
        }
    }
}