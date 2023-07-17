using BattleCruisers.AI;
using BattleCruisers.Buildables.Colours;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Damage;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Data;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using BattleCruisers.Tutorial;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.Clouds.Stats;
using BattleCruisers.UI.BattleScene.HelpLabels;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras;
using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.UI.Music;
using BattleCruisers.UI.Sound;
using BattleCruisers.UI.Sound.Wind;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene;
using BattleCruisers.Utils.BattleScene.Lifetime;
using BattleCruisers.Utils.Debugging;
using BattleCruisers.Utils.Factories;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Fetchers.Cache;
using BattleCruisers.Utils.Fetchers.Sprites;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Utils.PlatformAbstractions;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using BattleCruisers.Utils.PlatformAbstractions.Time;
using BattleCruisers.Utils.Threading;
using NSubstitute;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using BattleCruisers.Buildables;
using System.Collections.Generic;
using BattleCruisers.Data.Settings;
using BattleCruisers.Data.Static;
using Unity.Services.Analytics;

// === Tag keys :D ===
// FELIX    => Code todo
// TEMP     => Temporary for testing
// DLC      => For DLC
// PERF     => Potential performance hit


namespace BattleCruisers.Scenes.BattleScene
{
    public class BattleSceneGod : MonoBehaviour
    {
        private static IGameEndMonitor _gameEndMonitor;
        // Hold references to avoid garbage collection
        private AudioInitialiser _audioInitialiser;
        private ITutorialProvider _tutorialProvider;
        private UserTargetTracker _userTargetTracker;
        private BuildableButtonColourController _buildableButtonColourController;
        private CruiserDeathManager _cruiserDeathManager;
        private LifetimeManager _lifetimeManager;
        private InformatorDismisser _informatorDismisser;
        private PausableAudioListener _pausableAudioListener;

        public int defaultLevel;
        public bool isTutorial = false;

        public CameraInitialiser cameraInitialiser;
        public TopPanelInitialiser topPanelInitialiser;
        public LeftPanelInitialiser leftPanelInitialiser;
        public RightPanelInitialiser rightPanelInitialiser;
        public TutorialInitialiser tutorialInitialiser;
        public WaterSplashVolumeController waterSplashVolumeController;
        public GameObject enemyCharacterImages;
        private IDataProvider dataProvider;
        private Cruiser playerCruiser;
        private Cruiser aiCruiser;
        private NavigationPermitters navigationPermitters;
        private BattleSceneGodComponents components;
        private FactoryProvider factoryProvider;
        private ICameraComponents cameraComponents;
        public ToolTipActivator toolTipActivator;
        public static Dictionary<TargetType, DeadBuildableCounter> deadBuildables;
        public static Sprite enemyCruiserSprite;
        public static string enemyCruiserName;
        private static float difficultyDestructionScoreMultiplier;
        private static bool GameOver;
        public GameObject ultraPanel;
        private IApplicationModel applicationModel;

