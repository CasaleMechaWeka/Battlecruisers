using BattleCruisers.AI;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Data;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using BattleCruisers.Tutorial;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.UI.BattleScene;
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
namespace BattleCruisers.Scenes.BattleScene
{
    public class BattleSceneGod : MonoBehaviour
    {
        private AudioInitialiser _audioInitialiser;
        private UserChosenTargetHighligher _userChosenTargetHighligher;
        private IArtificialIntelligence _ai;
        private CruiserDestroyedMonitor _cruiserDestroyedMonitor;
        private ITutorialProvider _tutorialProvider;

        private const int CRUISER_OFFSET_IN_M = 35;

        private void Start()
        {
            Assert.raiseExceptions = true;

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

            // TEMP  Force  tutorial
            //applicationModel.IsTutorial = true;

            IDataProvider dataProvider = applicationModel.DataProvider;
            IBattleCompletionHandler battleCompletionHandler = new BattleCompletionHandler(applicationModel, sceneNavigator);

            // Common setup
            IPrefabFactory prefabFactory = new PrefabFactory(new PrefabFetcher());
            ISpriteProvider spriteProvider = new SpriteProvider(new SpriteFetcher());
            IBattleSceneHelper helper = CreateHelper(applicationModel, prefabFactory, components.VariableDelayDeferrer);
            IUserChosenTargetManager playerCruiserUserChosenTargetManager = new UserChosenTargetManager();
            IUserChosenTargetManager aiCruiserUserChosenTargetManager = new DummyUserChosenTargetManager();
            ITime time = new TimeBC();
            IPauseGameManager pauseGameManager = new PauseGameManager(time);
            IUIManager uiManager = helper.CreateUIManager();

            // Create cruisers
            ICruiserFactory cruiserFactory
                = new CruiserFactory(
                    prefabFactory,
                    components,
                    spriteProvider,
                    components.Camera,
                    helper,
                    applicationModel,
                    uiManager,
                    playerCruiserUserChosenTargetManager);

            Cruiser playerCruiser = cruiserFactory.CreatePlayerCruiser();
            Cruiser aiCruiser = cruiserFactory.CreateAICruiser();

            // Camera
            CameraInitialiserNEW cameraInitialiser = FindObjectOfType<CameraInitialiserNEW>();
            Assert.IsNotNull(cameraInitialiser);

            ICameraComponents cameraComponents
                = cameraInitialiser.Initialise(
                    components.Camera,
                    dataProvider.SettingsManager,
                    playerCruiser,
                    aiCruiser,
                    helper.CreateNavigationWheelEnabledFilter());
            cameraComponents.CameraFocuser.FocusOnPlayerCruiser();

            // Initialise player cruiser
            cruiserFactory.InitialisePlayerCruiser(playerCruiser, aiCruiser, cameraComponents.CameraFocuser);

            // Initialise AI cruiser
            IUserChosenTargetHelper userChosenTargetHelper 
                = helper.CreateUserChosenTargetHelper(
                    playerCruiserUserChosenTargetManager,
                    playerCruiser.FactoryProvider.Sound.PrioritisedSoundPlayer);
            cruiserFactory
                .InitialiseAICruiser(
                    playerCruiser,
                    aiCruiser,
                    cameraComponents.CameraFocuser,
                    userChosenTargetHelper);

            // UI
            IButtonVisibilityFilters buttonVisibilityFilters = helper.CreateButtonVisibilityFilters(playerCruiser.DroneManager);

            LeftPanelInitialiser leftPanelInitialiser = FindObjectOfType<LeftPanelInitialiser>();
            Assert.IsNotNull(leftPanelInitialiser);
            LeftPanelComponents leftPanelComponents 
                = leftPanelInitialiser.Initialise(
                    playerCruiser.DroneManager,
                    new DroneManagerMonitor(playerCruiser.DroneManager, components.VariableDelayDeferrer),
                    uiManager,
                    helper.GetPlayerLoadout(),
                    prefabFactory,
                    spriteProvider,
                    buttonVisibilityFilters,
                    new PlayerCruiserFocusHelper(components.Camera, cameraComponents.CameraFocuser, playerCruiser),
                    helper.GetBuildableButtonSoundPlayer(playerCruiser),
                    playerCruiser);

            RightPanelInitialiser rightPanelInitialiser = FindObjectOfType<RightPanelInitialiser>();
            Assert.IsNotNull(rightPanelInitialiser);
            RightPanelComponents rightPanelComponents
                = rightPanelInitialiser.Initialise(
                    applicationModel,
                    sceneNavigator,
                    uiManager,
                    playerCruiser,
                    userChosenTargetHelper,
                    buttonVisibilityFilters,
                    pauseGameManager);

            ManagerArgs args
                = new ManagerArgs(
                    playerCruiser,
                    aiCruiser,
                    leftPanelComponents.BuildMenu,
                    new ItemDetailsManager(rightPanelInitialiser.Informator));
            helper.InitialiseUIManager(args);

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
            components.SkyboxInitialiser.Initialise(cameraComponents.Skybox, currentLevel);
            _cruiserDestroyedMonitor = new CruiserDestroyedMonitor(playerCruiser, aiCruiser, battleCompletionHandler, pauseGameManager);

            StartTutorialIfNecessary(
                prefabFactory, 
                applicationModel, 
                playerCruiser, 
                aiCruiser, 
                components, 
                cameraComponents, 
                leftPanelComponents, 
                rightPanelComponents, 
                uiManager);
        }

        private IBattleSceneHelper CreateHelper(IApplicationModel applicationModel, IPrefabFactory prefabFactory, IVariableDelayDeferrer variableDelayDeferrer)
        {
            if (applicationModel.IsTutorial)
            {
                TutorialHelper helper = new TutorialHelper(applicationModel.DataProvider, prefabFactory);
                _tutorialProvider = helper;
                return helper;
            }
            else
            {
                return new NormalHelper(applicationModel.DataProvider, prefabFactory, variableDelayDeferrer);
            }
        }

        private void StartTutorialIfNecessary(
            IPrefabFactory prefabFactory,
            IApplicationModel applicationModel,
            ICruiser playerCruiser,
            ICruiser aiCruiser,
            IBattleSceneGodComponents battleSceneGodComponents,
            ICameraComponents cameraComponents,
            LeftPanelComponents leftPanelComponents,
            RightPanelComponents rightPanelComponents,
            IUIManager uiManager)
        {
            if (applicationModel.IsTutorial)
            {
                applicationModel.DataProvider.GameModel.LastBattleResult = null;
                applicationModel.DataProvider.GameModel.HasAttemptedTutorial = true;
                applicationModel.DataProvider.SaveGame();

                ITutorialArgs tutorialArgs
                    = new TutorialArgs(
                        playerCruiser,
                        aiCruiser,
                        _tutorialProvider,
                        prefabFactory,
                        battleSceneGodComponents,
                        cameraComponents,
                        leftPanelComponents,
                        rightPanelComponents,
                        uiManager);

                TutorialManager tutorialManager = FindObjectOfType<TutorialManager>();
                Assert.IsNotNull(tutorialManager);
                tutorialManager.Initialise(tutorialArgs);
                tutorialManager.StartTutorial();
            }
        }
    }
}