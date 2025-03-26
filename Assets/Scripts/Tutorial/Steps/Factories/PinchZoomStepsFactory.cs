using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Localisation;
using System.Collections.Generic;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class PinchZoomStepsFactory : TutorialFactoryBase, ITutorialStepsFactory
    {
        private readonly FeaturePermitterStepFactory _featurePermitterStepFactory;
        private readonly IPermitter _pinchZoomPermitter, _swipePermitter;
        private readonly ExplanationDismissableStepFactory _explanationDismissableStepFactory;

        public PinchZoomStepsFactory(
            TutorialStepArgsFactory argsFactory,
            FeaturePermitterStepFactory featurePermitterStepFactory,
            IPermitter pinchZoomPermitter,
            IPermitter swipePermitter,
            ExplanationDismissableStepFactory explanationDismissableStepFactory)
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
                    _argsFactory.CreateTutorialStepArgs(
                        LocTableCache.TutorialTable.GetString("Steps/PinchZoom"))));

            // Disable pinch zoom & swiping
            steps.Add(_featurePermitterStepFactory.CreateStep(_pinchZoomPermitter, enableFeature: false));
            steps.Add(_featurePermitterStepFactory.CreateStep(_swipePermitter, enableFeature: false));

            return steps;
        }
    }
}