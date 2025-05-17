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
        private readonly FeaturePermitterStepFactory _featurePermitterStepFactory;
        private readonly IPermitter _navigationButtonsPermitter, _hotkeysPermitter;
        private readonly ExplanationDismissableStepFactory _explanationDismissableStepFactory;
        private readonly CameraComponents _cameraComponents;

        public NavigationButtonsStepsFactory(
            TutorialStepArgsFactory argsFactory,
            FeaturePermitterStepFactory featurePermitterStepFactory,
            IPermitter navigationButtonsPermitter,
            IPermitter hotkeysPermitter,
            ExplanationDismissableStepFactory explanationDismissableStepFactory,
            CameraComponents cameraComponents)
            : base(argsFactory)
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

            // Explain navigation buttons
            steps.Add(
                _explanationDismissableStepFactory.CreateStep(
                    _argsFactory.CreateTutorialStepArgs(
                        LocTableCache.TutorialTable.GetString("Steps/NavigationButtons/Buttons"),
                        _cameraComponents.NavigationButtonsPanel)));

            // Encourage user to experiment
            string text
                = SystemInfoBC.Instance.IsHandheld ?
                    LocTableCache.TutorialTable.GetString("Steps/NavigationButtons/TouchInstructions") :
                    LocTableCache.TutorialTable.GetString("Steps/NavigationButtons/MouseInstructions");
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