using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Static;
using BattleCruisers.Scenes.BattleScene;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Cameras;
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
using TestHelper = BattleCruisers.Scenes.Test.Utilities;

namespace BattleCruisers.Scenes.Test
{
    public class BattleSceneUITestGod : MonoBehaviour
    {
        // Just for test scene, should not be transferred to new BattleSceneGod :)
        private IPrefabFactory _tempPrefabFactory;
        private IUIManager _tempUIManager;
        private ICruiser _tempPlayerCruiser;
        private IDroneManagerMonitor _tempDroneManagerMonitor;

        private void Start()
        {
            Assert.raiseExceptions = true;

            // TEMP  Only while we have both UIs (legacy and new :) )
            ApplicationModelProvider.IsNewUI = true;

            IVariableDelayDeferrer variableDelayDeferrer = GetComponent<IVariableDelayDeferrer>();

            Helper.AssertIsNotNull(variableDelayDeferrer);

            ISceneNavigator sceneNavigator = LandingSceneGod.SceneNavigator;
            IApplicationModel applicationModel = ApplicationModelProvider.ApplicationModel;
            
            // TEMP  Only because I'm starting the the scene without a previous Choose Level Scene
            if (applicationModel.SelectedLevel == -1)
            {
                // TEMP  Force level I'm currently testing :)
                applicationModel.SelectedLevel = 1;

                sceneNavigator = Substitute.For<ISceneNavigator>();
            }

            IDataProvider dataProvider = applicationModel.DataProvider;

            LeftPanelInitialiser leftPanelInitialiser = FindObjectOfType<LeftPanelInitialiser>();
            Assert.IsNotNull(leftPanelInitialiser);

            RightPanelInitialiser rightPanelInitialiser = FindObjectOfType<RightPanelInitialiser>();
            Assert.IsNotNull(rightPanelInitialiser);

            // Common setup
            IPrefabFactory prefabFactory = new PrefabFactory(new PrefabFetcher());
            _tempPrefabFactory = prefabFactory;
            ISpriteProvider spriteProvider = new SpriteProvider(new SpriteFetcher());
            IBattleSceneHelper helper = CreateHelper(dataProvider, prefabFactory, variableDelayDeferrer);


            TestHelper.Helper testHelper = new TestHelper.Helper();
            ICruiser playerCruiser = testHelper.CreateCruiser(facingDirection: Direction.Right, faction: Faction.Blues);
            _tempPlayerCruiser = playerCruiser;

            IDroneManager droneManager = Substitute.For<IDroneManager>();
            droneManager.NumOfDrones.Returns(12);
            playerCruiser.DroneManager.Returns(droneManager);

            ICruiser aiCruiser = testHelper.CreateCruiser(facingDirection: Direction.Left, faction: Faction.Reds);

            IButtonVisibilityFilters buttonVisibilityFilters = new StaticButtonVisibilityFilters(isMatch: true);

            // Instantiate player cruiser
            ILoadout playerLoadout = helper.GetPlayerLoadout();

            UIManagerNEW uiManager = new UIManagerNEW();
            _tempUIManager = uiManager;

            _tempDroneManagerMonitor = Substitute.For<IDroneManagerMonitor>();

            leftPanelInitialiser
                .Initialise(
                    playerCruiser.DroneManager,
                    _tempDroneManagerMonitor,
                    uiManager,
                    playerLoadout,
                    prefabFactory,
                    spriteProvider,
                    buttonVisibilityFilters,
                    Substitute.For<IPlayerCruiserFocusHelper>(),
                    Substitute.For<IPrioritisedSoundPlayer>());

            rightPanelInitialiser
                .Initialise(
                    applicationModel,
                    sceneNavigator,
                    uiManager,
                    playerCruiser,
                    Substitute.For<IUserChosenTargetHelper>(),
                    buttonVisibilityFilters,
                    new PauseGameManager(new TimeBC()));

            ManagerArgsNEW args
                = new ManagerArgsNEW(
                    playerCruiser,
                    aiCruiser,
                    leftPanelInitialiser.BuildMenu,
                    new ItemDetailsManager(rightPanelInitialiser.Informator));
            uiManager.Initialise(args);

            // Camera
            Camera platformCamera = FindObjectOfType<Camera>();
            Assert.IsNotNull(platformCamera);
            ICamera camera = new CameraBC(platformCamera);

            CameraInitialiserNEW cameraInitialiser = FindObjectOfType<CameraInitialiserNEW>();
            Assert.IsNotNull(cameraInitialiser);
            cameraInitialiser.Initialise(camera, dataProvider.SettingsManager, playerCruiser, aiCruiser);
        }

        private IBattleSceneHelper CreateHelper(IDataProvider dataProvider, IPrefabFactory prefabFactory, IVariableDelayDeferrer variableDelayDeferrer)
        {
            {
                return new NormalHelper(dataProvider, prefabFactory, variableDelayDeferrer);
            }
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