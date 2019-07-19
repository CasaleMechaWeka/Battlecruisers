using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;
using System.Collections.Generic;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class SwipeStepsFactory : TutorialFactoryBase, ITutorialStepsFactory
    {
        private readonly IFeaturePermitterStepFactory _featurePermitterStepFactory;
        private readonly IPermitter _swipePermitter;
        private readonly IExplanationDismissableStepFactory _explanationDismissableStepFactory;

        public SwipeStepsFactory(
            ITutorialStepArgsFactory argsFactory,
            IFeaturePermitterStepFactory featurePermitterStepFactory,
            IPermitter swipePermitter,
            IExplanationDismissableStepFactory explanationDismissableStepFactory) 
            : base(argsFactory)
        {
            Helper.AssertIsNotNull(featurePermitterStepFactory, swipePermitter, explanationDismissableStepFactory);

            _featurePermitterStepFactory = featurePermitterStepFactory;
            _swipePermitter = swipePermitter;
            _explanationDismissableStepFactory = explanationDismissableStepFactory;
        }

        public IList<ITutorialStep> CreateSteps()
        {
            IList<ITutorialStep> steps = new List<ITutorialStep>();

            // Enable swiping
            steps.Add(_featurePermitterStepFactory.CreateStep(_swipePermitter, enableFeature: true));

            // Explain swiping and encourage user to experiment
            steps.Add(
                _explanationDismissableStepFactory.CreateStepWithSecondaryButton(
                    _argsFactory.CreateTutorialStepArgs("You can also swipe your finger to look around.  Swipe horizontally to scroll.  Swipe vertically to zoom.  (Click \"Done\" when you have had enough.)")));

            // Disable swiping
            steps.Add(_featurePermitterStepFactory.CreateStep(_swipePermitter, enableFeature: false));

            return steps;
        }
    }
}