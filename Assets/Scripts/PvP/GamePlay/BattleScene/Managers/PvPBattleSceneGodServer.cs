using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using BattleCruisers.Data;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Threading;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Navigation;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Scenes.BattleScene;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers.Cache;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers.Sprites;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers.UserChosen;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Utils.Localisation;
using UnityEngine;
using UnityEngine.Assertions;
using Unity.Multiplayer.Samples.Utilities;
using Unity.Netcode;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones;
using System;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Construction;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Time;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Buildables;
using BattleCruisers.Data.Settings;
using BattleCruisers.Scenes.BattleScene;
using UnityEngine.Analytics;
using Unity.Services.Analytics;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI;
using BattleCruisers.Scenes.Test.Utilities;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene
{

    [RequireComponent(typeof(NetcodeHooks))]
    public class PvPBattleSceneGodServer : MonoBehaviour
    {
        private static IPvPGameEndMonitor _gameEndMonitor;
        public PvPBattleSceneGodTunnel _battleSceneGodTunnel;
        public IPvPPrefabFactory prefabFactory;
        private IApplicationModel applicationModel;
        public IDataProvider dataProvider;
        private PvPBattleSceneGodComponents components;
        public PvPFactoryProvider factoryProvider;
        private PvPCruiser playerACruiser;
        private PvPCruiser playerBCruiser;
        private PvPPopulationLimitAnnouncer _populationLimitAnnouncerA;
        private PvPPopulationLimitAnnouncer _populationLimitAnnouncerB;
        private static float difficultyDestructionScoreMultiplier;
        private static bool GameOver;
        private IPvPBattleSceneHelper pvpBattleHelper;

        public static Dictionary<PvPTargetType, PvPDeadBuildableCounter> deadBuildables;

        [SerializeField]
        NetcodeHooks m_NetcodeHooks;

        // Hold reference to avoid garbage collection
#pragma warning disable CS0414  // Variable is assigned but never used
        private PvPDroneManagerMonitor droneManagerMonitorA;
        private PvPDroneManagerMonitor droneManagerMonitorB;
#pragma warning restore CS0414  // Variable is assigned but never used


        public static PvPBattleSceneGodServer Instance
        {
            get
            {
                if (s_pvpBattleSceneGodServer == null)
                {
                    s_pvpBattleSceneGodServer = FindObjectOfType<PvPBattleSceneGodServer>();
                }
                if (s_pvpBattleSceneGodServer == null)
                {
                    Debug.LogError("No ServerSingleton in scene, did you run this from the bootstrap scene?");
                    return null;
                }
                return s_pvpBattleSceneGodServer;
            }
        }



        static PvPBattleSceneGodServer s_pvpBattleSceneGodServer;
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
            if (!NetworkManager.Singleton.IsServer)
            {
                enabled = false;
                return;
            }
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
        public async void Initialise()
        {
            await _Initialise();
        }
        private async Task _Initialise()
        {

            applicationModel = ApplicationModelProvider.ApplicationModel;
            dataProvider = applicationModel.DataProvider;

            ILocTable commonStrings = await LocTableFactory.Instance.LoadCommonTableAsync();
            ILocTable storyStrings = await LocTableFactory.Instance.LoadStoryTableAsync();
            IPvPPrefabCacheFactory prefabCacheFactory = new PvPPrefabCacheFactory(commonStrings);
            IPvPPrefabFetcher prefabFetcher = new PvPPrefabFetcher();
            IPvPPrefabCache prefabCache = await prefabCacheFactory.CreatePrefabCacheAsync(prefabFetcher);
            prefabFactory = new PvPPrefabFactory(prefabCache, null, commonStrings);
            IPvPSpriteProvider spriteProvider = new PvPSpriteProvider(new PvPSpriteFetcher());


            components = GetComponent<PvPBattleSceneGodComponents>();
            _battleSceneGodTunnel = GetComponent<PvPBattleSceneGodTunnel>();
            Assert.IsNotNull(components);
            components.Initialise_Server();
            components.UpdaterProvider.SwitchableUpdater.Enabled = false;
            pvpBattleHelper = CreatePvPBattleHelper(applicationModel, prefabFetcher, prefabFactory, components.Deferrer, storyStrings);
            IPvPUserChosenTargetManager playerACruiserUserChosenTargetManager = new PvPUserChosenTargetManager();
            IPvPUserChosenTargetHelper playerBCruiseruserChosenTargetHelper = pvpBattleHelper.CreateUserChosenTargetHelper(
                playerACruiserUserChosenTargetManager
                /* playerACruiser.FactoryProvider.Sound.PrioritisedSoundPlayer,
                components.TargetIndicator */);
            IPvPUserChosenTargetManager playerBCruiserUserChosenTargetManager = new PvPUserChosenTargetManager();
            factoryProvider = new PvPFactoryProvider(components, prefabFactory, spriteProvider);
            await factoryProvider.Initialise();
            // StartCoroutine(iInitialiseFactoryProvider());
            IPvPCruiserFactory cruiserFactory = new PvPCruiserFactory(factoryProvider, pvpBattleHelper, applicationModel /*, uiManager */);
            playerACruiser = await cruiserFactory.CreatePlayerACruiser(Team.LEFT);
            playerBCruiser = await cruiserFactory.CreatePlayerBCruiser(Team.RIGHT);

            cruiserFactory.InitialisePlayerACruiser(playerACruiser, playerBCruiser, /*cameraComponents.CameraFocuser,*/ playerACruiserUserChosenTargetManager);
            cruiserFactory.InitialisePlayerBCruiser(playerBCruiser, playerACruiser, playerBCruiserUserChosenTargetManager /*, playerBCruiseruserChosenTargetHelper*/);

            // IPvPLevel currentLevel = pvpBattleHelper.GetPvPLevel();

            droneManagerMonitorA = new PvPDroneManagerMonitor(playerACruiser.DroneManager, components.Deferrer);
            droneManagerMonitorA.IdleDronesStarted += _droneManagerMonitorA_IdleDronesStarted;
            droneManagerMonitorA.IdleDronesEnded += _droneManagerMonitorA_IdleDronesEnded;
            droneManagerMonitorA.DroneNumIncreased += _droneManagerMonitorA_DroneNumIncreased;



            droneManagerMonitorB = new PvPDroneManagerMonitor(playerBCruiser.DroneManager, components.Deferrer);
            droneManagerMonitorB.IdleDronesStarted += _droneManagerMonitorB_IdleDronesStarted;
            droneManagerMonitorB.IdleDronesEnded += _droneManagerMonitorB_IdleDronesEnded;
            droneManagerMonitorB.DroneNumIncreased += _droneManagerMonitorB_DroneNumIncreased;

            IPvPTime time = PvPTimeBC.Instance;
            _populationLimitAnnouncerA = CreatePopulationLimitAnnouncer(playerACruiser);
            _populationLimitAnnouncerB = CreatePopulationLimitAnnouncer(playerBCruiser);

            components.UpdaterProvider.SwitchableUpdater.Enabled = true;

            _battleSceneGodTunnel.RegisteredAllUnlockedBuildables += RegisteredAllBuildalbesToServer;








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


        private void RegisteredAllBuildalbesToServer()
        {

            // ai should be created when the player leave battle arena.
/*            IPvPArtificialIntelligence ai_LeftPlayer = pvpBattleHelper.CreateAI(playerACruiser, playerBCruiser, 1 *//*current level num*//*);
            IPvPArtificialIntelligence ai_RightPlayer = pvpBattleHelper.CreateAI(playerBCruiser, playerACruiser, 1 *//*current level num*//*);
*/

            _gameEndMonitor
                = new PvPGameEndMonitor(
                    new PvPCruiserDestroyedMonitor(
                        playerACruiser,
                        playerBCruiser),
                        _battleSceneGodTunnel,
                    new PvPGameEndHandler(
                        playerACruiser,
                        playerBCruiser,
/*                        ai_LeftPlayer,
                        ai_RightPlayer,*/
                        _battleSceneGodTunnel,
                        components.Deferrer
                        //    cameraComponents.CruiserDeathCameraFocuser,
                        //    navigationPermitters.NavigationFilter,
                        //    uiManager,
                        //    components.TargetIndicator,
                        //    //windManager,
                        //    helper.BuildingCategoryPermitter,
                        //    rightPanelComponents.SpeedComponents.SpeedButtonGroup
                        ));

            deadBuildables = new Dictionary<PvPTargetType, PvPDeadBuildableCounter>();
            deadBuildables.Add(PvPTargetType.Aircraft, new PvPDeadBuildableCounter());
            deadBuildables.Add(PvPTargetType.Ships, new PvPDeadBuildableCounter());
            deadBuildables.Add(PvPTargetType.Cruiser, new PvPDeadBuildableCounter());
            deadBuildables.Add(PvPTargetType.Buildings, new PvPDeadBuildableCounter());
            deadBuildables.Add(PvPTargetType.PlayedTime, new PvPDeadBuildableCounter());


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


        }

        public void RegisterAIOfLeftPlayer()
        {
            IPvPArtificialIntelligence ai_LeftPlayer = pvpBattleHelper.CreateAI(playerACruiser, playerBCruiser, 1 /* current level num*/);
            //     IPvPArtificialIntelligence ai_RightPlayer = pvpBattleHelper.CreateAI(playerBCruiser, playerACruiser, 1 current level num);
            _gameEndMonitor.RegisterAIOfLeftPlayer(ai_LeftPlayer);
        }

        public void RegisterAIOfRightPlayer()
        {
            IPvPArtificialIntelligence ai_RightPlayer = pvpBattleHelper.CreateAI(playerBCruiser, playerACruiser, 1 /*current level num*/);
            _gameEndMonitor.RegisterAIOfRightPlayer(ai_RightPlayer);
        }
        private PvPPopulationLimitAnnouncer CreatePopulationLimitAnnouncer(PvPCruiser playerCruiser)
        {
            return
                new PvPPopulationLimitAnnouncer(
                    playerCruiser,
                    playerCruiser.PopulationLimitMonitor
                    );
        }
        private void _droneManagerMonitorA_IdleDronesStarted(object sender, EventArgs e)
        {
            playerACruiser.pvp_IdleDronesStarted.Value = !playerACruiser.pvp_IdleDronesStarted.Value;
        }

        private void _droneManagerMonitorA_IdleDronesEnded(object sender, EventArgs e)
        {
            playerACruiser.pvp_IdleDronesEnded.Value = !playerACruiser.pvp_IdleDronesEnded.Value;
        }

        private void _droneManagerMonitorA_DroneNumIncreased(object sender, EventArgs e)
        {
            playerACruiser.pvp_DroneNumIncreased.Value = !playerACruiser.pvp_DroneNumIncreased.Value;
        }

        private void _droneManagerMonitorB_IdleDronesStarted(object sender, EventArgs e)
        {
            playerBCruiser.pvp_IdleDronesStarted.Value = !playerBCruiser.pvp_IdleDronesStarted.Value;
        }

        private void _droneManagerMonitorB_IdleDronesEnded(object sender, EventArgs e)
        {
            playerBCruiser.pvp_IdleDronesEnded.Value = !playerBCruiser.pvp_IdleDronesEnded.Value;
        }

        private void _droneManagerMonitorB_DroneNumIncreased(object sender, EventArgs e)
        {
            playerBCruiser.pvp_DroneNumIncreased.Value = !playerBCruiser.pvp_DroneNumIncreased.Value;
        }
        private IPvPBattleSceneHelper CreatePvPBattleHelper(
            IApplicationModel applicationModel,
            IPvPPrefabFetcher prefabFetcher,
            IPvPPrefabFactory prefabFactory,
            IPvPDeferrer deferrer,
            // PvPNavigationPermitters navigationPermitters,
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

