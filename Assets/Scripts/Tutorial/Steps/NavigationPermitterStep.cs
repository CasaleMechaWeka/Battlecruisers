using System;
using BattleCruisers.UI;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.EnemyCruiser
{
    // FELIX  Test
    public class NavigationPermitterStep : TutorialStep
    {
        private readonly BasicFilter _navigationPermitter;
        private readonly bool _isNavigationEnabled;

        public NavigationPermitterStep(
            ITutorialStepArgs args, 
            BasicFilter navigationPermitter,
            bool isNavigationEnabled)
            : base(args)
        {
            Assert.IsNotNull(navigationPermitter);

            _navigationPermitter = navigationPermitter;
            _isNavigationEnabled = isNavigationEnabled;
        }

        public override void Start(Action completionCallback)
        {
            base.Start(completionCallback);

            _navigationPermitter.IsMatch = _isNavigationEnabled;

            OnCompleted();
        }
    }
}
