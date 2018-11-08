using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Settings;
using BattleCruisers.Data.Static;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.BattleScene.BuildMenus;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.Cruisers;
using BattleCruisers.UI.BattleScene.GameSpeed;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras;
using BattleCruisers.UI.Cameras.Adjusters;
using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions;
using BattleCruisers.Utils.Sorting;
using BattleCruisers.Utils.Threading;
using NSubstitute;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test
{
    public class BattleSceneUITestGod : MonoBehaviour
    {
        private ICameraAdjuster _cameraAdjuster;
        private ISceneNavigator _sceneNavigator;
        private IApplicationModel _applicationModel;
        private IDataProvider _dataProvider;
        private IPauseGameManager _pauseGameManager;
        private IBattleCompletionHandler _battleCompletionHandler;

        // Just for test scene, should not be transferred to new BattleSceneGod :)
        private IPrefabFactory _tempPrefabFactory;
        private IUIManager _tempUIManager;
        private ICruiser _tempPlayerCruiser;

        public float smoothTime;
        public BuildMenuControllerNEW buildMenu;
        public ModalMenuController modalMenu;
        public InformatorPanelController informator;

        // NEWUI  Remove this bool :P
        public static bool IsNewUI = true;

        // FELIX  Split up into Left-/Right-PanelController, they initialise?
        private void Start()
        {
            // FELIX  Extract GetComponents() to separate method?
            IVariableDelayDeferrer variableDelayDeferrer = GetComponent<IVariableDelayDeferrer>();

            Helper.AssertIsNotNull(buildMenu, modalMenu, informator, variableDelayDeferrer);

            _sceneNavigator = LandingSceneGod.SceneNavigator;
            _applicationModel = ApplicationModelProvider.ApplicationModel;

            
            // TEMP  Only because I'm starting the the scene without a previous Choose Level Scene
            if (_applicationModel.SelectedLevel == -1)
            {
                // TEMP  Force level I'm currently testing :)
                _applicationModel.SelectedLevel = 1;

                //musicPlayer = Substitute.For<IMusicPlayer>();
                _sceneNavigator = Substitute.For<ISceneNavigator>();
            }


            _dataProvider = _applicationModel.DataProvider;

            // Common setup
            IPrefabFactory prefabFactory = new PrefabFactory(new PrefabFetcher());
            _tempPrefabFactory = prefabFactory;
            ISpriteProvider spriteProvider = new SpriteProvider(new SpriteFetcher());
            IBattleSceneHelper helper = CreateHelper(prefabFactory, variableDelayDeferrer);
            ITime time = new TimeBC();
            _pauseGameManager = new PauseGameManager(time);
            _battleCompletionHandler = new BattleCompletionHandler(_applicationModel, _sceneNavigator);
            modalMenu.Initialise(_applicationModel.IsTutorial);

            ICruiser playerCruiser = Substitute.For<ICruiser>();
            _tempPlayerCruiser = playerCruiser;

            IDroneManager droneManager = Substitute.For<IDroneManager>();
            droneManager.NumOfDrones.Returns(12);
            playerCruiser.DroneManager.Returns(droneManager);


            ICruiser aiCruiser = Substitute.For<ICruiser>();

            // FELIX Pass real implementation :P
            IButtonVisibilityFilters buttonVisibilityFilters = new StaticButtonVisibilityFilters(isMatch: true);

            // Instantiate player cruiser
            ILoadout playerLoadout = helper.GetPlayerLoadout();

            // Informator has circular dependency with UIManager :/
            informator.StaticInitialise();
            IUIManager uiManager = CreateUIManager(playerCruiser, aiCruiser);
            _tempUIManager = uiManager;
            SetupInformator(buttonVisibilityFilters, playerCruiser, uiManager);
            SetupSpeedPanel();
            SetupNavigationWheel();
            SetupBuildMenuController(uiManager, playerLoadout, prefabFactory, spriteProvider, buttonVisibilityFilters);
            SetupMainMenuButton();
            SetupDronesPanel(playerCruiser.DroneManager, new DroneManagerMonitor(playerCruiser.DroneManager, variableDelayDeferrer));
        }

        private static void SetupSpeedPanel()
        {
            SpeedPanelController speedPanelInitialiser = FindObjectOfType<SpeedPanelController>();
            Assert.IsNotNull(speedPanelInitialiser);
            speedPanelInitialiser.Initialise();
        }

        // NEWUI  Inject SettingsManager & Camera :D
        private void SetupNavigationWheel()
        {
            NavigationWheelInitialiser navigationWheelInitialiser = FindObjectOfType<NavigationWheelInitialiser>();
            INavigationWheelPanel navigationWheelPanel = navigationWheelInitialiser.InitialiseNavigationWheel();

            Camera platformCamera = FindObjectOfType<Camera>();
            ICamera camera = new CameraBC(platformCamera);
            ICameraCalculatorSettings settings
                = new CameraCalculatorSettings(
                    Substitute.For<ISettingsManager>(),
                    camera.Aspect);
            ICameraCalculator cameraCalculator = new CameraCalculator(camera, settings);

            ICameraNavigationWheelCalculator cameraNavigationWheelCalculator = new CameraNavigationWheelCalculator(navigationWheelPanel, cameraCalculator, settings.ValidOrthographicSizes);
            ICameraTargetFinder cameraTargetFinder = new NavigationWheelCameraTargetFinder(cameraNavigationWheelCalculator, camera);
            ICameraTargetProvider cameraTargetProvider = new NavigationWheelCameraTargetProvider(navigationWheelPanel.NavigationWheel, cameraTargetFinder);

            // Smooth adjuster
            _cameraAdjuster
                = new SmoothCameraAdjuster(
                    cameraTargetProvider,
                    new SmoothZoomAdjuster(camera, smoothTime),
                    new SmoothPositionAdjuster(camera.Transform, smoothTime));
        }

        private IUIManager CreateUIManager(ICruiser playerCruiser, ICruiser aiCruiser)
        {
            return
                new UIManagerNEW(
                    buildMenu,
                    new BuildableDetailsManager(informator),
                    playerCruiser,
                    aiCruiser);
        }

        private void SetupBuildMenuController(
            IUIManager uiManager,
            ILoadout playerLoadout,
            IPrefabFactory prefabFactory,
            ISpriteProvider spriteProvider,
            IButtonVisibilityFilters buttonVisibilityFilters)
        {

            IBuildingGroupFactory buildingGroupFactory = new BuildingGroupFactory();
            IPrefabOrganiser prefabOrganiser = new PrefabOrganiser(playerLoadout, prefabFactory, buildingGroupFactory);
            IList<IBuildingGroup> buildingGroups = prefabOrganiser.GetBuildingGroups();
            IDictionary<UnitCategory, IList<IBuildableWrapper<IUnit>>> units = prefabOrganiser.GetUnits();
            IBuildableSorterFactory sorterFactory = new BuildableSorterFactory();
            // FELIX  Pass real class, or remove use :P
            IPlayerCruiserFocusHelper playerCruiserFocusHelper = Substitute.For<IPlayerCruiserFocusHelper>();
            IPrioritisedSoundPlayer soundPlayer = Substitute.For<IPrioritisedSoundPlayer>();

            buildMenu
                .Initialise(
                    uiManager,
                    buildingGroups,
                    units,
                    sorterFactory,
                    buttonVisibilityFilters,
                    spriteProvider,
                    playerCruiserFocusHelper,
                    // FELIX  Pass real sound player :)
                    soundPlayer);
                    //helper.GetBuildableButtonSoundPlayer(_playerCruiser));
        }

        private IBattleSceneHelper CreateHelper(IPrefabFactory prefabFactory, IVariableDelayDeferrer variableDelayDeferrer)
        {
            // FELIX  Handle tutorial :)
            //if (ApplicationModel.IsTutorial)
            //{
            //    TutorialHelper helper = new TutorialHelper(_dataProvider, prefabFactory);
            //    _tutorialProvider = helper;
            //    return helper;
            //}
            //else
            {
                return new NormalHelper(_dataProvider, prefabFactory, variableDelayDeferrer);
            }
        }

        private void SetupMainMenuButton()
        {
            IMainMenuManager mainMenuManager = new MainMenuManager(_pauseGameManager, modalMenu, _battleCompletionHandler);

            MainMenuButtonController mainMenuButton = FindObjectOfType<MainMenuButtonController>();
            Assert.IsNotNull(mainMenuButton);
            mainMenuButton.Initialise(mainMenuManager);
        }

        private void SetupInformator(
            IButtonVisibilityFilters buttonVisibilityFilters, 
            ICruiser playerCruiser,
            IUIManager uiManager)
        {
            playerCruiser = Substitute.For<ICruiser>();
            IUserChosenTargetHelper userChosenTargetHelper = Substitute.For<IUserChosenTargetHelper>();

            informator
                .Initialise(
                    uiManager,
                    playerCruiser,
                    userChosenTargetHelper,
                    buttonVisibilityFilters);
        }

        private void SetupDronesPanel(IDroneManager droneManager, IDroneManagerMonitor droneManagerMonitor)
        {
            DronesPanelInitialiser dronesPanelInitialiser = FindObjectOfType<DronesPanelInitialiser>();
            Assert.IsNotNull(dronesPanelInitialiser);
            dronesPanelInitialiser.Initialise(droneManager, droneManagerMonitor);
        }

        // NEWUI  Move to CameraController?
        private void Update()
        {
            _cameraAdjuster.AdjustCamera();
        }

        // To test showing unit buttons
        public void SimulateSelectingPlayerFactory()
        {
            Debug.Log("SimulateSelectingPlayerFactory");

            IFactory factory = Substitute.For<IFactory>();
            factory.UnitCategory.Returns(UnitCategory.Naval);
            factory.ParentCruiser.Returns(_tempPlayerCruiser);
            factory.BuildableState.Returns(BuildableState.Completed);

            _tempUIManager.ShowFactoryUnits(factory);
        }

        // To test cruiser details
        public void SimulateSelectingCruiser()
        {
            Debug.Log("SimulateSelectingCruiser");

            Cruiser cruiser = _tempPrefabFactory.GetCruiserPrefab(StaticPrefabKeys.Hulls.Longbow);
            _tempUIManager.ShowCruiserDetails(cruiser);
        }

        // To test showing unit details
        public void SimulateSelectingUnit()
        {
            Debug.Log("SimulateSelectingUnit");

            IBuildableWrapper<IUnit> unit = _tempPrefabFactory.GetUnitWrapperPrefab(StaticPrefabKeys.Units.ArchonBattleship);
            _tempUIManager.ShowUnitDetails(unit.Buildable);
        }
    }
}