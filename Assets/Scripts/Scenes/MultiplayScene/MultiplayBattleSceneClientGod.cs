using BattleCruisers.Buildables.Colours;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Damage;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Data;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.Clouds.Stats;
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
using BattleCruisers.Utils.PlatformAbstractions;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using BattleCruisers.Utils.PlatformAbstractions.Time;
using BattleCruisers.Utils.Threading;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using BattleCruisers.Buildables;
using System.Collections.Generic;
using BattleCruisers.Data.Settings;
using BattleCruisers.Data.Static;
using BattleCruisers.Scenes.BattleScene;
using BattleCruisers.Scenes;
using BattleCruisers.Network.Multiplay.Gameplay.GameState;
using Unity.Netcode;
using Unity.Multiplayer.Samples.Utilities;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;


namespace BattleCruisers.Network.Multiplay.MultiplayBattleScene.Client
{
    public class MultiplayBattleSceneGod : GameStateBehaviour
    {
        public override GameState ActiveState { get { return GameState.MultiplayBattleScene; } }
        private static GameEndMonitor _gameEndMonitor;

        private AudioInitialiser _audioInitialiser;
        // private ITutorialProvider _tutorialProvider;
        private UserTargetTracker _userTargetTracker;
        private BuildableButtonColourController _buildableButtonColourController;
        private CruiserDeathManager _cruiserDeathManager;
        private LifetimeManager _lifetimeManager;
        private InformatorDismisser _informatorDismisser;
        private PausableAudioListener _pausableAudioListener;

        public int defaultLevel;
        public CameraInitialiser cameraInitialiser;
        public TopPanelInitialiser topPanelInitialiser;
        public LeftPanelInitialiser leftPanelInitialiser;
        public RightPanelInitialiser rightPanelInitialiser;
        // public TutorialInitialiser tutorialInitialiser;
        public WaterSplashVolumeController waterSplashVolumeController;
        public GameObject enemyCharacterImages;
        private Cruiser playerCruiser;
        private Cruiser aiCruiser;
        private NavigationPermitters navigationPermitters;
        private BattleSceneGodComponents components;
        private ICameraComponents cameraComponents;
        public ToolTipActivator toolTipActivator;
        public static Dictionary<TargetType, DeadBuildableCounter> deadBuildables;
        public static Sprite enemyCruiserSprite;
        public static string enemyCruiserName;
        private static float difficultyDestructionScoreMultiplier;
        private static bool GameOver;
        public GameObject nukeButton;

        [SerializeField]
        NetcodeHooks m_NetcodeHooks;


        protected override void Awake()
        {
            base.Awake();
            m_NetcodeHooks.OnNetworkSpawnHook += OnNetworkSpawn;
            m_NetcodeHooks.OnNetworkDespawnHook += OnNetworkDespawn;

        }


        void OnNetworkSpawn()
        {
            if (!NetworkManager.Singleton.IsServer)
            {
                enabled = false;
                return;
            }
        }

        void OnNetworkDespawn()
        {

        }

        protected override void OnDestroy()
        {
            if (m_NetcodeHooks)
            {
                m_NetcodeHooks.OnNetworkSpawnHook -= OnNetworkSpawn;
                m_NetcodeHooks.OnNetworkDespawnHook -= OnNetworkDespawn;
            }

            base.OnDestroy();
        }