        public GameObject[] ilegalTutorialSettings;
        private async void Start()
        {
            Logging.Log(Tags.BATTLE_SCENE, "Start");

            Helper.AssertIsNotNull(cameraInitialiser, topPanelInitialiser, leftPanelInitialiser, rightPanelInitialiser, tutorialInitialiser, waterSplashVolumeController);

            ISceneNavigator sceneNavigator = LandingSceneGod.SceneNavigator;
            applicationModel = ApplicationModelProvider.ApplicationModel;

            PrioritisedSoundKeys.SetSoundKeys(applicationModel.DataProvider.SettingsManager.AltDroneSounds);//Sets the drone sounds to either the normal or alt versions based on settings
            // TEMP  Only because I'm starting the the scene without a previous Choose Level Scene
            if (sceneNavigator == null)
            {
                // TEMP  Force level I'm currently testing :)
                applicationModel.SelectedLevel = defaultLevel;
                //Debug.Log(applicationModel.SelectedLevel);
                sceneNavigator = Substitute.For<ISceneNavigator>();
            }

            components = GetComponent<BattleSceneGodComponents>();
            Assert.IsNotNull(components);
            components.Initialise(applicationModel.DataProvider.SettingsManager);
            components.UpdaterProvider.SwitchableUpdater.Enabled = false;

            // TEMP  Force  tutorial
            if (isTutorial)
            {
                applicationModel.Mode = GameMode.Tutorial;
                applicationModel.SelectedLevel = 1;
            }

            if (applicationModel.Mode == GameMode.Tutorial)
            {
                foreach (GameObject setting in ilegalTutorialSettings)
                {
                    setting.SetActive(false);
                }
            }

            dataProvider = applicationModel.DataProvider;
            waterSplashVolumeController.Initialise(dataProvider.SettingsManager);

            // Common setup
            ILocTable commonStrings = await LocTableFactory.Instance.LoadCommonTableAsync();
            ILocTable storyStrings = await LocTableFactory.Instance.LoadStoryTableAsync();
            IPrefabCacheFactory prefabCacheFactory = new PrefabCacheFactory(commonStrings);
            IPrefabFetcher prefabFetcher = new PrefabFetcher();
            IPrefabCache prefabCache = await prefabCacheFactory.CreatePrefabCacheAsync(prefabFetcher);
            IPrefabFactory prefabFactory = new PrefabFactory(prefabCache, dataProvider.SettingsManager, commonStrings);
            ISpriteProvider spriteProvider = new SpriteProvider(new SpriteFetcher());
            navigationPermitters = new NavigationPermitters();

            IBattleSceneHelper helper = CreateHelper(applicationModel, prefabFetcher, prefabFactory, components.Deferrer, navigationPermitters, storyStrings);
            IUserChosenTargetManager playerCruiserUserChosenTargetManager = new UserChosenTargetManager();
            IUserChosenTargetManager aiCruiserUserChosenTargetManager = new DummyUserChosenTargetManager();
            ITime time = TimeBC.Instance;
            IPauseGameManager pauseGameManager = new PauseGameManager(time);
            IUIManager uiManager = helper.CreateUIManager();

            // Create cruisers
            Logging.Log(Tags.BATTLE_SCENE, "Cruiser setup");
            factoryProvider = new FactoryProvider(components, prefabFactory, spriteProvider, dataProvider.SettingsManager);
            factoryProvider.Initialise(uiManager);
            ICruiserFactory cruiserFactory = new CruiserFactory(factoryProvider, helper, applicationModel, uiManager);

            playerCruiser = cruiserFactory.CreatePlayerCruiser();
            IPrefabKey aiCruiserKey = helper.GetAiCruiserKey();
            aiCruiser = cruiserFactory.CreateAICruiser(aiCruiserKey);
            enemyCruiserSprite = aiCruiser.Sprite;
            enemyCruiserName = aiCruiser.Name;

            // Camera
            cameraComponents
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
            ILevel currentLevel = helper.GetLevel();
            string enemyName = await helper.GetEnemyNameAsync(currentLevel.Num);
            IBattleCompletionHandler battleCompletionHandler = new BattleCompletionHandler(applicationModel, sceneNavigator);

            TopPanelComponents topPanelComponents = topPanelInitialiser.Initialise(playerCruiser, aiCruiser, enemyName);
            LeftPanelComponents leftPanelComponents
                = leftPanelInitialiser.Initialise(
                    playerCruiser.DroneManager,
                    new DroneManagerMonitor(playerCruiser.DroneManager, components.Deferrer),
                    uiManager,
                    helper.GetPlayerLoadout(),
                    prefabFactory,
                    spriteProvider,
                    buttonVisibilityFilters,
                    new PlayerCruiserFocusHelper(cameraComponents.MainCamera, cameraComponents.CameraFocuser, playerCruiser, applicationModel.IsTutorial),
                    helper.GetBuildableButtonSoundPlayer(playerCruiser),
                    factoryProvider.Sound.UISoundPlayer,
                    playerCruiser.PopulationLimitMonitor,
                    dataProvider.StaticData);

            NavigationPermitterManager navigationPermitterManager = new NavigationPermitterManager(navigationPermitters);
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
                    navigationPermitterManager);
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
                    factoryProvider.Sound.PrioritisedSoundPlayer,
                    factoryProvider.Sound.UISoundPlayer);
            helper.InitialiseUIManager(args);

            _informatorDismisser = new InformatorDismisser(components.BackgroundClickableEmitter, uiManager);

