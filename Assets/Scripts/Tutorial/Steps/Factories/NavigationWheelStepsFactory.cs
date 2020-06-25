using BattleCruisers.UI.Cameras;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;
using System.Collections.Generic;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    // FELIX  Convert to NavigationButtonsStepFactory :)
    public class NavigationWheelStepsFactory : TutorialFactoryBase, ITutorialStepsFactory
    {
        private readonly IFeaturePermitterStepFactory _featurePermitterStepFactory;
        private readonly IPermitter _navigationButtonsPermitter;
        private readonly IExplanationDismissableStepFactory _explanationDismissableStepFactory;
        private readonly ICameraComponents _cameraComponents;

        public NavigationWheelStepsFactory(
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

            // Enable navigation wheel
            steps.Add(_featurePermitterStepFactory.CreateStep(_navigationButtonsPermitter, enableFeature: true));

            // Explain navigation wheel
            //steps.Add(
            //    _explanationDismissableStepFactory.CreateStep(
            //        _argsFactory.CreateTutorialStepArgs(
            //            "This is the map control, which you use to look around the map.",
            //            _cameraComponents.NavigationWheel)));

            // Encourage user to experiment
            steps.Add(
                _explanationDismissableStepFactory.CreateStepWithSecondaryButton(
                    _argsFactory.CreateTutorialStepArgs("Drag the map control to look around.  (Click \"DONE\" when you have had enough.)")));

            // Disable navigation
            steps.Add(_featurePermitterStepFactory.CreateStep(_navigationButtonsPermitter, enableFeature: false));

            return steps;
        }
    }
}