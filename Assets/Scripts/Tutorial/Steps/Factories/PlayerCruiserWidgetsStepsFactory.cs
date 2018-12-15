using BattleCruisers.UI.BattleScene;
using BattleCruisers.Utils;
using System.Collections.Generic;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class PlayerCruiserWidgetsStepsFactory : TutorialFactoryBase, ITutorialStepsFactory
    {
        private readonly LeftPanelComponents _leftPanelComponents;
        private readonly IAutoNavigationStepFactory _autNavigationStepFactory;
        private readonly IExplanationDismissableStepFactory _explanationDismissableStepFactory;

        public PlayerCruiserWidgetsStepsFactory(
            LeftPanelComponents leftPanelComponents,
            ITutorialStepArgsFactory argsFactory,
            ITutorialArgs tutorialArgs,
            IAutoNavigationStepFactory autoNavigationStepFactory,
            IExplanationDismissableStepFactory explanationDismissableStepFactory) 
            : base(argsFactory, tutorialArgs)
        {
            Helper.AssertIsNotNull(leftPanelComponents, autoNavigationStepFactory, explanationDismissableStepFactory);

            _leftPanelComponents = leftPanelComponents;
            _autNavigationStepFactory = autoNavigationStepFactory;
            _explanationDismissableStepFactory = explanationDismissableStepFactory;
        }

        public IList<ITutorialStep> CreateTutorialSteps()
        {
            List<ITutorialStep> steps = new List<ITutorialStep>();

            steps.AddRange(_autNavigationStepFactory.CreateSteps(CameraFocuserTarget.PlayerCruiser));

            // Health dial
            ITutorialStepArgs healthDialArgs
                = _argsFactory.CreateTutorialStepArgs(
                    "This is your cruiser's health dial.",
                    _leftPanelComponents.HealthDialHighlightable);

            steps.Add(_explanationDismissableStepFactory.CreateTutorialStep(healthDialArgs));

            // Drone number
            ITutorialStepArgs droneNumberArgs
                = _argsFactory.CreateTutorialStepArgs(
                    "Builders are the only resource.  This is how many builders you have.  The more builders you have the faster your cruiser works and the better buildings and units you can build.",
                    _leftPanelComponents.NumberOfDronesHighlightable);

            steps.Add(_explanationDismissableStepFactory.CreateTutorialStep(droneNumberArgs));

            return steps;
        }
    }
}