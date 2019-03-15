using BattleCruisers.UI.Cameras;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;
using System.Collections.Generic;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class NavigationWheelStepsFactory : TutorialFactoryBase, ITutorialStepsFactory
    {
        private readonly IFeaturePermitterStepFactory _featurePermitterStepFactory;
        private readonly BroadcastingFilter _navigationPermitter;
        private readonly IExplanationDismissableStepFactory _explanationDismissableStepFactory;
        private readonly ICameraComponents _cameraComponents;

        public NavigationWheelStepsFactory(
            ITutorialStepArgsFactory argsFactory,
            IFeaturePermitterStepFactory featurePermitterStepFactory,
            BroadcastingFilter navigationPermitter,
            IExplanationDismissableStepFactory explanationDismissableStepFactory,
            ICameraComponents cameraComponents) 
            : base(argsFactory)
        {
            Helper.AssertIsNotNull(featurePermitterStepFactory, navigationPermitter, explanationDismissableStepFactory, cameraComponents);

            _featurePermitterStepFactory = featurePermitterStepFactory;
            _navigationPermitter = navigationPermitter;
            _explanationDismissableStepFactory = explanationDismissableStepFactory;
            _cameraComponents = cameraComponents;
        }

        public IList<ITutorialStep> CreateSteps()
        {
            IList<ITutorialStep> steps = new List<ITutorialStep>();

            // Enable navigation wheel
            steps.Add(_featurePermitterStepFactory.CreateStep(_navigationPermitter, enableFeature: true));

            // Explain navigation wheel
            steps.Add(
                _explanationDismissableStepFactory.CreateStep(
                    _argsFactory.CreateTutorialStepArgs(
                        "This is the map control, which you use to look around the map.",
                        _cameraComponents.NavigationWheel)));

            // Encourage user to experiment
            steps.Add(
                _explanationDismissableStepFactory.CreateStepWithSecondaryButton(
                    _argsFactory.CreateTutorialStepArgs("Drag the map control to look around.  (Click \"Done\" when you have had enough.)")));

            // Disable navigation
            steps.Add(_featurePermitterStepFactory.CreateStep(_navigationPermitter, enableFeature: false));

            return steps;
        }
    }
}