            // Audio
            Logging.Log(Tags.BATTLE_SCENE, "Audio setup");
            ILayeredMusicPlayer layeredMusicPlayer
                = await components.MusicPlayerInitialiser.CreatePlayerAsync(
                    factoryProvider.Sound.SoundFetcher,
                    currentLevel.MusicKeys,
                    dataProvider.SettingsManager);
            ICruiserDamageMonitor playerCruiserDamageMonitor = new CruiserDamageMonitor(playerCruiser);
            _audioInitialiser
                = new AudioInitialiser(
                    helper,
                    layeredMusicPlayer,
                    playerCruiser,
                    aiCruiser,
                    components.Deferrer,
                    time,
                    battleCompletionHandler,
                    playerCruiserDamageMonitor,
                    leftPanelComponents.PopLimitReachedFeedback);

            IWindManager windManager
                = components.WindInitialiser.Initialise(
                    cameraComponents.MainCamera,
                    cameraComponents.Settings,
                    dataProvider.SettingsManager);
            windManager.Play();

            _pausableAudioListener
                = new PausableAudioListener(
                    new AudioListenerBC(),
                    pauseGameManager);

            // Other
            Logging.Log(Tags.BATTLE_SCENE, "Other setup");
            _cruiserDeathManager = new CruiserDeathManager(playerCruiser, aiCruiser);
            IArtificialIntelligence ai = helper.CreateAI(aiCruiser, playerCruiser, applicationModel.SelectedLevel);
            IPrefabContainer<BackgroundImageStats> backgroundStats = await helper.GetBackgroundStatsAsync(currentLevel.Num);
            components.CloudInitialiser.Initialise(currentLevel.SkyMaterialName, components.UpdaterProvider.VerySlowUpdater, cameraComponents.MainCamera.Aspect, backgroundStats);
            await components.SkyboxInitialiser.InitialiseAsync(cameraComponents.Skybox, currentLevel);
            components.HotkeyInitialiser.Initialise(
                dataProvider.GameModel.Hotkeys,
                InputBC.Instance,
                components.UpdaterProvider.SwitchableUpdater,
                navigationPermitters.HotkeyFilter,
                cameraComponents.CameraFocuser,
                rightPanelComponents.SpeedComponents,
                rightPanelComponents.MainMenuManager,
                uiManager);
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
                        uiManager,
                        components.TargetIndicator,
                        windManager,
                        helper.BuildingCategoryPermitter,
                        rightPanelComponents.SpeedComponents.SpeedButtonGroup));

            // Cheater is only there in debug builds
            Cheater cheater = GetComponentInChildren<Cheater>(includeInactive: true);
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
            await tutorialInitialiser.InitialiseAsync(tutorialArgs, helper.ShowInGameHints, playerCruiserDamageMonitor, commonStrings);
            if (helper.ShowInGameHints)
            {
                uiManager.SetExplanationPanel(tutorialInitialiser.explanationPanel);
            }
            // Do not enable updates until asynchronous loading is complete.
            components.UpdaterProvider.SwitchableUpdater.Enabled = true;

            sceneNavigator.SceneLoaded(SceneNames.BATTLE_SCENE);

            //Code that uses current level to set the image of the enemy robot on the enemy nav button
            //Make sure to add more images to the EnemyCharacterImages prefab if more enemies are added
            if(enemyCharacterImages != null)
            {
                Image[] enemyImages = enemyCharacterImages.GetComponentsInChildren<Image>(true);
                Assert.IsTrue(enemyImages.Length >= currentLevel.Num);
                enemyImages[currentLevel.Num - 1].enabled = true;
            }

            toolTipActivator.Initialise();

            if (!aiCruiser.isCruiser)
            {
                aiCruiser.AdjustStatsByDifficulty(applicationModel.DataProvider.SettingsManager.AIDifficulty);
                if (ultraPanel != null)
                {
                    foreach (Transform button in ultraPanel.transform)
                    {
                        BuildingButtonController temp = button.GetComponent<BuildingButtonController>();
                        if (temp.buildableName.text.Equals("Nuke Launcher"))
                        {
                            button.gameObject.SetActive(false);
                        }
                    }
                }
                //Debug.Log(applicationModel.DataProvider.SettingsManager.AIDifficulty);
            }
            deadBuildables = new Dictionary<TargetType, DeadBuildableCounter>();
            deadBuildables.Add(TargetType.Aircraft, new DeadBuildableCounter());
            deadBuildables.Add(TargetType.Ships, new DeadBuildableCounter());
            deadBuildables.Add(TargetType.Cruiser, new DeadBuildableCounter());
            deadBuildables.Add(TargetType.Buildings, new DeadBuildableCounter());
            deadBuildables.Add(TargetType.PlayedTime, new DeadBuildableCounter());

            if (applicationModel.DataProvider.SettingsManager.AIDifficulty == Difficulty.Normal)
            {
                difficultyDestructionScoreMultiplier = 1.0f;
            }
            if (applicationModel.DataProvider.SettingsManager.AIDifficulty == Difficulty.Hard)
            {
                difficultyDestructionScoreMultiplier = 1.5f;
            }
            if (applicationModel.DataProvider.SettingsManager.AIDifficulty == Difficulty.Harder)
            {
                difficultyDestructionScoreMultiplier = 2.0f;
            }

            GameOver = false;
            string logName = "Battle_Begin";