        private async void Start()
        {
            // Logging.Log(Tags.BATTLE_SCENE, "Start");

            Helper.AssertIsNotNull(cameraInitialiser, topPanelInitialiser, leftPanelInitialiser, rightPanelInitialiser, waterSplashVolumeController);

            PrioritisedSoundKeys.SetSoundKeys(DataProvider.SettingsManager.AltDroneSounds);//Sets the drone sounds to either the normal or alt versions based on settings

            components = GetComponent<BattleSceneGodComponents>();
            Assert.IsNotNull(components);
            components.Initialise();
            components.UpdaterProvider.SwitchableUpdater.Enabled = false;


            // if (ApplicationModel.Mode == GameMode.Tutorial)
            // {
            //     foreach (GameObject setting in ilegalTutorialSettings)
            //     {
            //         setting.SetActive(false);
            //     }
            // }

            waterSplashVolumeController.Initialise(DataProvider.SettingsManager);

            // Common setup
            navigationPermitters = new NavigationPermitters();

            IBattleSceneHelper helper = CreateHelper(components.Deferrer);
            IUserChosenTargetManager playerCruiserUserChosenTargetManager = new UserChosenTargetManager();
            IUserChosenTargetManager aiCruiserUserChosenTargetManager = new DummyUserChosenTargetManager();
            ITime time = TimeBC.Instance;
            PauseGameManager pauseGameManager = new PauseGameManager(time);
            UIManager uiManager = helper.CreateUIManager();

            // Create cruisers
            Logging.Log(Tags.BATTLE_SCENE, "Cruiser setup");
            FactoryProvider.Initialise(components, DataProvider.SettingsManager, uiManager);
            ICruiserFactory cruiserFactory = new CruiserFactory(helper, uiManager);
            playerCruiser = cruiserFactory.CreatePlayerCruiser();
            IPrefabKey aiCruiserKey = helper.GetAiCruiserKey();
            aiCruiser = cruiserFactory.CreateAICruiser(aiCruiserKey);
            enemyCruiserSprite = aiCruiser.Sprite;
            enemyCruiserName = aiCruiser.stringKeyBase;

            // Camera
            cameraComponents
                = cameraInitialiser.Initialise(
                    DataProvider.SettingsManager,
                    playerCruiser,
                    aiCruiser,
                    navigationPermitters,
                    components.UpdaterProvider.SwitchableUpdater,
                    PvPFactoryProvider.Sound.UISoundPlayer);
            cameraComponents.CameraFocuser.FocusOnLeftCruiser();

            // Initialise player cruiser
            cruiserFactory.InitialisePlayerCruiser(playerCruiser, aiCruiser, cameraComponents.CameraFocuser, playerCruiserUserChosenTargetManager);

            // Initialise AI cruiser
            IUserChosenTargetHelper userChosenTargetHelper
                = helper.CreateUserChosenTargetHelper(
                    playerCruiserUserChosenTargetManager,
                    FactoryProvider.Sound.PrioritisedSoundPlayer,
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
            BattleCompletionHandler battleCompletionHandler = new BattleCompletionHandler();

            TopPanelComponents topPanelComponents = topPanelInitialiser.Initialise(playerCruiser, aiCruiser, enemyName);
            LeftPanelComponents leftPanelComponents
                = leftPanelInitialiser.Initialise(
                    playerCruiser.DroneManager,
                    new DroneManagerMonitor(playerCruiser.DroneManager, components.Deferrer),
                    uiManager,
                    helper.GetPlayerLoadout(),
                    buttonVisibilityFilters,
                    new PlayerCruiserFocusHelper(cameraComponents.MainCamera, cameraComponents.CameraFocuser, playerCruiser, ApplicationModel.IsTutorial),
                    helper.GetBuildableButtonSoundPlayer(playerCruiser),
                    PvPFactoryProvider.Sound.UISoundPlayer,
                    playerCruiser.PopulationLimitMonitor);

            NavigationPermitterManager navigationPermitterManager = new NavigationPermitterManager(navigationPermitters);
            RightPanelComponents rightPanelComponents
                = rightPanelInitialiser.Initialise(
                    uiManager,
                    playerCruiser,
                    userChosenTargetHelper,
                    buttonVisibilityFilters,
                    pauseGameManager,
                    battleCompletionHandler,
                    PvPFactoryProvider.Sound.UISoundPlayer,
                    navigationPermitterManager);
            _lifetimeManager = new LifetimeManager(components.LifetimeEvents, rightPanelComponents.MainMenuManager);

            IItemDetailsManager itemDetailsManager = new ItemDetailsManager(rightPanelComponents.InformatorPanel);
            _userTargetTracker = new UserTargetTracker(itemDetailsManager.SelectedItem, new UserTargetsColourChanger());
            _buildableButtonColourController = new BuildableButtonColourController(itemDetailsManager.SelectedItem, leftPanelComponents.BuildMenu.BuildableButtons);

            helper.InitialiseUIManager(
                playerCruiser,
                aiCruiser,
                leftPanelComponents.BuildMenu,
                itemDetailsManager,
                PvPFactoryProvider.Sound.PrioritisedSoundPlayer,
                PvPFactoryProvider.Sound.UISoundPlayer);

            _informatorDismisser = new InformatorDismisser(components.BackgroundClickableEmitter, uiManager);

            // Audio
            Logging.Log(Tags.BATTLE_SCENE, "Audio setup");
            ILayeredMusicPlayer layeredMusicPlayer
                = await components.MusicPlayerInitialiser.CreatePlayerAsync(
                    currentLevel.MusicKeys,
                    DataProvider.SettingsManager);
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
                    DataProvider.SettingsManager);
            windManager.Play();

            _pausableAudioListener
                = new PausableAudioListener(
                    new AudioListenerBC(),
                    pauseGameManager);

            // Other
            Logging.Log(Tags.BATTLE_SCENE, "Other setup");
            _cruiserDeathManager = new CruiserDeathManager(playerCruiser, aiCruiser);
            IManagedDisposable ai = helper.CreateAI(aiCruiser, playerCruiser, ApplicationModel.SelectedLevel);
            PrefabContainer<BackgroundImageStats> backgroundStats = await helper.GetBackgroundStatsAsync(currentLevel.Num);
            components.CloudInitialiser.Initialise(currentLevel.SkyMaterialName, components.UpdaterProvider.VerySlowUpdater, cameraComponents.MainCamera.Aspect, backgroundStats);
            await components.SkyboxInitialiser.InitialiseAsync(cameraComponents.Skybox, currentLevel.SkyMaterialName);



            components.HotkeyInitialiser.Initialise(
                DataProvider.GameModel.Hotkeys,
                InputBC.Instance,
                components.UpdaterProvider.SwitchableUpdater,
                navigationPermitters.HotkeyFilter,
                cameraComponents.CameraFocuser,
                rightPanelComponents.SpeedComponents,
                rightPanelComponents.MainMenuManager);
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
            cheater?.Initialise(playerCruiser, aiCruiser);

            // Tutorial
            // ITutorialArgsBase tutorialArgs
            //     = new TutorialArgsBase(
            //         applicationModel,
            //         playerCruiser,
            //         aiCruiser,
            //         _tutorialProvider,
            //         prefabFactory,
            //         components,
            //         cameraComponents,
            //         topPanelComponents,
            //         leftPanelComponents,
            //         rightPanelComponents,
            //         uiManager,
            //         _gameEndMonitor);
            // await tutorialInitialiser.InitialiseAsync(tutorialArgs, helper.ShowInGameHints, playerCruiserDamageMonitor);


            // if (helper.ShowInGameHints)
            // {
            //     uiManager.SetExplanationPanel(tutorialInitialiser.explanationPanel);
            // }


            // Do not enable updates until asynchronous loading is complete.
            components.UpdaterProvider.SwitchableUpdater.Enabled = true;

            SceneNavigator.SceneLoaded(SceneNames.BATTLE_SCENE);

            //Code that uses current level to set the image of the enemy robot on the enemy nav button
            //Make sure to add more images to the EnemyCharacterImages prefab if more enemies are added
            Image[] enemyImages = enemyCharacterImages.GetComponentsInChildren<Image>(true);
            Assert.IsTrue(enemyImages.Length >= currentLevel.Num);
            enemyImages[currentLevel.Num - 1].enabled = true;

            toolTipActivator.Initialise();

            if (!aiCruiser.isCruiser)
            {
                aiCruiser.AdjustStatsByDifficulty(DataProvider.SettingsManager.AIDifficulty);
                if (nukeButton != null)
                {
                    nukeButton.SetActive(false);
                }
                //Debug.Log(DataProvider.SettingsManager.AIDifficulty);
            }
            deadBuildables = new Dictionary<TargetType, DeadBuildableCounter>();
            deadBuildables.Add(TargetType.Aircraft, new DeadBuildableCounter());
            deadBuildables.Add(TargetType.Ships, new DeadBuildableCounter());
            deadBuildables.Add(TargetType.Cruiser, new DeadBuildableCounter());
            deadBuildables.Add(TargetType.Buildings, new DeadBuildableCounter());

            if (DataProvider.SettingsManager.AIDifficulty == Difficulty.Normal)
            {
                difficultyDestructionScoreMultiplier = 1.0f;
            }
            if (DataProvider.SettingsManager.AIDifficulty == Difficulty.Hard)
            {
                difficultyDestructionScoreMultiplier = 1.5f;
            }
            if (DataProvider.SettingsManager.AIDifficulty == Difficulty.Harder)
            {
                difficultyDestructionScoreMultiplier = 2.0f;
            }




            GameOver = false;
            /*
            string logName = "Battle_Begin";
#if LOG_ANALYTICS
    Debug.Log("Analytics: " + logName);
#endif
            try
            {
                AnalyticsService.Instance.CustomData("Battle", DataProvider.GameModel.Analytics(ApplicationModel.Mode.ToString(), logName, ApplicationModel.UserWonSkirmish));
                AnalyticsService.Instance.Flush();
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }
            */

        }


        private IBattleSceneHelper CreateHelper(IDeferrer deferrer)
        {
            switch (ApplicationModel.Mode)
            {
                // case GameMode.Tutorial:
                //     TutorialHelper helper = new TutorialHelper(prefabFetcher, storyStrings, prefabFactory, navigationPermitters);
                //     _tutorialProvider = helper;
                //     return helper;

                case GameMode.Campaign:
                    return new NormalHelper(deferrer);

                case GameMode.Skirmish:
                    return new SkirmishHelper(deferrer);
                // case GameMode.PvP_1VS1:
                //     return;

                default:
                    throw new InvalidOperationException($"Unknow enum value: {ApplicationModel.Mode}");
            }
        }


        public void UpdateCamera()
        {
            if (ApplicationModel.Mode == GameMode.Tutorial)
            {
                return;
            }
            cameraComponents = cameraInitialiser.UpdateCamera(
                    DataProvider.SettingsManager,
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

