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
        public ITutorialStepsFactory YourCruiserStepsFactory { get; private set; }
        public ITutorialStepsFactory NavigationWheelStepsFactory { get; private set; }
        public ITutorialStepsFactory EnemyCruiserStepsFactory { get; private set; }
        public ITutorialStepsFactory PlayerCruiserWidgetsStepsFactory { get; private set; }
        public ITutorialStepsFactory ConstructDroneStationStepsFactory { get; private set; }
        public ITutorialStepsFactory EnemyBomberStepsFactory { get; private set; }
        public ITutorialStepsFactory EnemyShipStepsFactory { get; private set; }
        public ITutorialStepsFactory DroneFocusStepsFactory { get; private set; }
        public ITutorialStepsFactory GameSpeedStepsFactory { get; private set; }
        public ITutorialStepsFactory EndgameStepsFactory { get; private set; }

        public TutorialStepsFactoriesProvider(
            IHighlighter highlighter,
            IExplanationPanel explanationPanel,
            IVariableDelayDeferrer deferrer,
            ITutorialArgs tutorialArgs)
        {
            Helper.AssertIsNotNull(highlighter, explanationPanel, deferrer, tutorialArgs);

            ITutorialStepArgsFactory argsFactory = new TutorialStepArgsFactory(highlighter, explanationPanel.TextDisplayer);
            ITutorialStepFactory cameraAdjustmentWaitStepFactory = new CameraAdjustmentWaitStepFactory(argsFactory, tutorialArgs.CameraComponents);
            IExplanationDismissableStepFactory explanationDismissableStepFactory 
                = new ExplanationDismissableStepFactory(argsFactory, explanationPanel.DismissButton);
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
                    tutorialArgs.TutorialProvider.NavigationPermitter,
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
                    tutorialArgs.TutorialProvider.NavigationPermitter,
                    tutorialArgs.RightPanelComponents,
                    tutorialArgs.UIManager);

            EndgameStepsFactory
                = new EndgameStepsFactory(
                    argsFactory,
                    changeCruiserBuildSpeedStepFactory,
                    tutorialArgs.TutorialProvider,
                    tutorialArgs.PlayerCruiser,
                    tutorialArgs.AICruiser);
        }
    }
}