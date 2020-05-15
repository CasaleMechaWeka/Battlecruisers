using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Utils;
using System.Collections.Generic;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class PlayerCruiserWidgetsStepsFactory : TutorialFactoryBase, ITutorialStepsFactory
    {
        private readonly IHighlightable _playerCruiserHealthBar, _numOfDrones;
        private readonly IAutoNavigationStepFactory _autNavigationStepFactory;
        private readonly IExplanationDismissableStepFactory _explanationDismissableStepFactory;

        public PlayerCruiserWidgetsStepsFactory(
            ITutorialStepArgsFactory argsFactory,
            IHighlightable playerCruiserHealthBar,
            IHighlightable numOfDrones,
            IAutoNavigationStepFactory autoNavigationStepFactory,
            IExplanationDismissableStepFactory explanationDismissableStepFactory) 
            : base(argsFactory)
        {
            Helper.AssertIsNotNull(playerCruiserHealthBar, numOfDrones, autoNavigationStepFactory, explanationDismissableStepFactory);

            _playerCruiserHealthBar = playerCruiserHealthBar;
            _numOfDrones = numOfDrones;
            _autNavigationStepFactory = autoNavigationStepFactory;
            _explanationDismissableStepFactory = explanationDismissableStepFactory;
        }

        public IList<ITutorialStep> CreateSteps()
        {
            List<ITutorialStep> steps = new List<ITutorialStep>();

            steps.AddRange(_autNavigationStepFactory.CreateSteps(CameraFocuserTarget.PlayerCruiser));

            // Health dial
            ITutorialStepArgs healthDialArgs
                = _argsFactory.CreateTutorialStepArgs(
                    "This is your cruiser's health.",
                    _playerCruiserHealthBar);

            steps.Add(_explanationDismissableStepFactory.CreateStep(healthDialArgs));

            // Drone number
            ITutorialStepArgs droneNumberArgs
                = _argsFactory.CreateTutorialStepArgs(
                    "Builders are the only resource.  This is how many builders you have.  The more builders you have the faster your cruiser works and the better buildings and units you can build.",
                    _numOfDrones);

            steps.Add(_explanationDismissableStepFactory.CreateStep(droneNumberArgs));

            return steps;
        }
    }
}