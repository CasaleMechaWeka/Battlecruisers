using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.BuildProgress;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data.Static;
using BattleCruisers.Tutorial.Steps.ClickSteps;
using BattleCruisers.Tutorial.Steps.FeatureModifierSteps;
using BattleCruisers.Tutorial.Steps.Providers;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Localisation;
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
        private readonly ISlidingPanelWaitStepFactory _slidingPanelWaitStepFactory;

        public DroneFocusStepsFactory(
            ITutorialStepArgsFactory argsFactory,
            ILocTable tutorialStrings,
            IAutoNavigationStepFactory autoNavigationStepFactory, 
            IExplanationDismissableStepFactory explanationDismissableStepFactory, 
            IChangeCruiserBuildSpeedStepFactory changeCruiserBuildSpeedStepFactory, 
            IConstructBuildingStepsFactory constructBuildingStepsFactory, 
            ITutorialProvider tutorialProvider, 
            ISingleBuildableProvider lastPlayerIncompleteBuildingStartedProvider, 
            RightPanelComponents rightPanelComponents,
            ISlidingPanelWaitStepFactory slidingPanelWaitStepFactory)
            : base(argsFactory, tutorialStrings)
        {
            Helper.AssertIsNotNull(
                autoNavigationStepFactory,
                explanationDismissableStepFactory,
                changeCruiserBuildSpeedStepFactory,
                constructBuildingStepsFactory,
                tutorialProvider,
                lastPlayerIncompleteBuildingStartedProvider,
                rightPanelComponents,
                slidingPanelWaitStepFactory);

            _autoNavigationStepFactory = autoNavigationStepFactory;
            _explanationDismissableStepFactory = explanationDismissableStepFactory;
            _changeCruiserBuildSpeedStepFactory = changeCruiserBuildSpeedStepFactory;
            _constructBuildingStepsFactory = constructBuildingStepsFactory;
            _tutorialProvider = tutorialProvider;
            _lastPlayerIncompleteBuildingStartedProvider = lastPlayerIncompleteBuildingStartedProvider;
            _rightPanelComponents = rightPanelComponents;
            _slidingPanelWaitStepFactory = slidingPanelWaitStepFactory;
        }

        public IList<ITutorialStep> CreateSteps()
        {
            List<ITutorialStep> steps = new List<ITutorialStep>();

            // Navigate to player cruiser
            steps.AddRange(_autoNavigationStepFactory.CreateSteps(CameraFocuserTarget.PlayerCruiser));

            // FELIX  Loc
            // Explanation
            steps.Add(
                _explanationDismissableStepFactory.CreateStep(
                    _argsFactory.CreateTutorialStepArgs(
                    "It's vital to manage your Builders.  Let's see how that works...")));

            // Infinitely slow build speed
            steps.Add(
                _changeCruiserBuildSpeedStepFactory.CreateStep(
                    _tutorialProvider.PlayerCruiserBuildSpeedController, 
                    BuildSpeed.InfinitelySlow));

            // Start 2 buildings
            steps.AddRange(
                _constructBuildingStepsFactory.CreateSteps(
                    BuildingCategory.Factory,
                    new BuildableInfo(StaticPrefabKeys.Buildings.DroneStation, "Builder Bay"),
                    new SlotSpecification(SlotType.Utility, BuildingFunction.Generic, preferCruiserFront: false),
                    "First, contruct another Builder Bay.",
                    waitForBuildingToComplete: false));

            steps.Add(_slidingPanelWaitStepFactory.CreateSelectorHiddenWaitStep());

            steps.AddRange(
                _constructBuildingStepsFactory.CreateSteps(
                    BuildingCategory.Offence,
                    new BuildableInfo(StaticPrefabKeys.Buildings.Artillery, "Artillery"),
                    new SlotSpecification(SlotType.Platform, BuildingFunction.Generic, preferCruiserFront: false),
                    "Now construct an Artillery.",
                    waitForBuildingToComplete: false));

            // Slow build speed explanation
            steps.Add(
                _explanationDismissableStepFactory.CreateStep(
                    _argsFactory.CreateTutorialStepArgs("The build speed has been slowed down for this step...")));

            // Show informator
            steps.Add(
                new UIManagerPermissionsStep(
                    _argsFactory.CreateTutorialStepArgs(),
                    _tutorialProvider.UIManagerPermissions,
                    canShowItemDetails: true,
                    canDismissItemDetails: true));

            steps.Add(
                new ExplanationClickStep(
                    _argsFactory.CreateTutorialStepArgs("Click on a building", _lastPlayerIncompleteBuildingStartedProvider, shouldUnhighlight: false),
                    _lastPlayerIncompleteBuildingStartedProvider));

            steps.Add(_slidingPanelWaitStepFactory.CreateInformatorShownWaitStep());

            // Explain drone focus buttons
            steps.Add(
                _explanationDismissableStepFactory.CreateStep(
                    _argsFactory.CreateTutorialStepArgs(
                        "Click \"BUILDERS\" to toggle the construction speed.  You can also double click the building itself.",
                        _rightPanelComponents.InformatorPanel.BuildingDetails.DroneFocusButton)));

            // Encourage user to experiment
            steps.Add(
                _explanationDismissableStepFactory.CreateStepWithSecondaryButton(
                    _argsFactory.CreateTutorialStepArgs("Switch priority between your Artillery and Builder Bay to get a feel for this.")));

            return steps;
        }
    }
}