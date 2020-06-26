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
using BattleCruisers.UI.Sound.Wind;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene;
using BattleCruisers.Utils.BattleScene.Lifetime;
using BattleCruisers.Utils.Debugging;
using BattleCruisers.Utils.Factories;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Fetchers.Cache;
using BattleCruisers.Utils.Threading;
using NSubstitute;
using UnityCommon.PlatformAbstractions.Time;
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

        public int defaultLevel = 1;
        public bool isTutorial = false;

        public CameraInitialiser cameraInitialiser;
        public TopPanelInitialiser topPanelInitialiser;
        public LeftPanelInitialiser leftPanelInitialiser;
        public RightPanelInitialiser rightPanelInitialiser;
        public TutorialInitialiser tutorialInitialiser;

        private async void Start()
        {
            Logging.Log(Tags.BATTLE_SCENE, "Start");

            Assert.raiseExceptions = true;
            Helper.AssertIsNotNull(cameraInitialiser, topPanelInitialiser, leftPanelInitialiser, rightPanelInitialiser, tutorialInitialiser);

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
                applicationModel.SelectedLevel = defaultLevel;

                sceneNavigator = Substitute.For<ISceneNavigator>();
            }

            // TEMP  Force  tutorial
            if (isTutorial)
            {
                applicationModel.IsTutorial = true;
                applicationModel.SelectedLevel = 1;
            }

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
            Logging.Log(Tags.BATTLE_SCENE, "Cruiser setup");
            FactoryProvider factoryProvider = new FactoryProvider(components, prefabFactory, spriteProvider);
            factoryProvider.Initialise(uiManager);
            ICruiserFactory cruiserFactory = new CruiserFactory(factoryProvider, helper, applicationModel, uiManager);

            Cruiser playerCruiser = cruiserFactory.CreatePlayerCruiser();
            Cruiser aiCruiser = cruiserFactory.CreateAICruiser();

            // Camera
            ICameraComponents cameraComponents
                = cameraInitialiser.Initialise(
                    dataProvider.SettingsManager,
                    playerCruiser,
                    aiCruiser,
                    navigationPermitters,
                    components.UpdaterProvider.SwitchableUpdater,
                    factoryProvider.Sound.UISoundPlayer);
            cameraComponents.CameraFocuser.FocusOnPlayerCruiser();

            // Initialise player cruiser
            cruiserFactory.InitialisePlayerCruiser(playerCruiser, aiCruiser, cameraComponents.CameraFocuser, playerCruiserUserChosenTargetManager);

            // Initialise AI cruiser
            IUserChosenTargetHelper userChosenTargetHelper 
                = helper.CreateUserChosenTargetHelper(
                    playerCruiserUserChosenTargetManager,
                    playerCruiser.FactoryProvider.Sound.PrioritisedSoundPlayer,
                    components.TargetIndicator);
            cruiserFactory
                .InitialiseAICruiser(
                    playerCruiser,
                    aiCruiser,
                    cameraComponents.CameraFocuser,
                    aiCruiserUserChosenTargetManager,
                    userChosenTargetHelper);

            // UI
            Logging.Log(Tags.BATTLE_SCENE, "UI setup");
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
                    new PlayerCruiserFocusHelper(cameraComponents.MainCamera, cameraComponents.CameraFocuser, playerCruiser),
                    helper.GetBuildableButtonSoundPlayer(playerCruiser),
                    factoryProvider.Sound.UISoundPlayer,
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
                    factoryProvider.Sound.UISoundPlayer,
                    sceneNavigator);
            _lifetimeManager = new LifetimeManager(components.LifetimeEvents, rightPanelComponents.MainMenuManager);

            IItemDetailsManager itemDetailsManager = new ItemDetailsManager(rightPanelComponents.InformatorPanel);
            _userTargetTracker = new UserTargetTracker(itemDetailsManager.SelectedItem, new UserTargetsColourChanger());
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

            // Audio
            Logging.Log(Tags.BATTLE_SCENE, "Audio setup");
            ILevel currentLevel = applicationModel.DataProvider.GetLevel(applicationModel.SelectedLevel);
            ILayeredMusicPlayer layeredMusicPlayer
                = await components.MusicPlayerInitialiser.CreatePlayerAsync(
                    factoryProvider.Sound.SoundFetcher,
                    currentLevel.MusicKeys,
                    dataProvider.SettingsManager.MuteMusic);
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

            IWindManager windManager
                = components.WindInitialiser.Initialise(
                    cameraComponents.MainCamera,
                    cameraComponents.Settings);
            windManager.Play();

            // Other
            Logging.Log(Tags.BATTLE_SCENE, "Other setup");
            _cruiserDeathManager = new CruiserDeathManager(playerCruiser, aiCruiser);
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
                        uiManager,
                        components.TargetIndicator,
                        windManager));

            // Cheater is only there in debug builds
            Cheater cheater = GetComponentInChildren<Cheater>();
            cheater?.Initialise(factoryProvider, playerCruiser, aiCruiser);

            // Tutorial
            ITutorialArgsBase tutorialArgs
                = new TutorialArgsBase(
                    applicationModel, 
                    playerCruiser, 
                    aiCruiser, 
                    _tutorialProvider,
                    prefabFactory,
                    components, 
                    cameraComponents, 
                    topPanelComponents,
                    leftPanelComponents, 
                    rightPanelComponents, 
                    uiManager,
                    _gameEndMonitor);
            tutorialInitialiser.Initialise(tutorialArgs);

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
   }
}