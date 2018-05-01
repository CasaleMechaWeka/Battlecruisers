using System.Collections.Generic;
using BattleCruisers.Cruisers;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Tutorial.Steps;
using BattleCruisers.Utils;

namespace BattleCruisers.Tutorial
{
    public class TutorialStepsFactory : ITutorialStepsFactory
    {
        private readonly IHighlighter _highlighter;
        private readonly ITextDisplayer _displayer;
        private readonly ICruiser _playerCruiser;

        public TutorialStepsFactory(
            IHighlighter highlighter,
            ITextDisplayer displayer,
            ICruiser playerCruiser)
        {
            Helper.AssertIsNotNull(highlighter, displayer, playerCruiser);

            _highlighter = highlighter;
            _displayer = displayer;
            _playerCruiser = playerCruiser;
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
