using BattleCruisers.Tutorial.Explanation;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Tutorial.Steps.Factories.EnemyUnit;
using BattleCruisers.Tutorial.Steps.Providers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using BattleCruisers.Utils.Threading;
using System.Collections.Generic;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class MasterTutorialStepsFactory : ITutorialStepsFactory
    {
        private ITutorialStepsFactory YourCruiserStepsFactory;
        private ITutorialStepsFactory MainMenuStepsFactory;
        private ITutorialStepsFactory NavigationButtonsStepsFactory;
        private ITutorialStepsFactory ScrollWheelStepsFactory;
        private ITutorialStepsFactory TouchSwipeStepsFactory;
        private ITutorialStepsFactory MousePanStepsFactory;
        private ITutorialStepsFactory PinchZoomStepsFactory;
        private ITutorialStepsFactory EnemyCruiserStepsFactory;
        private ITutorialStepsFactory PlayerCruiserWidgetsStepsFactory;
        private ITutorialStepsFactory ConstructDroneStationStepsFactory;
        private ITutorialStepsFactory EnemyBomberStepsFactory;
        private ITutorialStepsFactory EnemyShipStepsFactory;
        private ITutorialStepsFactory DroneFocusStepsFactory;
        private ITutorialStepsFactory GameSpeedStepsFactory;
        private ITutorialStepsFactory EndgameStepsFactory;

        public MasterTutorialStepsFactory(
            Highlighter highlighter,
            ExplanationPanel explanationPanel,
            IDeferrer deferrer,
            ITutorialArgs tutorialArgs)
        {
            Helper.AssertIsNotNull(highlighter, explanationPanel, deferrer, tutorialArgs);
            CreateFactories(highlighter, explanationPanel, deferrer, tutorialArgs);
        }

        public IList<TutorialStep> CreateSteps()
        {
            List<TutorialStep> steps = new List<TutorialStep>();

            // 1. Player cruiser
            steps.AddRange(YourCruiserStepsFactory.CreateSteps());

            // 1.5 Main menu bottn
            steps.AddRange(MainMenuStepsFactory.CreateSteps());

            // 2. Navigation buttons
            steps.AddRange(NavigationButtonsStepsFactory.CreateSteps());

            // 2.5 Scroll wheel
            if (!SystemInfoBC.Instance.IsHandheld)
            {
                steps.AddRange(ScrollWheelStepsFactory.CreateSteps());
                steps.AddRange(MousePanStepsFactory.CreateSteps());
            }
            else
            {
                steps.AddRange(TouchSwipeStepsFactory.CreateSteps());
                steps.AddRange(PinchZoomStepsFactory.CreateSteps());
            }

            // 3. Enemy cruiser
            steps.AddRange(EnemyCruiserStepsFactory.CreateSteps());

            // 4. Player cruiser widgets
            steps.AddRange(PlayerCruiserWidgetsStepsFactory.CreateSteps());

            // 5. Construct drone station
            steps.AddRange(ConstructDroneStationStepsFactory.CreateSteps());

            // 6. Enemy ship
            steps.AddRange(EnemyShipStepsFactory.CreateSteps());

            // 7. Enemy bomber
            steps.AddRange(EnemyBomberStepsFactory.CreateSteps());

            // 8. Drone focus
            steps.AddRange(DroneFocusStepsFactory.CreateSteps());

            // 9. Game speed
            steps.AddRange(GameSpeedStepsFactory.CreateSteps());

            // 10. Endgame
            steps.AddRange(EndgameStepsFactory.CreateSteps());

            return steps;
        }

        void CreateFactories(
            Highlighter highlighter,
            ExplanationPanel explanationPanel,
            IDeferrer deferrer,
            ITutorialArgs tutorialArgs)
        {
            Helper.AssertIsNotNull(highlighter, explanationPanel, deferrer, tutorialArgs);

            TutorialStepArgsFactory argsFactory = new TutorialStepArgsFactory(highlighter, explanationPanel.TextDisplayer);
            CameraAdjustmentWaitStepFactory cameraAdjustmentWaitStepFactory = new CameraAdjustmentWaitStepFactory(argsFactory, tutorialArgs.CameraComponents);
            ExplanationDismissableStepFactory explanationDismissableStepFactory
                = new ExplanationDismissableStepFactory(argsFactory, explanationPanel.OkButton, explanationPanel.DoneButton);
            FeaturePermitterStepFactory featurePermitterStepFactory = new FeaturePermitterStepFactory(argsFactory);
            AutoNavigationStepFactory autoNavigationStepFactory
                = new AutoNavigationStepFactory(argsFactory, cameraAdjustmentWaitStepFactory, tutorialArgs.CameraComponents);
            ISingleBuildableProvider lastPlayerIncompleteBuildingStartedProvider
                = tutorialArgs.TutorialProvider.CreateLastIncompleteBuildingStartedProvider(tutorialArgs.PlayerCruiser);
            SlidingPanelWaitStepFactory slidingPanelWaitStepFactory
                = new SlidingPanelWaitStepFactory(
                    argsFactory,
                    tutorialArgs.LeftPanelComponents.BuildMenu.SelectorPanel,
                    tutorialArgs.RightPanelComponents.InformatorPanel);

            ConstructBuildingStepsFactory constructBuildingStepsFactory
                = new ConstructBuildingStepsFactory(
                    argsFactory,
                    tutorialArgs.LeftPanelComponents,
                    tutorialArgs.TutorialProvider,
                    tutorialArgs.PlayerCruiser,
                    lastPlayerIncompleteBuildingStartedProvider,
                    slidingPanelWaitStepFactory);

            YourCruiserStepsFactory
                    = new YourCruiserStepsFactory(
                        argsFactory,
                        tutorialArgs.PlayerCruiser,
                        cameraAdjustmentWaitStepFactory,
                        explanationDismissableStepFactory,
                        featurePermitterStepFactory,
                        tutorialArgs.TutorialProvider.NavigationPermitters.NavigationFilter);

            MainMenuStepsFactory
                    = new MainMenuStepsFactory(
                        argsFactory,
                        tutorialArgs.ModalMainMenuButton,
                        tutorialArgs.RightPanelComponents.MainMenu);

            NavigationButtonsStepsFactory
                    = new NavigationButtonsStepsFactory(
                        argsFactory,
                        featurePermitterStepFactory,
                        tutorialArgs.TutorialProvider.NavigationPermitters.NavigationButtonsFilter,
                        tutorialArgs.TutorialProvider.NavigationPermitters.HotkeyFilter,
                        explanationDismissableStepFactory,
                        tutorialArgs.CameraComponents);

            ScrollWheelStepsFactory
                    = new ScrollWheelStepsFactory(
                        argsFactory,
                        featurePermitterStepFactory,
                        tutorialArgs.TutorialProvider.NavigationPermitters.ScrollWheelAndPinchZoomFilter,
                        explanationDismissableStepFactory);

            TouchSwipeStepsFactory
                    = new TouchSwipeStepsFactory(
                        argsFactory,
                        featurePermitterStepFactory,
                        tutorialArgs.TutorialProvider.NavigationPermitters.SwipeFilter,
                        explanationDismissableStepFactory);

            MousePanStepsFactory
                    = new MousePanStepsFactory(
                        argsFactory,
                        featurePermitterStepFactory,
                        tutorialArgs.TutorialProvider.NavigationPermitters.SwipeFilter,
                        tutorialArgs.TutorialProvider.NavigationPermitters.ScrollWheelAndPinchZoomFilter,
                        explanationDismissableStepFactory);

            PinchZoomStepsFactory
                    = new PinchZoomStepsFactory(
                        argsFactory,
                        featurePermitterStepFactory,
                        tutorialArgs.TutorialProvider.NavigationPermitters.ScrollWheelAndPinchZoomFilter,
                        tutorialArgs.TutorialProvider.NavigationPermitters.SwipeFilter,
                        explanationDismissableStepFactory);

            EnemyCruiserStepsFactory
                    = new EnemyCruiserStepsFactory(
                        argsFactory,
                        tutorialArgs.AICruiser,
                        autoNavigationStepFactory,
                        explanationDismissableStepFactory);

            PlayerCruiserWidgetsStepsFactory
                    = new PlayerCruiserWidgetsStepsFactory(
                        argsFactory,
                        tutorialArgs.TopPanelComponents.PlayerCruiserHealthBar,
                        tutorialArgs.LeftPanelComponents.NumberOfDronesHighlightable,
                        autoNavigationStepFactory,
                        explanationDismissableStepFactory);

            ConstructDroneStationStepsFactory
                    = new ConstructDroneStationStepsFactory(
                        argsFactory,
                        constructBuildingStepsFactory,
                        explanationDismissableStepFactory);

            ChangeCruiserBuildSpeedStepFactory changeCruiserBuildSpeedStepFactory = new ChangeCruiserBuildSpeedStepFactory(argsFactory);
            CreateProducingFactoryStepsFactory createProducingFactoryStepsFactory
                = new CreateProducingFactoryStepsFactory(
                    argsFactory,
                    changeCruiserBuildSpeedStepFactory,
                    tutorialArgs.TutorialProvider,
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
                        tutorialArgs.RightPanelComponents,
                        slidingPanelWaitStepFactory);

            GameSpeedStepsFactory
                    = new GameSpeedStepsFactory(
                        argsFactory,
                        explanationDismissableStepFactory,
                        featurePermitterStepFactory,
                        tutorialArgs.TutorialProvider.SpeedButtonsPermitter,
                        tutorialArgs.TutorialProvider.NavigationPermitters.NavigationFilter,
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