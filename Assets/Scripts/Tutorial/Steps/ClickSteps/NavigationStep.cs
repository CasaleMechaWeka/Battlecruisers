using System;
using BattleCruisers.UI;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.ClickSteps
{
    // FELIX  Test :)
    public class NavigationStep : ClickStep
    {
        private readonly BasicDecider _navigationDecider;

        public NavigationStep(ITutorialStepArgs args, BasicDecider navigationDecider, params IClickable[] completionClickables)
            : base(args, new StaticClickablesProvider(completionClickables))
        {
            Assert.IsNotNull(navigationDecider);
            _navigationDecider = navigationDecider;
        }

        public override void Start(Action completionCallback)
        {
            base.Start(completionCallback);
            _navigationDecider.ShouldBeEnabled = true;
        }

        protected override void OnCompleted()
        {
            _navigationDecider.ShouldBeEnabled = false;
            base.OnCompleted();
        }
    }
}
