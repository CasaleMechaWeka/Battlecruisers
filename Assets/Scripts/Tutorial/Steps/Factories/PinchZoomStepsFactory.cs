using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;
using System.Collections.Generic;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class PinchZoomStepsFactory : TutorialFactoryBase, ITutorialStepsFactory
    {
        private readonly IFeaturePermitterStepFactory _featurePermitterStepFactory;
        private readonly IPermitter _pinchZoomPermitter, _swipePermitter;
        private readonly IExplanationDismissableStepFactory _explanationDismissableStepFactory;

        public PinchZoomStepsFactory(
            ITutorialStepArgsFactory argsFactory,
            IFeaturePermitterStepFactory featurePermitterStepFactory,
            IPermitter pinchZoomPermitter,
            IPermitter swipePermitter,
            IExplanationDismissableStepFactory explanationDismissableStepFactory) 
            : base(argsFactory)
        {
            Helper.AssertIsNotNull(featurePermitterStepFactory, pinchZoomPermitter, swipePermitter, explanationDismissableStepFactory);

            _featurePermitterStepFactory = featurePermitterStepFactory;
            _pinchZoomPermitter = pinchZoomPermitter;
            _swipePermitter = swipePermitter;
            _explanationDismissableStepFactory = explanationDismissableStepFactory;
        }

        public IList<ITutorialStep> CreateSteps()
        {
            IList<ITutorialStep> steps = new List<ITutorialStep>();

            // Enable pinch zoom & swiping
            steps.Add(_featurePermitterStepFactory.CreateStep(_pinchZoomPermitter, enableFeature: true));
            steps.Add(_featurePermitterStepFactory.CreateStep(_swipePermitter, enableFeature: true));

            // Explain pinch zoom, encourage user to experiment
            steps.Add(
                _explanationDismissableStepFactory.CreateStepWithSecondaryButton(
                    _argsFactory.CreateTutorialStepArgs("You can also pinch zoom.")));

            // Disable pinch zoom & swiping
            steps.Add(_featurePermitterStepFactory.CreateStep(_pinchZoomPermitter, enableFeature: false));
            steps.Add(_featurePermitterStepFactory.CreateStep(_swipePermitter, enableFeature: false));

            return steps;
        }
    }
}