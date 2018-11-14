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

            // FELIX  Move panel initialisers down to whre they get initialised...
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

            ICameraController cameraController = Substitute.For<ICameraController>();
            UIManagerNEW uiManager = new UIManagerNEW();

            // Create cruisers
            ICruiserFactoryNEW cruiserFactory
                = new CruiserFactoryNEW(
                    prefabFactory,
                    components,
                    spriteProvider,
                    components.Camera,
                    helper,
                    applicationModel,
                    cameraController,
                    uiManager,
                    playerCruiserUserChosenTargetManager);

            ICruiser playerCruiser = cruiserFactory.CreatePlayerCruiser();
            ICruiser aiCruiser = cruiserFactory.CreateAICruiser();

            // Not with GetComponent(), because of circular dependency with cruisers.
            

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

            ManagerArgsNEW args
                = new ManagerArgsNEW(
                    playerCruiser,
                    aiCruiser,
                    leftPanelInitialiser.BuildMenu,
                    new ItemDetailsManager(rightPanelInitialiser.Informator));
            uiManager.Initialise(args);

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

            // Camera
            CameraInitialiserNEW cameraInitialiser = FindObjectOfType<CameraInitialiserNEW>();
            Assert.IsNotNull(cameraInitialiser);
            cameraInitialiser.Initialise(components.Camera, dataProvider.SettingsManager, playerCruiser, aiCruiser);

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