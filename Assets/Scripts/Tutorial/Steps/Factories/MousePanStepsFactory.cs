using BattleCruisers.UI.Filters;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class MousePanStepsFactory : SwipeStepsFactoryBase
    {
        private readonly IPermitter _scrollWheelPermitter;

        public MousePanStepsFactory(
            ITutorialStepArgsFactory argsFactory,
            IFeaturePermitterStepFactory featurePermitterStepFactory,
            IPermitter swipePermitter,
            IPermitter scrollWheelPermitter,
            IExplanationDismissableStepFactory explanationDismissableStepFactory) 
            : base(
                  argsFactory,
                  featurePermitterStepFactory,
                  swipePermitter,
                  explanationDismissableStepFactory,
                  "You can click drag to pan.  Give it a try :)")
        {
            Assert.IsNotNull(scrollWheelPermitter);
            _scrollWheelPermitter = scrollWheelPermitter;
        }

        protected override void EnableNavigation(IList<ITutorialStep> steps)
        {
            base.EnableNavigation(steps);
            steps.Add(_featurePermitterStepFactory.CreateStep(_scrollWheelPermitter, enableFeature: true));
        }

        protected override void DisableNavigation(IList<ITutorialStep> steps)
        {
            base.EnableNavigation(steps);
            steps.Add(_featurePermitterStepFactory.CreateStep(_scrollWheelPermitter, enableFeature: false));
        }
    }
}