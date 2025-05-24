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
using BattleCruisers.UI.ScreensScene.ProfileScreen;

// === Tag keys :D ===
// FELIX    => Code todo
// TEMP     => Temporary for testing
// DLC      => For DLC
// PERF     => Potential performance hit


namespace BattleCruisers.Scenes.BattleScene
{
    public class BattleSceneGod : MonoBehaviour
    {
        private static GameEndMonitor _gameEndMonitor;
        // Hold references to avoid garbage collection
        private AudioInitialiser _audioInitialiser;
        private TutorialHelper _tutorialProvider;
        private UserTargetTracker _userTargetTracker;
        private BuildableButtonColourController _buildableButtonColourController;
        private CruiserDeathManager _cruiserDeathManager;
        private LifetimeManager _lifetimeManager;
        private InformatorDismisser _informatorDismisser;
        private PausableAudioListener _pausableAudioListener;

        public int defaultLevel;
        public int defaultSideQuest;
        public bool isTutorial = false;

        public CameraInitialiser cameraInitialiser;
        public TopPanelInitialiser topPanelInitialiser;
        public LeftPanelInitialiser leftPanelInitialiser;
        public RightPanelInitialiser rightPanelInitialiser;
        public TutorialInitialiser tutorialInitialiser;
        public WaterSplashVolumeController waterSplashVolumeController;
        public GameObject enemyCharacterImages;
        private Cruiser playerCruiser;
        private Cruiser aiCruiser;
        private NavigationPermitters navigationPermitters;
        private BattleSceneGodComponents components;
        private CameraComponents cameraComponents;
        public ToolTipActivator toolTipActivator;
        public static Dictionary<TargetType, DeadBuildableCounter> deadBuildables;
        public static Sprite enemyCruiserSprite;
        public static string enemyCruiserName;
        private static float difficultyDestructionScoreMultiplier;
        private static bool GameOver;
        public GameObject ultraPanel;
        public UIManager uiManager;

        public GameObject PlayerCaptain;
        public GameObject EnemyCaptain;
        public Transform playerCaptainContainer;
        public Transform AICaptainContainer;
        public GameObject PlayerName;
        public GameObject EnemyName;

