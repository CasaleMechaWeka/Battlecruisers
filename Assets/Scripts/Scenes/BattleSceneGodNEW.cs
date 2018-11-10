using BattleCruisers.AI;
using BattleCruisers.Buildables;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Construction;
using BattleCruisers.Cruisers.Damage;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Cruisers.Helpers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.BattleScene.BuildMenus;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.Clouds;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Cameras;
using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.UI.Music;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using BattleCruisers.Utils.Threading;
// FELIX  Replace all Substitutes :D  (Don't remove this comment until "using NSubstitute;" is removed :)
using NSubstitute;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes
{
    public class BattleSceneGodNEW : MonoBehaviour
    {
        // FELIX  Remove all unused fields :)
        private IBattleCompletionHandler _battleCompletionHandler;
        private IPauseGameManager _pauseGameManager;
        // FELIX  Dispose or suppress warning :/
        private UserChosenTargetHighligher _userChosenTargetHighligher;
        private IArtificialIntelligence _ai;
        private CruiserEventMonitor _cruiserEventMonitor;
        private IManagedDisposable _droneEventSoundPlayer;
        private UltrasConstructionMonitor _ultrasConstructionMonitor;
        private DangerMusicPlayer _dangerMusicPlayer;

        public float smoothTime;

        // NEWUI  Remove this bool :P
        public static bool IsNewUI = true;

        private const int CRUISER_OFFSET_IN_M = 35;

        // FELIX  Split up into Left-/Right-PanelController, they initialise?
        private void Start()
        {
            Assert.raiseExceptions = true;
            // FELIX  Hm, time scale should be someone else's responsibility :/
            Time.timeScale = 1;


            // FELIX  Extract GetComponents() to separate method?
            IDeferrer deferrer = GetComponent<IDeferrer>();
            IVariableDelayDeferrer variableDelayDeferrer = GetComponent<IVariableDelayDeferrer>();
            IHighlightFactory highlightFactory = GetComponent<IHighlightFactory>();

            AudioSource platformAudioSource = GetComponent<AudioSource>();
            Assert.IsNotNull(platformAudioSource);
            IAudioSource audioSource = new AudioSourceBC(platformAudioSource);

            Helper.AssertIsNotNull(deferrer, variableDelayDeferrer, highlightFactory);


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
            _battleCompletionHandler = new BattleCompletionHandler(applicationModel, sceneNavigator);

            LeftPanelInitialiser leftPanelInitialiser = FindObjectOfType<LeftPanelInitialiser>();
            Assert.IsNotNull(leftPanelInitialiser);

            RightPanelInitialiser rightPanelInitialiser = FindObjectOfType<RightPanelInitialiser>();
            Assert.IsNotNull(rightPanelInitialiser);

            // Common setup
            IPrefabFactory prefabFactory = new PrefabFactory(new PrefabFetcher());
            ISpriteProvider spriteProvider = new SpriteProvider(new SpriteFetcher());
            IBattleSceneHelper helper = CreateHelper(dataProvider, prefabFactory, variableDelayDeferrer);
            IUserChosenTargetManager playerCruiserUserChosenTargetManager = new UserChosenTargetManager();
            IUserChosenTargetManager aiCruiserUserChosenTargetManager = new DummyUserChosenTargetManager();
            ITime time = new TimeBC();
            _pauseGameManager = new PauseGameManager(time);


            // FELIX  Abstract camera related functionality (currently camera moving
            // in LeftPanelInitialiser.Update() :P)
            Camera platformCamera = FindObjectOfType<Camera>();
            Assert.IsNotNull(platformCamera);
            ICamera camera = new CameraBC(platformCamera);


            ICameraController cameraController = Substitute.For<ICameraController>();


            // Create cruisers
            ICruiserFactoryNEW cruiserFactory
                = new CruiserFactoryNEW(
                    prefabFactory,
                    deferrer,
                    variableDelayDeferrer,
                    spriteProvider,
                    camera,
                    audioSource,
                    helper,
                    applicationModel,
                    cameraController);

            ICruiser playerCruiser = cruiserFactory.CreatePlayerCruiser();
            ICruiser aiCruiser = cruiserFactory.CreateAICruiser();
            

            // Have circular dependency between cruisers and UI manager.  Hence
            // create and initialise cruisers separately.
            IUIManager uiManager = CreateUIManager(playerCruiser, aiCruiser, leftPanelInitialiser.BuildMenu, rightPanelInitialiser.Informator);


            // Initialise player cruiser
            cruiserFactory
                .InitialisePlayerCruiser(
                    uiManager,
                    playerCruiserUserChosenTargetManager);
            playerCruiser.Destroyed += PlayerCruiser_Destroyed;


            // Initialise AI cruiser
            IUserChosenTargetHelper userChosenTargetHelper
                = new UserChosenTargetHelper(
                    playerCruiserUserChosenTargetManager,
                    playerCruiser.FactoryProvider.Sound.PrioritisedSoundPlayer);
            cruiserFactory
                .InitialiseAICruiser(
                    uiManager,
                    aiCruiserUserChosenTargetManager,
                    userChosenTargetHelper);
            aiCruiser.Destroyed += AiCruiser_Destroyed;


            // UI
            IButtonVisibilityFilters buttonVisibilityFilters = helper.CreateButtonVisibilityFilters(playerCruiser.DroneManager);
            

            leftPanelInitialiser
                .Initialise(
                    playerCruiser.DroneManager,
                    new DroneManagerMonitor(playerCruiser.DroneManager, variableDelayDeferrer),
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
                    _pauseGameManager);


            // FELIX  Abstract??
            // User chosen target highlighter
            IHighlightHelper highlightHelper = new HighlightHelper(highlightFactory);
            _userChosenTargetHighligher = new UserChosenTargetHighligher(playerCruiserUserChosenTargetManager, highlightHelper);


            _ai = helper.CreateAI(aiCruiser, playerCruiser, applicationModel.SelectedLevel);
            ILevel currentLevel = applicationModel.DataProvider.GetLevel(applicationModel.SelectedLevel);
            GenerateClouds(currentLevel);
            _droneEventSoundPlayer = helper.CreateDroneEventSoundPlayer(playerCruiser, variableDelayDeferrer);
            _dangerMusicPlayer = CreateDangerMusicPlayer(musicPlayer, playerCruiser, aiCruiser, variableDelayDeferrer);


            // FELIX  Abstract event monitors?
            _cruiserEventMonitor = CreateCruiserEventMonitor(playerCruiser, time);
            _ultrasConstructionMonitor = CreateUltrasConstructionMonitor(aiCruiser);
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

        // FELIX  Abstract :/
        private void PlayerCruiser_Destroyed(object sender, DestroyedEventArgs e)
        {
            _pauseGameManager.PauseGame();
            _battleCompletionHandler.CompleteBattle(wasVictory: false);
        }

        // FELIX  Abstract :/
        private void AiCruiser_Destroyed(object sender, DestroyedEventArgs e)
        {
            _pauseGameManager.PauseGame();
            _battleCompletionHandler.CompleteBattle(wasVictory: true);
        }

        // FELIX  Abstract :D
        private void GenerateClouds(ILevel level)
        {
            CloudFactory cloudFactory = GetComponent<CloudFactory>();
            Assert.IsNotNull(cloudFactory);
            cloudFactory.Initialise();

            ICloudGenerator cloudGenerator = new CloudGenerator(cloudFactory);
            cloudGenerator.GenerateClouds(level.CloudStats);
        }

        private DangerMusicPlayer CreateDangerMusicPlayer(
            IMusicPlayer musicPlayer,
            ICruiser playerCruiser,
            ICruiser aiCruiser,
            IVariableDelayDeferrer deferrer)
        {
            return
                new DangerMusicPlayer(
                    musicPlayer,
                    new DangerMonitor(
                        playerCruiser,
                        aiCruiser,
                        new HealthThresholdMonitor(playerCruiser, thresholdProportion: 0.3f),
                        new HealthThresholdMonitor(aiCruiser, thresholdProportion: 0.3f)),
                    deferrer);
        }

        private CruiserEventMonitor CreateCruiserEventMonitor(ICruiser playerCruiser, ITime time)
        {
            return
                new CruiserEventMonitor(
                    new HealthThresholdMonitor(playerCruiser, thresholdProportion: 0.3f),
                    new CruiserDamagedMonitorDebouncer(
                        new CruiserDamageMonitor(playerCruiser),
                        time),
                    playerCruiser.FactoryProvider.Sound.PrioritisedSoundPlayer);
        }

        private UltrasConstructionMonitor CreateUltrasConstructionMonitor(ICruiser aiCruiser)
        {
            return
                new UltrasConstructionMonitor(
                    aiCruiser,
                    aiCruiser.FactoryProvider.Sound.PrioritisedSoundPlayer);
        }
    }
}