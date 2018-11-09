using BattleCruisers.Buildables;
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
using BattleCruisers.UI.BattleScene.GameSpeed;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions;
using BattleCruisers.Utils.Threading;
using NSubstitute;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test
{
    public class BattleSceneUITestGod : MonoBehaviour
    {
        private IApplicationModel _applicationModel;
        private IDataProvider _dataProvider;

        // Just for test scene, should not be transferred to new BattleSceneGod :)
        private IPrefabFactory _tempPrefabFactory;
        private IUIManager _tempUIManager;
        private ICruiser _tempPlayerCruiser;
        private IDroneManagerMonitor _tempDroneManagerMonitor;

        public float smoothTime;

        // NEWUI  Remove this bool :P
        public static bool IsNewUI = true;

        // FELIX  Split up into Left-/Right-PanelController, they initialise?
        private void Start()
        {
            // FELIX  Extract GetComponents() to separate method?
            IVariableDelayDeferrer variableDelayDeferrer = GetComponent<IVariableDelayDeferrer>();

            Helper.AssertIsNotNull(variableDelayDeferrer);

            ISceneNavigator sceneNavigator = LandingSceneGod.SceneNavigator;
            _applicationModel = ApplicationModelProvider.ApplicationModel;

            
            // TEMP  Only because I'm starting the the scene without a previous Choose Level Scene
            if (_applicationModel.SelectedLevel == -1)
            {
                // TEMP  Force level I'm currently testing :)
                _applicationModel.SelectedLevel = 1;

                //musicPlayer = Substitute.For<IMusicPlayer>();
                sceneNavigator = Substitute.For<ISceneNavigator>();
            }


            _dataProvider = _applicationModel.DataProvider;

            LeftPanelInitialiser leftPanelInitialiser = FindObjectOfType<LeftPanelInitialiser>();
            Assert.IsNotNull(leftPanelInitialiser);

            RightPanelInitialiser rightPanelInitialiser = FindObjectOfType<RightPanelInitialiser>();
            Assert.IsNotNull(rightPanelInitialiser);

            // Common setup
            IPrefabFactory prefabFactory = new PrefabFactory(new PrefabFetcher());
            _tempPrefabFactory = prefabFactory;
            ISpriteProvider spriteProvider = new SpriteProvider(new SpriteFetcher());
            IBattleSceneHelper helper = CreateHelper(prefabFactory, variableDelayDeferrer);

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
            IUIManager uiManager = CreateUIManager(playerCruiser, aiCruiser, leftPanelInitialiser.BuildMenu, rightPanelInitialiser.Informator);
            _tempUIManager = uiManager;
            //SetupInformator(buttonVisibilityFilters, playerCruiser, uiManager);
            //SetupSpeedPanel();
            //SetupNavigationWheel();
            //SetupBuildMenuController(uiManager, playerLoadout, prefabFactory, spriteProvider, buttonVisibilityFilters);
            //SetupMainMenuButton();

            _tempDroneManagerMonitor = Substitute.For<IDroneManagerMonitor>();
            //SetupDronesPanel(playerCruiser.DroneManager, _tempDroneManagerMonitor);
            //SetupDronesPanel(playerCruiser.DroneManager, new DroneManagerMonitor(playerCruiser.DroneManager, variableDelayDeferrer));

            Camera platformCamera = FindObjectOfType<Camera>();
            ICamera camera = new CameraBC(platformCamera);

            leftPanelInitialiser
                .Initialise(
                    playerCruiser.DroneManager,
                    _tempDroneManagerMonitor,
                    camera,
                    Substitute.For<ISettingsManager>(),
                    smoothTime,
                    uiManager,
                    playerLoadout,
                    prefabFactory,
                    spriteProvider,
                    buttonVisibilityFilters,
                    Substitute.For<IPlayerCruiserFocusHelper>(),
                    Substitute.For<IPrioritisedSoundPlayer>());

            rightPanelInitialiser
                .Initialise(
                    _applicationModel,
                    sceneNavigator,
                    uiManager,
                    playerCruiser,
                    Substitute.For<IUserChosenTargetHelper>(),
                    buttonVisibilityFilters);
        }

        private static void SetupSpeedPanel()
        {
            SpeedPanelController speedPanelInitialiser = FindObjectOfType<SpeedPanelController>();
            Assert.IsNotNull(speedPanelInitialiser);
            speedPanelInitialiser.Initialise();
        }

        private IUIManager CreateUIManager(ICruiser playerCruiser, ICruiser aiCruiser, IBuildMenuNEW buildMenu, IInformatorPanel informator)
        {
            return
                new UIManagerNEW(
                    buildMenu,
                    new ItemDetailsManager(informator),
                    playerCruiser,
                    aiCruiser);
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

        //private void SetupMainMenuButton()
        //{
        //    IMainMenuManager mainMenuManager = new MainMenuManager(_pauseGameManager, modalMenu, _battleCompletionHandler);

        //    MainMenuButtonController mainMenuButton = FindObjectOfType<MainMenuButtonController>();
        //    Assert.IsNotNull(mainMenuButton);
        //    mainMenuButton.Initialise(mainMenuManager);
        //}

        //private void SetupInformator(
        //    IButtonVisibilityFilters buttonVisibilityFilters, 
        //    ICruiser playerCruiser,
        //    IUIManager uiManager)
        //{
        //    playerCruiser = Substitute.For<ICruiser>();
        //    IUserChosenTargetHelper userChosenTargetHelper = Substitute.For<IUserChosenTargetHelper>();

        //    informator
        //        .Initialise(
        //            uiManager,
        //            playerCruiser,
        //            userChosenTargetHelper,
        //            buttonVisibilityFilters);
        //}

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

        // To test idle drone highlighting
        private bool _areDronesIdle = false;
        public void ToggleIdleDrones()
        {
            if (_areDronesIdle)
            {
                _tempDroneManagerMonitor.IdleDronesEnded += Raise.Event();
            }
            else
            {
                _tempDroneManagerMonitor.IdleDronesStarted += Raise.Event();
            }

            _areDronesIdle = !_areDronesIdle;
        }
    }
}