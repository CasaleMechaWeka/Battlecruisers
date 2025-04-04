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
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Helpers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails;
using BattleCruisers.Scenes;
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
        private IPvPCameraComponents cameraComponents;

        public PvPCameraInitialiser cameraInitialiser;
        public PvPTopPanelInitialiser topPanelInitialiser;
        public PvPLeftPanelInitialiser leftPanelInitialiser;
        public PvPRightPanelInitialiser rightPanelInitialiser;
        public PvPBattleSceneGodComponents components;
        private IPvPBattleSceneHelper pvpBattleHelper;
        private IPvPLevel currentLevel;
        private PvPLeftPanelComponents leftPanelComponents;
        private ITime time;
        private IDebouncer _debouncer;
        private PvPBuildableButtonColourController _buildableButtonColourController;
        private PvPInformatorDismisser _informatorDismisser;
        private IWindManager windManager;
        ISceneNavigator sceneNavigator;
        IDictionary<ulong, NetworkObject> storageOfNetworkObject = new Dictionary<ulong, NetworkObject>();
        private bool isReadyToShowCaptainExo = false;
        public IPvPUIManager uiManager;
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
        public IPvPBattleCompletionHandler battleCompletionHandler;
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

        void Awake()
        {
            s_pvpBattleSceneGodClient = this;
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
                            MatchmakingScreenController.Instance.isProcessing = false;
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
                            MatchmakingScreenController.Instance.isProcessing = false;
                        }
                    }
                    else
                    {
                        HandleClientDisconnected();
                        PvPBattleSceneGodTunnel.isDisconnected = 1;
                        IsBattleCompleted = true;
                        //    messageBox.ShowMessage(LocTableFactory.CommonTable.GetString("EnemyLeft"), () => { messageBox.HideMessage(); });
                        battleCompletionHandler.CompleteBattle(wasVictory: true, retryLevel: false);
                        MatchmakingScreenController.Instance.isProcessing = false;
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
                            MatchmakingScreenController.Instance.isProcessing = false;
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
                            MatchmakingScreenController.Instance.isProcessing = false;
                        }
                    }
                    else
                    {
                        HandleClientDisconnected();
                        PvPBattleSceneGodTunnel.isDisconnected = 2;
                        IsBattleCompleted = true;
                        //    messageBox.ShowMessage(LocTableFactory.CommonTable.GetString("EnemyLeft"), () => { messageBox.HideMessage(); });
                        battleCompletionHandler.CompleteBattle(wasVictory: true, retryLevel: false);
                        MatchmakingScreenController.Instance.isProcessing = false;
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
            components = GetComponent<PvPBattleSceneGodComponents>();

            _battleSceneGodTunnel = GetComponent<PvPBattleSceneGodTunnel>();
            sceneNavigator = LandingSceneGod.SceneNavigator;
            battleCompletionHandler = new PvPBattleCompletionHandler(sceneNavigator);

            messageBox.gameObject.SetActive(true);
            messageBox.Initialize();
            messageBox.HideMessage();

            Assert.IsNotNull(components);
            components.Initialise();
            navigationPermitters = new NavigationPermitters();
            pvpBattleHelper = CreatePvPBattleHelper();
            uiManager = pvpBattleHelper.CreateUIManager();
            PvPFactoryProvider.Initialise(components);
            components.UpdaterProvider.SwitchableUpdater.Enabled = false;
            captainController = GetComponent<PvPCaptainExoHUDController>();
            MatchmakingScreenController.Instance.isProcessing = false;
        }
        private async Task StaticInitialiseAsync_Client()
        {
            PrioritisedSoundKeys.SetSoundKeys(DataProvider.SettingsManager.AltDroneSounds);
            components = GetComponent<PvPBattleSceneGodComponents>();

            _battleSceneGodTunnel = GetComponent<PvPBattleSceneGodTunnel>();
            sceneNavigator = LandingSceneGod.SceneNavigator;
            battleCompletionHandler = new PvPBattleCompletionHandler(sceneNavigator);

            messageBox.gameObject.SetActive(true);
            messageBox.Initialize();
            messageBox.HideMessage();

            Assert.IsNotNull(components);
            components.Initialise();

            await PvPPrefabCache.CreatePvPPrefabCacheAsync();

            navigationPermitters = new NavigationPermitters();

            pvpBattleHelper = CreatePvPBattleHelper();
            uiManager = pvpBattleHelper.CreateUIManager();
            PvPFactoryProvider.Initialise(components);
            components.UpdaterProvider.SwitchableUpdater.Enabled = false;
            captainController = GetComponent<PvPCaptainExoHUDController>();
            MatchmakingScreenController.Instance.isProcessing = false;
        }
        private async void InitialiseAsync()
        {
            if (!NetworkManager.Singleton.IsHost)
            {
                PvPBattleSceneGodTunnel._playerACruiserName = SynchedServerData.Instance.playerAPrefabName.Value;
                // variants
                //            GetComponent<PvPBattleSceneGodServer>().GetSelectedVariantsFromString();
            }
            PvPHelper.AssertIsNotNull(playerCruiser, enemyCruiser);
            battleCompletionHandler.registeredTime = Time.time;
            MatchmakingScreenController.Instance.isProcessing = false;
            MatchmakingScreenController.Instance.isLoaded = true;
            MatchmakingScreenController.Instance.AddProgress(1000);
            playerCruiser.StaticInitialise();
            enemyCruiser.StaticInitialise();
            cameraComponents = cameraInitialiser.Initialise(
                DataProvider.SettingsManager,
                playerCruiser,
                enemyCruiser,
                navigationPermitters,
                components.UpdaterProvider.SwitchableUpdater,
                PvPFactoryProvider.Sound.UISoundPlayer,
                SynchedServerData.Instance.GetTeam()
            );
            IPvPCruiserHelper helper = CreatePlayerHelper(uiManager, cameraComponents.CameraFocuser);
            playerCruiser.Initialise_Client_PvP(uiManager, helper);
            enemyCruiser.Initialise_Client_PvP(uiManager, helper);
            currentLevel = pvpBattleHelper.GetPvPLevel();
            PrefabContainer<BackgroundImageStats> backgroundStats = await pvpBattleHelper.GetBackgroundStatsAsync(currentLevel.Num);
            components.CloudInitialiser.Initialise(currentLevel.SkyMaterialName, components.UpdaterProvider.VerySlowUpdater, cameraComponents.MainCamera.Aspect, backgroundStats);
            await components.SkyboxInitialiser.InitialiseAsync(cameraComponents.Skybox, currentLevel);

            IPvPButtonVisibilityFilters buttonVisibilityFilters = pvpBattleHelper.CreateButtonVisibilityFilters(playerCruiser);
            _battleSceneGodTunnel.battleCompletionHandler = battleCompletionHandler;
            PvPTopPanelComponents topPanelComponents = topPanelInitialiser.Initialise(playerCruiser, enemyCruiser, SynchedServerData.Instance.playerAName.Value, SynchedServerData.Instance.playerBName.Value);
            leftPanelComponents
                = leftPanelInitialiser.Initialise(
                    playerCruiser,
                    uiManager,
                    pvpBattleHelper.GetPlayerLoadout(),
                    buttonVisibilityFilters,
                    new PvPPlayerCruiserFocusHelper(cameraComponents.MainCamera, cameraComponents.CameraFocuser, playerCruiser, ApplicationModel.IsTutorial),
                    PvPFactoryProvider.Sound.PrioritisedSoundPlayer,
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
                            PvPFactoryProvider.Sound.PrioritisedSoundPlayer,
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

            IPvPItemDetailsManager itemDetailsManager = new PvPItemDetailsManager(rightPanelComponents.InformatorPanel);
            _userTargetTracker = new UserTargetTracker(itemDetailsManager.SelectedItem, new UserTargetsColourChanger());
            _buildableButtonColourController = new PvPBuildableButtonColourController(itemDetailsManager.SelectedItem, leftPanelComponents.BuildMenu.BuildableButtons);
            PvPManagerArgs args
                = new PvPManagerArgs(
                    playerCruiser,
                    enemyCruiser,
                    leftPanelComponents.BuildMenu,
                    itemDetailsManager,
                    PvPFactoryProvider.Sound.PrioritisedSoundPlayer,
                    PvPFactoryProvider.Sound.UISoundPlayer);
            pvpBattleHelper.InitialiseUIManager(args);
            _informatorDismisser = new PvPInformatorDismisser(components.BackgroundClickableEmitter, uiManager, rightPanelComponents.HacklePanelController);
            // Audio
            ILayeredMusicPlayer layeredMusicPlayer
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
            // Captains
            if (SynchedServerData.Instance != null)
            {
                SynchedServerData.Instance.captainAPrefabName.OnValueChanged += CaptainAPrefabNameChanged;
                SynchedServerData.Instance.captainBPrefabName.OnValueChanged += CaptainBPrefabNameChanged;
            }
            isReadyToShowCaptainExo = true;
            await Task.Delay(1000);
            await LoadAllCaptains();
            countdownAnimator = countdownGameObject.GetComponent<Animator>();
            if (countdownAnimator == null)
                Debug.LogError("CountdownGameObject is missing the animator component");
            // pvp
            PvPHeckleMessageManager.Instance.Initialise(PvPFactoryProvider.Sound.UISoundPlayer);
            MatchmakingScreenController.Instance.FoundCompetitor();
            StartCoroutine(iLoadedPvPScene());
            ApplicationModel.Mode = BattleCruisers.Data.GameMode.PvP_1VS1;
            // apply economy because here is end of starting PvPbattle.
            Invoke("ApplyEconomy", 60f);
            isStartedPvP = true;
            if (NetworkManager.Singleton.IsHost)
            {
                PvPBattleSceneGodServer.Instance.playerASelectedVariants = DataProvider.GameModel.PlayerLoadout.SelectedVariants;
                PvPBattleSceneGodServer.Instance.Initialise_Rest();
            }
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


                if (SynchedServerData.Instance.captainBPrefabName.Value.ToString() != string.Empty)
                {
                    PrefabContainer<Prefab> resultB = await PrefabFetcher.GetPrefabAsync<Prefab>(new CaptainExoKey(SynchedServerData.Instance.captainBPrefabName.Value.ToString()));
                    resultB.Prefab.StaticInitialise();
                    if (rightCaptain == null)
                    {
                        rightCaptain = Instantiate(resultB.Prefab, rightContainer) as CaptainExo;
                        rightCaptain.transform.localScale = Vector3.one * 0.5f;
                    }
                }
                else
                {
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
                    if (SynchedServerData.Instance.captainAPrefabName.Value.ToString() != string.Empty)
                    {
                        PrefabContainer<Prefab> resultA = await PrefabFetcher.GetPrefabAsync<Prefab>(new CaptainExoKey(SynchedServerData.Instance.captainAPrefabName.Value.ToString()));
                        resultA.Prefab.StaticInitialise();
                        if (leftCaptain == null)
                        {
                            leftCaptain = Instantiate(resultA.Prefab, leftContainer) as CaptainExo;
                            leftCaptain.transform.localScale = Vector3.one * 0.5f;
                        }
                    }
                    else
                    {
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
                    Debug.LogError(e.Message);
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
            if (newVal.ToString() != string.Empty)
            {
                PrefabContainer<Prefab> resultA = await PrefabFetcher.GetPrefabAsync<Prefab>(new CaptainExoKey(newVal.ToString()));
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
            if (newVal.ToString() != string.Empty)
            {
                PrefabContainer<Prefab> resultB = await PrefabFetcher.GetPrefabAsync<Prefab>(new CaptainExoKey(newVal.ToString()));
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
            PvPFactoryProvider.Sound.PrioritisedSoundPlayer.Enabled = false;
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

        private IPvPCruiserHelper CreatePlayerHelper(IPvPUIManager uiManager, ICameraFocuser cameraFocuser)
        {
            return new PvPPlayerCruiserHelper(uiManager, cameraFocuser);
        }

        private void IsPopulationLimitReached_ValueChanged(bool oldVal, bool newVal)
        {
            if (newVal)
            {
                _debouncer.Debounce(() => PvPFactoryProvider.Sound.PrioritisedSoundPlayer.PlaySound(PrioritisedSoundKeys.Events.PopulationLimitReached));
            }
            leftPanelComponents.PopLimitReachedFeedback.IsVisible = newVal;
        }

        private void DroneNumIncreased_ValueChanged(bool oldVal, bool newVal)
        {
            PvPFactoryProvider.Sound.PrioritisedSoundPlayer.PlaySound(PrioritisedSoundKeys.Events.Drones.NewDronesReady);
        }

        private void IdleDronesStarted_ValueChanged(bool oldVal, bool newVal)
        {
            new Debouncer(TimeBC.Instance.RealTimeSinceGameStartProvider, debounceTimeInS: 20).Debounce(() => PvPFactoryProvider.Sound.PrioritisedSoundPlayer.PlaySound(PrioritisedSoundKeys.Events.Drones.Idle));
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
            // Register all unlocked buildables to server
            /*            if (IsAIBotMode)
                        {
                            foreach (BuildingKey buildingKey in DataProvider.GameModel.UnlockedBuildings)
                            {
                                _battleSceneGodTunnel.AddUnlockedBuilding_RightPlayer(buildingKey);
                                yield return null;
                            }
                            foreach (UnitKey unitKey in DataProvider.GameModel.UnlockedUnits)
                            {
                                _battleSceneGodTunnel.AddUnlockedUnit_RightPlayer(unitKey);
                                yield return null;
                            }
                            foreach (BuildingKey buildingKey in StaticPrefabKeys.Buildings.AllKeys)
                            {
                                _battleSceneGodTunnel.AddUnlockedBuilding_RightPlayer(buildingKey);
                                yield return null;
                            }
                            foreach (UnitKey unitKey in StaticPrefabKeys.Units.AllKeys)
                            {
                                _battleSceneGodTunnel.AddUnlockedUnit_RightPlayer(unitKey);
                                yield return null;
                            }
                            PvPBattleSceneGodServer.Instance.RegisterAIOfRightPlayer();
                        }           else
                                    {
                                        if (SynchedServerData.Instance.GetTeam() == Team.LEFT)
                                        {
                                            foreach (BuildingKey buildingKey in DataProvider.GameModel.UnlockedBuildings)
                                            {
                                                _battleSceneGodTunnel.AddUnlockedBuilding_LeftPlayer(buildingKey.BuildingCategory, buildingKey.PrefabName);
                                                yield return null;
                                            }
                                            foreach (UnitKey unitKey in DataProvider.GameModel.UnlockedUnits)
                                            {
                                                _battleSceneGodTunnel.AddUnlockedUnit_LeftPlayer(unitKey.UnitCategory, unitKey.PrefabName);
                                                yield return null;
                                            }
                                            _battleSceneGodTunnel.RegisteredAllBuildableLeftPlayer();
                                        }
                                        else
                                        {
                                            foreach (BuildingKey buildingKey in DataProvider.GameModel.UnlockedBuildings)
                                            {
                                                _battleSceneGodTunnel.AddUnlockedBuilding_RightPlayer(buildingKey.BuildingCategory, buildingKey.PrefabName);
                                                yield return null;
                                            }
                                            foreach (UnitKey unitKey in DataProvider.GameModel.UnlockedUnits)
                                            {
                                                _battleSceneGodTunnel.AddUnlockedUnit_RightPlayer(unitKey.UnitCategory, unitKey.PrefabName);
                                                yield return null;
                                            }
                                            _battleSceneGodTunnel.RegisteredAllBuildableRightPlayer();
                                        }
                                    }*/
            yield return new WaitForSeconds(5f); // to show matchmaking animation 
            MatchmakingScreenController.Instance.animator.SetBool("Completed", false);
            MatchmakingScreenController.Instance.DisableAllAnimatedGameObjects();
            sceneNavigator.SceneLoaded(SceneNames.PvP_BOOT_SCENE);
            PlayCountDownAnimation();
            if (SynchedServerData.Instance != null)
            {
                // Optionally focus based on team...
                if (SynchedServerData.Instance.GetTeam() == Team.LEFT)
                    cameraComponents.CameraFocuser.FocusOnLeftCruiser();
                else if (SynchedServerData.Instance.GetTeam() == Team.RIGHT)
                    cameraComponents.CameraFocuser.FocusOnRightCruiser();

                // In either case, mark loading as done.
                MatchmakingScreenController.Instance.isProcessing = false;
                MatchmakingScreenController.Instance.isLoaded = true;
                components.UpdaterProvider.SwitchableUpdater.Enabled = true;
            }
        }

        public void RegisterAsPlayer(PvPCruiser _cruiser)
        {
            Assert.IsNotNull(_cruiser);
            playerCruiser = _cruiser;
            if (enemyCruiser != null)
            {
                InitialiseAsync();
            }
        }

        public void RegisterAsEnemy(PvPCruiser _cruiser)
        {
            Assert.IsNotNull(_cruiser);
            enemyCruiser = _cruiser;
            if (playerCruiser != null)
            {
                InitialiseAsync();
            }
        }

        private IPvPBattleSceneHelper CreatePvPBattleHelper()
        {
            return new PvPBattleHelper();
        }
        private void PlayCountDownAnimation()
        {
            if (countdownGameObject == null || countdownAnimator == null) return;

            countdownGameObject.SetActive(true);

            countdownAnimator.Play(0);

            StartCoroutine(WaitForAnimationToEnd());
        }
        private IEnumerator WaitForAnimationToEnd()
        {
            yield return new WaitForSeconds(countdownAnimator.GetCurrentAnimatorStateInfo(0).length);

            countdownGameObject.SetActive(false);
        }
    }
}