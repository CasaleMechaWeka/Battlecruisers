using System.Collections.Generic;
using BattleCruisers.Cruisers;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Tutorial.Steps;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.Utils;

namespace BattleCruisers.Tutorial
{
    public class TutorialStepsFactory : ITutorialStepsFactory
    {
        private readonly IHighlighter _highlighter;
        private readonly ITextDisplayer _displayer;
        private readonly ICruiser _playerCruiser;
        private readonly INavigationButtonsWrapper _navigationButtonsWrapper;

        public TutorialStepsFactory(
            IHighlighter highlighter,
            ITextDisplayer displayer,
            ICruiser playerCruiser,
            INavigationButtonsWrapper navigationButtonsWrapper)
        {
            Helper.AssertIsNotNull(highlighter, displayer, playerCruiser, navigationButtonsWrapper);

            _highlighter = highlighter;
            _displayer = displayer;
            _playerCruiser = playerCruiser;
            _navigationButtonsWrapper = navigationButtonsWrapper;
        }

        public Queue<ITutorialStep> CreateTutorialSteps()
        {
            Queue<ITutorialStep> steps = new Queue<ITutorialStep>();

            // 1. Your cruiser
            ITutorialStepArgs yourCruiserArgs
                = new TutorialStepArgs(
                    _highlighter,
                    "This is your cruiser",
                    _displayer,
                    _playerCruiser);
            steps.Enqueue(new ClickStep(yourCruiserArgs, _playerCruiser));


            // 2. Navigation buttons
            ITutorialStepArgs navigationButtonArgs
                = new TutorialStepArgs(
                    _highlighter,
    				"These are your navigation buttons.  They help you move around the map.  Play around a bit with these.",
                    _displayer,
                    _navigationButtonsWrapper);
            steps.Enqueue(new ClickStep(navigationButtonArgs, _navigationButtonsWrapper.AICruiserButton));

            // 3. Enemy cruiser
            // TEMP  4. Add step for [Navigating via mouse / touch(eventually: P)]
            // 5. Speed controls
            // 6. Drones
            // 7. Building a building
            // 8. Enemy ship
            // 9. Enemy bomber
            // 10. Drone Focus

            return steps;
        }
    }
}
