using BattleCruisers.UI.Filters;
using BattleCruisers.Utils.Localisation;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class MousePanStepsFactory : SwipeStepsFactoryBase
    {
        // FELIX  Loc, key?
        private const string STEP_EXPLANATION = "Click drag to pan.";
        private readonly IPermitter _scrollWheelPermitter;

        public MousePanStepsFactory(
            ITutorialStepArgsFactory argsFactory,
            ILocTable tutorialStrings,
            IFeaturePermitterStepFactory featurePermitterStepFactory,
            IPermitter swipePermitter,
            IPermitter scrollWheelPermitter,
            IExplanationDismissableStepFactory explanationDismissableStepFactory) 
            : base(
                  argsFactory,
                  tutorialStrings,
                  featurePermitterStepFactory,
                  swipePermitter,
                  explanationDismissableStepFactory,
                  STEP_EXPLANATION)
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
            base.DisableNavigation(steps);
            steps.Add(_featurePermitterStepFactory.CreateStep(_scrollWheelPermitter, enableFeature: false));
        }
    }
}