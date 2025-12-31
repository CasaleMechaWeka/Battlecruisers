using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Localisation;
using System.Collections.Generic;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class PlayerCruiserWidgetsStepsFactory : TutorialFactoryBase, ITutorialStepsFactory
    {
        private readonly IHighlightable _playerCruiserHealthBar, _numOfDrones;
        private readonly AutoNavigationStepFactory _autNavigationStepFactory;
        private readonly ExplanationDismissableStepFactory _explanationDismissableStepFactory;

        public PlayerCruiserWidgetsStepsFactory(
            TutorialStepArgsFactory argsFactory,
            IHighlightable playerCruiserHealthBar,
            IHighlightable numOfDrones,
            AutoNavigationStepFactory autoNavigationStepFactory,
            ExplanationDismissableStepFactory explanationDismissableStepFactory)

            : base(argsFactory)
        {
            Helper.AssertIsNotNull(playerCruiserHealthBar, numOfDrones, autoNavigationStepFactory, explanationDismissableStepFactory);

            _playerCruiserHealthBar = playerCruiserHealthBar;
            _numOfDrones = numOfDrones;
            _autNavigationStepFactory = autoNavigationStepFactory;
            _explanationDismissableStepFactory = explanationDismissableStepFactory;
        }

        public IList<TutorialStep> CreateSteps()
        {
            List<TutorialStep> steps = new List<TutorialStep>();

            steps.AddRange(_autNavigationStepFactory.CreateSteps(CameraFocuserTarget.PlayerCruiser));

            // Health dial
            string healthBase = LocTableCache.TutorialTable.GetString("Steps/PlayerCruiserWidgets/PlayerHealthBar");
            string protagonistName = LocTableCache.CommonTable.GetString("Names/Protagonist");
            TutorialStepArgs healthDialArgs
                = _argsFactory.CreateTutorialStepArgs(
                    string.Format(healthBase, protagonistName),
                    _playerCruiserHealthBar);
            steps.Add(_explanationDismissableStepFactory.CreateStep(healthDialArgs));

            // Drone number
            TutorialStepArgs droneNumberArgs
                = _argsFactory.CreateTutorialStepArgs(
                    LocTableCache.TutorialTable.GetString("Steps/PlayerCruiserWidgets/Builders"),
                    _numOfDrones);
            steps.Add(_explanationDismissableStepFactory.CreateStep(droneNumberArgs));

            // More drones is better
            TutorialStepArgs moreDronesArgs
                = _argsFactory.CreateTutorialStepArgs(
                    LocTableCache.TutorialTable.GetString("Steps/PlayerCruiserWidgets/MoreBuilders"),
                    _numOfDrones);
            steps.Add(_explanationDismissableStepFactory.CreateStep(moreDronesArgs));

            return steps;
        }
    }
}