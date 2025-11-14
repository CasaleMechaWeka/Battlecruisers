using System.Collections;
using System.Collections.Generic;
using BattleCruisers.Cruisers.Damage;
using BattleCruisers.Data;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Scenes.BattleScene;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Helpers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails;
using BattleCruisers.Scenes;
using BattleCruisers.Network.Multiplay.Scenes;
using UnityEngine;
using UnityEngine.Assertions;
using Unity.Multiplayer.Samples.Utilities;
using Unity.Netcode;
using BattleCruisers.UI.ScreensScene.Multiplay.ArenaScreen;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons;
using BattleCruisers.Network.Multiplay.MultiplayBattleScene.UI.BattleScene;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Helpers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Damage;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Timers;
using System.Threading.Tasks;
using BattleCruisers.Network.Multiplay.ConnectionManagement;
using BattleCruisers.Network.Multiplay.ApplicationLifecycle;
using BattleCruisers.Network.Multiplay.Gameplay.UI;
using BattleCruisers.Network.Multiplay.Infrastructure;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI;
using BattleCruisers.Data.Static;
using BattleCruisers.Utils.PlatformAbstractions.Time;
using BattleCruisers.UI.Music;
using BattleCruisers.UI.Sound.Wind;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.Buildables.Colours;
using BattleCruisers.Utils.PlatformAbstractions;
using System;
using BattleCruisers.UI.BattleScene.Clouds.Stats;
using BattleCruisers.UI.ScreensScene.BattleHubScreen;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene
{
    [RequireComponent(typeof(NetcodeHooks))]
    public class PvPBattleSceneGodClient : MonoBehaviour
    {
        private UserTargetTracker _userTargetTracker;
        private PvPAudioInitialiser _audioInitialiser;
        private PvPCruiserDeathManager _cruiserDeathManager;
        private PvPBattleSceneGodTunnel _battleSceneGodTunnel;
        private NavigationPermitters navigationPermitters;
        private PvPCameraComponents cameraComponents;

        public PvPCameraInitialiser cameraInitialiser;
        public PvPTopPanelInitialiser topPanelInitialiser;
        public PvPLeftPanelInitialiser leftPanelInitialiser;
        public PvPRightPanelInitialiser rightPanelInitialiser;
        public PvPBattleSceneGodComponents components;
        private PvPBattleSceneHelper pvpBattleHelper;
        private PvPLevel currentLevel;
        private PvPLeftPanelComponents leftPanelComponents;
        private ITime time;
        private IDebouncer _debouncer;
        private PvPBuildableButtonColourController _buildableButtonColourController;
        private PvPInformatorDismisser _informatorDismisser;
        private WindManager windManager;
        IDictionary<ulong, NetworkObject> storageOfNetworkObject = new Dictionary<ulong, NetworkObject>();
        private bool isReadyToShowCaptainExo = false;
        public PvPUIManager uiManager;
        public PvPCruiser playerCruiser;
        public PvPCruiser enemyCruiser;
        public CaptainExo leftCaptain, rightCaptain;
        public Transform leftContainer, rightContainer;
        private PvPCaptainExoHUDController captainController;
        public IUserChosenTargetHelper userChosenTargetHelper;
        public bool canFlee = true;
        private bool isCompletedBattleByFlee = false;
        private bool isStartedPvP = false;
        public bool wasOpponentDisconnected = false;
        public PvPBattleCompletionHandler battleCompletionHandler;
        public PvPMessageBox messageBox;

        // we need to have all UI refernece here to handle disconnection
        public GameObject obj_RedSeaGlow;
        public GameObject obj_HealthBarPanel;
        public GameObject obj_LeftBackgroundPanel;
        public GameObject obj_RightBackgroundPanel;
        public GameObject obj_ToolTipActivator;
        public bool IsBattleCompleted = false;
        public bool IsConnectedClient = false;   // this is only for Host
        public bool WasLeftMatch = false;
        private bool IsAIBotMode = false;
        public GameObject countdownGameObject;
        private Animator countdownAnimator;
        [SerializeField]
        NetcodeHooks m_NetcodeHooks;

        public static PvPBattleSceneGodClient Instance
        {
            get
            {
                if (s_pvpBattleSceneGodClient == null)
                {
                    s_pvpBattleSceneGodClient = FindObjectOfType<PvPBattleSceneGodClient>();
                }
                if (s_pvpBattleSceneGodClient == null)
                {
                    Debug.Log("No ServerSingleton in scene, did you run this from the bootstrap scene?");
                    return null;
                }
                return s_pvpBattleSceneGodClient;
            }
        }
        public void SetAIBotMode()
        {
            IsAIBotMode = true;
        }

        public bool IsAIBot()
        {
            return IsAIBotMode;
        }
        public void AddNetworkObject(NetworkObject obj)
        {
            Assert.IsNotNull(obj);
            Assert.IsFalse(storageOfNetworkObject.ContainsKey(obj.NetworkObjectId));
            storageOfNetworkObject.Add(obj.NetworkObjectId, obj);
        }
        public void RemoveNetworkObject(NetworkObject obj)
        {
            Assert.IsNotNull(obj);
            Assert.IsTrue(storageOfNetworkObject.ContainsKey(obj.NetworkObjectId));
            storageOfNetworkObject.Remove(obj.NetworkObjectId);
        }
        public NetworkObject GetNetworkObject(ulong networkObjectId)
        {
            if (!storageOfNetworkObject.ContainsKey(networkObjectId))
                return null;
            return storageOfNetworkObject[networkObjectId];
        }

        static PvPBattleSceneGodClient s_pvpBattleSceneGodClient;

        private void SetMatchmakingProcessing(bool value)
        {
            if (MatchmakingScreenController.Instance != null)
                MatchmakingScreenController.Instance.isProcessing = value;
            else if (PrivateMatchmakingController.Instance != null)
                PrivateMatchmakingController.Instance.isProcessing = value;
        }

        private void SetMatchmakingLoaded(bool value)
        {
            if (MatchmakingScreenController.Instance != null)
                MatchmakingScreenController.Instance.isLoaded = value;
            else if (PrivateMatchmakingController.Instance != null)
                PrivateMatchmakingController.Instance.isLoaded = value;
        }

        private void CallMatchmakingFoundCompetitor()
        {
            if (MatchmakingScreenController.Instance != null)
                MatchmakingScreenController.Instance.FoundCompetitor();
            else if (PrivateMatchmakingController.Instance != null)
                PrivateMatchmakingController.Instance.FoundCompetitor();
        }

        private void CallMatchmakingDisableAllAnimated()
        {
            if (MatchmakingScreenController.Instance != null)
                MatchmakingScreenController.Instance.DisableAllAnimatedGameObjects();
            else if (PrivateMatchmakingController.Instance != null)
                PrivateMatchmakingController.Instance.DisableAllAnimatedGameObjects();
        }

        private void SetMatchmakingAnimatorCompleted(bool value)
        {
            if (MatchmakingScreenController.Instance != null && MatchmakingScreenController.Instance.animator != null)
                MatchmakingScreenController.Instance.animator.SetBool("Completed", value);
            else if (PrivateMatchmakingController.Instance != null && PrivateMatchmakingController.Instance.animator != null)
                PrivateMatchmakingController.Instance.animator.SetBool("Completed", value);
        }

        void Awake()
        {
            Debug.Log($"PVP: Current scene={UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}");
            Debug.Log($"PVP: NetworkManager exists={NetworkManager.Singleton != null}");
            if (NetworkManager.Singleton != null)
            {
                Debug.Log($"PVP: IsHost={NetworkManager.Singleton.IsHost}, IsClient={NetworkManager.Singleton.IsClient}, ConnectedClients={NetworkManager.Singleton.ConnectedClientsIds.Count}");
            }
            Debug.Log($"PVP: SynchedServerData exists={SynchedServerData.Instance != null}");
            Debug.Log($"PVP: PrivateMatch={ArenaSelectPanelScreenController.PrivateMatch}");
            s_pvpBattleSceneGodClient = this;

            components = GetComponent<PvPBattleSceneGodComponents>();
            components.Initialise();
            components.UpdaterProvider.SwitchableUpdater.Enabled = false;
            PvPFactoryProvider.Setup(components);

            if (m_NetcodeHooks)
            {
                m_NetcodeHooks.OnNetworkSpawnHook += OnNetworkSpawn;
                m_NetcodeHooks.OnNetworkDespawnHook += OnNetworkDespawn;
            }
        }

        async void OnNetworkSpawn()
        {
            await StaticInitialiseAsync_Client();
        }
        void OnNetworkDespawn()
        {

        }

        void DetectClientDisconnection()
        {
            if (NetworkManager.Singleton != null && !WasLeftMatch)
            {
                if (!isCompletedBattleByFlee && canFlee && !NetworkManager.Singleton.IsHost && !NetworkManager.Singleton.IsConnectedClient)
                {
                    // the Host left match
                    isCompletedBattleByFlee = true;
                    wasOpponentDisconnected = true;
                    if (isStartedPvP)
                    {
                        // No-fault Disconnect:
                        if (!PvPBattleSceneGodTunnel.OpponentQuit)
                        {
                            Debug.Log("No-fault Disconnect");
                            HandleCruiserDestroyed();
                            cameraComponents.CruiserDeathCameraFocuser.FocusOnDisconnectedCruiser(NetworkManager.Singleton.IsHost);
                            _cruiserDeathManager.ShowDisconnectedCruiserExplosion();
                            PvPBattleSceneGodTunnel.isDisconnected = 1;
                            messageBox.ShowMessage(LocTableCache.CommonTable.GetString("EnemyLeft"), () => { messageBox.HideMessage(); });
                            IsBattleCompleted = true;
                            components.Deferrer.Defer(() => battleCompletionHandler.CompleteBattle(wasVictory: false, retryLevel: false, 1000), 5);
                            SetMatchmakingProcessing(false);
                        }
                        else
                        {
                            Debug.Log("Opponent Quit");
                            HandleCruiserDestroyed();
                            cameraComponents.CruiserDeathCameraFocuser.FocusOnDisconnectedCruiser(NetworkManager.Singleton.IsHost);
                            _cruiserDeathManager.ShowDisconnectedCruiserExplosion();
                            PvPBattleSceneGodTunnel.isDisconnected = 1;
                            messageBox.ShowMessage(LocTableCache.CommonTable.GetString("EnemyLeft"), () => { messageBox.HideMessage(); });
                            IsBattleCompleted = true;
                            components.Deferrer.Defer(() => battleCompletionHandler.CompleteBattle(wasVictory: true, retryLevel: false, 1000), 5);
                            SetMatchmakingProcessing(false);
                        }
                    }
                    else
                    {
                        HandleClientDisconnected();
                        PvPBattleSceneGodTunnel.isDisconnected = 1;
                        IsBattleCompleted = true;
                        //    messageBox.ShowMessage(LocTableFactory.CommonTable.GetString("EnemyLeft"), () => { messageBox.HideMessage(); });
                        battleCompletionHandler.CompleteBattle(wasVictory: true, retryLevel: false);
                        SetMatchmakingProcessing(false);
                    }
                }
                if (!isCompletedBattleByFlee && canFlee && NetworkManager.Singleton.IsHost && NetworkManager.Singleton.ConnectedClientsIds.Count != 2)
                {
                    // the Client left match
                    isCompletedBattleByFlee = true;
                    wasOpponentDisconnected = true;
                    if (isStartedPvP)
                    {
                        // No-fault Disconnect:
                        if (!PvPBattleSceneGodTunnel.OpponentQuit)
                        {
                            Debug.Log("No-fault Disconnect");
                            HandleCruiserDestroyed();
                            cameraComponents.CruiserDeathCameraFocuser.FocusOnDisconnectedCruiser(NetworkManager.Singleton.IsHost);
                            _cruiserDeathManager.ShowDisconnectedCruiserExplosion();
                            PvPBattleSceneGodTunnel.isDisconnected = 2;
                            messageBox.ShowMessage(LocTableCache.CommonTable.GetString("EnemyLeft"), () => { messageBox.HideMessage(); });
                            IsBattleCompleted = true;
                            components.Deferrer.Defer(() => battleCompletionHandler.CompleteBattle(wasVictory: false, retryLevel: false, 1000), 5);
                            SetMatchmakingProcessing(false);
                        }
                        // Opponent Quit:
                        else
                        {
                            Debug.Log("Opponent Quit");
                            HandleCruiserDestroyed();
                            cameraComponents.CruiserDeathCameraFocuser.FocusOnDisconnectedCruiser(NetworkManager.Singleton.IsHost);
                            _cruiserDeathManager.ShowDisconnectedCruiserExplosion();
                            PvPBattleSceneGodTunnel.isDisconnected = 2;
                            messageBox.ShowMessage(LocTableCache.CommonTable.GetString("EnemyLeft"), () => { messageBox.HideMessage(); });
                            IsBattleCompleted = true;
                            components.Deferrer.Defer(() => battleCompletionHandler.CompleteBattle(wasVictory: true, retryLevel: false, 1000), 5f);
                            SetMatchmakingProcessing(false);
                        }
                    }
                    else
                    {
                        HandleClientDisconnected();
                        PvPBattleSceneGodTunnel.isDisconnected = 2;
                        IsBattleCompleted = true;
                        //    messageBox.ShowMessage(LocTableFactory.CommonTable.GetString("EnemyLeft"), () => { messageBox.HideMessage(); });
                        battleCompletionHandler.CompleteBattle(wasVictory: true, retryLevel: false);
                        SetMatchmakingProcessing(false);
                    }
                }
            }
        }

        public async void DestroyAllNetworkObjects()
        {
            await Task.Delay(10);
            if (GameObject.Find("ApplicationController") != null)
                GameObject.Find("ApplicationController").GetComponent<ApplicationController>().DestroyNetworkObject();

            if (GameObject.Find("ConnectionManager") != null)
                GameObject.Find("ConnectionManager").GetComponent<ConnectionManager>().DestroyNetworkObject();

            if (GameObject.Find("PopupPanelManager") != null)
                GameObject.Find("PopupPanelManager").GetComponent<PopupManager>().DestroyNetworkObject();

            if (GameObject.Find("UIMessageManager") != null)
                GameObject.Find("UIMessageManager").GetComponent<ConnectionStatusMessageUIManager>().DestroyNetworkObject();

            if (GameObject.Find("UpdateRunner") != null)
                GameObject.Find("UpdateRunner").GetComponent<UpdateRunner>().DestroyNetworkObject();

            if (GameObject.Find("NetworkManager") != null)
                GameObject.Find("NetworkManager").GetComponent<BCNetworkManager>().DestroyNetworkObject();
        }
        void OnDestroy()
        {
            windManager?.Stop();
            windManager?.DisposeManagedState();
            if (m_NetcodeHooks)
            {
                m_NetcodeHooks.OnNetworkSpawnHook -= OnNetworkSpawn;
                m_NetcodeHooks.OnNetworkDespawnHook -= OnNetworkDespawn;
            }
        }
        public void StaticInitialiseAsync_Host()
        {
            PrioritisedSoundKeys.SetSoundKeys(DataProvider.SettingsManager.AltDroneSounds);

            _battleSceneGodTunnel = GetComponent<PvPBattleSceneGodTunnel>();
            battleCompletionHandler = new PvPBattleCompletionHandler();

            messageBox.gameObject.SetActive(true);
            messageBox.Initialize();
            messageBox.HideMessage();

            navigationPermitters = new NavigationPermitters();
            pvpBattleHelper = CreatePvPBattleHelper();
            uiManager = pvpBattleHelper.CreateUIManager();
            components.UpdaterProvider.SwitchableUpdater.Enabled = false;
            captainController = GetComponent<PvPCaptainExoHUDController>();
            SetMatchmakingProcessing(false);
        }
        private async Task StaticInitialiseAsync_Client()
        {
            PrioritisedSoundKeys.SetSoundKeys(DataProvider.SettingsManager.AltDroneSounds);

            _battleSceneGodTunnel = GetComponent<PvPBattleSceneGodTunnel>();
            battleCompletionHandler = new PvPBattleCompletionHandler();

            messageBox.gameObject.SetActive(true);
            messageBox.Initialize();
            messageBox.HideMessage();
            PvPFactoryProvider.Initialise_Sound();
            navigationPermitters = new NavigationPermitters();

            pvpBattleHelper = CreatePvPBattleHelper();
            uiManager = pvpBattleHelper.CreateUIManager();
            components.UpdaterProvider.SwitchableUpdater.Enabled = false;
            captainController = GetComponent<PvPCaptainExoHUDController>();

            SetMatchmakingProcessing(false);
        }
        private async void InitialiseAsync()
        {
            Debug.Log("PVP: InitialiseAsync START");

            if (!NetworkManager.Singleton.IsHost)
            {
                PvPBattleSceneGodTunnel._playerACruiserName = SynchedServerData.Instance.playerAPrefabName.Value;
            }
            PvPHelper.AssertIsNotNull(playerCruiser, enemyCruiser);
            battleCompletionHandler.registeredTime = Time.time;
            SetMatchmakingProcessing(false);
            SetMatchmakingLoaded(true);

            playerCruiser.StaticInitialise();
            enemyCruiser.StaticInitialise();

            Debug.Log("PVP: InitialiseAsync - cruisers initialized, starting camera/UI setup");

            cameraComponents = cameraInitialiser.Initialise(
            DataProvider.SettingsManager,
            playerCruiser,
            enemyCruiser,
            navigationPermitters,
            components.UpdaterProvider.SwitchableUpdater,
            PvPFactoryProvider.Sound.UISoundPlayer,
            SynchedServerData.Instance.GetTeam()
            );
            PvPCruiserHelper helper = CreatePlayerHelper(uiManager, cameraComponents.CameraFocuser);
            playerCruiser.Initialise_Client_PvP(uiManager, helper);
            enemyCruiser.Initialise_Client_PvP(uiManager, helper);
            currentLevel = pvpBattleHelper.GetPvPLevel();
            PrefabContainer<BackgroundImageStats> backgroundStats = await pvpBattleHelper.GetBackgroundStatsAsync(currentLevel.Num);
            components.CloudInitialiser.Initialise(currentLevel.SkyMaterialName, components.UpdaterProvider.VerySlowUpdater, cameraComponents.MainCamera.Aspect, backgroundStats);
            cameraComponents.Skybox.material = await MaterialFetcher.GetMaterialAsync(currentLevel.SkyMaterialName);

            PvPButtonVisibilityFilters buttonVisibilityFilters = pvpBattleHelper.CreateButtonVisibilityFilters(playerCruiser);
            _battleSceneGodTunnel.battleCompletionHandler = battleCompletionHandler;
            PvPTopPanelComponents topPanelComponents = topPanelInitialiser.Initialise(playerCruiser, enemyCruiser, SynchedServerData.Instance.playerAName.Value, SynchedServerData.Instance.playerBName.Value);
            leftPanelComponents
            = leftPanelInitialiser.Initialise(
            playerCruiser,
            uiManager,
            pvpBattleHelper.GetPlayerLoadout(),
            buttonVisibilityFilters,
            new PvPPlayerCruiserFocusHelper(cameraComponents.MainCamera, cameraComponents.CameraFocuser, playerCruiser, ApplicationModel.IsTutorial),
            PvPFactoryProvider.Sound.IPrioritisedSoundPlayer,
            PvPFactoryProvider.Sound.UISoundPlayer,
            playerCruiser.PopulationLimitMonitor,
            SynchedServerData.Instance.GetTeam() == Team.RIGHT);
            time = TimeBC.Instance;
            PauseGameManager pauseGameManager = new PauseGameManager(time);
            _debouncer = new Debouncer(time.RealTimeSinceGameStartProvider, debounceTimeInS: 30);
            playerCruiser.pvp_popLimitReachedFeedback.OnValueChanged += IsPopulationLimitReached_ValueChanged;
            playerCruiser.pvp_DroneNumIncreased.OnValueChanged += DroneNumIncreased_ValueChanged;
            playerCruiser.pvp_IdleDronesStarted.OnValueChanged += IdleDronesStarted_ValueChanged;
            playerCruiser.pvp_IdleDronesEnded.OnValueChanged += IdleDronesEnded_ValueChanged;

            IUserChosenTargetManager playerCruiserUserChosenTargetManager = new UserChosenTargetManager();
            userChosenTargetHelper
            = pvpBattleHelper.CreateUserChosenTargetHelper(
            playerCruiserUserChosenTargetManager,
            PvPFactoryProvider.Sound.IPrioritisedSoundPlayer,
            components.TargetIndicator);

            NavigationPermitterManager navigationPermitterManager = new NavigationPermitterManager(navigationPermitters);
            PvPRightPanelComponents rightPanelComponents
            = rightPanelInitialiser.Initialise(
            uiManager,
            playerCruiser,
            userChosenTargetHelper,
            buttonVisibilityFilters,
            PvPFactoryProvider.UpdaterProvider.PerFrameUpdater,
            pauseGameManager,
            battleCompletionHandler,
            PvPFactoryProvider.Sound.UISoundPlayer,
            navigationPermitterManager
            );

            PvPItemDetailsManager itemDetailsManager = new PvPItemDetailsManager(rightPanelComponents.InformatorPanel);
            _userTargetTracker = new UserTargetTracker(itemDetailsManager.SelectedItem, new UserTargetsColourChanger());
            _buildableButtonColourController = new PvPBuildableButtonColourController(itemDetailsManager.SelectedItem, leftPanelComponents.BuildMenu.BuildableButtons);

            pvpBattleHelper.InitialiseUIManager(
            playerCruiser,
            enemyCruiser,
            leftPanelComponents.BuildMenu,
            itemDetailsManager,
            PvPFactoryProvider.Sound.IPrioritisedSoundPlayer,
            PvPFactoryProvider.Sound.UISoundPlayer);

            _informatorDismisser = new PvPInformatorDismisser(components.BackgroundClickableEmitter, uiManager, rightPanelComponents.HacklePanelController);

            Debug.Log("PVP: InitialiseAsync - UI setup complete, loading music/audio");

            LayeredMusicPlayer layeredMusicPlayer
            = await components.MusicPlayerInitialiser.CreatePlayerAsync(
            currentLevel.MusicKeys,
            DataProvider.SettingsManager);
            ICruiserDamageMonitor playerCruiserDamageMonitor = new PvPCruiserDamageMonitor(playerCruiser);
            _audioInitialiser
            = new PvPAudioInitialiser(
            pvpBattleHelper,
            layeredMusicPlayer,
            playerCruiser,
            enemyCruiser,
            components.Deferrer,
            time,
            battleCompletionHandler,
            playerCruiserDamageMonitor,
            leftPanelComponents.PopLimitReachedFeedback);
            windManager
            = components.WindInitialiser.Initialise(
            cameraComponents.MainCamera,
            cameraComponents.Settings,
            DataProvider.SettingsManager);
            windManager.Play();
            _cruiserDeathManager = new PvPCruiserDeathManager(playerCruiser, enemyCruiser);
            components.HotkeyInitialiser.Initialise(
            DataProvider.GameModel.Hotkeys,
            InputBC.Instance,
            components.UpdaterProvider.SwitchableUpdater,
            navigationPermitters.HotkeyFilter,
            cameraComponents.CameraFocuser,
            rightPanelComponents.MainMenuManager);
            playerCruiser.Destroyed += PlayerCruiser_Destroyed;
            enemyCruiser.Destroyed += EnemyCruiser_Destroyed;

            if (SynchedServerData.Instance != null)
            {
                SynchedServerData.Instance.captainAPrefabName.OnValueChanged += CaptainAPrefabNameChanged;
                SynchedServerData.Instance.captainBPrefabName.OnValueChanged += CaptainBPrefabNameChanged;
            }
            isReadyToShowCaptainExo = true;

            Debug.Log("PVP: InitialiseAsync - loading captains");
            await LoadAllCaptains();
            Debug.Log("PVP: InitialiseAsync - captains loaded, finalizing");

            countdownAnimator = countdownGameObject.GetComponent<Animator>();
            if (countdownAnimator == null)
                Debug.LogError("CountdownGameObject is missing the animator component");

            PvPHeckleMessageManager.Instance.Initialise(PvPFactoryProvider.Sound.UISoundPlayer);
            _ = PvPPrefabCache.CreatePvPPrefabCacheAsync();
            CallMatchmakingFoundCompetitor();

            Debug.Log("PVP: InitialiseAsync - starting wait loop coroutine");
            StartCoroutine(iLoadedPvPScene());

            ApplicationModel.Mode = BattleCruisers.Data.GameMode.PvP_1VS1;
            Invoke("ApplyEconomy", 60f);
            if (NetworkManager.Singleton.IsHost)
            {
                PvPBattleSceneGodServer.Instance.playerASelectedVariants = DataProvider.GameModel.PlayerLoadout.SelectedVariants;
            }

            Debug.Log("PVP: InitialiseAsync COMPLETE");
        }
        private async void ApplyEconomy()
        {
            if (!WasLeftMatch && !wasOpponentDisconnected)
            {
                DataProvider.GameModel.Coins -= StaticData.Arenas[DataProvider.GameModel.GameMap + 1].costcoins;
                DataProvider.GameModel.Credits -= StaticData.Arenas[DataProvider.GameModel.GameMap + 1].costcredits;
                DataProvider.SaveGame();
                PvPBattleSceneGodTunnel.isCost = true;
                await DataProvider.SyncCoinsToCloud();
                await DataProvider.SyncCreditsToCloud();
            }
        }

        private async Task LoadAllCaptains()
        {
            if (SynchedServerData.Instance == null)
                return;
            if (SynchedServerData.Instance.GetTeam() == Team.LEFT)
            {

                PrefabContainer<Prefab> resultA = await PrefabFetcher.GetPrefabAsync<Prefab>(DataProvider.GameModel.PlayerLoadout.CurrentCaptain);
                resultA.Prefab.StaticInitialise();
                if (leftCaptain == null)
                {
                    leftCaptain = Instantiate(resultA.Prefab, leftContainer) as CaptainExo;
                    leftCaptain.transform.localScale = Vector3.one * 0.5f;
                }


                string captainBName = SynchedServerData.Instance.captainBPrefabName.Value.ToString();
                if (!string.IsNullOrWhiteSpace(captainBName) && captainBName.StartsWith("Captain"))
                {
                    PrefabContainer<Prefab> resultB = await PrefabFetcher.GetPrefabAsync<Prefab>(new CaptainExoKey(captainBName));
                    resultB.Prefab.StaticInitialise();
                    if (rightCaptain == null)
                    {
                        rightCaptain = Instantiate(resultB.Prefab, rightContainer) as CaptainExo;
                        rightCaptain.transform.localScale = Vector3.one * 0.5f;
                    }
                }
                else
                {
                    Debug.LogWarning($"PVP: LoadAllCaptains - Invalid captainBPrefabName, using default CaptainExo000");
                    PrefabContainer<Prefab> resultB = await PrefabFetcher.GetPrefabAsync<Prefab>(new CaptainExoKey("CaptainExo000"));
                    resultB.Prefab.StaticInitialise();
                    if (rightCaptain == null)
                    {
                        rightCaptain = Instantiate(resultB.Prefab, rightContainer) as CaptainExo;
                        rightCaptain.transform.localScale = Vector3.one * 0.5f;
                    }
                }
            }
            else
            {
                PrefabContainer<Prefab> resultB = await PrefabFetcher.GetPrefabAsync<Prefab>(DataProvider.GameModel.PlayerLoadout.CurrentCaptain);
                resultB.Prefab.StaticInitialise();
                if (rightCaptain == null)
                {
                    rightCaptain = Instantiate(resultB.Prefab, rightContainer) as CaptainExo;
                    rightCaptain.transform.localScale = Vector3.one * 0.5f;
                }

                try
                {
                    string captainAName = SynchedServerData.Instance.captainAPrefabName.Value.ToString();
                    if (!string.IsNullOrWhiteSpace(captainAName) && captainAName.StartsWith("Captain"))
                    {
                        PrefabContainer<Prefab> resultA = await PrefabFetcher.GetPrefabAsync<Prefab>(new CaptainExoKey(captainAName));
                        resultA.Prefab.StaticInitialise();
                        if (leftCaptain == null)
                        {
                            leftCaptain = Instantiate(resultA.Prefab, leftContainer) as CaptainExo;
                            leftCaptain.transform.localScale = Vector3.one * 0.5f;
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"PVP: LoadAllCaptains - Invalid captainAPrefabName, using default CaptainExo000");
                        PrefabContainer<Prefab> resultA = await PrefabFetcher.GetPrefabAsync<Prefab>(new CaptainExoKey("CaptainExo000"));
                        resultA.Prefab.StaticInitialise();
                        if (leftCaptain == null)
                        {
                            leftCaptain = Instantiate(resultA.Prefab, leftContainer) as CaptainExo;
                            leftCaptain.transform.localScale = Vector3.one * 0.5f;
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"PVP: LoadAllCaptains exception loading captainA: {e.Message}");
                    PrefabContainer<Prefab> resultA = await PrefabFetcher.GetPrefabAsync<Prefab>(new CaptainExoKey("CaptainExo000"));
                    resultA.Prefab.StaticInitialise();
                    if (leftCaptain == null)
                    {
                        leftCaptain = Instantiate(resultA.Prefab, leftContainer) as CaptainExo;
                        leftCaptain.transform.localScale = Vector3.one * 0.5f;
                    }
                }
            }
            captainController.Initialize(leftCaptain, rightCaptain);
        }
        private async void CaptainAPrefabNameChanged(NetworkString oldVal, NetworkString newVal)
        {
            string captainName = newVal.ToString();
            if (!string.IsNullOrWhiteSpace(captainName) && captainName.StartsWith("Captain"))
            {
                PrefabContainer<Prefab> resultA = await PrefabFetcher.GetPrefabAsync<Prefab>(new CaptainExoKey(captainName));
                resultA.Prefab.StaticInitialise();

                if (SynchedServerData.Instance.GetTeam() == Team.LEFT)
                {
                    if (leftCaptain == null)
                        leftCaptain = Instantiate(resultA.Prefab, leftContainer) as CaptainExo;
                }
                else
                {
                    if (rightCaptain == null)
                        rightCaptain = Instantiate(resultA.Prefab, rightContainer) as CaptainExo;
                }
            }
        }

        private async void CaptainBPrefabNameChanged(NetworkString oldVal, NetworkString newVal)
        {
            string captainName = newVal.ToString();
            if (!string.IsNullOrWhiteSpace(captainName) && captainName.StartsWith("Captain"))
            {
                PrefabContainer<Prefab> resultB = await PrefabFetcher.GetPrefabAsync<Prefab>(new CaptainExoKey(captainName));
                resultB.Prefab.StaticInitialise();

                if (SynchedServerData.Instance.GetTeam() == Team.LEFT)
                {
                    if (rightCaptain == null)
                        rightCaptain = Instantiate(resultB.Prefab, rightContainer) as CaptainExo;
                }
                else
                {
                    if (leftCaptain == null)
                        leftCaptain = Instantiate(resultB.Prefab, leftContainer) as CaptainExo;
                }
            }
        }

        private void PlayerCruiser_Destroyed(object sender, DestroyedEventArgs e)
        {
            cameraComponents.CruiserDeathCameraFocuser.FocusOnLosingCruiser(playerCruiser);
            HandleCruiserDestroyed();
            Debug.Log($"Left Destruction Score: {SynchedServerData.Instance.left_destructionScore.Value} Right Destruction Score: {SynchedServerData.Instance.right_destructionScore.Value}");
            playerCruiser.Destroyed -= PlayerCruiser_Destroyed;
            /* 
            if (SynchedServerData.Instance.GetTeam() == Team.LEFT)
            {
                PvPBattleSceneGodTunnel._levelTimeInSeconds = SynchedServerData.Instance.left_levelTimeInSeconds.Value;
                PvPBattleSceneGodTunnel._aircraftVal = SynchedServerData.Instance.left_aircraftVal.Value;
                PvPBattleSceneGodTunnel._shipsVal = SynchedServerData.Instance.left_shipsVal.Value;
                PvPBattleSceneGodTunnel._cruiserVal = SynchedServerData.Instance.left_cruiserVal.Value;
                PvPBattleSceneGodTunnel._buildingsVal = SynchedServerData.Instance.left_buildingsVal.Value;
                PvPBattleSceneGodTunnel._enemyCruiserName = enemyCruiser.stringKeyBase;
                PvPBattleSceneGodTunnel._totalDestroyed = new long[4]
                {   SynchedServerData.Instance.left_totalDestroyed1.Value,
                    SynchedServerData.Instance.left_totalDestroyed2.Value,
                    SynchedServerData.Instance.left_totalDestroyed3.Value,
                    SynchedServerData.Instance.left_totalDestroyed4.Value,
                };
                GetComponent<PvPBattleSceneGodTunnel>().battleCompletionHandler?.CompleteBattle(false, false, SynchedServerData.Instance.left_destructionScore.Value);
            }
            */
        }

        private void EnemyCruiser_Destroyed(object sender, DestroyedEventArgs e)
        {
            cameraComponents.CruiserDeathCameraFocuser.FocusOnLosingCruiser(enemyCruiser);
            HandleCruiserDestroyed();
            Debug.Log($"Left Destruction Score: {SynchedServerData.Instance.left_destructionScore.Value} Right Destruction Score: {SynchedServerData.Instance.right_destructionScore.Value}");
            enemyCruiser.Destroyed -= EnemyCruiser_Destroyed;
            /*
            if (SynchedServerData.Instance.GetTeam() == Team.LEFT)
            {
                PvPBattleSceneGodTunnel._levelTimeInSeconds = SynchedServerData.Instance.left_levelTimeInSeconds.Value;
                PvPBattleSceneGodTunnel._aircraftVal = SynchedServerData.Instance.left_aircraftVal.Value;
                PvPBattleSceneGodTunnel._shipsVal = SynchedServerData.Instance.left_shipsVal.Value;
                PvPBattleSceneGodTunnel._cruiserVal = SynchedServerData.Instance.left_cruiserVal.Value;
                PvPBattleSceneGodTunnel._buildingsVal = SynchedServerData.Instance.left_buildingsVal.Value;
                PvPBattleSceneGodTunnel._enemyCruiserName = enemyCruiser.stringKeyBase;
                PvPBattleSceneGodTunnel._totalDestroyed = new long[4]
                {   SynchedServerData.Instance.right_totalDestroyed1.Value,
                    SynchedServerData.Instance.right_totalDestroyed2.Value,
                    SynchedServerData.Instance.right_totalDestroyed3.Value,
                    SynchedServerData.Instance.right_totalDestroyed4.Value,
                };
                GetComponent<PvPBattleSceneGodTunnel>().battleCompletionHandler?.CompleteBattle(true, false, SynchedServerData.Instance.left_destructionScore.Value);
            }
            */
        }

        public void HandleCruiserDestroyed()
        {
            canFlee = false;
            PvPFactoryProvider.Sound.IPrioritisedSoundPlayer.Enabled = false;
            navigationPermitters.NavigationFilter.IsMatch = false;
            uiManager.HideCurrentlyShownMenu();
            uiManager.HideItemDetails();
            components.targetIndicator.Hide();
            pvpBattleHelper.BuildingCategoryPermitter.AllowNoCategories();
        }

        public void HandleClientDisconnected()
        {
            canFlee = false;
            if (null != obj_RedSeaGlow)
                obj_RedSeaGlow.SetActive(false);

            if (null != obj_HealthBarPanel)
                obj_HealthBarPanel.SetActive(false);

            if (null != obj_LeftBackgroundPanel)
                obj_LeftBackgroundPanel.SetActive(false);

            if (null != obj_RightBackgroundPanel)
                obj_RightBackgroundPanel.SetActive(false);

            if (null != obj_ToolTipActivator)
                obj_ToolTipActivator.SetActive(false);

            if (null != countdownGameObject)
                countdownGameObject.SetActive(false);
        }

        public void OnTunnelBattleCompleted_ValueChanged(/*Tunnel_BattleCompletedState oldVal, Tunnel_BattleCompletedState newVal*/)
        {
            windManager?.Stop();
            windManager?.DisposeManagedState();
        }

        private PvPCruiserHelper CreatePlayerHelper(PvPUIManager uiManager, ICameraFocuser cameraFocuser)
        {
            return new PvPPlayerCruiserHelper(uiManager, cameraFocuser);
        }

        private void IsPopulationLimitReached_ValueChanged(bool oldVal, bool newVal)
        {
            if (newVal)
            {
                _debouncer.Debounce(() => PvPFactoryProvider.Sound.IPrioritisedSoundPlayer.PlaySound(PrioritisedSoundKeys.Events.PopulationLimitReached));
            }
            leftPanelComponents.PopLimitReachedFeedback.IsVisible = newVal;
        }

        private void DroneNumIncreased_ValueChanged(bool oldVal, bool newVal)
        {
            PvPFactoryProvider.Sound.IPrioritisedSoundPlayer.PlaySound(PrioritisedSoundKeys.Events.Drones.NewDronesReady);
        }

        private void IdleDronesStarted_ValueChanged(bool oldVal, bool newVal)
        {
            new Debouncer(TimeBC.Instance.RealTimeSinceGameStartProvider, debounceTimeInS: 20).Debounce(() => PvPFactoryProvider.Sound.IPrioritisedSoundPlayer.PlaySound(PrioritisedSoundKeys.Events.Drones.Idle));
        }

        private void IdleDronesEnded_ValueChanged(bool oldVal, bool newVal)
        {

        }
        private async void Update()
        {
            if (isReadyToShowCaptainExo && (leftCaptain == null || rightCaptain == null))
            {
                await LoadAllCaptains();
            }

            if (battleCompletionHandler == null)
                return;

            if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsHost)
            {
                if (IsConnectedClient)
                    DetectClientDisconnection();
                if (NetworkManager.Singleton.ConnectedClientsIds.Count == 2)
                {
                    if (!IsConnectedClient)
                        IsConnectedClient = true;
                }
            }
            else
            {
                DetectClientDisconnection();
            }
        }
        IEnumerator iLoadedPvPScene()
        {
            float timeout = 30f;
            float elapsed = 0f;

            Debug.Log($"PVP: wait loop START - PrivateMatch={BattleCruisers.UI.ScreensScene.BattleHubScreen.ArenaSelectPanelScreenController.PrivateMatch}");

            while (elapsed < timeout)
            {
                bool bothClientsConnected = NetworkManager.Singleton != null
                    && NetworkManager.Singleton.ConnectedClientsIds.Count >= 2;
                bool syncDataReady = SynchedServerData.Instance != null;

                if (bothClientsConnected && syncDataReady)
                {
                    break;
                }

                yield return new WaitForSeconds(0.1f);
                elapsed += 0.1f;
            }

            Debug.Log($"PVP: wait loop DONE after {elapsed:F1}s - ConnectedClients={NetworkManager.Singleton?.ConnectedClientsIds.Count ?? 0}, SyncData={SynchedServerData.Instance != null}");

            if (elapsed >= timeout)
            {
                Debug.LogWarning($"iLoadedPvPScene timeout after {timeout}s - ConnectedClients={NetworkManager.Singleton?.ConnectedClientsIds.Count ?? 0}, SyncData={SynchedServerData.Instance != null}");
            }

            // yield return new WaitForSeconds(5f);

            Debug.Log("PVP: About to call SetMatchmakingAnimatorCompleted");
            SetMatchmakingAnimatorCompleted(false);
            Debug.Log("PVP: About to call CallMatchmakingDisableAllAnimated");
            CallMatchmakingDisableAllAnimated();

            string matchmakingScene = ArenaSelectPanelScreenController.PrivateMatch
                ? SceneNames.PRIVATE_PVP_INITIALIZER_SCENE
                : SceneNames.PvP_INITIALIZE_SCENE;
            Debug.Log($"PVP: About to call SceneNavigator.SceneLoaded({matchmakingScene})");
            SceneNavigator.SceneLoaded(matchmakingScene);

            if (PvPBootManager.Instance?.LobbyServiceFacade != null)
            {
                Debug.Log("PVP: About to pause lobby tracking");
                PvPBootManager.Instance.LobbyServiceFacade.PauseTracking();
            }

            Debug.Log("PVP: About to start countdown animation");
            PlayCountDownAnimation();
            if (SynchedServerData.Instance != null)
            {
                if (SynchedServerData.Instance.GetTeam() == Team.LEFT)
                    cameraComponents.CameraFocuser.FocusOnLeftCruiser();
                else if (SynchedServerData.Instance.GetTeam() == Team.RIGHT)
                    cameraComponents.CameraFocuser.FocusOnRightCruiser();

                SetMatchmakingProcessing(false);
                SetMatchmakingLoaded(true);
                components.UpdaterProvider.SwitchableUpdater.Enabled = true;
            }
        }
        public void RegisterAsPlayer(PvPCruiser _cruiser)
        {
            Assert.IsNotNull(_cruiser);
            Debug.Log("PVP: RegisterAsPlayer called");
            playerCruiser = _cruiser;
            if (enemyCruiser != null)
            {
                Debug.Log("PVP: Both cruisers registered - starting InitialiseAsync");
                InitialiseAsync();
            }
            else
            {
                Debug.Log("PVP: Waiting for enemy cruiser registration");
            }
        }
        public void RegisterAsEnemy(PvPCruiser _cruiser)
        {
            Assert.IsNotNull(_cruiser);
            Debug.Log("PVP: RegisterAsEnemy called");
            enemyCruiser = _cruiser;
            if (playerCruiser != null)
            {
                Debug.Log("PVP: Both cruisers registered - starting InitialiseAsync");
                InitialiseAsync();
            }
            else
            {
                Debug.Log("PVP: Waiting for player cruiser registration");
            }
        }
        private PvPBattleSceneHelper CreatePvPBattleHelper()
        {
            return new PvPBattleHelper();
        }
        private void PlayCountDownAnimation()
        {
            Debug.Log("PVP: PlayCountDownAnimation called");
            if (countdownGameObject == null || countdownAnimator == null)
            {
                Debug.LogWarning("PVP: Countdown objects are null!");
                return;
            }

            countdownGameObject.SetActive(true);
            Debug.Log($"PVP: Countdown GameObject activated");

            countdownAnimator.Play(0);
            Debug.Log($"PVP: Countdown animation started (length={countdownAnimator.GetCurrentAnimatorStateInfo(0).length}s)");

            StartCoroutine(WaitForAnimationToEnd());
        }
        private IEnumerator WaitForAnimationToEnd()
        {
            float animLength = countdownAnimator.GetCurrentAnimatorStateInfo(0).length;
            Debug.Log($"PVP: Waiting {animLength}s for countdown animation to complete");
            yield return new WaitForSeconds(animLength);

            countdownGameObject.SetActive(false);
            Debug.Log("PVP: Countdown complete - gameplay started!");

            // Start gameplay AFTER countdown completes (both HOST and CLIENT run this independently)
            isStartedPvP = true;
            Debug.Log("PVP: Countdown complete - gameplay started!");
        }
    }
}