using System.Collections.Generic;
using BattleCruisers.Cruisers;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Tutorial.Steps;
using BattleCruisers.UI.BattleScene.GameSpeed;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.Utils;

namespace BattleCruisers.Tutorial
{
    public class TutorialStepsFactory : ITutorialStepsFactory
    {
        private readonly IHighlighter _highlighter;
        private readonly ITextDisplayer _displayer;
        private readonly ITutorialArgs _tutorialArgs;
        private readonly ICruiser _playerCruiser, _aiCruiser;
        private readonly INavigationButtonsWrapper _navigationButtonsWrapper;
        private readonly IGameSpeedWrapper _gameSpeedWrapper;

        public TutorialStepsFactory(IHighlighter highlighter, ITextDisplayer displayer, ITutorialArgs tutorialArgs)
        {
            Helper.AssertIsNotNull(highlighter, displayer, tutorialArgs);

            _highlighter = highlighter;
            _displayer = displayer;
            _tutorialArgs = tutorialArgs;
            _playerCruiser = tutorialArgs.PlayerCruiser;
            _aiCruiser = tutorialArgs.AICruiser;
            _navigationButtonsWrapper = tutorialArgs.NavigationButtonsWrapper;
            _gameSpeedWrapper = tutorialArgs.GameSpeedWrapper;
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

            // 2. Navigation buttons
            steps.Enqueue(CreateStep_NavigateToPlayerCruiser());

            // TEMP  4. Add step for [Navigating via mouse / touch(eventually: P)]

            // 5. Speed controls
            steps.Enqueue(CreateSteps_SpeedControls());

            // 6. Drones
            steps.Enqueue(CreateStep_Drones());

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

        private ITutorialStep CreateStep_NavigateToPlayerCruiser()
        {
            ITutorialStepArgs navigateToPlayerCruiserArgs
                = new TutorialStepArgs(
                    _highlighter,
                    "Navigate back to your cruiser",
                    _displayer,
                    _navigationButtonsWrapper.PlayerCruiserButton);
            return new ClickStep(navigateToPlayerCruiserArgs, _navigationButtonsWrapper.PlayerCruiserButton);
        }
    
        private IList<ITutorialStep> CreateSteps_SpeedControls()
        {
            IList<ITutorialStep> speedControlSteps = new List<ITutorialStep>();

            // Deviate from normal speed
            ITutorialStepArgs deviateFromNormalSpeedArgs
                = new TutorialStepArgs(
                    _highlighter,
                    "These control the game speed.  Change the game speed!",
                    _displayer,
                    _gameSpeedWrapper.SlowMotionButton,
                    _gameSpeedWrapper.PlayButton,
                    _gameSpeedWrapper.FastForwardButton,
                    _gameSpeedWrapper.DoubleFastForwardButton);

            ITutorialStep deviateFromNormalSpeedStep
                = new ClickStep(
                    deviateFromNormalSpeedArgs, 
                    _gameSpeedWrapper.SlowMotionButton, 
                    _gameSpeedWrapper.FastForwardButton, 
                    _gameSpeedWrapper.DoubleFastForwardButton);

            speedControlSteps.Add(deviateFromNormalSpeedStep);

            // Return to normal speed
            ITutorialStepArgs returnToNormalSpeedArgs
                = new TutorialStepArgs(
                    _highlighter,
                    "Nice!  Now change it back :D",
                    _displayer,
                    _gameSpeedWrapper.PlayButton);
            speedControlSteps.Add(new ClickStep(returnToNormalSpeedArgs, _gameSpeedWrapper.PlayButton));

            return speedControlSteps;
        }

        private ITutorialStep CreateStep_Drones()
        {
            ITutorialStepArgs dronesArgs
                = new TutorialStepArgs(
                    _highlighter,
                    "Drones are the only resource.  This is how many drones you have.  The " +
                    "more drones you have the faster your cruiser works and the better " +
                    "buildings / units you can build.",
                    _displayer,
                    _tutorialArgs.PlayerCruiserInfo.NumOfDronesButton);
            return new ClickStep(dronesArgs, _tutorialArgs.PlayerCruiserInfo.NumOfDronesButton);
        }
    }
}
