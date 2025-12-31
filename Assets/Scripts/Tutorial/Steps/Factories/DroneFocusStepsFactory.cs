using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.BuildProgress;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data.Static;
using BattleCruisers.Scenes.BattleScene;
using BattleCruisers.Tutorial.Steps.ClickSteps;
using BattleCruisers.Tutorial.Steps.FeatureModifierSteps;
using BattleCruisers.Tutorial.Steps.Providers;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Localisation;
using System.Collections.Generic;
using UnityEngine.Localization.Settings;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class DroneFocusStepsFactory : TutorialFactoryBase, ITutorialStepsFactory
    {
        private readonly AutoNavigationStepFactory _autoNavigationStepFactory;
        private readonly ExplanationDismissableStepFactory _explanationDismissableStepFactory;
        private readonly ChangeCruiserBuildSpeedStepFactory _changeCruiserBuildSpeedStepFactory;
        private readonly ConstructBuildingStepsFactory _constructBuildingStepsFactory;
        private readonly TutorialHelper _tutorialProvider;
        private readonly ISingleBuildableProvider _lastPlayerIncompleteBuildingStartedProvider;
        private readonly RightPanelComponents _rightPanelComponents;
        private readonly SlidingPanelWaitStepFactory _slidingPanelWaitStepFactory;

        public DroneFocusStepsFactory(
            TutorialStepArgsFactory argsFactory,
            AutoNavigationStepFactory autoNavigationStepFactory,
            ExplanationDismissableStepFactory explanationDismissableStepFactory,
            ChangeCruiserBuildSpeedStepFactory changeCruiserBuildSpeedStepFactory,
            ConstructBuildingStepsFactory constructBuildingStepsFactory,
            TutorialHelper tutorialProvider,
            ISingleBuildableProvider lastPlayerIncompleteBuildingStartedProvider,
            RightPanelComponents rightPanelComponents,
            SlidingPanelWaitStepFactory slidingPanelWaitStepFactory)

            : base(argsFactory)
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

        public IList<TutorialStep> CreateSteps()
        {
            List<TutorialStep> steps = new List<TutorialStep>();

            // Navigate to player cruiser
            steps.AddRange(_autoNavigationStepFactory.CreateSteps(CameraFocuserTarget.PlayerCruiser));

            // Explanation
            steps.Add(
                _explanationDismissableStepFactory.CreateStep(
                    _argsFactory.CreateTutorialStepArgs(
                        LocTableCache.TutorialTable.GetString("Steps/DroneFocus/IntroMessage"))));

            // Infinitely slow build speed
            steps.Add(
                _changeCruiserBuildSpeedStepFactory.CreateStep(
                    _tutorialProvider.PlayerCruiserBuildSpeedController,
                    BuildSpeed.InfinitelySlow));

            // Start 2 buildings
            string builderBayName = PrefabFactory.GetBuildingWrapperPrefab(StaticPrefabKeys.Buildings.DroneStation).Buildable.Name;
            string constructBuilderBayBase = LocTableCache.TutorialTable.GetString("Steps/DroneFocus/ConstructBuilderBay");
            steps.AddRange(
                _constructBuildingStepsFactory.CreateSteps(
                    BuildingCategory.Factory,
                    new BuildableInfo(StaticPrefabKeys.Buildings.DroneStation, builderBayName),
                    new SlotSpecification(SlotType.Utility, BuildingFunction.Generic, preferCruiserFront: false),
                    string.Format(constructBuilderBayBase, builderBayName),
                    waitForBuildingToComplete: false));

            steps.Add(_slidingPanelWaitStepFactory.CreateSelectorHiddenWaitStep());

            string artilleryName = PrefabFactory.GetBuildingWrapperPrefab(StaticPrefabKeys.Buildings.Artillery).Buildable.Name;
            string constructArtilleryBase = LocTableCache.TutorialTable.GetString("Steps/DroneFocus/ConstructArtillery");
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
                        LocTableCache.TutorialTable.GetString("Steps/DroneFocus/BuildSpeedExplanation"))));

            // Show informator
            steps.Add(
                new UIManagerPermissionsStep(
                    _argsFactory.CreateTutorialStepArgs(),
                    _tutorialProvider.UIManagerPermissions,
                    canShowItemDetails: true,
                    canDismissItemDetails: true));

            steps.Add(
                new ExplanationClickStep(
                    _argsFactory.CreateTutorialStepArgs(LocTableCache.TutorialTable.GetString("Steps/DroneFocus/ClickBuilding"), _lastPlayerIncompleteBuildingStartedProvider, shouldUnhighlight: false),
                    _lastPlayerIncompleteBuildingStartedProvider));

            steps.Add(_slidingPanelWaitStepFactory.CreateInformatorShownWaitStep());

            // Explain drone focus buttons
            var stringDatabase = LocalizationSettings.StringDatabase;
            var battleSceneTable = stringDatabase.GetTable("BattleScene");
            var entry = battleSceneTable.GetEntry("ToggleDronesButton");
            string buttonText = entry.GetLocalizedString();

            string clickBuildersButtonBase = LocTableCache.TutorialTable.GetString("Steps/DroneFocus/ClickBuildersButton");
            steps.Add(
                _explanationDismissableStepFactory.CreateStep(
                    _argsFactory.CreateTutorialStepArgs(
                        string.Format(clickBuildersButtonBase, buttonText),
                        _rightPanelComponents.InformatorPanel.Buttons.ToggleDronesButton)));

            // Encourage user to experiment
            string switchBuildFocusBase = LocTableCache.TutorialTable.GetString("Steps/DroneFocus/SwitchBuilderFocus");
            steps.Add(
                _explanationDismissableStepFactory.CreateStepWithSecondaryButton(
                    _argsFactory.CreateTutorialStepArgs(
                        string.Format(switchBuildFocusBase, artilleryName, builderBayName))));

            return steps;
        }
    }
}