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

        public NavigationWheelStepsFactory(
            ITutorialStepArgsFactory argsFactory,
            ITutorialArgs tutorialArgs,
            IFeaturePermitterStepFactory featurePermitterStepFactory,
            BroadcastingFilter navigationPermitter,
            IExplanationDismissableStepFactory explanationDismissableStepFactory) 
            : base(argsFactory, tutorialArgs)
        {
            Helper.AssertIsNotNull(featurePermitterStepFactory, navigationPermitter, explanationDismissableStepFactory);

            _featurePermitterStepFactory = featurePermitterStepFactory;
            _navigationPermitter = navigationPermitter;
            _explanationDismissableStepFactory = explanationDismissableStepFactory;
        }

        public IList<ITutorialStep> CreateTutorialSteps()
        {
            IList<ITutorialStep> steps = new List<ITutorialStep>();

            // Enable navigation wheel
            steps.Add(_featurePermitterStepFactory.CreateTutorialStep(_navigationPermitter, enableFeature: true));

            // Explain navigation wheel
            steps.Add(
                _explanationDismissableStepFactory.CreateTutorialStep(
                    _argsFactory.CreateTutorialStepArgs(
                        "This is the navigation wheel, which you use to navigate around the map.",
                        _tutorialArgs.CameraComponents.NavigationWheel)));

            // Encourage user to experiment
            steps.Add(
                _explanationDismissableStepFactory.CreateTutorialStep(
                    _argsFactory.CreateTutorialStepArgs("Drag the navigation wheel to navigate.  (Click the checkmark when you have had enough.)")));

            // Disable navigation
            steps.Add(_featurePermitterStepFactory.CreateTutorialStep(_navigationPermitter, enableFeature: false));

            return steps;
        }
    }
}