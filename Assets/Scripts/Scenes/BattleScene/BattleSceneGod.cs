using BattleCruisers.AI;
using BattleCruisers.Buildables.Colours;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Data;
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
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene;
using BattleCruisers.Utils.BattleScene.Lifetime;
using BattleCruisers.Utils.Debugging;
using BattleCruisers.Utils.Factories;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Fetchers.Cache;
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
        private CruiserDeathManager _cruiserDeathManager;
        private LifetimeManager _lifetimeManager;
        private InformatorDismisser _informatorDismisser;

        private const int CRUISER_OFFSET_IN_M = 35;

        public int DEFAULT_LEVEL = 1;
        public CameraInitialiser cameraInitialiser;
        public TopPanelInitialiser topPanelInitialiser;
        public LeftPanelInitialiser leftPanelInitialiser;
        public RightPanelInitialiser rightPanelInitialiser;
        public TutorialManager tutorialManager;
        private async void Start()
        {
            Assert.raiseExceptions = true;
            Helper.AssertIsNotNull(cameraInitialiser, topPanelInitialiser, leftPanelInitialiser, rightPanelInitialiser, tutorialManager);

            BattleSceneGodComponents components = GetComponent<BattleSceneGodComponents>();
            Assert.IsNotNull(components);
            components.Initialise();
            components.UpdaterProvider.SwitchableUpdater.Enabled = false;

            ISceneNavigator sceneNavigator = LandingSceneGod.SceneNavigator;
            IApplicationModel applicationModel = ApplicationModelProvider.ApplicationModel;
            
            // TEMP  Only because I'm starting the the scene without a previous Choose Level Scene
            if (applicationModel.SelectedLevel == -1)
            {
                // TEMP  Force level I'm currently testing :)
                applicationModel.SelectedLevel = DEFAULT_LEVEL;

                sceneNavigator = Substitute.For<ISceneNavigator>();
            }


            //// TEMP  Force  tutorial
            //applicationModel.IsTutorial = true;
            //applicationModel.SelectedLevel = 1;

            IDataProvider dataProvider = applicationModel.DataProvider;
            IBattleCompletionHandler battleCompletionHandler = new BattleCompletionHandler(applicationModel, sceneNavigator);

            // Common setup
            IPrefabCacheFactory prefabCacheFactory = new PrefabCacheFactory();
            IPrefabCache prefabCache = await prefabCacheFactory.CreatePrefabCacheAsync(new PrefabFetcher());
            IPrefabFactory prefabFactory = new PrefabFactory(prefabCache);
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
            ICameraComponents cameraComponents
                = cameraInitialiser.Initialise(
                    components.Camera,
                    dataProvider.SettingsManager,
                    playerCruiser,
                    aiCruiser,
                    navigationPermitters,
                    components.UpdaterProvider.SwitchableUpdater);
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

            TopPanelComponents topPanelComponents = topPanelInitialiser.Initialise(playerCruiser, aiCruiser, buttonVisibilityFilters.HelpLabelsVisibilityFilter);
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
            _lifetimeManager = new LifetimeManager(components.LifetimeEvents, rightPanelComponents.MainMenuManager);

            IItemDetailsManager itemDetailsManager = new ItemDetailsManager(rightPanelComponents.InformatorPanel);
            _userTargetTracker = new UserTargetTracker(itemDetailsManager.SelectedItem, playerCruiserUserChosenTargetManager, new UserTargetsColourChanger());
            _buildableButtonColourController = new BuildableButtonColourController(itemDetailsManager.SelectedItem, leftPanelComponents.BuildMenu.BuildableButtons);

            ManagerArgs args
                = new ManagerArgs(
                    playerCruiser,
                    aiCruiser,
                    leftPanelComponents.BuildMenu,
                    itemDetailsManager,
                    factoryProvider.Sound.PrioritisedSoundPlayer);
            helper.InitialiseUIManager(args);

            _informatorDismisser = new InformatorDismisser(components.BackgroundClickableEmitter, uiManager);

            // Other
            _cruiserDeathManager = new CruiserDeathManager(playerCruiser, aiCruiser);
            ILevel currentLevel = applicationModel.DataProvider.GetLevel(applicationModel.SelectedLevel);
            IArtificialIntelligence ai = helper.CreateAI(aiCruiser, playerCruiser, applicationModel.SelectedLevel);
            components.CloudInitialiser.Initialise(currentLevel.SkyMaterialName, components.UpdaterProvider.SlowerUpdater);
            await components.SkyboxInitialiser.InitialiseAsync(cameraComponents.Skybox, currentLevel);
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
                        cameraComponents.CruiserDeathCameraFocuser,
                        navigationPermitters.NavigationFilter,
                        time,
                        uiManager));
            Cheater cheater = GetComponentInChildren<Cheater>();
            Assert.IsNotNull(cheater);
            cheater.Initialise(factoryProvider);

            // Audio
            ILayeredMusicPlayer layeredMusicPlayer
                = await components.MusicPlayerInitialiser.CreatePlayerAsync(
                    factoryProvider.Sound.SoundFetcher,
                    currentLevel.MusicKeys);
            LandingSceneGod.MusicPlayer?.Stop();
            _audioInitialiser
                = new AudioInitialiser(
                    helper,
                    layeredMusicPlayer,
                    playerCruiser,
                    aiCruiser,
                    components.Deferrer,
                    time,
                    battleCompletionHandler);

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

            // Do not enable updates until asynchronous loading is complete.
            components.UpdaterProvider.SwitchableUpdater.Enabled = true;

            sceneNavigator.SceneLoaded(SceneNames.BATTLE_SCENE);
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

                tutorialManager.Initialise(tutorialArgs);
                tutorialManager.StartTutorial();
            }
            else
            {
                Destroy(tutorialManager.gameObject);
            }
        }
    }
}