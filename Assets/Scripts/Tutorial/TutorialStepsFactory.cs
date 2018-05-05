using System.Collections.Generic;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Tutorial.Steps;
using BattleCruisers.Tutorial.Steps.ClickSteps;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial
{
    public class TutorialStepsFactory : ITutorialStepsFactory
    {
        private readonly IHighlighter _highlighter;
        private readonly ITextDisplayer _displayer;
        private readonly ITutorialArgs _tutorialArgs;

        public TutorialStepsFactory(IHighlighter highlighter, ITextDisplayer displayer, ITutorialArgs tutorialArgs)
        {
            Helper.AssertIsNotNull(highlighter, displayer, tutorialArgs);

            _highlighter = highlighter;
            _displayer = displayer;
            _tutorialArgs = tutorialArgs;
        }

        public Queue<ITutorialStep> CreateTutorialSteps()
        {
            Queue<ITutorialStep> steps = new Queue<ITutorialStep>();

            // TEMP  For end game enable all tutorial steps :)
            //// 1. Your cruiser
            //steps.Enqueue(CreateStep_YourCruiser());

            //// 2. Navigation buttons
            //steps.Enqueue(CreateStep_NavigationButtons());

            //// 3. Enemy cruiser
            //steps.Enqueue(CreateStep_EnemyCruiser());

            //// 2. Navigation buttons
            //steps.Enqueue(CreateStep_NavigateToPlayerCruiser());

            //// TEMP  4. Add step for [Navigating via mouse / touch(eventually: P)]

            //// 5. Speed controls
            //steps.Enqueue(CreateSteps_SpeedControls());

            // 6. Drones
            steps.Enqueue(CreateStep_Drones());

            // 7. Building a building
            steps.Enqueue(CreateSteps_BuildDroneStation());

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
                    _tutorialArgs.PlayerCruiser);
            return new ClickStep(yourCruiserArgs, _tutorialArgs.PlayerCruiser);
        }

        private ITutorialStep CreateStep_NavigationButtons()
		{
			ITutorialStepArgs navigationButtonArgs
			    = new TutorialStepArgs(
    				_highlighter,
    				"These are your navigation buttons.  They help you move around the map.  Play around a bit with these.",
    				_displayer,
                    _tutorialArgs.NavigationButtonsWrapper.PlayerCruiserButton,
                    _tutorialArgs.NavigationButtonsWrapper.MidLeftButton,
                    _tutorialArgs.NavigationButtonsWrapper.OverviewButton,
                    _tutorialArgs.NavigationButtonsWrapper.MidRightButton,
                    _tutorialArgs.NavigationButtonsWrapper.AICruiserButton);
            
            return 
                new NavigationStep(
                    navigationButtonArgs, 
                    _tutorialArgs.PermitterProvider.NavigationPermitter, 
                    _tutorialArgs.NavigationButtonsWrapper.AICruiserButton);
		}

        private ITutorialStep CreateStep_EnemyCruiser()
        {
            ITutorialStepArgs yourCruiserArgs
                = new TutorialStepArgs(
                    _highlighter,
                    "This is the enemy cruiser.  You win if you destroy their cruiser before it destroys you.",
                    _displayer,
                    _tutorialArgs.AICruiser);
            return new ClickStep(yourCruiserArgs, _tutorialArgs.AICruiser);
        }

        private ITutorialStep CreateStep_NavigateToPlayerCruiser()
        {
            ITutorialStepArgs navigateToPlayerCruiserArgs
                = new TutorialStepArgs(
                    _highlighter,
                    "Navigate back to your cruiser",
                    _displayer,
                    _tutorialArgs.NavigationButtonsWrapper.PlayerCruiserButton);
            return 
                new NavigationStep(
                    navigateToPlayerCruiserArgs, 
                    _tutorialArgs.PermitterProvider.NavigationPermitter, 
                    _tutorialArgs.NavigationButtonsWrapper.PlayerCruiserButton);
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
                    _tutorialArgs.GameSpeedWrapper.SlowMotionButton,
                    _tutorialArgs.GameSpeedWrapper.PlayButton,
                    _tutorialArgs.GameSpeedWrapper.FastForwardButton,
                    _tutorialArgs.GameSpeedWrapper.DoubleFastForwardButton);

            ITutorialStep deviateFromNormalSpeedStep
                = new ClickStep(
                    deviateFromNormalSpeedArgs, 
                    _tutorialArgs.GameSpeedWrapper.SlowMotionButton, 
                    _tutorialArgs.GameSpeedWrapper.FastForwardButton, 
                    _tutorialArgs.GameSpeedWrapper.DoubleFastForwardButton);

            speedControlSteps.Add(deviateFromNormalSpeedStep);

            // Return to normal speed
            ITutorialStepArgs returnToNormalSpeedArgs
                = new TutorialStepArgs(
                    _highlighter,
                    "Nice!  Now change it back :D",
                    _displayer,
                    _tutorialArgs.GameSpeedWrapper.PlayButton);
            speedControlSteps.Add(new ClickStep(returnToNormalSpeedArgs, _tutorialArgs.GameSpeedWrapper.PlayButton));

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

        private IList<ITutorialStep> CreateSteps_BuildDroneStation()
        {
            IList<ITutorialStep> buildDroneStationSteps = new List<ITutorialStep>();

            // Select factories building category
            IBuildingCategoryButton factoriesCategoryButton = _tutorialArgs.CategoryButtonsPanel.GetCategoryButton(BuildingCategory.Factory);
            Assert.IsNotNull(factoriesCategoryButton);

            ITutorialStepArgs factoriesCategoryArgs
                = new TutorialStepArgs(
                    _highlighter,
                    "To get more drones build a drone station.",
                    _displayer,
                factoriesCategoryButton);

            buildDroneStationSteps.Add(new CategoryButtonStep(factoriesCategoryArgs, factoriesCategoryButton, _tutorialArgs.PermitterProvider.BuildingCategoryPermitter));

            return buildDroneStationSteps;
        }
    }
}
