using BattleCruisers.AI;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Data;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.BattleScene.BuildMenus;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Cameras;
using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.UI.Music;
using BattleCruisers.Utils.BattleScene;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions;
using BattleCruisers.Utils.Threading;
// FELIX  Replace all Substitutes :D  (Don't remove this comment until "using NSubstitute;" is removed :)
using NSubstitute;
using UnityEngine;
using UnityEngine.Assertions;

// === Tag keys :D ===
// FELIX    => Code todo
// TEMP     => Temporary for testing
// IPAD     => Update for IPad (usualy input related)
// PERF     => Potential performance hit
// NEWUI    => Should be resolved once legacy UI is replaced :)
namespace BattleCruisers.Scenes.BattleScene
{
    public class BattleSceneGodNEW : MonoBehaviour
    {
        private AudioInitialiser _audioInitialiser;
        private UserChosenTargetHighligher _userChosenTargetHighligher;
        private IArtificialIntelligence _ai;
        private CruiserDestroyedMonitor _cruiserDestroyedMonitor;

        public float smoothTime;

        private const int CRUISER_OFFSET_IN_M = 35;

        private void Start()
        {
            Assert.raiseExceptions = true;

            // TEMP  Only while we have both UIs (legacy and new :) )
            ApplicationModelProvider.IsNewUI = true;

            BattleSceneGodComponents components = GetComponent<BattleSceneGodComponents>();
            Assert.IsNotNull(components);
            components.Initialise();

            ISceneNavigator sceneNavigator = LandingSceneGod.SceneNavigator;
            IMusicPlayer musicPlayer = LandingSceneGod.MusicPlayer;
            IApplicationModel applicationModel = ApplicationModelProvider.ApplicationModel;
            
            // TEMP  Only because I'm starting the the scene without a previous Choose Level Scene
            if (applicationModel.SelectedLevel == -1)
            {
                // TEMP  Force level I'm currently testing :)
                applicationModel.SelectedLevel = 1;

                sceneNavigator = Substitute.For<ISceneNavigator>();
                musicPlayer = Substitute.For<IMusicPlayer>();
            }

            IDataProvider dataProvider = applicationModel.DataProvider;
            IBattleCompletionHandler battleCompletionHandler = new BattleCompletionHandler(applicationModel, sceneNavigator);

            LeftPanelInitialiser leftPanelInitialiser = FindObjectOfType<LeftPanelInitialiser>();
            Assert.IsNotNull(leftPanelInitialiser);

            RightPanelInitialiser rightPanelInitialiser = FindObjectOfType<RightPanelInitialiser>();
            Assert.IsNotNull(rightPanelInitialiser);

            // Common setup
            IPrefabFactory prefabFactory = new PrefabFactory(new PrefabFetcher());
            ISpriteProvider spriteProvider = new SpriteProvider(new SpriteFetcher());
            IBattleSceneHelper helper = CreateHelper(dataProvider, prefabFactory, components.VariableDelayDeferrer);
            IUserChosenTargetManager playerCruiserUserChosenTargetManager = new UserChosenTargetManager();
            IUserChosenTargetManager aiCruiserUserChosenTargetManager = new DummyUserChosenTargetManager();
            ITime time = new TimeBC();
            IPauseGameManager pauseGameManager = new PauseGameManager(time);

            // FELIX  Abstract camera related functionality (currently camera moving
            // in LeftPanelInitialiser.Update() :P)
            Camera platformCamera = FindObjectOfType<Camera>();
            Assert.IsNotNull(platformCamera);
            ICamera camera = new CameraBC(platformCamera);

            ICameraController cameraController = Substitute.For<ICameraController>();
            UIManagerNEW uiManager = new UIManagerNEW();

            // Create cruisers
            ICruiserFactoryNEW cruiserFactory
                = new CruiserFactoryNEW(
                    prefabFactory,
                    components,
                    spriteProvider,
                    camera,
                    helper,
                    applicationModel,
                    cameraController,
                    uiManager,
                    playerCruiserUserChosenTargetManager);

            ICruiser playerCruiser = cruiserFactory.CreatePlayerCruiser();
            ICruiser aiCruiser = cruiserFactory.CreateAICruiser();

            // Initialise player cruiser
            cruiserFactory.InitialisePlayerCruiser(playerCruiser, aiCruiser);

            // Initialise AI cruiser
            IUserChosenTargetHelper userChosenTargetHelper
                = new UserChosenTargetHelper(
                    playerCruiserUserChosenTargetManager,
                    playerCruiser.FactoryProvider.Sound.PrioritisedSoundPlayer);
            cruiserFactory
                .InitialiseAICruiser(
                    playerCruiser,
                    aiCruiser,
                    userChosenTargetHelper);

            // UI
            IButtonVisibilityFilters buttonVisibilityFilters = helper.CreateButtonVisibilityFilters(playerCruiser.DroneManager);

            leftPanelInitialiser
                .Initialise(
                    playerCruiser.DroneManager,
                    new DroneManagerMonitor(playerCruiser.DroneManager, components.VariableDelayDeferrer),
                    camera,
                    dataProvider.SettingsManager,
                    smoothTime,
                    uiManager,
                    helper.GetPlayerLoadout(),
                    prefabFactory,
                    spriteProvider,
                    buttonVisibilityFilters,
                    Substitute.For<IPlayerCruiserFocusHelper>(),
                    helper.GetBuildableButtonSoundPlayer(playerCruiser));

            rightPanelInitialiser
                .Initialise(
                    applicationModel,
                    sceneNavigator,
                    uiManager,
                    playerCruiser,
                    userChosenTargetHelper,
                    buttonVisibilityFilters,
                    pauseGameManager);

            uiManager
                .Initialise(
                    leftPanelInitialiser.BuildMenu,
                    new ItemDetailsManager(rightPanelInitialiser.Informator),
                    playerCruiser,
                    aiCruiser);

            // User chosen target highlighter
            IHighlightHelper highlightHelper = new HighlightHelper(components.HighlightFactory);
            _userChosenTargetHighligher = new UserChosenTargetHighligher(playerCruiserUserChosenTargetManager, highlightHelper);

            // Audio
            _audioInitialiser
                = new AudioInitialiser(
                    helper,
                    musicPlayer,
                    playerCruiser,
                    aiCruiser,
                    components.VariableDelayDeferrer,
                    time);

            // Other
            _ai = helper.CreateAI(aiCruiser, playerCruiser, applicationModel.SelectedLevel);
            ILevel currentLevel = applicationModel.DataProvider.GetLevel(applicationModel.SelectedLevel);
            components.CloudInitialiser.Initialise(currentLevel);
            _cruiserDestroyedMonitor = new CruiserDestroyedMonitor(playerCruiser, aiCruiser, battleCompletionHandler, pauseGameManager);
        }

        private IBattleSceneHelper CreateHelper(IDataProvider dataProvider, IPrefabFactory prefabFactory, IVariableDelayDeferrer variableDelayDeferrer)
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
                return new NormalHelper(dataProvider, prefabFactory, variableDelayDeferrer);
            }
        }
    }
}