#if LOG_ANALYTICS
    Debug.Log("Analytics: " + logName);
#endif
            try
            {
                AnalyticsService.Instance.CustomData("Battle", applicationModel.DataProvider.GameModel.Analytics(applicationModel.Mode.ToString(), logName, applicationModel.UserWonSkirmish));
                AnalyticsService.Instance.Flush();
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }

        /*        private void OnEnable()
                {
                    LandingSceneGod.SceneNavigator.SceneLoaded(SceneNames.BATTLE_SCENE);
                }*/

        private IBattleSceneHelper CreateHelper(
            IApplicationModel applicationModel,
            IPrefabFetcher prefabFetcher,
            IPrefabFactory prefabFactory,
            IDeferrer deferrer,
            NavigationPermitters navigationPermitters,
            ILocTable storyStrings)
        {
            switch (applicationModel.Mode)
            {
                case GameMode.Tutorial:
                    TutorialHelper helper = new TutorialHelper(applicationModel, prefabFetcher, storyStrings, prefabFactory, navigationPermitters);
                    _tutorialProvider = helper;
                    return helper;

                case GameMode.Campaign:
                    return new NormalHelper(applicationModel, prefabFetcher, storyStrings, prefabFactory, deferrer);

                case GameMode.Skirmish:
                    return new SkirmishHelper(applicationModel, prefabFetcher, storyStrings, prefabFactory, deferrer);

                case GameMode.CoinBattle:
                    return new CoinBattleHelper(applicationModel, prefabFetcher, storyStrings, prefabFactory, deferrer);

                default:
                    throw new InvalidOperationException($"Unknow enum value: {applicationModel.Mode}");
            }
        }

        public void UpdateCamera()
        {
            if (applicationModel.Mode == GameMode.Tutorial)
            {
                return;
            }
            cameraComponents = cameraInitialiser.UpdateCamera(
                    dataProvider.SettingsManager,
                    navigationPermitters);
            Debug.Log("camera changed");
        }

        public static void AddDeadBuildable(TargetType type, int value)
        {
            if (!GameOver)
            {
                if (type == TargetType.Satellite || type == TargetType.Rocket)
                {
                    return;
                }
                deadBuildables[type].AddDeadBuildable((int)(difficultyDestructionScoreMultiplier * ((float)value)));
                //Debug.Log("" + (int)(difficultyDestructionScoreMultiplier*((float)value)) + " added");
                if (type == TargetType.Cruiser)
                {
                    GameOver = true;
                }
            }
        }

        public static void AddPlayedTime(TargetType type, float dt)
        {
            if (!GameOver)
            {
                deadBuildables?[type]?.AddPlayedTime(dt);
            }
        }

        public static void ShowDeadBuildableStats()
        {
            foreach (KeyValuePair<TargetType, DeadBuildableCounter> kvp in deadBuildables)
            {
                Debug.Log(kvp.Key);
                Debug.Log("Destroyed: " + kvp.Value.GetTotalDestroyed());
                Debug.Log("Damage in credits: " + kvp.Value.GetTotalDamageInCredits());
            }
        }
    }
}