using BattleCruisers.Tutorial.Explanation;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Tutorial.Steps.Factories.EnemyUnit;
using BattleCruisers.Tutorial.Steps.Providers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Threading;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class TutorialStepsFactoriesProvider
    {
        public ITutorialStepsFactory YourCruiserStepsFactory { get; }
        public ITutorialStepsFactory NavigationWheelStepsFactory { get; }
        public ITutorialStepsFactory EnemyCruiserStepsFactory { get; }
        public ITutorialStepsFactory PlayerCruiserWidgetsStepsFactory { get; }
        public ITutorialStepsFactory ConstructDroneStationStepsFactory { get; }
        public ITutorialStepsFactory EnemyBomberStepsFactory { get; }
        public ITutorialStepsFactory EnemyShipStepsFactory { get; }
        public ITutorialStepsFactory DroneFocusStepsFactory { get; }
        public ITutorialStepsFactory GameSpeedStepsFactory { get; }
        public ITutorialStepsFactory EndgameStepsFactory { get; }

        public TutorialStepsFactoriesProvider(
            IHighlighter highlighter,
            IExplanationPanel explanationPanel,
            IDeferrer deferrer,
            ITutorialArgs tutorialArgs)
        {
            Helper.AssertIsNotNull(highlighter, explanationPanel, deferrer, tutorialArgs);

            ITutorialStepArgsFactory argsFactory = new TutorialStepArgsFactory(highlighter, explanationPanel.TextDisplayer);
            ITutorialStepFactory cameraAdjustmentWaitStepFactory = new CameraAdjustmentWaitStepFactory(argsFactory, tutorialArgs.CameraComponents);
            IExplanationDismissableStepFactory explanationDismissableStepFactory 
                = new ExplanationDismissableStepFactory(argsFactory, explanationPanel.OkButton, explanationPanel.DoneButton);
            IFeaturePermitterStepFactory featurePermitterStepFactory = new FeaturePermitterStepFactory(argsFactory);
            IAutoNavigationStepFactory autoNavigationStepFactory 
                = new AutoNavigationStepFactory(argsFactory, cameraAdjustmentWaitStepFactory, tutorialArgs.CameraComponents);
            ISingleBuildableProvider lastPlayerIncompleteBuildingStartedProvider 
                = tutorialArgs.TutorialProvider.CreateLastIncompleteBuildingStartedProvider(tutorialArgs.PlayerCruiser);

            IConstructBuildingStepsFactory constructBuildingStepsFactory
                = new ConstructBuildingStepsFactory(
                    argsFactory,
                    tutorialArgs.LeftPanelComponents,
                    tutorialArgs.TutorialProvider,
                    tutorialArgs.PlayerCruiser,
                    lastPlayerIncompleteBuildingStartedProvider);

            YourCruiserStepsFactory
                = new YourCruiserStepsFactory(
                    argsFactory,
                    tutorialArgs.PlayerCruiser,
                    cameraAdjustmentWaitStepFactory,
                    explanationDismissableStepFactory);

            NavigationWheelStepsFactory
                = new NavigationWheelStepsFactory(
                    argsFactory,
                    featurePermitterStepFactory,
                    tutorialArgs.TutorialProvider.NavigationWheelPermitter,
                    explanationDismissableStepFactory,
                    tutorialArgs.CameraComponents);

            EnemyCruiserStepsFactory
                = new EnemyCruiserStepsFactory(
                    argsFactory,
                    tutorialArgs.AICruiser,
                    autoNavigationStepFactory,
                    explanationDismissableStepFactory);

            PlayerCruiserWidgetsStepsFactory
                = new PlayerCruiserWidgetsStepsFactory(
                    argsFactory,
                    tutorialArgs.LeftPanelComponents,
                    autoNavigationStepFactory,
                    explanationDismissableStepFactory);

            ConstructDroneStationStepsFactory
                = new ConstructDroneStationStespFactory(
                    argsFactory,
                    constructBuildingStepsFactory,
                    explanationDismissableStepFactory);

            IChangeCruiserBuildSpeedStepFactory changeCruiserBuildSpeedStepFactory
                = new ChangeCruiserBuildSpeedStepFactory(argsFactory);
            ICreateProducingFactoryStepsFactory createProducingFactoryStepsFactory
                = new CreateProducingFactoryStepsFactory(
                    argsFactory,
                    changeCruiserBuildSpeedStepFactory,
                    tutorialArgs.TutorialProvider,
                    tutorialArgs.PrefabFactory,
                    tutorialArgs.AICruiser);
            EnemyUnitArgs enemyUnitArgs
                = new EnemyUnitArgs(
                    createProducingFactoryStepsFactory,
                    autoNavigationStepFactory,
                    explanationDismissableStepFactory,
                    constructBuildingStepsFactory,
                    changeCruiserBuildSpeedStepFactory,
                    tutorialArgs.TutorialProvider);

            EnemyBomberStepsFactory
                = new EnemyBomberStepsFactory(
                    argsFactory,
                    enemyUnitArgs,
                    tutorialArgs.AICruiser,
                    deferrer,
                    tutorialArgs.TutorialProvider.SingleAircraftProvider);

            EnemyShipStepsFactory
                = new EnemyShipStepsFactory(
                    argsFactory,
                    enemyUnitArgs,
                    tutorialArgs.TutorialProvider.SingleShipProvider);

            DroneFocusStepsFactory
                = new DroneFocusStepsFactory(
                    argsFactory,
                    autoNavigationStepFactory,
                    explanationDismissableStepFactory,
                    changeCruiserBuildSpeedStepFactory,
                    constructBuildingStepsFactory,
                    tutorialArgs.TutorialProvider,
                    lastPlayerIncompleteBuildingStartedProvider,
                    tutorialArgs.RightPanelComponents);

            GameSpeedStepsFactory
                = new GameSpeedStepsFactory(
                    argsFactory,
                    explanationDismissableStepFactory,
                    featurePermitterStepFactory,
                    tutorialArgs.TutorialProvider.SpeedButtonsPermitter,
                    tutorialArgs.TutorialProvider.NavigationWheelPermitter,
                    tutorialArgs.TutorialProvider.ScrollWheelPermitter,
                    tutorialArgs.RightPanelComponents,
                    tutorialArgs.UIManager);

            EndgameStepsFactory
                = new EndgameStepsFactory(
                    argsFactory,
                    changeCruiserBuildSpeedStepFactory,
                    autoNavigationStepFactory,
                    tutorialArgs.TutorialProvider,
                    tutorialArgs.PlayerCruiser,
                    tutorialArgs.AICruiser);
        }
    }
}