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
        public ITutorialStepsFactory MainMenuStepsFactory { get; }
        public ITutorialStepsFactory NavigationButtonsStepsFactory { get; }
        public ITutorialStepsFactory ScrollWheelStepsFactory { get; }
        public ITutorialStepsFactory TouchSwipeStepsFactory { get; }
        public ITutorialStepsFactory MousePanStepsFactory { get; }
        public ITutorialStepsFactory PinchZoomStepsFactory { get; }
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
            ITutorialStepFactory cameraAdjustmentWaitStepFactory = new CameraAdjustmentWaitStepFactory(argsFactory, tutorialArgs.TutorialStrings, tutorialArgs.CameraComponents);
            IExplanationDismissableStepFactory explanationDismissableStepFactory 
                = new ExplanationDismissableStepFactory(argsFactory, tutorialArgs.TutorialStrings, explanationPanel.OkButton, explanationPanel.DoneButton);
            IFeaturePermitterStepFactory featurePermitterStepFactory = new FeaturePermitterStepFactory(argsFactory, tutorialArgs.TutorialStrings);
            IAutoNavigationStepFactory autoNavigationStepFactory 
                = new AutoNavigationStepFactory(argsFactory, tutorialArgs.TutorialStrings, cameraAdjustmentWaitStepFactory, tutorialArgs.CameraComponents);
            ISingleBuildableProvider lastPlayerIncompleteBuildingStartedProvider 
                = tutorialArgs.TutorialProvider.CreateLastIncompleteBuildingStartedProvider(tutorialArgs.PlayerCruiser);
            ISlidingPanelWaitStepFactory slidingPanelWaitStepFactory
                = new SlidingPanelWaitStepFactory(
                    argsFactory,
                    tutorialArgs.TutorialStrings,
                    tutorialArgs.LeftPanelComponents.BuildMenu.SelectorPanel,
                    tutorialArgs.RightPanelComponents.InformatorPanel);

            IConstructBuildingStepsFactory constructBuildingStepsFactory
                = new ConstructBuildingStepsFactory(
                    argsFactory,
                    tutorialArgs.TutorialStrings,
                    tutorialArgs.LeftPanelComponents,
                    tutorialArgs.TutorialProvider,
                    tutorialArgs.PlayerCruiser,
                    lastPlayerIncompleteBuildingStartedProvider,
                    slidingPanelWaitStepFactory);

            YourCruiserStepsFactory
                = new YourCruiserStepsFactory(
                    argsFactory,
                    tutorialArgs.TutorialStrings,
                    tutorialArgs.PlayerCruiser,
                    cameraAdjustmentWaitStepFactory,
                    explanationDismissableStepFactory,
                    featurePermitterStepFactory,
                    tutorialArgs.TutorialProvider.NavigationPermitters.NavigationFilter);

            MainMenuStepsFactory
                = new MainMenuStepsFactory(
                    argsFactory,
                    tutorialArgs.TutorialStrings,
                    tutorialArgs.ModalMainMenuButton,
                    tutorialArgs.RightPanelComponents.MainMenu);

            NavigationButtonsStepsFactory
                = new NavigationButtonsStepsFactory(
                    argsFactory,
                    tutorialArgs.TutorialStrings,
                    featurePermitterStepFactory,
                    tutorialArgs.TutorialProvider.NavigationPermitters.NavigationButtonsFilter,
                    tutorialArgs.TutorialProvider.NavigationPermitters.HotkeyFilter,
                    explanationDismissableStepFactory,
                    tutorialArgs.CameraComponents);

            ScrollWheelStepsFactory
                = new ScrollWheelStepsFactory(
                    argsFactory,
                    tutorialArgs.TutorialStrings,
                    featurePermitterStepFactory,
                    tutorialArgs.TutorialProvider.NavigationPermitters.ScrollWheelAndPinchZoomFilter,
                    explanationDismissableStepFactory);

            TouchSwipeStepsFactory
                = new TouchSwipeStepsFactory(
                    argsFactory,
                    tutorialArgs.TutorialStrings,
                    featurePermitterStepFactory,
                    tutorialArgs.TutorialProvider.NavigationPermitters.SwipeFilter,
                    explanationDismissableStepFactory);

            MousePanStepsFactory
                = new MousePanStepsFactory(
                    argsFactory,
                    tutorialArgs.TutorialStrings,
                    featurePermitterStepFactory,
                    tutorialArgs.TutorialProvider.NavigationPermitters.SwipeFilter,
                    tutorialArgs.TutorialProvider.NavigationPermitters.ScrollWheelAndPinchZoomFilter,
                    explanationDismissableStepFactory);

            PinchZoomStepsFactory
                = new PinchZoomStepsFactory(
                    argsFactory,
                    tutorialArgs.TutorialStrings,
                    featurePermitterStepFactory,
                    tutorialArgs.TutorialProvider.NavigationPermitters.ScrollWheelAndPinchZoomFilter,
                    tutorialArgs.TutorialProvider.NavigationPermitters.SwipeFilter,
                    explanationDismissableStepFactory);

            EnemyCruiserStepsFactory
                = new EnemyCruiserStepsFactory(
                    argsFactory,
                    tutorialArgs.TutorialStrings,
                    tutorialArgs.AICruiser,
                    autoNavigationStepFactory,
                    explanationDismissableStepFactory);

            PlayerCruiserWidgetsStepsFactory
                = new PlayerCruiserWidgetsStepsFactory(
                    argsFactory,
                    tutorialArgs.TutorialStrings,
                    tutorialArgs.TopPanelComponents.PlayerCruiserHealthBar,
                    tutorialArgs.LeftPanelComponents.NumberOfDronesHighlightable,
                    autoNavigationStepFactory,
                    explanationDismissableStepFactory);

            ConstructDroneStationStepsFactory
                = new ConstructDroneStationStepsFactory(
                    argsFactory,
                    tutorialArgs.TutorialStrings,
                    constructBuildingStepsFactory,
                    explanationDismissableStepFactory,
                    tutorialArgs.PrefabFactory);

            IChangeCruiserBuildSpeedStepFactory changeCruiserBuildSpeedStepFactory = new ChangeCruiserBuildSpeedStepFactory(argsFactory, tutorialArgs.TutorialStrings);
            ICreateProducingFactoryStepsFactory createProducingFactoryStepsFactory
                = new CreateProducingFactoryStepsFactory(
                    argsFactory,
                    tutorialArgs.TutorialStrings,
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
                    tutorialArgs.TutorialStrings,
                    enemyUnitArgs,
                    tutorialArgs.AICruiser,
                    deferrer,
                    tutorialArgs.TutorialProvider.SingleAircraftProvider);

            EnemyShipStepsFactory
                = new EnemyShipStepsFactory(
                    argsFactory,
                    tutorialArgs.TutorialStrings,
                    enemyUnitArgs,
                    tutorialArgs.TutorialProvider.SingleShipProvider);

            DroneFocusStepsFactory
                = new DroneFocusStepsFactory(
                    argsFactory,
                    tutorialArgs.TutorialStrings,
                    autoNavigationStepFactory,
                    explanationDismissableStepFactory,
                    changeCruiserBuildSpeedStepFactory,
                    constructBuildingStepsFactory,
                    tutorialArgs.TutorialProvider,
                    lastPlayerIncompleteBuildingStartedProvider,
                    tutorialArgs.RightPanelComponents,
                    slidingPanelWaitStepFactory,
                    tutorialArgs.PrefabFactory,
                    tutorialArgs.CommonStrings);

            GameSpeedStepsFactory
                = new GameSpeedStepsFactory(
                    argsFactory,
                    tutorialArgs.TutorialStrings,
                    explanationDismissableStepFactory,
                    featurePermitterStepFactory,
                    tutorialArgs.TutorialProvider.SpeedButtonsPermitter,
                    tutorialArgs.TutorialProvider.NavigationPermitters.NavigationFilter,
                    tutorialArgs.RightPanelComponents,
                    tutorialArgs.UIManager);

            EndgameStepsFactory
                = new EndgameStepsFactory(
                    argsFactory,
                    tutorialArgs.TutorialStrings,
                    changeCruiserBuildSpeedStepFactory,
                    autoNavigationStepFactory,
                    tutorialArgs.TutorialProvider,
                    tutorialArgs.PlayerCruiser,
                    tutorialArgs.AICruiser,
                    tutorialArgs.PrefabFactory);
        }
    }
}