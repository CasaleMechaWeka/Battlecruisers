using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.BuildProgress;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data.Static;
using BattleCruisers.Tutorial.Steps.ClickSteps;
using BattleCruisers.Tutorial.Steps.FeatureModifierSteps;
using BattleCruisers.Tutorial.Steps.Providers;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
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
        private readonly IPrefabFactory _prefabFactory;
        private readonly ILocTable _commonStrings;

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
            ISlidingPanelWaitStepFactory slidingPanelWaitStepFactory,
            IPrefabFactory prefabFactory,
            ILocTable commonStrings)
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
                slidingPanelWaitStepFactory,
                prefabFactory,
                commonStrings);

            _autoNavigationStepFactory = autoNavigationStepFactory;
            _explanationDismissableStepFactory = explanationDismissableStepFactory;
            _changeCruiserBuildSpeedStepFactory = changeCruiserBuildSpeedStepFactory;
            _constructBuildingStepsFactory = constructBuildingStepsFactory;
            _tutorialProvider = tutorialProvider;
            _lastPlayerIncompleteBuildingStartedProvider = lastPlayerIncompleteBuildingStartedProvider;
            _rightPanelComponents = rightPanelComponents;
            _slidingPanelWaitStepFactory = slidingPanelWaitStepFactory;
            _prefabFactory = prefabFactory;
            _commonStrings = commonStrings;
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
                        _tutorialStrings.GetString("Steps/DroneFocus/IntroMessage"))));

            // Infinitely slow build speed
            steps.Add(
                _changeCruiserBuildSpeedStepFactory.CreateStep(
                    _tutorialProvider.PlayerCruiserBuildSpeedController, 
                    BuildSpeed.InfinitelySlow));

            // Start 2 buildings
            string builderBayName = _prefabFactory.GetBuildingWrapperPrefab(StaticPrefabKeys.Buildings.DroneStation).Buildable.Name;
            string constructBuilderBayBase = _tutorialStrings.GetString("Steps/DroneFocus/ConstructBuilderBay");
            steps.AddRange(
                _constructBuildingStepsFactory.CreateSteps(
                    BuildingCategory.Factory,
                    new BuildableInfo(StaticPrefabKeys.Buildings.DroneStation, builderBayName),
                    new SlotSpecification(SlotType.Utility, BuildingFunction.Generic, preferCruiserFront: false),
                    string.Format(constructBuilderBayBase, builderBayName),
                    waitForBuildingToComplete: false));

            steps.Add(_slidingPanelWaitStepFactory.CreateSelectorHiddenWaitStep());

            string artilleryName = _prefabFactory.GetBuildingWrapperPrefab(StaticPrefabKeys.Buildings.Artillery).Buildable.Name;
            string constructArtilleryBase = _tutorialStrings.GetString("Steps/DroneFocus/ConstructArtillery");
            steps.AddRange(
                _constructBuildingStepsFactory.CreateSteps(
                    BuildingCategory.Offence,
                    new BuildableInfo(StaticPrefabKeys.Buildings.Artillery, artilleryName),
                    new SlotSpecification(SlotType.Platform, BuildingFunction.Generic, preferCruiserFront: false),
                    string.Format(constructArtilleryBase, artilleryName),
                    waitForBuildingToComplete: false));

            // Slow build speed explanation
            steps.Add(
                _explanationDismissableStepFactory.CreateStep(
                    _argsFactory.CreateTutorialStepArgs(
                        _tutorialStrings.GetString("Steps/DroneFocus/BuildSpeedExplanation"))));

            // Show informator
            steps.Add(
                new UIManagerPermissionsStep(
                    _argsFactory.CreateTutorialStepArgs(),
                    _tutorialProvider.UIManagerPermissions,
                    canShowItemDetails: true,
                    canDismissItemDetails: true));

            steps.Add(
                new ExplanationClickStep(
                    _argsFactory.CreateTutorialStepArgs(_tutorialStrings.GetString("Steps/DroneFocus/ClickBuilding"), _lastPlayerIncompleteBuildingStartedProvider, shouldUnhighlight: false),
                    _lastPlayerIncompleteBuildingStartedProvider));

            steps.Add(_slidingPanelWaitStepFactory.CreateInformatorShownWaitStep());

            // Explain drone focus buttons
            string buttonText = _commonStrings.GetString("UI/Informator/DronesButton");
            string clickBuildersButtonBase = _tutorialStrings.GetString("Steps/DroneFocus/ClickBuildersButton");
            steps.Add(
                _explanationDismissableStepFactory.CreateStep(
                    _argsFactory.CreateTutorialStepArgs(
                        string.Format(clickBuildersButtonBase, buttonText),
                        _rightPanelComponents.InformatorPanel.BuildingDetails.DroneFocusButton)));

            // Encourage user to experiment
            string switchBuildFocusBase = _tutorialStrings.GetString("Steps/DroneFocus/SwitchBuilderFocus");
            steps.Add(
                _explanationDismissableStepFactory.CreateStepWithSecondaryButton(
                    _argsFactory.CreateTutorialStepArgs(
                        string.Format(switchBuildFocusBase, artilleryName, builderBayName))));

            return steps;
        }
    }
}