        public GameObject[] ilegalTutorialSettings;
        public static BattleSceneGod Instance;
        private void Awake()
        {
            Instance = this;
        }
        private async void Start()
        {
            Logging.Log(Tags.BATTLE_SCENE, "Start");

            Helper.AssertIsNotNull(cameraInitialiser, topPanelInitialiser, leftPanelInitialiser, rightPanelInitialiser, tutorialInitialiser, waterSplashVolumeController);

            PrioritisedSoundKeys.SetSoundKeys(DataProvider.SettingsManager.AltDroneSounds);//Sets the drone sounds to either the normal or alt versions based on settings

            components = GetComponent<BattleSceneGodComponents>();
            Assert.IsNotNull(components);
            components.Initialise();
            components.UpdaterProvider.SwitchableUpdater.Enabled = false;

            // TEMP  Force  tutorial
            if (isTutorial)
            {
                ApplicationModel.Mode = GameMode.Tutorial;
                ApplicationModel.SelectedLevel = 1;
            }

            if (ApplicationModel.Mode == GameMode.Tutorial)
            {
                foreach (GameObject setting in ilegalTutorialSettings)
                {
                    setting.SetActive(false);
                }
            }

            waterSplashVolumeController.Initialise(DataProvider.SettingsManager);

            // Common setup
            navigationPermitters = new NavigationPermitters();

            IBattleSceneHelper helper = CreateHelper(components.Deferrer, navigationPermitters);
            IUserChosenTargetManager playerCruiserUserChosenTargetManager = new UserChosenTargetManager();
            IUserChosenTargetManager aiCruiserUserChosenTargetManager = new DummyUserChosenTargetManager();
            ITime time = TimeBC.Instance;
            PauseGameManager pauseGameManager = new PauseGameManager(time);
            uiManager = helper.CreateUIManager();

            // Create cruisers
            Logging.Log(Tags.BATTLE_SCENE, "Cruiser setup");
            FactoryProvider.Initialise(components, DataProvider.SettingsManager);
            PrefabFactory.CreatePools();
            ICruiserFactory cruiserFactory = new CruiserFactory(helper, uiManager);

            playerCruiser = cruiserFactory.CreatePlayerCruiser();
            IPrefabKey aiCruiserKey = helper.GetAiCruiserKey();
            aiCruiser = cruiserFactory.CreateAICruiser(aiCruiserKey);
            enemyCruiserSprite = aiCruiser.Sprite;
            enemyCruiserName = aiCruiser.Name;

            // Camera
            cameraComponents
                = cameraInitialiser.Initialise(
                    DataProvider.SettingsManager,
                    playerCruiser,
                    aiCruiser,
                    navigationPermitters,
                    components.UpdaterProvider.SwitchableUpdater,
                    FactoryProvider.Sound.UISoundPlayer);
            cameraComponents.CameraFocuser.FocusOnLeftCruiser();

            // Initialise player cruiser
            cruiserFactory.InitialisePlayerCruiser(playerCruiser, aiCruiser, cameraComponents.CameraFocuser, playerCruiserUserChosenTargetManager);

            // Initialise AI cruiser
            IUserChosenTargetHelper userChosenTargetHelper
                = helper.CreateUserChosenTargetHelper(
                    playerCruiserUserChosenTargetManager,
                    FactoryProvider.Sound.IPrioritisedSoundPlayer,
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
            ButtonVisibilityFilters buttonVisibilityFilters = helper.CreateButtonVisibilityFilters(playerCruiser.DroneManager);
            ILevel currentLevel = null;
            SideQuestData currentSideQuest = null;
            string enemyName;

            // Prior to setting up Captain Names
            CaptainExo aiCaptainExoPrefab;
            if (ApplicationModel.Mode == GameMode.SideQuest)
            {
                currentSideQuest = helper.GetSideQuest();
                enemyName = await helper.GetEnemyNameAsync(currentSideQuest.SideLevelNum);
                aiCaptainExoPrefab = PrefabFactory.GetCaptainExo(currentSideQuest.EnemyCaptainExo);
            }
            else
            {
                currentLevel = helper.GetLevel();
                enemyName = await helper.GetEnemyNameAsync(currentLevel.Num);
                aiCaptainExoPrefab = PrefabFactory.GetCaptainExo(currentLevel.Captains);
            }

            Debug.Log($"Enemy name before instantiating: {enemyName}");

            BattleCompletionHandler battleCompletionHandler = new BattleCompletionHandler();

            TopPanelComponents topPanelComponents = topPanelInitialiser.Initialise(playerCruiser, aiCruiser, enemyName);

            // Setting up Captains
            CaptainExo playerCaptainExoPrefab = PrefabFactory.GetCaptainExo(DataProvider.GameModel.PlayerLoadout.CurrentCaptain);
            CaptainExo playerCaptain = Instantiate(playerCaptainExoPrefab, playerCaptainContainer);

            CaptainExo AICaptain = Instantiate(aiCaptainExoPrefab, AICaptainContainer);
            AICaptain.captainName = enemyName; // Ensure this assignment
            Debug.Log($"AI Captain name after instantiating and assigning: {AICaptain.captainName}");

            foreach (SpriteRenderer spriteRenderer in playerCaptain.gameObject.GetComponentsInChildren<SpriteRenderer>())
                spriteRenderer.color = new Vector4(0.7607843f, 0.2313726f, 0.1294118f, 1f);

            foreach (SpriteRenderer spriteRenderer in AICaptain.gameObject.GetComponentsInChildren<SpriteRenderer>())
                spriteRenderer.color = new Vector4(0.7607843f, 0.2313726f, 0.1294118f, 1f);

            playerCaptain.gameObject.transform.localScale = Vector3.one;
            AICaptain.gameObject.transform.localScale = Vector3.one;
            PlayerCaptain = playerCaptain.gameObject;
            EnemyCaptain = AICaptain.gameObject;

            // Setting up Captain Names
            Text playerName = PlayerName.gameObject.GetComponent<Text>();
            playerName.text = DataProvider.GameModel.PlayerName;

            if (ApplicationModel.Mode == GameMode.PvP_1VS1)
            {
                // Enemy player name
            }
            else
            {
                Text AIName = EnemyName.gameObject.GetComponent<Text>();
                Debug.Log($"AI Captain resolved name: {AICaptain.captainName}");
                AIName.text = AICaptain.captainName; // Directly assign the resolved name
                Debug.Log($"Displayed AI Captain name: {AIName.text}");
            }

            LeftPanelComponents leftPanelComponents
                = leftPanelInitialiser.Initialise(
                    playerCruiser.DroneManager,
                    new DroneManagerMonitor(playerCruiser.DroneManager, components.Deferrer),
                    uiManager,
                    helper.GetPlayerLoadout(),
                    buttonVisibilityFilters,
                    new PlayerCruiserFocusHelper(cameraComponents.MainCamera, cameraComponents.CameraFocuser, playerCruiser, ApplicationModel.IsTutorial),
                    helper.GetBuildableButtonSoundPlayer(playerCruiser),
                    FactoryProvider.Sound.UISoundPlayer,
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
                    FactoryProvider.Sound.UISoundPlayer,
                    navigationPermitterManager);
            _lifetimeManager = new LifetimeManager(components.LifetimeEvents, rightPanelComponents.MainMenuManager);

            ItemDetailsManager itemDetailsManager = new ItemDetailsManager(rightPanelComponents.InformatorPanel);
            _userTargetTracker = new UserTargetTracker(itemDetailsManager.SelectedItem, new UserTargetsColourChanger());
            _buildableButtonColourController = new BuildableButtonColourController(itemDetailsManager.SelectedItem, leftPanelComponents.BuildMenu.BuildableButtons);


            helper.InitialiseUIManager(
                playerCruiser,
                aiCruiser,
                leftPanelComponents.BuildMenu,
                itemDetailsManager,
                FactoryProvider.Sound.IPrioritisedSoundPlayer,
                FactoryProvider.Sound.UISoundPlayer);

            _informatorDismisser = new InformatorDismisser(components.BackgroundClickableEmitter, uiManager);

            // Audio
            Logging.Log(Tags.BATTLE_SCENE, "Audio setup");
            LayeredMusicPlayer layeredMusicPlayer;
            if (ApplicationModel.Mode == GameMode.SideQuest)
                layeredMusicPlayer = await components.MusicPlayerInitialiser.CreatePlayerAsync(
                    currentSideQuest.MusicBackgroundKey,
                    DataProvider.SettingsManager);
            else
                layeredMusicPlayer = await components.MusicPlayerInitialiser.CreatePlayerAsync(
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

            WindManager windManager
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
            PrefabContainer<BackgroundImageStats> backgroundStats;
            IManagedDisposable ai;
            if (ApplicationModel.Mode != GameMode.SideQuest)
            {
                ai = helper.CreateAI(aiCruiser, playerCruiser, ApplicationModel.SelectedLevel);
                backgroundStats = await helper.GetBackgroundStatsAsync(currentLevel.Num);
                components.CloudInitialiser.Initialise(currentLevel.SkyMaterialName, components.UpdaterProvider.VerySlowUpdater, cameraComponents.MainCamera.Aspect, backgroundStats);
                cameraComponents.Skybox.material = await MaterialFetcher.GetMaterialAsync(currentLevel.SkyMaterialName);
            }
            else
            {
                ai = helper.CreateAI(aiCruiser, playerCruiser, ApplicationModel.SelectedSideQuestID);
                backgroundStats = await helper.GetBackgroundStatsAsync(ApplicationModel.SelectedSideQuestID);
                components.CloudInitialiser.Initialise(currentSideQuest.SkyMaterial, components.UpdaterProvider.VerySlowUpdater, cameraComponents.MainCamera.Aspect, backgroundStats);

                cameraComponents.Skybox.material = await MaterialFetcher.GetMaterialAsync(currentSideQuest.SkyMaterial);
            }
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
            TutorialArgsBase tutorialArgs
                = new TutorialArgsBase(
                    playerCruiser,
                    aiCruiser,
                    _tutorialProvider,
                    components,
                    cameraComponents,
                    topPanelComponents,
                    leftPanelComponents,
                    rightPanelComponents,
                    uiManager,
                    _gameEndMonitor);
            tutorialInitialiser.Initialise(tutorialArgs, helper.ShowInGameHints, playerCruiserDamageMonitor);
            if (helper.ShowInGameHints)
            {
                uiManager.SetExplanationPanel(tutorialInitialiser.explanationPanel);
            }
            // Do not enable updates until asynchronous loading is complete.
            components.UpdaterProvider.SwitchableUpdater.Enabled = true;

            SceneNavigator.SceneLoaded(SceneNames.BATTLE_SCENE);

            //Code that uses current level to set the image of the enemy robot on the enemy nav button
            //Make sure to add more images to the EnemyCharacterImages prefab if more enemies are added
            /*if(enemyCharacterImages != null)
            {
                Image[] enemyImages = enemyCharacterImages.GetComponentsInChildren<Image>(true);
                Assert.IsTrue(enemyImages.Length >= currentLevel.Num);
                enemyImages[currentLevel.Num - 1].enabled = true;
            }*/

            toolTipActivator.Initialise();

            if (!aiCruiser.isCruiser)
            {
                aiCruiser.AdjustStatsByDifficulty(DataProvider.SettingsManager.AIDifficulty);
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
                //Debug.Log(DataProvider.SettingsManager.AIDifficulty);
            }
            deadBuildables = new Dictionary<TargetType, DeadBuildableCounter>();
            deadBuildables.Add(TargetType.Aircraft, new DeadBuildableCounter());
            deadBuildables.Add(TargetType.Ships, new DeadBuildableCounter());
            deadBuildables.Add(TargetType.Cruiser, new DeadBuildableCounter());
            deadBuildables.Add(TargetType.Buildings, new DeadBuildableCounter());
            deadBuildables.Add(TargetType.PlayedTime, new DeadBuildableCounter());

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
            if (LandingSceneGod.Instance.coinBattleLevelNum > 0)
                LandingSceneGod.Instance.coinBattleLevelNum = -2; //Erik - DestructionSceneGod will detect Coin battle mode through this
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

        /*        private void OnEnable()
                {
                    LandingSceneGod.SceneNavigator.SceneLoaded(SceneNames.BATTLE_SCENE);
                }*/

        private IBattleSceneHelper CreateHelper(
            IDeferrer deferrer,
            NavigationPermitters navigationPermitters)
        {
            switch (ApplicationModel.Mode)
            {
                case GameMode.Tutorial:
                    TutorialHelper helper = new TutorialHelper(navigationPermitters);
                    _tutorialProvider = helper;
                    return helper;

                case GameMode.Campaign:
                    return new NormalHelper(deferrer);

                case GameMode.Skirmish:
                    return new SkirmishHelper(deferrer);

                case GameMode.CoinBattle:
                    return new CoinBattleHelper(deferrer);

                case GameMode.SideQuest:
                    return new SideQuestHelper(deferrer);

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

        public void ToggleEnemyGodMode(bool isGodMode)
        {
            if (isGodMode)
            {
                aiCruiser.MakeInvincible();
            }
            else
            {
                aiCruiser.MakeDamagable();
            }
        }

        void OnApplicationQuit()
        {
            DataProvider.SaveGame();
            Debug.Log(DataProvider.GameModel.LifetimeDestructionScore);
            try
            {
                DataProvider.SaveGame();
                DataProvider.SyncCoinsToCloud();
                DataProvider.SyncCreditsToCloud();

                // Save changes:
                DataProvider.CloudSave();
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
        }
    }
}