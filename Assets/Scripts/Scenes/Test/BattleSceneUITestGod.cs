using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Settings;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.BattleScene.BuildMenus;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.GameSpeed;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras;
using BattleCruisers.UI.Cameras.Adjusters;
using BattleCruisers.UI.Cameras.Helpers;
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

        public float smoothTime;
        public BuildMenuControllerNEW buildMenuController;
        public ModalMenuController modalMenu;

        private void Start()
        {
            // FELIX  Extract GetComponents() to separate method?
            IVariableDelayDeferrer variableDelayDeferrer = GetComponent<IVariableDelayDeferrer>();

            Helper.AssertIsNotNull(buildMenuController, modalMenu, variableDelayDeferrer);

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
            ISpriteProvider spriteProvider = new SpriteProvider(new SpriteFetcher());
            IBattleSceneHelper helper = CreateHelper(prefabFactory, variableDelayDeferrer);
            ITime time = new TimeBC();
            _pauseGameManager = new PauseGameManager(time);
            _battleCompletionHandler = new BattleCompletionHandler(_applicationModel, _sceneNavigator);
            modalMenu.Initialise(_applicationModel.IsTutorial);

            // Instantiate player cruiser
            ILoadout playerLoadout = helper.GetPlayerLoadout();

            // FELIX  Rename to Setup() :)
            InitialiseSpeedPanel();
            SetupNavigationWheel();
            InitialiseBuildMenuController(playerLoadout, prefabFactory, spriteProvider);
            SetupMainMenuButton();
        }

        private static void InitialiseSpeedPanel()
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

        private void InitialiseBuildMenuController(
            ILoadout playerLoadout,
            IPrefabFactory prefabFactory,
            ISpriteProvider spriteProvider)
        {
            // FELIX  Create functional UIManager :P
            IUIManager uiManager = new UIManagerNEW(buildMenuController);

            IBuildingGroupFactory buildingGroupFactory = new BuildingGroupFactory();
            IPrefabOrganiser prefabOrganiser = new PrefabOrganiser(playerLoadout, prefabFactory, buildingGroupFactory);
            IList<IBuildingGroup> buildingGroups = prefabOrganiser.GetBuildingGroups();
            IDictionary<UnitCategory, IList<IBuildableWrapper<IUnit>>> units = prefabOrganiser.GetUnits();
            IBuildableSorterFactory sorterFactory = new BuildableSorterFactory();
            // FELIX Pass real implementation :P
            IButtonVisibilityFilters buttonVisibilityFilters = new StaticButtonVisibilityFilters(isMatch: true);
            // FELIX  Pass real class, or remove use :P
            IPlayerCruiserFocusHelper playerCruiserFocusHelper = Substitute.For<IPlayerCruiserFocusHelper>();
            IPrioritisedSoundPlayer soundPlayer = Substitute.For<IPrioritisedSoundPlayer>();

            buildMenuController
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

        // NEWUI  Move to CameraController?
        private void Update()
        {
            _cameraAdjuster.AdjustCamera();
        }
    }
}