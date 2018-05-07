using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Tutorial.Steps;
using BattleCruisers.Tutorial.Steps.ClickSteps;
using BattleCruisers.Tutorial.Steps.Providers;
using BattleCruisers.UI;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Threading;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial
{
    public class TutorialStepsFactory : ITutorialStepsFactory
    {
        private readonly IHighlighter _highlighter;
        private readonly ITextDisplayer _displayer;
        private readonly IVariableDelayDeferrer _deferrer;
        private readonly ITutorialArgs _tutorialArgs;

        public TutorialStepsFactory(
            IHighlighter highlighter, 
            ITextDisplayer displayer, 
            IVariableDelayDeferrer deferrer,
            ITutorialArgs tutorialArgs)
        {
            Helper.AssertIsNotNull(highlighter, displayer, deferrer, tutorialArgs);

            _highlighter = highlighter;
            _displayer = displayer;
            _deferrer = deferrer;
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

            //// 6. Drones
            //steps.Enqueue(CreateStep_Drones());

            // 7. Building a building
            //steps.Enqueue(CreateSteps_BuildDroneStation());

            // 8. Enemy ship
            steps.Enqueue(CreateSteps_EnemyShipDefence());

            // 9. Enemy bomber
            // 10. Drone Focus

            return steps;
        }

        private ITutorialStep CreateStep_YourCruiser()
        {
            ITutorialStepArgs yourCruiserArgs = CreateTutorialStepArgs("This is your cruiser", _tutorialArgs.PlayerCruiser);
            return CreateClickStep(yourCruiserArgs, _tutorialArgs.PlayerCruiser);
        }

        private ITutorialStep CreateStep_NavigationButtons()
		{
			ITutorialStepArgs navigationButtonArgs
                = CreateTutorialStepArgs(
    				"These are your navigation buttons.  They help you move around the map.  Play around a bit with these.",
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
                = CreateTutorialStepArgs(
                    "This is the enemy cruiser.  You win if you destroy their cruiser before it destroys you.",
                    _tutorialArgs.AICruiser);
            return CreateClickStep(yourCruiserArgs, _tutorialArgs.AICruiser);
        }

        private IList<ITutorialStep> CreateSteps_SpeedControls()
        {
            IList<ITutorialStep> speedControlSteps = new List<ITutorialStep>();

            // Deviate from normal speed
            ITutorialStepArgs deviateFromNormalSpeedArgs
                = CreateTutorialStepArgs(
                    "These control the game speed.  Change the game speed!",
                    _tutorialArgs.GameSpeedWrapper.SlowMotionButton,
                    _tutorialArgs.GameSpeedWrapper.PlayButton,
                    _tutorialArgs.GameSpeedWrapper.FastForwardButton,
                    _tutorialArgs.GameSpeedWrapper.DoubleFastForwardButton);

            ITutorialStep deviateFromNormalSpeedStep
                = CreateClickStep(
                    deviateFromNormalSpeedArgs, 
                    _tutorialArgs.GameSpeedWrapper.SlowMotionButton, 
                    _tutorialArgs.GameSpeedWrapper.FastForwardButton, 
                    _tutorialArgs.GameSpeedWrapper.DoubleFastForwardButton);

            speedControlSteps.Add(deviateFromNormalSpeedStep);

            // Return to normal speed
            ITutorialStepArgs returnToNormalSpeedArgs
                = CreateTutorialStepArgs(
                    "Nice!  Now change it back :D",
                    _tutorialArgs.GameSpeedWrapper.PlayButton);
            speedControlSteps.Add(CreateClickStep(returnToNormalSpeedArgs, _tutorialArgs.GameSpeedWrapper.PlayButton));

            return speedControlSteps;
        }

        private ITutorialStep CreateStep_Drones()
        {
            ITutorialStepArgs dronesArgs
                = CreateTutorialStepArgs(
                    "Drones are the only resource.  This is how many drones you have.  The " +
                    "more drones you have the faster your cruiser works and the better " +
                    "buildings / units you can build.",
                    _tutorialArgs.PlayerCruiserInfo.NumOfDronesButton);
            return CreateClickStep(dronesArgs, _tutorialArgs.PlayerCruiserInfo.NumOfDronesButton);
        }

        private IList<ITutorialStep> CreateSteps_BuildDroneStation()
        {
            IList<ITutorialStep> buildDroneStationSteps
                = CreateSteps_ConstructBuilding(
                    BuildingCategory.Factory,
                    StaticPrefabKeys.Buildings.DroneStation,
                    SlotType.Utility,
                    "To get more drones build a drone station.",
                    "drone station");

            // Congrats!  Wait 3 seconds
            ITutorialStepArgs droneStationCompletedArgs
                = CreateTutorialStepArgs(
                    "Nice!  You have gained 2 drones :D",
                    _tutorialArgs.PlayerCruiserInfo.NumOfDronesButton);

            buildDroneStationSteps.Add(
                new DelayWaitStep(
                    droneStationCompletedArgs,
                    _deferrer,
                    waitTimeInS: 3));

            return buildDroneStationSteps;
        }

        private IList<ITutorialStep> CreateSteps_EnemyShipDefence()
        {
            List<ITutorialStep> enemyShipSteps = new List<ITutorialStep>();

            // FELIX  Add AI step(s) to insta-build naval factory, and infinitely slow build attack boat :)

            // Navigate to enemey cruiser
            enemyShipSteps.Add(CreateStep_NavigateToEnemyCruiser("Uh oh, the enemy is building an attack boat!  Have a look!"));

            // Click on attack boat
            string textToDisplay = null;
            ISingleBuildableProvider attackBoatProvider = _tutorialArgs.PermitterProvider.SingleShipProvider;
            ITutorialStepArgs clickAttackBoatArgs = CreateTutorialStepArgs(textToDisplay, attackBoatProvider);
            // FELIX  Uncomment, once AI step above is implemented :/
            //enemyShipSteps.Add(CreateClickStep(clickAttackBoatArgs, attackBoatProvider));

            // Navigate back to player cruiser
            enemyShipSteps.Add(CreateStep_NavigateToPlayerCruiser());

            // Build anti-ship turret
            IList<ITutorialStep> buildTurretSteps
                = CreateSteps_ConstructBuilding(
                    BuildingCategory.Defence,
                    StaticPrefabKeys.Buildings.AntiShipTurret,
                    SlotType.Deck,
                    "Quick, build an anti-ship turret!",
                    "anti-ship turret");
            enemyShipSteps.AddRange(buildTurretSteps);

            // FELIX Insta-complete attack boat

            // Wait for anti-ship turret to destroy attack boat

            // Congrats!  Wait 2 seconds

            return enemyShipSteps;
        }

        // FELIX  Allow specification of frontmost slot :)
        public IList<ITutorialStep> CreateSteps_ConstructBuilding(
            BuildingCategory buildingCategory, 
            IPrefabKey buildingToConstruct,
            SlotType buildingSlotType,
            string constructBuildingInstruction,
            string buildingName)
        {
            IList<ITutorialStep> constructionSteps = new List<ITutorialStep>();

            // Select building category
            IBuildingCategoryButton buildingCategoryButton = _tutorialArgs.BuildMenuButtons.GetCategoryButton(buildingCategory);
            Assert.IsNotNull(buildingCategoryButton);
            ITutorialStepArgs buildingCategoryArgs = CreateTutorialStepArgs(constructBuildingInstruction, buildingCategoryButton);
            constructionSteps.Add(new CategoryButtonStep(buildingCategoryArgs, buildingCategoryButton, _tutorialArgs.PermitterProvider.BuildingCategoryPermitter));

            // Select building
            IBuildableButton buildingButton = FindBuildableButton(buildingCategory, buildingToConstruct);
            string textToDisplay = null;  // Means previous text is displayed
            ITutorialStepArgs buldingButtonArgs = CreateTutorialStepArgs(textToDisplay, buildingButton);
            constructionSteps.Add(
                new BuildingButtonStep(
                    buldingButtonArgs,
                    buildingButton,
                    _tutorialArgs.PermitterProvider.BuildingPermitter,
                    buildingToConstruct));

            // Select a slot
            IList<ISlot> buildingSlots = _tutorialArgs.PlayerCruiser.SlotWrapper.GetSlotsForType(buildingSlotType);
            ISlot[] buildingSlotsArray = buildingSlots.ToArray();
            ITutorialStepArgs buildingSlotsArgs = CreateTutorialStepArgs(textToDisplay, buildingSlotsArray);
            constructionSteps.Add(
                new SlotsStep(
                    buildingSlotsArgs,
                    _tutorialArgs.PermitterProvider.SlotPermitter,
                    buildingSlotsArray));

            // Wait for building to complete construction
            ILastBuildingStartedProvider lastBuildingStartedProvider = _tutorialArgs.PermitterProvider.CreateLastBuildingStartedProvider(_tutorialArgs.PlayerCruiser);
			string waitText = "Wait for " + buildingName + " to complete, patience :)";
            ITutorialStepArgs waitForCompletionArgs = CreateTutorialStepArgs(waitText, lastBuildingStartedProvider);
            constructionSteps.Add(new BuildableCompletedWaitStep(waitForCompletionArgs, lastBuildingStartedProvider));

            return constructionSteps;
        }

        private ITutorialStep CreateStep_NavigateToPlayerCruiser()
        {
            string textToDisplay = "Navigate back to your cruiser";
            IButton playerCruiserButton = _tutorialArgs.NavigationButtonsWrapper.PlayerCruiserButton;
            return CreateStep_NavigateToCruiser(textToDisplay, playerCruiserButton);
        }

        private ITutorialStep CreateStep_NavigateToEnemyCruiser(string textToDisplay)
        {
            IButton enemyCruiserButton = _tutorialArgs.NavigationButtonsWrapper.AICruiserButton;
            return CreateStep_NavigateToCruiser(textToDisplay, enemyCruiserButton);
        }

        private ITutorialStep CreateStep_NavigateToCruiser(string textToDisplay, IButton navigationButton)
        {
            ITutorialStepArgs navigateToCruiserArgs = CreateTutorialStepArgs(textToDisplay, navigationButton);
            return new NavigationStep(navigateToCruiserArgs, _tutorialArgs.PermitterProvider.NavigationPermitter, navigationButton);
        }

        private IBuildableButton FindBuildableButton(BuildingCategory buildingCategory, IPrefabKey buildingKey)
        {
            _tutorialArgs.PermitterProvider.BuildingPermitter.PermittedBuilding = buildingKey;

            ReadOnlyCollection<IBuildableButton> categoryButtons = _tutorialArgs.BuildMenuButtons.GetBuildableButtons(buildingCategory);

            IBuildableButton buildableButton
                = categoryButtons
                    .FirstOrDefault(button => _tutorialArgs.PermitterProvider.BuildingActivenessDecider.ShouldBeEnabled(button.Buildable));

            _tutorialArgs.PermitterProvider.BuildingPermitter.PermittedBuilding = null;

            Assert.IsNotNull(buildableButton);
            return buildableButton;
        }

        private ITutorialStepArgs CreateTutorialStepArgs(string textToDisplay, params IHighlightable[] elementsToHighlight)
        {
            return CreateTutorialStepArgs(textToDisplay, new StaticHighlightableProvider(elementsToHighlight));
        }

        private ITutorialStepArgs CreateTutorialStepArgs(string textToDisplay, IHighlightablesProvider highlightablesProvider)
        {
            return
                new TutorialStepArgs(
                    _highlighter,
                    textToDisplay,
                    _displayer,
                    highlightablesProvider);
        }

        private ITutorialStep CreateClickStep(ITutorialStepArgs args, params IClickable[] completionClickables)
        {
            return CreateClickStep(args, new StaticClickablesProvider(completionClickables));
        }

        private ITutorialStep CreateClickStep(ITutorialStepArgs args, IClickablesProvider clickablesProvider)
        {
            return new ClickStep(args, clickablesProvider);
        }
    }
}