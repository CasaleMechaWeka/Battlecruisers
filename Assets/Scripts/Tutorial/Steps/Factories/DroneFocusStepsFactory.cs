using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.BuildProgress;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data.Static;
using BattleCruisers.Tutorial.Steps.ClickSteps;
using BattleCruisers.Tutorial.Steps.FeatureModifierSteps;
using BattleCruisers.Tutorial.Steps.Providers;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.Utils;
using System.Collections.Generic;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class DroneFocusStepsFactory : TutorialFactoryBase, ITutorialStepsFactory
    {
        private readonly IAutoNavigationStepFactory _autoNavigationStepFactory;
        private readonly IExplanationDismissableStepFactory _explanationDismissableStepFactory;
        private readonly IChangeCruiserBuildSpeedStepFactory _changeCruiserBuildSpeedStepFactory;
        private readonly IConstructBuildingStepsFactory _constructBuildingStepsFactory;
        private readonly ITutorialProvider _tutorialProvider;
        private readonly ISingleBuildableProvider _lastPlayerIncompleteBuildingStartedProvider;
        private readonly RightPanelComponents _rightPanelComponents;

        public DroneFocusStepsFactory(
            ITutorialStepArgsFactory argsFactory, 
            IAutoNavigationStepFactory autoNavigationStepFactory, 
            IExplanationDismissableStepFactory explanationDismissableStepFactory, 
            IChangeCruiserBuildSpeedStepFactory changeCruiserBuildSpeedStepFactory, 
            IConstructBuildingStepsFactory constructBuildingStepsFactory, 
            ITutorialProvider tutorialProvider, 
            ISingleBuildableProvider lastPlayerIncompleteBuildingStartedProvider, 
            RightPanelComponents rightPanelComponents)
            : base(argsFactory)
        {
            Helper.AssertIsNotNull(
                autoNavigationStepFactory,
                explanationDismissableStepFactory,
                changeCruiserBuildSpeedStepFactory,
                constructBuildingStepsFactory,
                tutorialProvider,
                lastPlayerIncompleteBuildingStartedProvider,
                rightPanelComponents);

            _autoNavigationStepFactory = autoNavigationStepFactory;
            _explanationDismissableStepFactory = explanationDismissableStepFactory;
            _changeCruiserBuildSpeedStepFactory = changeCruiserBuildSpeedStepFactory;
            _constructBuildingStepsFactory = constructBuildingStepsFactory;
            _tutorialProvider = tutorialProvider;
            _lastPlayerIncompleteBuildingStartedProvider = lastPlayerIncompleteBuildingStartedProvider;
            _rightPanelComponents = rightPanelComponents;
        }

        public IList<ITutorialStep> CreateSteps()
        {
            List<ITutorialStep> steps = new List<ITutorialStep>();

            // Navigate to player cruiser
            steps.AddRange(_autoNavigationStepFactory.CreateSteps(CameraFocuserTarget.PlayerCruiser));

            // Explanation
            steps.Add(
                _explanationDismissableStepFactory.CreateStep(
                    _argsFactory.CreateTutorialStepArgs(
                    "Managing your builders is vital.  Let's start 3 buildings, so we can see how this works :)")));

            // Infinitely slow build speed
            steps.Add(
                _changeCruiserBuildSpeedStepFactory.CreateStep(
                    _tutorialProvider.PlayerCruiserBuildSpeedController, 
                    BuildSpeed.InfinitelySlow));

            // Start 3 buildings
            steps.AddRange(
                _constructBuildingStepsFactory.CreateSteps(
                    BuildingCategory.Defence,
                    new BuildableInfo(StaticPrefabKeys.Buildings.AntiAirTurret, "anti-air turret"),
                    new SlotSpecification(SlotType.Deck, BuildingFunction.AntiAir, preferCruiserFront: false),
                    "First, another anti-air turret.",
                    waitForBuildingToComplete: false));

            steps.AddRange(
                _constructBuildingStepsFactory.CreateSteps(
                    BuildingCategory.Factory,
                    new BuildableInfo(StaticPrefabKeys.Buildings.DroneStation, "builder bay"),
                    new SlotSpecification(SlotType.Utility, BuildingFunction.Generic, preferCruiserFront: false),
                    "Second, another builder bay.",
                    waitForBuildingToComplete: false));

            steps.AddRange(
                _constructBuildingStepsFactory.CreateSteps(
                    BuildingCategory.Offence,
                    new BuildableInfo(StaticPrefabKeys.Buildings.Artillery, "artillery"),
                    new SlotSpecification(SlotType.Platform, BuildingFunction.Generic, preferCruiserFront: false),
                    "And lastly, an artillery.",
                    waitForBuildingToComplete: false));

            // Slow build speed explanation
            steps.Add(
                _explanationDismissableStepFactory.CreateStep(
                    _argsFactory.CreateTutorialStepArgs("Note the build speed has been slowed down, so you can play around with managing your builders without the buildings completing.")));

            // Show informator
            steps.Add(
                new UIManagerPermissionsStep(
                    _argsFactory.CreateTutorialStepArgs(),
                    _tutorialProvider.UIManagerPermissions,
                    canShowItemDetails: true,
                    canDismissItemDetails: true));

            steps.Add(
                new ExplanationClickStep(
                    _argsFactory.CreateTutorialStepArgs("Click on a building", _lastPlayerIncompleteBuildingStartedProvider),
                    _lastPlayerIncompleteBuildingStartedProvider));

            // Explain drone focus buttons
            steps.Add(
                _explanationDismissableStepFactory.CreateStep(
                    _argsFactory.CreateTutorialStepArgs(
                        "This is the \"builders\" button.  You can change how many builders a building uses via this button (or by double clicking the building).",
                        _rightPanelComponents.InformatorPanel.BuildingDetails.DroneFocusButton)));

            // Encourage user to experiment
            steps.Add(
                _explanationDismissableStepFactory.CreateStep(
                    _argsFactory.CreateTutorialStepArgs("Now play around with the \"builders\" button for these 3 buildings, and see how the builders move between buildings.  (Click the \"OK\" when you have had enough.)")));

            return steps;
        }
    }
}