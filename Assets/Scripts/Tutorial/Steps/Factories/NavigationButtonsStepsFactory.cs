using BattleCruisers.UI.Cameras;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;
using System.Collections.Generic;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class NavigationButtonsStepsFactory : TutorialFactoryBase, ITutorialStepsFactory
    {
        private readonly IFeaturePermitterStepFactory _featurePermitterStepFactory;
        private readonly IPermitter _navigationButtonsPermitter;
        private readonly IExplanationDismissableStepFactory _explanationDismissableStepFactory;
        private readonly ICameraComponents _cameraComponents;

        public NavigationButtonsStepsFactory(
            ITutorialStepArgsFactory argsFactory,
            IFeaturePermitterStepFactory featurePermitterStepFactory,
            IPermitter navigationButtonsPermitter,
            IExplanationDismissableStepFactory explanationDismissableStepFactory,
            ICameraComponents cameraComponents) 
            : base(argsFactory)
        {
            Helper.AssertIsNotNull(featurePermitterStepFactory, navigationButtonsPermitter, explanationDismissableStepFactory, cameraComponents);

            _featurePermitterStepFactory = featurePermitterStepFactory;
            _navigationButtonsPermitter = navigationButtonsPermitter;
            _explanationDismissableStepFactory = explanationDismissableStepFactory;
            _cameraComponents = cameraComponents;
        }

        public IList<ITutorialStep> CreateSteps()
        {
            IList<ITutorialStep> steps = new List<ITutorialStep>();

            // Enable navigation buttons
            steps.Add(_featurePermitterStepFactory.CreateStep(_navigationButtonsPermitter, enableFeature: true));

            // Explain navigation buttons
            steps.Add(
                _explanationDismissableStepFactory.CreateStep(
                    _argsFactory.CreateTutorialStepArgs(
                        "These are the navigation buttons.",
                        _cameraComponents.NavigationButtonsPanel)));

            // Encourage user to experiment
            steps.Add(
                _explanationDismissableStepFactory.CreateStepWithSecondaryButton(
                    _argsFactory.CreateTutorialStepArgs("Click them to look around the map.  Give it a try :)")));

            // Disable navigation
            steps.Add(_featurePermitterStepFactory.CreateStep(_navigationButtonsPermitter, enableFeature: false));

            return steps;
        }
    }
}