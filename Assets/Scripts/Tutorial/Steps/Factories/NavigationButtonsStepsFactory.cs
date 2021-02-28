using BattleCruisers.UI.Cameras;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Utils.PlatformAbstractions;
using System.Collections.Generic;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class NavigationButtonsStepsFactory : TutorialFactoryBase, ITutorialStepsFactory
    {
        private readonly IFeaturePermitterStepFactory _featurePermitterStepFactory;
        private readonly IPermitter _navigationButtonsPermitter, _hotkeysPermitter;
        private readonly IExplanationDismissableStepFactory _explanationDismissableStepFactory;
        private readonly ICameraComponents _cameraComponents;

        public NavigationButtonsStepsFactory(
            ITutorialStepArgsFactory argsFactory,
            ILocTable tutorialStrings,
            IFeaturePermitterStepFactory featurePermitterStepFactory,
            IPermitter navigationButtonsPermitter,
            IPermitter hotkeysPermitter,
            IExplanationDismissableStepFactory explanationDismissableStepFactory,
            ICameraComponents cameraComponents) 
            : base(argsFactory, tutorialStrings)
        {
            Helper.AssertIsNotNull(featurePermitterStepFactory, navigationButtonsPermitter, hotkeysPermitter, explanationDismissableStepFactory, cameraComponents);

            _featurePermitterStepFactory = featurePermitterStepFactory;
            _navigationButtonsPermitter = navigationButtonsPermitter;
            _hotkeysPermitter = hotkeysPermitter;
            _explanationDismissableStepFactory = explanationDismissableStepFactory;
            _cameraComponents = cameraComponents;
        }

        public IList<ITutorialStep> CreateSteps()
        {
            IList<ITutorialStep> steps = new List<ITutorialStep>();

            // Enable navigation buttons
            steps.Add(_featurePermitterStepFactory.CreateStep(_navigationButtonsPermitter, enableFeature: true));
            steps.Add(_featurePermitterStepFactory.CreateStep(_hotkeysPermitter, enableFeature: true));

            // FELIX  Loc
            // Explain navigation buttons
            steps.Add(
                _explanationDismissableStepFactory.CreateStep(
                    _argsFactory.CreateTutorialStepArgs(
                        _tutorialStrings.GetString("Steps/NavigationButtons/Buttons"),
                        _cameraComponents.NavigationButtonsPanel)));

            // Encourage user to experiment
            string text
                = SystemInfoBC.Instance.IsHandheld ?
                    _tutorialStrings.GetString("Steps/NavigationButtons/TouchInstructions") :
                    _tutorialStrings.GetString("Steps/NavigationButtons/MouseInstructions");
            steps.Add(
                _explanationDismissableStepFactory.CreateStepWithSecondaryButton(
                    _argsFactory.CreateTutorialStepArgs(text)));

            // Disable navigation
            steps.Add(_featurePermitterStepFactory.CreateStep(_navigationButtonsPermitter, enableFeature: false));
            steps.Add(_featurePermitterStepFactory.CreateStep(_hotkeysPermitter, enableFeature: false));

            return steps;
        }
    }
}