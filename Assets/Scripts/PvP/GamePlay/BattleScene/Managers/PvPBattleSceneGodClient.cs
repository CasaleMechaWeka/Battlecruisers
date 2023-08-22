using System.Collections;
using System.Collections.Generic;
using BattleCruisers.Data;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Threading;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Navigation;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Clouds.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers.Sprites;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Scenes.BattleScene;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers.Cache;
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
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers.UserChosen;
using BattleCruisers.Scenes;
using UnityEngine;
using UnityEngine.Assertions;
using Unity.Multiplayer.Samples.Utilities;
using Unity.Netcode;
using BattleCruisers.UI.ScreensScene.Multiplay.ArenaScreen;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Time;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Timers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons;
using BattleCruisers.Network.Multiplay.MultiplayBattleScene.Utils.BattleScene;
using BattleCruisers.Network.Multiplay.MultiplayBattleScene.UI.BattleScene;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Helpers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Colours;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Music;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Damage;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Wind;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene
{
    [RequireComponent(typeof(NetcodeHooks))]
    public class PvPBattleSceneGodClient : MonoBehaviour
    {
        private PvPUserTargetTracker _userTargetTracker;
        private PvPAudioInitialiser _audioInitialiser;
        private PvPCruiserDeathManager _cruiserDeathManager;
        private PvPBattleSceneGodTunnel _battleSceneGodTunnel;
        private IApplicationModel applicationModel;
        private PvPBattleSceneGodComponents components;
        private PvPNavigationPermitters navigationPermitters;
        private IPvPSpriteProvider spriteProvider;
        private IPvPCameraComponents cameraComponents;


        public PvPCameraInitialiser cameraInitialiser;
        public PvPTopPanelInitialiser topPanelInitialiser;
        public PvPLeftPanelInitialiser leftPanelInitialiser;
        public PvPRightPanelInitialiser rightPanelInitialiser;
        private IPvPBattleSceneHelper pvpBattleHelper;
        private IPvPLevel currentLevel;
        private PvPLeftPanelComponents leftPanelComponents;
        private IPvPTime time;
        private IPvPPauseGameManager pauseGameManager;
        private IPvPDebouncer _debouncer;
        private PvPBuildableButtonColourController _buildableButtonColourController;
        private PvPInformatorDismisser _informatorDismisser;
        private IPvPWindManager windManager;
        ISceneNavigator sceneNavigator;
        IDictionary<ulong, NetworkObject> storageOfNetworkObject = new Dictionary<ulong, NetworkObject>();
        private bool isReadyToShowCaptainExo = false;


        public IPvPUIManager uiManager;
        public ILocTable commonStrings;
        public Dictionary<string, AudioClip> projectileImpactSounds = new Dictionary<string, AudioClip>();
        public IDataProvider dataProvider;
        public IPvPPrefabFactory prefabFactory;
        public PvPFactoryProvider factoryProvider;
        public PvPCruiser playerCruiser;
        public PvPCruiser enemyCruiser;
        public CaptainExo leftCaptain, rightCaptain;
        public Transform leftContainer, rightContainer;

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
                    Debug.LogError("No ServerSingleton in scene, did you run this from the bootstrap scene?");
                    return null;
                }
                return s_pvpBattleSceneGodClient;
            }
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
            if (m_NetcodeHooks)
            {
                m_NetcodeHooks.OnNetworkSpawnHook += OnNetworkSpawn;
                m_NetcodeHooks.OnNetworkDespawnHook += OnNetworkDespawn;
            }
        }



        void OnNetworkSpawn()
        {
            if (NetworkManager.Singleton.IsServer)
            {
                enabled = false;
                return;
            }

            StaticInitialiseAsync();
        }
        void OnNetworkDespawn()
        {

        }
        void OnDestroy()
        {
            if (m_NetcodeHooks)
            {
                m_NetcodeHooks.OnNetworkSpawnHook -= OnNetworkSpawn;
                m_NetcodeHooks.OnNetworkDespawnHook -= OnNetworkDespawn;
            }
        }


        private async void StaticInitialiseAsync()
        {
            applicationModel = ApplicationModelProvider.ApplicationModel;
            dataProvider = applicationModel.DataProvider;

            PvPPrioritisedSoundKeys.SetSoundKeys(applicationModel.DataProvider.SettingsManager.AltDroneSounds);

            commonStrings = await LocTableFactory.Instance.LoadCommonTableAsync();
            ILocTable storyStrings = await LocTableFactory.Instance.LoadStoryTableAsync();
            IPvPPrefabCacheFactory prefabCacheFactory = new PvPPrefabCacheFactory(commonStrings);
            IPvPPrefabFetcher prefabFetcher = new PvPPrefabFetcher();
            IPvPPrefabCache prefabCache = await prefabCacheFactory.CreatePrefabCacheAsync(prefabFetcher);
            prefabFactory = new PvPPrefabFactory(prefabCache, dataProvider.SettingsManager, commonStrings);
            IPvPSpriteProvider spriteProvider = new PvPSpriteProvider(new PvPSpriteFetcher());
            navigationPermitters = new PvPNavigationPermitters();

            components = GetComponent<PvPBattleSceneGodComponents>();
            _battleSceneGodTunnel = GetComponent<PvPBattleSceneGodTunnel>();
            //     _battleSceneGodTunnel.BattleCompleted.OnValueChanged += OnTunnelBattleCompleted_ValueChanged;
            Assert.IsNotNull(components);
            components.Initialise_Client(applicationModel.DataProvider.SettingsManager);


            pvpBattleHelper = CreatePvPBattleHelper(applicationModel, prefabFetcher, prefabFactory, null, navigationPermitters, storyStrings);
            uiManager = pvpBattleHelper.CreateUIManager();
            factoryProvider = new PvPFactoryProvider(components, prefabFactory, spriteProvider, dataProvider.SettingsManager);
            factoryProvider.Initialise(uiManager);
            /*currentLevel = pvpBattleHelper.GetPvPLevel();*/

            components.UpdaterProvider.SwitchableUpdater.Enabled = false;

            // components.CloudInitialiser.Initialise(currentLevel.SkyMaterialName, components.UpdaterProvider.VerySlowUpdater,);

        }

        private async void InitialiseAsync()
        {
            PvPHelper.AssertIsNotNull(playerCruiser, enemyCruiser);

            playerCruiser.StaticInitialise(commonStrings);
            enemyCruiser.StaticInitialise(commonStrings);

            cameraComponents = cameraInitialiser.Initialise(
                dataProvider.SettingsManager,
                playerCruiser,
                enemyCruiser,
                navigationPermitters,
                components.UpdaterProvider.SwitchableUpdater,
                factoryProvider.Sound.UISoundPlayer,
                SynchedServerData.Instance.GetTeam()
            );

            IPvPCruiserHelper helper = CreatePlayerHelper(uiManager, cameraComponents.CameraFocuser);
            playerCruiser.Initialise_Client_PvP(factoryProvider, uiManager, helper);
            enemyCruiser.Initialise_Client_PvP(factoryProvider, uiManager, helper);
            currentLevel = pvpBattleHelper.GetPvPLevel();
            IPvPPrefabContainer<PvPBackgroundImageStats> backgroundStats = await pvpBattleHelper.GetBackgroundStatsAsync(currentLevel.Num);
            components.CloudInitialiser.Initialise(currentLevel.SkyMaterialName, components.UpdaterProvider.VerySlowUpdater, cameraComponents.MainCamera.Aspect, backgroundStats);
            await components.SkyboxInitialiser.InitialiseAsync(cameraComponents.Skybox, currentLevel);

            IPvPButtonVisibilityFilters buttonVisibilityFilters = pvpBattleHelper.CreateButtonVisibilityFilters(playerCruiser);
            sceneNavigator = LandingSceneGod.SceneNavigator;
            IPvPBattleCompletionHandler battleCompletionHandler = new PvPBattleCompletionHandler(applicationModel, sceneNavigator, _battleSceneGodTunnel);
            _battleSceneGodTunnel.battleCompletionHandler = battleCompletionHandler;
            PvPTopPanelComponents topPanelComponents = topPanelInitialiser.Initialise(playerCruiser, enemyCruiser, "Player A", "Player B");
            leftPanelComponents
                = leftPanelInitialiser.Initialise(
                    playerCruiser,
                    uiManager,
                    pvpBattleHelper.GetPlayerLoadout(),
                    prefabFactory,
                    spriteProvider,
                    buttonVisibilityFilters,
                    new PvPPlayerCruiserFocusHelper(cameraComponents.MainCamera, cameraComponents.CameraFocuser, playerCruiser, applicationModel.IsTutorial),
                    // pvpBattleHelper.GetBuildableButtonSoundPlayer(playerCruiser),
                    factoryProvider.Sound.PrioritisedSoundPlayer,
                    factoryProvider.Sound.UISoundPlayer,
                    playerCruiser.PopulationLimitMonitor,
                    dataProvider.StaticData);
            time = PvPTimeBC.Instance;
            IPvPPauseGameManager pauseGameManager = new PvPPauseGameManager(time);
            _debouncer = new PvPDebouncer(time.RealTimeSinceGameStartProvider, debounceTimeInS: 30);
            playerCruiser.pvp_popLimitReachedFeedback.OnValueChanged += IsPopulationLimitReached_ValueChanged;
            playerCruiser.pvp_DroneNumIncreased.OnValueChanged += DroneNumIncreased_ValueChanged;
            playerCruiser.pvp_IdleDronesStarted.OnValueChanged += IdleDronesStarted_ValueChanged;
            playerCruiser.pvp_IdleDronesEnded.OnValueChanged += IdleDronesEnded_ValueChanged;

            IPvPUserChosenTargetManager playerCruiserUserChosenTargetManager = new PvPUserChosenTargetManager();
            IPvPUserChosenTargetHelper userChosenTargetHelper
                = pvpBattleHelper.CreateUserChosenTargetHelper(
                            playerCruiserUserChosenTargetManager,
                            factoryProvider.Sound.PrioritisedSoundPlayer,
                            components.TargetIndicator);

            PvPNavigationPermitterManager navigationPermitterManager = new PvPNavigationPermitterManager(navigationPermitters);
            PvPRightPanelComponents rightPanelComponents
                = rightPanelInitialiser.Initialise(
                    applicationModel,
                    uiManager,
                    playerCruiser,
                    userChosenTargetHelper,
                    buttonVisibilityFilters,
                    factoryProvider.UpdaterProvider.PerFrameUpdater,
                    pauseGameManager,
                    battleCompletionHandler,
                    factoryProvider.Sound.UISoundPlayer,
                    navigationPermitterManager
                );

            IPvPItemDetailsManager itemDetailsManager = new PvPItemDetailsManager(rightPanelComponents.InformatorPanel);
            _userTargetTracker = new PvPUserTargetTracker(itemDetailsManager.SelectedItem, new PvPUserTargetsColourChanger());
            _buildableButtonColourController = new PvPBuildableButtonColourController(itemDetailsManager.SelectedItem, leftPanelComponents.BuildMenu.BuildableButtons);

            PvPManagerArgs args
                = new PvPManagerArgs(
                    playerCruiser,
                    enemyCruiser,
                    leftPanelComponents.BuildMenu,
                    itemDetailsManager,
                    factoryProvider.Sound.PrioritisedSoundPlayer,
                    factoryProvider.Sound.UISoundPlayer);
            pvpBattleHelper.InitialiseUIManager(args);
            _informatorDismisser = new PvPInformatorDismisser(components.BackgroundClickableEmitter, uiManager);


            // Audio
            IPvPLayeredMusicPlayer layeredMusicPlayer
                = await components.MusicPlayerInitialiser.CreatePlayerAsync(
                    factoryProvider.Sound.SoundFetcher,
                    currentLevel.MusicKeys,
                    dataProvider.SettingsManager);
            IPvPCruiserDamageMonitor playerCruiserDamageMonitor = new PvPCruiserDamageMonitor(playerCruiser);

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
                    dataProvider.SettingsManager);
            windManager.Play();

            _cruiserDeathManager = new PvPCruiserDeathManager(playerCruiser, enemyCruiser);

            components.HotkeyInitialiser.Initialise(
                    dataProvider.GameModel.Hotkeys,
                    PvPInputBC.Instance,
                    components.UpdaterProvider.SwitchableUpdater,
                    navigationPermitters.HotkeyFilter,
                    cameraComponents.CameraFocuser,
                    //    rightPanelComponents.SpeedComponents,
                    rightPanelComponents.MainMenuManager,
                    uiManager);
            playerCruiser.Destroyed += PlayerCruiser_Destroyed;
            enemyCruiser.Destroyed += EnemyCruiser_Destroyed;

            // Captains
            if (SynchedServerData.Instance != null)
            {
                SynchedServerData.Instance.captainAPrefabName.OnValueChanged += CaptainAPrefabNameChanged;
                SynchedServerData.Instance.captainBPrefabName.OnValueChanged += CaptainBPrefabNameChanged;
            }
            isReadyToShowCaptainExo = true;
            LoadAllCaptains();
            // pvp
            PvPHeckleMessageManager.Instance.Initialise(dataProvider, factoryProvider.Sound.UISoundPlayer);
            MatchmakingScreenController.Instance.FoundCompetitor();
            StartCoroutine(iLoadedPvPScene());
        }

        private async void LoadAllCaptains()
        {
            if (SynchedServerData.Instance.captainAPrefabName.Value.ToString() != string.Empty)
            {
                IPrefabFetcher prefabFetcher = new PrefabFetcher();

                IPrefabContainer<Prefab> resultA = await prefabFetcher.GetPrefabAsync<Prefab>(new CaptainExoKey(SynchedServerData.Instance.captainAPrefabName.Value.ToString()));
                resultA.Prefab.StaticInitialise(commonStrings);

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
            if (SynchedServerData.Instance.captainBPrefabName.Value.ToString() != string.Empty)
            {
                IPrefabFetcher prefabFetcher = new PrefabFetcher();

                IPrefabContainer<Prefab> resultB = await prefabFetcher.GetPrefabAsync<Prefab>(new CaptainExoKey(SynchedServerData.Instance.captainBPrefabName.Value.ToString()));
                resultB.Prefab.StaticInitialise(commonStrings);

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
        private async void CaptainAPrefabNameChanged(NetworkString oldVal, NetworkString newVal)
        {
            if (newVal.ToString() != string.Empty)
            {
                IPrefabFetcher prefabFetcher = new PrefabFetcher();

                IPrefabContainer<Prefab> resultA = await prefabFetcher.GetPrefabAsync<Prefab>(new CaptainExoKey(newVal.ToString()));
                resultA.Prefab.StaticInitialise(commonStrings);

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
                IPrefabFetcher prefabFetcher = new PrefabFetcher();

                IPrefabContainer<Prefab> resultB = await prefabFetcher.GetPrefabAsync<Prefab>(new CaptainExoKey(newVal.ToString()));
                resultB.Prefab.StaticInitialise(commonStrings);

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


        private void PlayerCruiser_Destroyed(object sender, PvPDestroyedEventArgs e)
        {
            cameraComponents.CruiserDeathCameraFocuser.FocusOnLosingCruiser(playerCruiser);
            playerCruiser.Destroyed -= PlayerCruiser_Destroyed;
        }

        private void EnemyCruiser_Destroyed(object sender, PvPDestroyedEventArgs e)
        {
            cameraComponents.CruiserDeathCameraFocuser.FocusOnLosingCruiser(enemyCruiser);
            enemyCruiser.Destroyed -= EnemyCruiser_Destroyed;
        }

        public void HandleCruiserDestroyed()
        {
            playerCruiser.FactoryProvider.Sound.PrioritisedSoundPlayer.Enabled = false;
            navigationPermitters.NavigationFilter.IsMatch = false;
            uiManager.HideCurrentlyShownMenu();
            uiManager.HideItemDetails();
            components.targetIndicator.Hide();
            pvpBattleHelper.BuildingCategoryPermitter.AllowNoCategories();
        }

        public void OnTunnelBattleCompleted_ValueChanged(/*Tunnel_BattleCompletedState oldVal, Tunnel_BattleCompletedState newVal*/)
        {
            /*            if (newVal == Tunnel_BattleCompletedState.Completed)
                        {*/
            windManager?.Stop();
            windManager?.DisposeManagedState();
            // _battleSceneGodTunnel.BattleCompleted.OnValueChanged -= OnTunnelBattleCompleted_ValueChanged;
            // }
        }

        private IPvPCruiserHelper CreatePlayerHelper(IPvPUIManager uiManager, IPvPCameraFocuser cameraFocuser)
        {
            return new PvPPlayerCruiserHelper(uiManager, cameraFocuser);
        }

        private void IsPopulationLimitReached_ValueChanged(bool oldVal, bool newVal)
        {
            if (newVal)
            {
                _debouncer.Debounce(() => factoryProvider.Sound.PrioritisedSoundPlayer.PlaySound(PvPPrioritisedSoundKeys.PvPEvents.PopulationLimitReached));
            }
            leftPanelComponents.PopLimitReachedFeedback.IsVisible = newVal;
        }

        private void DroneNumIncreased_ValueChanged(bool oldVal, bool newVal)
        {
            factoryProvider.Sound.PrioritisedSoundPlayer.PlaySound(PvPPrioritisedSoundKeys.PvPEvents.PvPDrones.NewDronesReady);
        }

        private void IdleDronesStarted_ValueChanged(bool oldVal, bool newVal)
        {
            new PvPDebouncer(PvPTimeBC.Instance.RealTimeSinceGameStartProvider, debounceTimeInS: 20).Debounce(() => factoryProvider.Sound.PrioritisedSoundPlayer.PlaySound(PvPPrioritisedSoundKeys.PvPEvents.PvPDrones.Idle));
        }

        private void IdleDronesEnded_ValueChanged(bool oldVal, bool newVale)
        {

        }


        private void Update()
        {
            if(isReadyToShowCaptainExo && (leftCaptain == null || rightCaptain == null))
            {
                LoadAllCaptains();
                Debug.Log("===> Loading CaptainExos");
            }
        }
        IEnumerator iLoadedPvPScene()
        {

            // Register all unlocked buildables to server
            if (SynchedServerData.Instance.GetTeam() == Team.LEFT)
            {
                foreach (BuildingKey buildingKey in dataProvider.GameModel.UnlockedBuildings)
                {
                    _battleSceneGodTunnel.AddUnlockedBuilding_LeftPlayer(buildingKey.BuildingCategory, buildingKey.PrefabName);
                    yield return null;
                }
                foreach (UnitKey unitKey in dataProvider.GameModel.UnlockedUnits)
                {
                    _battleSceneGodTunnel.AddUnlockedUnit_LeftPlayer(unitKey.UnitCategory, unitKey.PrefabName);
                    yield return null;
                }
                _battleSceneGodTunnel.RegisteredAllBuildableLeftPlayer();
            }
            else
            {
                foreach (BuildingKey buildingKey in dataProvider.GameModel.UnlockedBuildings)
                {
                    _battleSceneGodTunnel.AddUnlockedBuilding_RightPlayer(buildingKey.BuildingCategory, buildingKey.PrefabName);
                    yield return null;
                }
                foreach (UnitKey unitKey in dataProvider.GameModel.UnlockedUnits)
                {
                    _battleSceneGodTunnel.AddUnlockedUnit_RightPlayer(unitKey.UnitCategory, unitKey.PrefabName);
                    yield return null;
                }
                _battleSceneGodTunnel.RegisteredAllBuildableRightPlayer();
            }

            yield return new WaitForSeconds(5f); // to show matchmaking animation 
            sceneNavigator.SceneLoaded(PvPSceneNames.PvP_BOOT_SCENE);
            if (SynchedServerData.Instance.GetTeam() == Team.LEFT)
                cameraComponents.CameraFocuser.FocusOnLeftPlayerCruiser();
            else
            {
                cameraComponents.CameraFocuser.FocusOnRightPlayerCruiser();
            }
            components.UpdaterProvider.SwitchableUpdater.Enabled = true;
        }

        public void RegisterAsPlayer(PvPCruiser _cruiser)
        {
            Assert.IsNotNull(_cruiser);
            playerCruiser = _cruiser;
            if (enemyCruiser != null)
            {
                // Invoke("InitialiseAsync", 0.5f);
                InitialiseAsync();
            }
        }

        public void RegisterAsEnemy(PvPCruiser _cruiser)
        {
            Assert.IsNotNull(_cruiser);
            enemyCruiser = _cruiser;
            if (playerCruiser != null)
            {
                //  Invoke("InitialiseAsync", 0.5f);
                InitialiseAsync();
            }
        }

        private IPvPBattleSceneHelper CreatePvPBattleHelper(
            IApplicationModel applicationModel,
            IPvPPrefabFetcher prefabFetcher,
            IPvPPrefabFactory prefabFactory,
            IPvPDeferrer deferrer,
            PvPNavigationPermitters navigationPermitters,
            ILocTable storyStrings
        )
        {
            return new PvPBattleHelper(applicationModel, prefabFetcher, storyStrings, prefabFactory, deferrer);
        }
    }
}

