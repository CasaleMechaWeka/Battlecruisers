using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;
using System.Collections.Generic;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class SwipeStepsFactory : TutorialFactoryBase, ITutorialStepsFactory
    {
        private readonly IFeaturePermitterStepFactory _featurePermitterStepFactory;
        private readonly IPermitter _swipePermitter, _pinchZoomPermitter;
        private readonly IExplanationDismissableStepFactory _explanationDismissableStepFactory;

        public SwipeStepsFactory(
            ITutorialStepArgsFactory argsFactory,
            IFeaturePermitterStepFactory featurePermitterStepFactory,
            IPermitter swipePermitter,
            IPermitter pinchZoomPermitter,
            IExplanationDismissableStepFactory explanationDismissableStepFactory) 
            : base(argsFactory)
        {
            Helper.AssertIsNotNull(featurePermitterStepFactory, swipePermitter, explanationDismissableStepFactory);

            _featurePermitterStepFactory = featurePermitterStepFactory;
            _swipePermitter = swipePermitter;
            _pinchZoomPermitter = pinchZoomPermitter;
            _explanationDismissableStepFactory = explanationDismissableStepFactory;
        }

        public IList<ITutorialStep> CreateSteps()
        {
            IList<ITutorialStep> steps = new List<ITutorialStep>();

            // Enable swiping and pinch zoom
            steps.Add(_featurePermitterStepFactory.CreateStep(_swipePermitter, enableFeature: true));
            steps.Add(_featurePermitterStepFactory.CreateStep(_pinchZoomPermitter, enableFeature: true));

            // Explain swiping and pinch zoom, encourage user to experiment
            steps.Add(
                _explanationDismissableStepFactory.CreateStepWithSecondaryButton(
                    _argsFactory.CreateTutorialStepArgs("You can also swipe your finger to look around.  Swipe horizontally to scroll.  Swipe vertically to zoom.  Or just pinch zoom.  (Click \"DONE\" when you have had enough.)")));

            // Disable swiping and pinch zoom
            steps.Add(_featurePermitterStepFactory.CreateStep(_swipePermitter, enableFeature: false));
            steps.Add(_featurePermitterStepFactory.CreateStep(_pinchZoomPermitter, enableFeature: false));

            return steps;
        }
    }
}