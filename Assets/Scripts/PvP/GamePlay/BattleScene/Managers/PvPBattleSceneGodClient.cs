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
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones;
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
using BattleCruisers.Cruisers.Construction;
using BattleCruisers.Utils.PlatformAbstractions.Time;
using BattleCruisers.Cruisers;
using BattleCruisers.Utils.PlatformAbstractions;
using BattleCruisers.Utils.Timers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Time;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Construction;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Timers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons;
using System;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.Utils.BattleScene;
using BattleCruisers.Network.Multiplay.MultiplayBattleScene.Utils.BattleScene;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.Network.Multiplay.MultiplayBattleScene.UI.BattleScene;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;
using BattleCruisers.Cruisers.Helpers;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Helpers;
using BattleCruisers.Buildables.Colours;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Colours;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene
{
    [RequireComponent(typeof(NetcodeHooks))]
    public class PvPBattleSceneGodClient : MonoBehaviour
    {
        private PvPUserTargetTracker _userTargetTracker;

        public PvPCameraInitialiser cameraInitialiser;
        public PvPTopPanelInitialiser topPanelInitialiser;
        public PvPLeftPanelInitialiser leftPanelInitialiser;
        public PvPRightPanelInitialiser rightPanelInitialiser;

        public  IPvPUIManager uiManager;
        public ILocTable commonStrings;

        private IApplicationModel applicationModel;
        private IDataProvider dataProvider;
        private IPvPPrefabFactory prefabFactory;
        private IPvPSpriteProvider spriteProvider;
        public PvPFactoryProvider factoryProvider;
        private PvPBattleSceneGodComponents components;
        private PvPNavigationPermitters navigationPermitters;
        private IPvPCameraComponents cameraComponents;
        private PvPCruiser playerCruiser;
        private PvPCruiser enemyCruiser;
        private IPvPBattleSceneHelper pvpBattleHelper;
        private IPvPLevel currentLevel;   
        private PvPLeftPanelComponents leftPanelComponents;
        private IPvPTime time;
        private IPvPPauseGameManager pauseGameManager;
        private IPvPDebouncer _debouncer;
        private PvPBuildableButtonColourController _buildableButtonColourController;
        private PvPInformatorDismisser _informatorDismisser;
        ISceneNavigator sceneNavigator;

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
            Assert.IsNotNull(components);
            components.Initialise_Client(applicationModel.DataProvider.SettingsManager);


            pvpBattleHelper = CreatePvPBattleHelper(applicationModel, prefabFetcher, prefabFactory, null, navigationPermitters, storyStrings);
            uiManager = pvpBattleHelper.CreateUIManager();
            factoryProvider = new PvPFactoryProvider(components, prefabFactory, spriteProvider);
            factoryProvider.Initialise(uiManager);
            currentLevel = pvpBattleHelper.GetPvPLevel();

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

            IPvPPrefabContainer<PvPBackgroundImageStats> backgroundStats = await pvpBattleHelper.GetBackgroundStatsAsync(currentLevel.Num);
            components.CloudInitialiser.Initialise(currentLevel.SkyMaterialName, components.UpdaterProvider.VerySlowUpdater, cameraComponents.MainCamera.Aspect, backgroundStats);
            await components.SkyboxInitialiser.InitialiseAsync(cameraComponents.Skybox, currentLevel);
            //  cameraComponents.CameraFocuser.FocusOnPlayerCruiser();
            IPvPButtonVisibilityFilters buttonVisibilityFilters = pvpBattleHelper.CreateButtonVisibilityFilters(playerCruiser);
            sceneNavigator = LandingSceneGod.SceneNavigator;
            IPvPBattleCompletionHandler battleCompletionHandler = new PvPBattleCompletionHandler(applicationModel, sceneNavigator);
            PvPTopPanelComponents topPanelComponents = topPanelInitialiser.Initialise(playerCruiser, enemyCruiser, "Player A", "Player B");
            leftPanelComponents
                = await leftPanelInitialiser.Initialise(
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


            MatchmakingScreenController.Instance.FoundCompetitor();
            StartCoroutine(iLoadedPvPScene());
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



        IEnumerator iLoadedPvPScene()
        {
            yield return new WaitForSeconds(5f);
            sceneNavigator.SceneLoaded(SceneNames.PvP_BOOT_SCENE);
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

        private void Start()
        {
            // InitialiseAsync();
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
        void Update()
        {

        }
    }
}

