using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.Tutorial.Explanation;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Tutorial.Highlighting.Masked;
using BattleCruisers.Tutorial.Providers;
using BattleCruisers.Tutorial.Steps;
using BattleCruisers.Tutorial.Steps.ClickSteps;
using BattleCruisers.Tutorial.Steps.FeatureModifierSteps;
using BattleCruisers.Tutorial.Steps.Providers;
using BattleCruisers.Tutorial.Steps.WaitSteps;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Threading;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial
{
    public class TutorialStepsFactoryNEW : ITutorialStepsFactory
    {
        private readonly IHighlighterNEW _highlighter;
        private readonly ITextDisplayer _displayer;
        private readonly IExplanationDismissButton _explanationDismissButton;
        private readonly IVariableDelayDeferrer _deferrer;
        private readonly ITutorialArgsNEW _tutorialArgs;
        private readonly ISingleBuildableProviderNEW _lastPlayerIncompleteBuildingStartedProvider;

        public TutorialStepsFactoryNEW(
            IHighlighterNEW highlighter,
            IExplanationPanel explanationPanel,
            IVariableDelayDeferrer deferrer,
            ITutorialArgsNEW tutorialArgs)
        {
            Helper.AssertIsNotNull(highlighter, explanationPanel, deferrer, tutorialArgs);

            _highlighter = highlighter;
            _displayer = explanationPanel.TextDisplayer;
            _explanationDismissButton = explanationPanel.DismissButton;
            _deferrer = deferrer;
            _tutorialArgs = tutorialArgs;

            _lastPlayerIncompleteBuildingStartedProvider = _tutorialArgs.TutorialProvider.CreateLastIncompleteBuildingStartedProvider(_tutorialArgs.PlayerCruiser);
        }

        public Queue<ITutorialStep> CreateTutorialSteps()
        {
            Queue<ITutorialStep> steps = new Queue<ITutorialStep>();

            // FELIX  Uncomment :)
            //// 1. Player cruiser
            //steps.Enqueue(CreateSteps_YourCruiser());

            //// 2. Navigation wheel
            //steps.Enqueue(CreateSteps_NavigationWheel());

            //// 3. Enemy cruiser
            //steps.Enqueue(CreateSteps_EnemyCruiser());

            //// 4. Player cruiser widgets
            //steps.Enqueue(CreateSteps_PlayerCruiserWidgets());

            // 5. Construct drone station
            steps.Enqueue(CreateSteps_ConstructDroneStation());

            return steps;
        }

        private IList<ITutorialStep> CreateSteps_YourCruiser()
        {
            IList<ITutorialStep> steps = new List<ITutorialStep>();

            steps.Add(CreateStep_CameraAdjustmentWaitStep());

            ITutorialStepArgsNEW args
                = CreateTutorialStepArgs(
                    "This is your awesome cruiser :D",
                    _tutorialArgs.PlayerCruiser);

            steps.Add(
                new ExplanationDismissableStep(
                    args,
                    _explanationDismissButton));

            return steps;
        }

        private IList<ITutorialStep> CreateSteps_NavigationWheel()
        {
            IList<ITutorialStep> steps = new List<ITutorialStep>();

            ITutorialStepArgsNEW navigationWheelStepArgs
                = CreateTutorialStepArgs(
                    "This is the navigation wheel, which you use to navigate around the map.",
                    _tutorialArgs.CameraComponents.NavigationWheel);
            steps.Add(
                new ExplanationDismissableStep(
                    navigationWheelStepArgs,
                    _explanationDismissButton));

            steps.Add(CreateStep_NavigationToggle(enableNavigation: true));

            ITutorialStepArgsNEW freeNavigationArgs
                = CreateTutorialStepArgs(
                    textToDisplay: "Drag the navigation wheel to navigate.");
            steps.Add(
                new ExplanationDismissableStep(
                    freeNavigationArgs,
                    _explanationDismissButton));

            steps.Add(CreateStep_NavigationToggle(enableNavigation: false));

            return steps;
        }

        private ITutorialStep CreateStep_NavigationToggle(bool enableNavigation)
        {
            return 
                new NavigationToggleStep(
                    CreateTutorialStepArgs(),
                    _tutorialArgs.TutorialProvider.IsNavigationEnabledFilter,
                    enableNavigation);
        }

        private IList<ITutorialStep> CreateSteps_EnemyCruiser()
        {
            List<ITutorialStep> steps = new List<ITutorialStep>();

            steps.AddRange(CreateSteps_AutoNavigation(CameraFocuserTarget.AICruiser));

            ITutorialStepArgsNEW args
                = CreateTutorialStepArgs(
                    "This is the enemy cruiser.  You win if you destroy their cruiser before it destroys you.",
                    _tutorialArgs.AICruiser);

            steps.Add(
                new ExplanationDismissableStep(
                    args,
                    _explanationDismissButton));

            return steps;
        }

        private IList<ITutorialStep> CreateSteps_PlayerCruiserWidgets()
        {
            List<ITutorialStep> steps = new List<ITutorialStep>();

            steps.AddRange(CreateSteps_AutoNavigation(CameraFocuserTarget.PlayerCruiser));

            // Health dial
            ITutorialStepArgsNEW healthDialArgs
                = CreateTutorialStepArgs(
                    "This is your cruiser's health dial.",
                    _tutorialArgs.LeftPanelComponents.HealthDialHighlightable);

            steps.Add(
                new ExplanationDismissableStep(
                    healthDialArgs,
                    _explanationDismissButton));

            // Drone number
            ITutorialStepArgsNEW droneNumberArgs
                = CreateTutorialStepArgs(
                    "Builders are the only resource.  This is how many builders you have.  The more builders you have the faster your cruiser works and the better buildings and units you can build.",
                    _tutorialArgs.LeftPanelComponents.NumberOfDronesHighlightable);

            steps.Add(
                new ExplanationDismissableStep(
                    droneNumberArgs,
                    _explanationDismissButton));

            return steps;
        }

        private IList<ITutorialStep> CreateSteps_ConstructDroneStation()
        {
            List<ITutorialStep> steps = new List<ITutorialStep>();

            steps.AddRange(
                CreateSteps_ConstructBuilding(
                    BuildingCategory.Factory,
                    new BuildableInfo(StaticPrefabKeys.Buildings.DroneStation, "builder bay"),
                    new SlotSpecification(SlotType.Utility, BuildingFunction.Generic, preferCruiserFront: true),
                    "To get more builders construct a builder bay."));

            ITutorialStepArgsNEW args = CreateTutorialStepArgs("Nice!  You have gained 2 builders :D");
            steps.Add(
                new ExplanationDismissableStep(
                    args,
                    _explanationDismissButton));

            return steps;
        }

        private IList<ITutorialStep> CreateSteps_AutoNavigation(CameraFocuserTarget cameraFocuserTarget)
        {
            IList<ITutorialStep> steps = new List<ITutorialStep>();

            steps.Add(
                new CameraFocuserStep(
                    CreateTutorialStepArgs(), 
                    _tutorialArgs.CameraComponents.CameraFocuser,
                    cameraFocuserTarget));

            steps.Add(CreateStep_CameraAdjustmentWaitStep());

            return steps;
        }

        private IList<ITutorialStep> CreateSteps_ConstructBuilding(
            BuildingCategory buildingCategory,
            BuildableInfo buildingToConstruct,
            SlotSpecification slotSpecification,
            string constructBuildingInstruction,
            bool waitForBuildingToComplete = true)
        {
            IList<ITutorialStep> constructionSteps = new List<ITutorialStep>();

            // Select building category
            IBuildingCategoryButton buildingCategoryButton = _tutorialArgs.LeftPanelComponents.BuildMenu.GetCategoryButton(buildingCategory);
            Assert.IsNotNull(buildingCategoryButton);
            ITutorialStepArgsNEW buildingCategoryArgs = CreateTutorialStepArgs(constructBuildingInstruction, buildingCategoryButton);
            constructionSteps.Add(new CategoryButtonStepNEW(buildingCategoryArgs, buildingCategoryButton, _tutorialArgs.TutorialProvider.BuildingCategoryPermitter));

            // Select building
            IBuildableButton buildingButton = FindBuildableButton(buildingCategory, buildingToConstruct.Key);
            string textToDisplay = null;  // Means previous text is displayed
            ITutorialStepArgsNEW buldingButtonArgs = CreateTutorialStepArgs(textToDisplay, buildingButton);
            ISlotProvider slotProvider = new SlotProvider(_tutorialArgs.PlayerCruiser.SlotAccessor, slotSpecification);
            constructionSteps.Add(
                new BuildingButtonStepNEW(
                    buldingButtonArgs,
                    buildingButton,
                    _tutorialArgs.TutorialProvider.BuildingPermitter,
                    buildingToConstruct.Key,
                    slotProvider,
                    _tutorialArgs.TutorialProvider.SlotPermitter));

            // Select a slot
            ITutorialStepArgsNEW buildingSlotsArgs = CreateTutorialStepArgs(textToDisplay, slotProvider);
            constructionSteps.Add(
                new SlotStep(
                    buildingSlotsArgs,
                    _tutorialArgs.TutorialProvider.SlotPermitter,
                    slotProvider));

            if (waitForBuildingToComplete)
            {
                // Wait for building to complete construction
                string waitText = "Wait for " + buildingToConstruct.Name + " to complete.  Patience :)";
                constructionSteps.Add(CreateStep_WaitForLastIncomlpeteBuildingToComplete(waitText));
            }

            return constructionSteps;
        }

        private IBuildableButton FindBuildableButton(BuildingCategory buildingCategory, IPrefabKey buildingKey)
        {
            _tutorialArgs.TutorialProvider.BuildingPermitter.PermittedBuilding = buildingKey;

            ReadOnlyCollection<IBuildableButton> categoryButtons = _tutorialArgs.LeftPanelComponents.BuildMenu.GetBuildableButtons(buildingCategory);

            IBuildableButton buildableButton
                = categoryButtons
                    .FirstOrDefault(button => _tutorialArgs.TutorialProvider.ShouldBuildingBeEnabledFilter.IsMatch(button.Buildable));

            _tutorialArgs.TutorialProvider.BuildingPermitter.PermittedBuilding = null;

            Assert.IsNotNull(buildableButton);
            return buildableButton;
        }

        private ITutorialStep CreateStep_WaitForLastIncomlpeteBuildingToComplete(string textToDisplay)
        {
            ITutorialStepArgsNEW args = CreateTutorialStepArgs(textToDisplay);
            return new BuildableCompletedWaitStepNEW(args, _lastPlayerIncompleteBuildingStartedProvider);
        }

        private ITutorialStep CreateStep_CameraAdjustmentWaitStep()
        {
            return
                new CameraAdjustmentWaitStep(
                    CreateTutorialStepArgs(),
                    _tutorialArgs.CameraComponents.CameraAdjuster);
        }

        private ITutorialStepArgsNEW CreateTutorialStepArgs(
            string textToDisplay = null,
            IMaskHighlightable highlightable = null)
        {
            return CreateTutorialStepArgs(textToDisplay, new StaticProvider<IMaskHighlightable>(highlightable));
        }

        private ITutorialStepArgsNEW CreateTutorialStepArgs(
            string textToDisplay,
            IItemProvider<IMaskHighlightable> highlightableProvider)
        {
            Assert.IsNotNull(highlightableProvider);

            return
                new TutorialStepArgsNEW(
                    _highlighter,
                    textToDisplay,
                    _displayer,
                    highlightableProvider);
        }
    }
}