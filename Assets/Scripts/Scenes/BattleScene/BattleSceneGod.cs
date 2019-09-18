using BattleCruisers.AI;
using BattleCruisers.Buildables.Colours;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Data;
using BattleCruisers.Data.Static;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using BattleCruisers.Tutorial;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras;
using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.UI.Music;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene;
using BattleCruisers.Utils.Factories;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Threading;
using NSubstitute;
using UnityCommon.PlatformAbstractions;
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
        private IGameEndMonitor _gameEndMonitor;
        // Hold references to avoid garbage collection
        private AudioInitialiser _audioInitialiser;
        private ITutorialProvider _tutorialProvider;
        private UserTargetTracker _userTargetTracker;
        private BuildableButtonColourController _buildableButtonColourController;

        private const int CRUISER_OFFSET_IN_M = 35;

        private void Start()
        {
            Assert.raiseExceptions = true;

            BattleSceneGodComponents components = GetComponent<BattleSceneGodComponents>();
            Assert.IsNotNull(components);
            components.Initialise();

            ISceneNavigator sceneNavigator = LandingSceneGod.SceneNavigator;
            LandingSceneGod.MusicPlayer?.Stop();
            IApplicationModel applicationModel = ApplicationModelProvider.ApplicationModel;
            
            // TEMP  Only because I'm starting the the scene without a previous Choose Level Scene
            if (applicationModel.SelectedLevel == -1)
            {
                // TEMP  Force level I'm currently testing :)
                applicationModel.SelectedLevel = 1;

                sceneNavigator = Substitute.For<ISceneNavigator>();
            }

            // TEMP  Force  tutorial
            //applicationModel.IsTutorial = true;

            IDataProvider dataProvider = applicationModel.DataProvider;
            IBattleCompletionHandler battleCompletionHandler = new BattleCompletionHandler(applicationModel, sceneNavigator);

            // Common setup
            IPrefabFactory prefabFactory = new PrefabFactory(new PrefabFetcher());
            ISpriteProvider spriteProvider = new SpriteProvider(new SpriteFetcher());
            NavigationPermitters navigationPermitters = new NavigationPermitters();

            IBattleSceneHelper helper = CreateHelper(applicationModel, prefabFactory, components.Deferrer, navigationPermitters);
            IUserChosenTargetManager playerCruiserUserChosenTargetManager = new UserChosenTargetManager();
            IUserChosenTargetManager aiCruiserUserChosenTargetManager = new DummyUserChosenTargetManager();
            ITime time = TimeBC.Instance;
            IPauseGameManager pauseGameManager = new PauseGameManager(time);
            IUIManager uiManager = helper.CreateUIManager();

            // Create cruisers
            FactoryProvider factoryProvider = new FactoryProvider(components, prefabFactory, spriteProvider);
            factoryProvider.Initialise(uiManager);
            ICruiserFactory cruiserFactory = new CruiserFactory(factoryProvider, helper, applicationModel, uiManager);

            Cruiser playerCruiser = cruiserFactory.CreatePlayerCruiser();
            Cruiser aiCruiser = cruiserFactory.CreateAICruiser();

            // Camera
            CameraInitialiser cameraInitialiser = FindObjectOfType<CameraInitialiser>();
            Assert.IsNotNull(cameraInitialiser);

            ICameraComponents cameraComponents
                = cameraInitialiser.Initialise(
                    components.Camera,
                    dataProvider.SettingsManager,
                    playerCruiser,
                    aiCruiser,
                    navigationPermitters);
            cameraComponents.CameraFocuser.FocusOnPlayerCruiser();

            // Initialise player cruiser
            cruiserFactory.InitialisePlayerCruiser(playerCruiser, aiCruiser, cameraComponents.CameraFocuser, playerCruiserUserChosenTargetManager);

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
                    aiCruiserUserChosenTargetManager,
                    userChosenTargetHelper);

            // UI
            IButtonVisibilityFilters buttonVisibilityFilters = helper.CreateButtonVisibilityFilters(playerCruiser.DroneManager);

            TopPanelInitialiser topPanelInitialiser = FindObjectOfType<TopPanelInitialiser>();
            Assert.IsNotNull(topPanelInitialiser);
            TopPanelComponents topPanelComponents = topPanelInitialiser.Initialise(playerCruiser, aiCruiser, buttonVisibilityFilters.HelpLabelsVisibilityFilter);

            LeftPanelInitialiser leftPanelInitialiser = FindObjectOfType<LeftPanelInitialiser>();
            Assert.IsNotNull(leftPanelInitialiser);
            LeftPanelComponents leftPanelComponents 
                = leftPanelInitialiser.Initialise(
                    playerCruiser.DroneManager,
                    new DroneManagerMonitor(playerCruiser.DroneManager, components.Deferrer),
                    uiManager,
                    helper.GetPlayerLoadout(),
                    prefabFactory,
                    spriteProvider,
                    buttonVisibilityFilters,
                    new PlayerCruiserFocusHelper(components.Camera, cameraComponents.CameraFocuser, playerCruiser),
                    helper.GetBuildableButtonSoundPlayer(playerCruiser),
                    factoryProvider.Sound.SoundPlayer,
                    playerCruiser.PopulationLimitMonitor);

            RightPanelInitialiser rightPanelInitialiser = FindObjectOfType<RightPanelInitialiser>();
            Assert.IsNotNull(rightPanelInitialiser);
            RightPanelComponents rightPanelComponents
                = rightPanelInitialiser.Initialise(
                    applicationModel,
                    uiManager,
                    playerCruiser,
                    userChosenTargetHelper,
                    buttonVisibilityFilters,
                    pauseGameManager,
                    battleCompletionHandler,
                    factoryProvider.Sound.SoundPlayer);

            IItemDetailsManager itemDetailsManager = new ItemDetailsManager(rightPanelComponents.InformatorPanel);
            _userTargetTracker = new UserTargetTracker(itemDetailsManager.SelectedItem, playerCruiserUserChosenTargetManager, new UserTargetsColourChanger());
            _buildableButtonColourController = new BuildableButtonColourController(itemDetailsManager.SelectedItem, leftPanelComponents.BuildMenu.BuildableButtons);

            ManagerArgs args
                = new ManagerArgs(
                    playerCruiser,
                    aiCruiser,
                    leftPanelComponents.BuildMenu,
                    itemDetailsManager);
            helper.InitialiseUIManager(args);

            // Audio
            ILevel currentLevel = applicationModel.DataProvider.GetLevel(applicationModel.SelectedLevel);
            ILayeredMusicPlayer layeredMusicPlayer
                = components.MusicPlayerInitialiser.CreatePlayer(
                    factoryProvider.Sound.SoundFetcher,
                    currentLevel.MusicKeys);
            _audioInitialiser
                = new AudioInitialiser(
                    helper,
                    layeredMusicPlayer,
                    playerCruiser,
                    aiCruiser,
                    components.Deferrer,
                    time,
                    battleCompletionHandler);

            // Other
            IArtificialIntelligence ai = helper.CreateAI(aiCruiser, playerCruiser, applicationModel.SelectedLevel);
            components.CloudInitialiser.Initialise(currentLevel.CloudStats, RandomGenerator.Instance);
            components.SkyboxInitialiser.Initialise(cameraComponents.Skybox, currentLevel);
            _gameEndMonitor 
                = new GameEndMonitor(
                    new CruiserDestroyedMonitor(
                        playerCruiser,
                        aiCruiser),
                    battleCompletionHandler,
                    new GameEndHandler(
                        playerCruiser,
                        aiCruiser,
                        ai,
                        battleCompletionHandler,
                        components.Deferrer,
                        cameraComponents.CameraFocuser,
                        navigationPermitters.NavigationFilter,
                        time));

            StartTutorialIfNecessary(
                prefabFactory, 
                applicationModel, 
                playerCruiser, 
                aiCruiser, 
                components, 
                cameraComponents, 
                topPanelComponents,
                leftPanelComponents, 
                rightPanelComponents, 
                uiManager);
        }

        private IBattleSceneHelper CreateHelper(
            IApplicationModel applicationModel, 
            IPrefabFactory prefabFactory, 
            IDeferrer deferrer,
            NavigationPermitters navigationPermitters)
        {
            if (applicationModel.IsTutorial)
            {
                TutorialHelper helper = new TutorialHelper(applicationModel.DataProvider, prefabFactory, navigationPermitters);
                _tutorialProvider = helper;
                return helper;
            }
            else
            {
                return new NormalHelper(applicationModel.DataProvider, prefabFactory, deferrer);
            }
        }

        private void StartTutorialIfNecessary(
            IPrefabFactory prefabFactory,
            IApplicationModel applicationModel,
            ICruiser playerCruiser,
            ICruiser aiCruiser,
            IBattleSceneGodComponents battleSceneGodComponents,
            ICameraComponents cameraComponents,
            TopPanelComponents topPanelComponents,
            LeftPanelComponents leftPanelComponents,
            RightPanelComponents rightPanelComponents,
            IUIManager uiManager)
        {
            if (applicationModel.IsTutorial)
            {
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
                        topPanelComponents,
                        leftPanelComponents,
                        rightPanelComponents,
                        uiManager,
                        _gameEndMonitor);

                TutorialManager tutorialManager = FindObjectOfType<TutorialManager>();
                Assert.IsNotNull(tutorialManager);
                tutorialManager.Initialise(tutorialArgs);
                tutorialManager.StartTutorial();
            }
        }
    }
}