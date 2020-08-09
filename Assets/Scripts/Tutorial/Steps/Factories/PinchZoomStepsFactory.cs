using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;
using System.Collections.Generic;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class PinchZoomStepsFactory : TutorialFactoryBase, ITutorialStepsFactory
    {
        private readonly IFeaturePermitterStepFactory _featurePermitterStepFactory;
        private readonly IPermitter _pinchZoomPermitter;
        private readonly IExplanationDismissableStepFactory _explanationDismissableStepFactory;

        public PinchZoomStepsFactory(
            ITutorialStepArgsFactory argsFactory,
            IFeaturePermitterStepFactory featurePermitterStepFactory,
            IPermitter pinchZoomPermitter,
            IExplanationDismissableStepFactory explanationDismissableStepFactory) 
            : base(argsFactory)
        {
            Helper.AssertIsNotNull(featurePermitterStepFactory, pinchZoomPermitter, explanationDismissableStepFactory);

            _featurePermitterStepFactory = featurePermitterStepFactory;
            _pinchZoomPermitter = pinchZoomPermitter;
            _explanationDismissableStepFactory = explanationDismissableStepFactory;
        }

        public IList<ITutorialStep> CreateSteps()
        {
            IList<ITutorialStep> steps = new List<ITutorialStep>();

            // Enable pinch zoom
            steps.Add(_featurePermitterStepFactory.CreateStep(_pinchZoomPermitter, enableFeature: true));

            // Explain pinch zoom, encourage user to experiment
            steps.Add(
                _explanationDismissableStepFactory.CreateStepWithSecondaryButton(
                    _argsFactory.CreateTutorialStepArgs("You can also pinch zoom.  Give it a try :)")));

            // Disable pinch zoom
            steps.Add(_featurePermitterStepFactory.CreateStep(_pinchZoomPermitter, enableFeature: false));

            return steps;
        }
    }
}