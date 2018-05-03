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
        private readonly ICruiser _playerCruiser, _aiCruiser;
        private readonly INavigationButtonsWrapper _navigationButtonsWrapper;

        public TutorialStepsFactory(
            IHighlighter highlighter,
            ITextDisplayer displayer,
            ICruiser playerCruiser,
            ICruiser aiCruiser,
            INavigationButtonsWrapper navigationButtonsWrapper)
        {
            Helper.AssertIsNotNull(highlighter, displayer, playerCruiser, aiCruiser, navigationButtonsWrapper);

            _highlighter = highlighter;
            _displayer = displayer;
            _playerCruiser = playerCruiser;
            _aiCruiser = aiCruiser;
            _navigationButtonsWrapper = navigationButtonsWrapper;
        }

        public Queue<ITutorialStep> CreateTutorialSteps()
        {
            Queue<ITutorialStep> steps = new Queue<ITutorialStep>();

            // 1. Your cruiser
            steps.Enqueue(CreateStep_YourCruiser());

            // 2. Navigation buttons
            steps.Enqueue(CreateStep_NavigationButtons());

            // 3. Enemy cruiser
            steps.Enqueue(CreateStep_EnemyCruiser());

            // TEMP  4. Add step for [Navigating via mouse / touch(eventually: P)]
            // 5. Speed controls
            // 6. Drones
            // 7. Building a building
            // 8. Enemy ship
            // 9. Enemy bomber
            // 10. Drone Focus

            return steps;
        }

        private ITutorialStep CreateStep_YourCruiser()
        {
            ITutorialStepArgs yourCruiserArgs
                = new TutorialStepArgs(
                    _highlighter,
                    "This is your cruiser",
                    _displayer,
                    _playerCruiser);
            return new ClickStep(yourCruiserArgs, _playerCruiser);
        }

        private ITutorialStep CreateStep_NavigationButtons()
		{
			ITutorialStepArgs navigationButtonArgs
			= new TutorialStepArgs(
				_highlighter,
				"These are your navigation buttons.  They help you move around the map.  Play around a bit with these.",
				_displayer,
				_navigationButtonsWrapper.PlayerCruiserButton,
				_navigationButtonsWrapper.MidLeftButton,
				_navigationButtonsWrapper.OverviewButton,
				_navigationButtonsWrapper.MidRightButton,
				_navigationButtonsWrapper.AICruiserButton);
			return new ClickStep(navigationButtonArgs, _navigationButtonsWrapper.AICruiserButton);
		}

        private ITutorialStep CreateStep_EnemyCruiser()
        {
            ITutorialStepArgs yourCruiserArgs
                = new TutorialStepArgs(
                    _highlighter,
                    "This is the enemy cruiser.  You win if you destroy their cruiser before it destroys you.",
                    _displayer,
                    _aiCruiser);
            return new ClickStep(yourCruiserArgs, _aiCruiser);
        }
    }
}
