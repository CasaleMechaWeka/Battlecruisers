using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Localisation;
using System.Collections.Generic;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class PlayerCruiserWidgetsStepsFactory : TutorialFactoryBase, ITutorialStepsFactory
    {
        private readonly IHighlightable _playerCruiserHealthBar, _numOfDrones;
        private readonly IAutoNavigationStepFactory _autNavigationStepFactory;
        private readonly IExplanationDismissableStepFactory _explanationDismissableStepFactory;

        private const string PLAYER_NAME = "Charlie";

        public PlayerCruiserWidgetsStepsFactory(
            ITutorialStepArgsFactory argsFactory,
            ILocTable tutorialStrings,
            IHighlightable playerCruiserHealthBar,
            IHighlightable numOfDrones,
            IAutoNavigationStepFactory autoNavigationStepFactory,
            IExplanationDismissableStepFactory explanationDismissableStepFactory) 
            : base(argsFactory, tutorialStrings)
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
            string healthBase = _tutorialStrings.GetString("Steps/PlayerCruiserWidgets/PlayerHealthBar");
            ITutorialStepArgs healthDialArgs
                = _argsFactory.CreateTutorialStepArgs(
                    string.Format(healthBase, PLAYER_NAME),
                    _playerCruiserHealthBar);
            steps.Add(_explanationDismissableStepFactory.CreateStep(healthDialArgs));

            // Drone number
            ITutorialStepArgs droneNumberArgs
                = _argsFactory.CreateTutorialStepArgs(
                    _tutorialStrings.GetString("Steps/PlayerCruiserWidgets/Builders"),
                    _numOfDrones);
            steps.Add(_explanationDismissableStepFactory.CreateStep(droneNumberArgs));

            // More drones is better
            ITutorialStepArgs moreDronesArgs
                = _argsFactory.CreateTutorialStepArgs(
                    _tutorialStrings.GetString("Steps/PlayerCruiserWidgets/MoreBuilders"),
                    _numOfDrones);
            steps.Add(_explanationDismissableStepFactory.CreateStep(moreDronesArgs));

            return steps;
        }
    }
}