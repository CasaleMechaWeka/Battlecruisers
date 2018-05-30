using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.BuildProgress;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Tutorial.Providers;
using BattleCruisers.Tutorial.Steps;
using BattleCruisers.Tutorial.Steps.ClickSteps;
using BattleCruisers.Tutorial.Steps.EnemyCruiser;
using BattleCruisers.Tutorial.Steps.Providers;
using BattleCruisers.Tutorial.Steps.WaitSteps;
using BattleCruisers.UI;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras;
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
        private readonly ISingleBuildableProvider _lastPlayerIncompleteBuildingStartedProvider;

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

            _lastPlayerIncompleteBuildingStartedProvider = _tutorialArgs.TutorialProvider.CreateLastIncompleteBuildingStartedProvider(_tutorialArgs.PlayerCruiser);
        }

        public Queue<ITutorialStep> CreateTutorialSteps()
        {
            Queue<ITutorialStep> steps = new Queue<ITutorialStep>();

			// TEMP  For end game enable all tutorial steps :)

			// 0. Disable navigation
			steps.Enqueue(CreateStep_NavigationPermitter(NavigationPermission.None));

            // 1. Your cruiser
            steps.Enqueue(CreateStep_YourCruiser());

            // 2. Navigation buttons
            steps.Enqueue(CreateStep_NavigationButtons());

			// FELIX  Create composite step (as all navigation button steps will need a matching wait step :) )
			steps.Enqueue(CreateStep_NavigationWaitStep(CameraState.AiCruiser));

            // 3. Enemy cruiser
            steps.Enqueue(CreateStep_EnemyCruiser());

            // Navigate back to player cruiser
            steps.Enqueue(CreateStep_NavigateToPlayerCruiser());

            // TEMP  4. Add step for [Navigating via mouse / touch(eventually: P)]

            // 5. Speed controls
            steps.Enqueue(CreateSteps_SpeedControls());

            // 6. Drones
            steps.Enqueue(CreateStep_Drones());

            // 7. Building a building
            steps.Enqueue(CreateSteps_BuildDroneStation());

            // 8. Enemy ship
            steps.Enqueue(
                CreateSteps_EnemyUnitDefence(
                    StaticPrefabKeys.Buildings.NavalFactory,
                    new BuildableInfo(StaticPrefabKeys.Units.AttackBoat, "attack boat"),
                    _tutorialArgs.TutorialProvider.SingleShipProvider,
                    new BuildableInfo(StaticPrefabKeys.Buildings.AntiShipTurret, "anti-ship turret"),
                    preferFrontmostSlot: true));

            // 9. Enemy bomber
            steps.Enqueue(
                CreateSteps_EnemyUnitDefence(
                    StaticPrefabKeys.Buildings.AirFactory,
                    new BuildableInfo(StaticPrefabKeys.Units.Bomber, "bomber"),
                    _tutorialArgs.TutorialProvider.SingleAircraftProvider,
                    new BuildableInfo(StaticPrefabKeys.Buildings.AntiAirTurret, "anti-air turret"),
                    preferFrontmostSlot: false));
			
			// Navigate back to player cruiser
			steps.Enqueue(CreateStep_NavigateToPlayerCruiser());

            // 10. Drone Focus
            steps.Enqueue(CreateSteps_DroneFocus());

            return steps;
        }

        private ITutorialStep CreateStep_YourCruiser()
        {
            ITutorialStepArgs yourCruiserArgs = CreateTutorialStepArgs("This is your awesome cruiser :D", _tutorialArgs.PlayerCruiser);
            return CreateClickStep(yourCruiserArgs, _tutorialArgs.PlayerCruiser);
        }

        private ITutorialStep CreateStep_NavigationButtons()
		{
			ITutorialStepArgs navigationButtonArgs
                = CreateTutorialStepArgs(
    				"These are navigation buttons.  They help you move around the map.  Play around a bit with these.",
                    _tutorialArgs.NavigationButtonsWrapper.PlayerCruiserButton,
                    _tutorialArgs.NavigationButtonsWrapper.MidLeftButton,
                    _tutorialArgs.NavigationButtonsWrapper.OverviewButton,
                    _tutorialArgs.NavigationButtonsWrapper.MidRightButton,
                    _tutorialArgs.NavigationButtonsWrapper.AICruiserButton);
            
            return 
                new NavigationButtonStep(
                    navigationButtonArgs, 
					_tutorialArgs.NavigationSettings, 
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
                    "These buttons control the game speed.  Change the game speed!",
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
                    new BuildableInfo(StaticPrefabKeys.Buildings.DroneStation, "drone station"),
                    SlotType.Utility,
                    "To get more drones build a drone station.");

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

        private IList<ITutorialStep> CreateSteps_EnemyUnitDefence(
            IPrefabKey factoryKey,
            BuildableInfo unitToBuild,
            ISingleBuildableProvider unitBuildProvider,
            BuildableInfo defenceToBuild,
            bool preferFrontmostSlot)
        {
            List<ITutorialStep> enemyUnitDefenceSteps = new List<ITutorialStep>();

            // 1. Create factory and start producing units
            FactoryStepsResult factoryStepsResult = CreateSteps_CreateProducingFactory(factoryKey, unitToBuild.Key);
            enemyUnitDefenceSteps.AddRange(factoryStepsResult.Steps);

            // 2. Navigate to enemey cruiser
            enemyUnitDefenceSteps.Add(CreateStep_NavigateToEnemyCruiser("Uh oh, the enemy is building a " + unitToBuild.Name + "!  Have a look!"));

            // 3. Click on the unit
            string textToDisplay = null;
            ITutorialStepArgs clickUnitArgs = CreateTutorialStepArgs(textToDisplay, unitBuildProvider);
            enemyUnitDefenceSteps.Add(CreateClickStep(clickUnitArgs, unitBuildProvider));

            // 4. Navigate back to player cruiser
            enemyUnitDefenceSteps.Add(CreateStep_NavigateToPlayerCruiser());

            // 5. Build defence turret
            IList<ITutorialStep> buildTurretSteps
                = CreateSteps_ConstructBuilding(
                    BuildingCategory.Defence,
                    defenceToBuild,
                    SlotType.Deck,
                    "Quick, build an " + defenceToBuild.Name + "!",
                    waitForBuildingToComplete: true,
                    preferFrontmostSlot: preferFrontmostSlot);
            enemyUnitDefenceSteps.AddRange(buildTurretSteps);
			
			// 6. Navigate to mid left
			enemyUnitDefenceSteps.Add(
				new NavigationButtonStep(
					CreateTutorialStepArgs("Nice!  Zoom out a bit", _tutorialArgs.NavigationButtonsWrapper.MidLeftButton),
					_tutorialArgs.NavigationSettings,
					_tutorialArgs.NavigationButtonsWrapper.MidLeftButton));

            // 7. Insta-complete unit
            enemyUnitDefenceSteps.Add(
                CreateChangeBuildSpeedStep(
                    _tutorialArgs.TutorialProvider.AICruiserBuildSpeedController, 
                    BuildSpeed.VeryFast));

            enemyUnitDefenceSteps.Add(
                new BuildableCompletedWaitStep(
                    CreateTutorialStepArgs(textToDisplay: null),
                    unitBuildProvider));

            enemyUnitDefenceSteps.Add(
                new StopUnitConstructionStep(
                    CreateTutorialStepArgs(textToDisplay: null), 
                    factoryStepsResult.FactoryProvider));

            // 8. Wait for defence turret to destroy unit
            enemyUnitDefenceSteps.Add(
                new TargetDestroyedWaitStep(
                    CreateTutorialStepArgs("Here comes the enemy " + unitToBuild.Name + "."),
                    new BuildableToTargetProvider(unitBuildProvider)));

            // 9. Congrats!  Wait 3 seconds
            enemyUnitDefenceSteps.Add(
                new DelayWaitStep(
                    CreateTutorialStepArgs("Nice!  You have successfully defended your cruiser."),
                    _deferrer,
                    waitTimeInS: 3));

            return enemyUnitDefenceSteps;
        }

        private FactoryStepsResult CreateSteps_CreateProducingFactory(IPrefabKey factoryKey, IPrefabKey unitKey)
        {
            IList<ITutorialStep> factorySteps = new List<ITutorialStep>();

            // These steps should complete very quickly and require no user input.
            // There is no need to display any text to the user or highlight any
            // elements.
            string textToDisplay = null;
            ITutorialStepArgs commonArgs = CreateTutorialStepArgs(textToDisplay);

            // 1. Change build speed to super fast
            factorySteps.Add(
                CreateChangeBuildSpeedStep(
                    _tutorialArgs.TutorialProvider.AICruiserBuildSpeedController, 
                    BuildSpeed.VeryFast));

            // 2. Start building factory
            StartConstructingBuildingStep startConstructingFactoryStep
                = new StartConstructingBuildingStep(
                    commonArgs,
                    factoryKey,
                    _tutorialArgs.PrefabFactory,
                    _tutorialArgs.AICruiser);
            factorySteps.Add(startConstructingFactoryStep);

            // 3. Wait for factory completion
            factorySteps.Add(new BuildableCompletedWaitStep(commonArgs, startConstructingFactoryStep));

            // 4. Change build speed to infinitely slow
            factorySteps.Add(
                CreateChangeBuildSpeedStep(
                    _tutorialArgs.TutorialProvider.AICruiserBuildSpeedController, 
                    BuildSpeed.InfinitelySlow));

            // 5. Start building unit
            IProvider<IFactory> factoryProvider = new BuildableToFactoryProvider(startConstructingFactoryStep);
            factorySteps.Add(
                new StartConstructingUnitStep(
                    commonArgs,
                    unitKey,
                    _tutorialArgs.PrefabFactory,
                    factoryProvider));

            return new FactoryStepsResult(factorySteps, factoryProvider);
        }

        public IList<ITutorialStep> CreateSteps_ConstructBuilding(
            BuildingCategory buildingCategory, 
            BuildableInfo buildingToConstruct,
            SlotType buildingSlotType,
            string constructBuildingInstruction,
            bool waitForBuildingToComplete = true,
            bool preferFrontmostSlot = false)
        {
            IList<ITutorialStep> constructionSteps = new List<ITutorialStep>();

            // Select building category
            IBuildingCategoryButton buildingCategoryButton = _tutorialArgs.BuildMenuButtons.GetCategoryButton(buildingCategory);
            Assert.IsNotNull(buildingCategoryButton);
            ITutorialStepArgs buildingCategoryArgs = CreateTutorialStepArgs(constructBuildingInstruction, buildingCategoryButton);
            constructionSteps.Add(new CategoryButtonStep(buildingCategoryArgs, buildingCategoryButton, _tutorialArgs.TutorialProvider.BuildingCategoryPermitter));

            // Select building
            IBuildableButton buildingButton = FindBuildableButton(buildingCategory, buildingToConstruct.Key);
            string textToDisplay = null;  // Means previous text is displayed
            ITutorialStepArgs buldingButtonArgs = CreateTutorialStepArgs(textToDisplay, buildingButton);
            constructionSteps.Add(
                new BuildingButtonStep(
                    buldingButtonArgs,
                    buildingButton,
                    _tutorialArgs.TutorialProvider.BuildingPermitter,
                    buildingToConstruct.Key));

            // Select a slot
            ISlotsProvider slotsProvider = new SlotsProvider(_tutorialArgs.PlayerCruiser.SlotWrapper, buildingSlotType, preferFrontmostSlot);
            ITutorialStepArgs buildingSlotsArgs = CreateTutorialStepArgs(textToDisplay, slotsProvider);
            constructionSteps.Add(
                new SlotsStep(
                    buildingSlotsArgs,
                    _tutorialArgs.TutorialProvider.SlotPermitter,
                    slotsProvider));

            if (waitForBuildingToComplete)
            {
                // Wait for building to complete construction
                string waitText = "Wait for " + buildingToConstruct.Name + " to complete, patience :)";
                constructionSteps.Add(CreateStep_WaitForLastIncomlpeteBuildingToComplete(waitText));
			}

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
			return new NavigationButtonStep(navigateToCruiserArgs, _tutorialArgs.NavigationSettings, navigationButton);
        }

        private IBuildableButton FindBuildableButton(BuildingCategory buildingCategory, IPrefabKey buildingKey)
        {
            _tutorialArgs.TutorialProvider.BuildingPermitter.PermittedBuilding = buildingKey;

            ReadOnlyCollection<IBuildableButton> categoryButtons = _tutorialArgs.BuildMenuButtons.GetBuildableButtons(buildingCategory);

            IBuildableButton buildableButton
                = categoryButtons
                    .FirstOrDefault(button => _tutorialArgs.TutorialProvider.ShouldBuildingBeEnabledFilter.IsMatch(button.Buildable));

            _tutorialArgs.TutorialProvider.BuildingPermitter.PermittedBuilding = null;

            Assert.IsNotNull(buildableButton);
            return buildableButton;
        }

        private IList<ITutorialStep> CreateSteps_DroneFocus()
		{
			List<ITutorialStep> droneFocusSteps = new List<ITutorialStep>();

			// 0. Change build speed to infinitely slow
			droneFocusSteps.Add(
				CreateChangeBuildSpeedStep(
					_tutorialArgs.TutorialProvider.PlayerCruiserBuildSpeedController,
					BuildSpeed.InfinitelySlow));

			// TEMP  For testing tutorial, when previous step creating drone staion is disabled :)
			if (_tutorialArgs.PlayerCruiser.DroneManager.NumOfDrones < 6)
			{
				_tutorialArgs.PlayerCruiser.DroneManager.NumOfDrones = 6;
			}

			// 1. Build artillery
			droneFocusSteps.AddRange(
				CreateSteps_ConstructBuilding(
					BuildingCategory.Offence,
					 new BuildableInfo(StaticPrefabKeys.Buildings.Artillery, "artillery"),
					SlotType.Platform,
					 "Build an artillery to destroy the enemy cruiser.",
					 waitForBuildingToComplete: false));

			// 2. Build drone station
			droneFocusSteps.AddRange(
				CreateSteps_ConstructBuilding(
					BuildingCategory.Factory,
					new BuildableInfo(StaticPrefabKeys.Buildings.DroneStation, "drone station"),
					SlotType.Utility,
					 "Build a drone station",
					 waitForBuildingToComplete: false));

			// 3. Bring up drone station details
			string focusText =
				"Note that all drones are building the artillery.  But say you want to build " +
				"the drone station first.  You can 'focus' the drones on the drone station.  " +
				"Select the drone station.";
			droneFocusSteps.Add(
				new BuildingDetailsStep(
					CreateTutorialStepArgs(focusText, _lastPlayerIncompleteBuildingStartedProvider),
					_lastPlayerIncompleteBuildingStartedProvider,
					_tutorialArgs.TutorialProvider.UIManagerPermissions));

			// 4. Focus drones button
			string droneFocusButtonText = "This is the building details panel.  Select the 'focus' drones button";
			IButton droneFocusButton = _tutorialArgs.BuildingDetails.DroneFocusButton;
			ITutorialStepArgs droneFocusButtonArgs = CreateTutorialStepArgs(droneFocusButtonText, droneFocusButton);
			droneFocusSteps.Add(
				CreateClickStep(
					droneFocusButtonArgs,
					droneFocusButton));

			// 6. Dismiss building details
			string dismissText =
				"Nice!  All the drones have moved from the artillery to the drone station.  " +
				"Now dismiss the details panel by clicking anywhere.";
			droneFocusSteps.Add(
				new DismissStep(
					CreateTutorialStepArgs(dismissText),
					_tutorialArgs.BuildingDetails,
					_tutorialArgs.TutorialProvider.UIManagerPermissions));

			// 5. Change build speed to normal
			droneFocusSteps.Add(
				CreateChangeBuildSpeedStep(
					_tutorialArgs.TutorialProvider.PlayerCruiserBuildSpeedController,
					BuildSpeed.Normal));

			// 6. Wait for drone station to complete
			droneFocusSteps.Add(CreateStep_WaitForLastIncomlpeteBuildingToComplete("Now we wait for your buildings to complete.  Just relax :)"));

			// 7. Wait for artillery to complete
			droneFocusSteps.Add(CreateStep_WaitForLastIncomlpeteBuildingToComplete("And the artillery..."));

			// 8. Enable navigation
			droneFocusSteps.Add(
				CreateStep_NavigationPermitter(
					NavigationPermission.Both,
					"Nice!  Your artillery will now bombard the enemy cruiser.  Feel free to look around"));

			// 9. Wait for enemy cruiser to be destroyed
			droneFocusSteps.Add(
				new TargetDestroyedWaitStep(
					CreateTutorialStepArgs(textToDisplay: null),
					new StaticProvider<ITarget>(_tutorialArgs.AICruiser)));

			return droneFocusSteps;
		}

		private ITutorialStep CreateStep_WaitForLastIncomlpeteBuildingToComplete(string textToDisplay)
        {
            ITutorialStepArgs args = CreateTutorialStepArgs(textToDisplay, _lastPlayerIncompleteBuildingStartedProvider);
            return new BuildableCompletedWaitStep(args, _lastPlayerIncompleteBuildingStartedProvider);
        }

        private ITutorialStepArgs CreateTutorialStepArgs(string textToDisplay, params IHighlightable[] elementsToHighlight)
        {
            return CreateTutorialStepArgs(textToDisplay, new StaticListProvider<IHighlightable>(elementsToHighlight));
        }

        private ITutorialStepArgs CreateTutorialStepArgs(string textToDisplay, IListProvider<IHighlightable> highlightablesProvider)
        {
            return
                new TutorialStepArgs(
                    _highlighter,
                    textToDisplay,
                    _displayer,
                    highlightablesProvider);
        }

        private ITutorialStep CreateClickStep(ITutorialStepArgs args, params IClickableEmitter[] completionClickables)
        {
            return CreateClickStep(args, new StaticListProvider<IClickableEmitter>(completionClickables));
        }

        private ITutorialStep CreateClickStep(ITutorialStepArgs args, IListProvider<IClickableEmitter> clickablesProvider)
        {
            return new ClickStep(args, clickablesProvider);
        }

        private ITutorialStep CreateChangeBuildSpeedStep(IBuildSpeedController speedController, BuildSpeed buildSpeed)
        {
            return
                new ChangeCruiserBuildSpeedStep(
                    CreateTutorialStepArgs(textToDisplay: null),
                    speedController,
                    buildSpeed);
        }

		private ITutorialStep CreateStep_NavigationPermitter(NavigationPermission permission, string textToDisplay = null)
        {
            return
                new NavigationPermitterStep(
                    CreateTutorialStepArgs(textToDisplay),
					_tutorialArgs.NavigationSettings,
                    permission);
        }

		private ITutorialStep CreateStep_NavigationWaitStep(CameraState targetState)
		{
			return
				new NavigationTransitionWaitStep(
					CreateTutorialStepArgs(textToDisplay: null),
				    _tutorialArgs.CameraMover,
					targetState,
					_tutorialArgs.NavigationSettings);
		}
    }
}