using System.Collections.Generic;
using System.Threading.Tasks;
using BattleCruisers.Data;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Threading;
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
using BattleCruisers.Data.Settings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;
using BattleCruisers.Network.Multiplay.Gameplay.Configuration;

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
        public IPvPUserChosenTargetManager playerACruiserUserChosenTargetManager;
        public IPvPUserChosenTargetManager playerBCruiserUserChosenTargetManager;

        public static Dictionary<PvPTargetType, PvPDeadBuildableCounter> deadBuildables_left;
        public static Dictionary<PvPTargetType, PvPDeadBuildableCounter> deadBuildables_right;
        public static Sprite enemyCruiserSprite;
        public static string enemyCruiserName;

        public static Sprite playerBCruiserSprite;
        public static string playerBCruiserName;

        public IPvPUserChosenTargetHelper playerBCruiseruserChosenTargetHelper;
        public IPvPUserChosenTargetHelper playerACruiseruserChosenTargetHelper;

        private bool isInitializingServer = false;

        [SerializeField]
        NetcodeHooks m_NetcodeHooks;

        // Hold reference to avoid garbage collection
#pragma warning disable CS0414  // Variable is assigned but never used
        private PvPDroneManagerMonitor droneManagerMonitorA;
        private PvPDroneManagerMonitor droneManagerMonitorB;
#pragma warning restore CS0414  // Variable is assigned but never used
        public NameGenerationData nameGenerator;
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
                    Debug.Log("No ServerSingleton in scene, did you run this from the bootstrap scene?");
                    return null;
                }
                return s_pvpBattleSceneGodServer;
            }
        }

        public List<int> playerASelectedVariants = new List<int>();
        public List<int> playerBSelectedVariants = new List<int>();

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
            if (!NetworkManager.Singleton.IsHost)
            {
                enabled = false;
                return;
            }
            NetworkManager.Singleton.SceneManager.OnSceneEvent += OnSceneEvent;
            // Initialise();
        }
        void OnNetworkDespawn()
        {
            NetworkManager.Singleton.SceneManager.OnSceneEvent -= OnSceneEvent;
        }

        async void OnSceneEvent(SceneEvent sceneEvent)
        {
            switch (sceneEvent.SceneEventType)
            {
                case SceneEventType.Load:

                    break;
                case SceneEventType.LoadEventCompleted:

                    break;
                case SceneEventType.Synchronize:

                    break;
                case SceneEventType.SynchronizeComplete:
                    if (NetworkManager.Singleton.IsHost)
                    {
                        if (this != null)
                            await Initialise();
                    }
                    break;
            }
        }


        void OnDestroy()
        {
            if (m_NetcodeHooks)
            {
                m_NetcodeHooks.OnNetworkSpawnHook -= OnNetworkSpawn;
                m_NetcodeHooks.OnNetworkDespawnHook -= OnNetworkDespawn;
            }
        }
        private async Task Initialise()
        {
            if (isInitializingServer)
                return;
            isInitializingServer = true;
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
            components.Initialise(applicationModel.DataProvider.SettingsManager);
            components.UpdaterProvider.SwitchableUpdater.Enabled = false;
            pvpBattleHelper = CreatePvPBattleHelper(applicationModel, prefabFetcher, prefabFactory, components.Deferrer, storyStrings);

            playerACruiserUserChosenTargetManager = new PvPUserChosenTargetManager();
            playerACruiseruserChosenTargetHelper = pvpBattleHelper.CreateUserChosenTargetHelper(
                playerACruiserUserChosenTargetManager);

            playerBCruiserUserChosenTargetManager = new PvPUserChosenTargetManager();
            playerBCruiseruserChosenTargetHelper = pvpBattleHelper.CreateUserChosenTargetHelper(
                playerBCruiserUserChosenTargetManager);

            factoryProvider = new PvPFactoryProvider(components, prefabFactory, spriteProvider, dataProvider.SettingsManager);
            await factoryProvider.Initialise();
            await GetComponent<PvPBattleSceneGodClient>().StaticInitialiseAsync_Host();
            await _Initialise_Rest();
        }
        public async Task _Initialise_Rest()
        {
            IPvPCruiserFactory cruiserFactory = new PvPCruiserFactory(factoryProvider, pvpBattleHelper, applicationModel /*, uiManager */);
            await Task.Delay(500);
            playerACruiser = await cruiserFactory.CreatePlayerACruiser(Team.LEFT);
            await Task.Delay(500);
            playerBCruiser = await cruiserFactory.CreatePlayerBCruiser(Team.RIGHT);
            cruiserFactory.InitialisePlayerACruiser(playerACruiser, playerBCruiser, playerACruiserUserChosenTargetManager);
            cruiserFactory.InitialisePlayerBCruiser(playerBCruiser, playerACruiser, playerBCruiserUserChosenTargetManager);

            enemyCruiserSprite = playerACruiser.Sprite;
            enemyCruiserName = playerACruiser.stringKeyBase;

            playerBCruiserSprite = playerBCruiser.Sprite;
            playerBCruiserName = playerBCruiser.stringKeyBase;

            droneManagerMonitorA = new PvPDroneManagerMonitor(playerACruiser.DroneManager, components.Deferrer);
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
            //    _battleSceneGodTunnel.RegisteredAllUnlockedBuildables += RegisteredAllBuildalbesToServer;
            RegisteredAllBuildalbesToServer();
            /*            string logName = "Battle_Begin";
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
                        */
        }

        /*        public void GetSelectedVariantsFromString()
                {
                    string[] str_VaraintsA = SynchedServerData.Instance.playerASelectedVariants.Value.ToString().Split(' ');
                    foreach(string str in str_VaraintsA) 
                    {
                        int tempI;
                        if(int.TryParse(str, out tempI))
                        {
                            playerASelectedVariants.Add(tempI);
                        }
                    }

                    string[] str_VariantsB = SynchedServerData.Instance.playerBSelectedVariants.Value.ToString().Split(' ');
                    foreach(string str in str_VariantsB)
                    {
                        int tempP;
                        if (int.TryParse(str, out tempP))
                        {
                            playerBSelectedVariants.Add(tempP);
                        }
                    }
                }*/

        public async void Initialise_Rest()
        {
            await factoryProvider.Initialise_Rest();
            Debug.Log("====> All initialized");
        }

        public async Task RunPvP_AIMode()
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
            GetComponent<PvPBattleSceneGodClient>().SetAIBotMode();
            Assert.IsNotNull(components);
            components.Initialise(applicationModel.DataProvider.SettingsManager);
            components.UpdaterProvider.SwitchableUpdater.Enabled = false;
            pvpBattleHelper = CreatePvPBattleHelper(applicationModel, prefabFetcher, prefabFactory, components.Deferrer, storyStrings);
            // AIBot
            SynchedServerData.Instance.playerBPrefabName.Value = pvpBattleHelper.PvPHullNames[UnityEngine.Random.Range(0, pvpBattleHelper.PvPHullNames.Length)]; ;
            SynchedServerData.Instance.captainBPrefabName.Value = "CaptainExo0" + UnityEngine.Random.Range(0, 41).ToString("00");
            SynchedServerData.Instance.playerBName.Value = nameGenerator.GenerateName();
            SynchedServerData.Instance.playerBScore.Value = dataProvider.GameModel.LifetimeDestructionScore;
            SynchedServerData.Instance.playerBRating.Value = UnityEngine.Random.Range(dataProvider.GameModel.BattleWinScore, dataProvider.GameModel.BattleWinScore + 100f);

            SynchedServerData.Instance.playerAPrefabName.Value = dataProvider.GameModel.PlayerLoadout.Hull.PrefabName;
            SynchedServerData.Instance.playerAName.Value = dataProvider.GameModel.PlayerName;
            SynchedServerData.Instance.playerAScore.Value = dataProvider.GameModel.LifetimeDestructionScore;
            SynchedServerData.Instance.playerARating.Value = dataProvider.GameModel.BattleWinScore;

            playerACruiserUserChosenTargetManager = new PvPUserChosenTargetManager();
            playerACruiseruserChosenTargetHelper = pvpBattleHelper.CreateUserChosenTargetHelper(
                playerACruiserUserChosenTargetManager);

            playerBCruiserUserChosenTargetManager = new PvPUserChosenTargetManager();
            playerBCruiseruserChosenTargetHelper = pvpBattleHelper.CreateUserChosenTargetHelper(
                playerBCruiserUserChosenTargetManager);

            factoryProvider = new PvPFactoryProvider(components, prefabFactory, spriteProvider, dataProvider.SettingsManager);
            await factoryProvider.Initialise();
            await GetComponent<PvPBattleSceneGodClient>().StaticInitialiseAsync_Host();

            IPvPCruiserFactory cruiserFactory = new PvPCruiserFactory(factoryProvider, pvpBattleHelper, applicationModel /*, uiManager */);
            await Task.Delay(500);
            playerACruiser = await cruiserFactory.CreatePlayerACruiser(Team.LEFT);
            await Task.Delay(500);
            playerBCruiser = await cruiserFactory.CreateAIBotCruiser(Team.RIGHT);
            cruiserFactory.InitialisePlayerACruiser(playerACruiser, playerBCruiser, playerACruiserUserChosenTargetManager);
            cruiserFactory.InitialisePlayerBCruiser(playerBCruiser, playerACruiser, playerBCruiserUserChosenTargetManager);

            enemyCruiserSprite = playerACruiser.Sprite;
            enemyCruiserName = playerACruiser.stringKeyBase;

            playerBCruiserSprite = playerBCruiser.Sprite;
            playerBCruiserName = playerBCruiser.stringKeyBase;

            droneManagerMonitorA = new PvPDroneManagerMonitor(playerACruiser.DroneManager, components.Deferrer);
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
            //    _battleSceneGodTunnel.RegisteredAllUnlockedBuildables += RegisteredAllBuildalbesToServer;
            RegisteredAllBuildalbesToServer();
        }

        private void RegisteredAllBuildalbesToServer()
        {
            _gameEndMonitor
                = new PvPGameEndMonitor(
                    new PvPCruiserDestroyedMonitor(
                        playerACruiser,
                        playerBCruiser),
                        _battleSceneGodTunnel,
                    new PvPGameEndHandler(
                        playerACruiser,
                        playerBCruiser,
                        _battleSceneGodTunnel,
                        components.Deferrer
                        ));

            deadBuildables_left = new Dictionary<PvPTargetType, PvPDeadBuildableCounter>();
            deadBuildables_left.Add(PvPTargetType.Aircraft, new PvPDeadBuildableCounter());
            deadBuildables_left.Add(PvPTargetType.Ships, new PvPDeadBuildableCounter());
            deadBuildables_left.Add(PvPTargetType.Cruiser, new PvPDeadBuildableCounter());
            deadBuildables_left.Add(PvPTargetType.Buildings, new PvPDeadBuildableCounter());
            deadBuildables_left.Add(PvPTargetType.PlayedTime, new PvPDeadBuildableCounter());


            deadBuildables_right = new Dictionary<PvPTargetType, PvPDeadBuildableCounter>();
            deadBuildables_right.Add(PvPTargetType.Aircraft, new PvPDeadBuildableCounter());
            deadBuildables_right.Add(PvPTargetType.Ships, new PvPDeadBuildableCounter());
            deadBuildables_right.Add(PvPTargetType.Cruiser, new PvPDeadBuildableCounter());
            deadBuildables_right.Add(PvPTargetType.Buildings, new PvPDeadBuildableCounter());
            deadBuildables_right.Add(PvPTargetType.PlayedTime, new PvPDeadBuildableCounter());


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
        public static void AddDeadBuildable_Left(PvPTargetType type, int value)
        {
            if (!GameOver)
            {
                if (type == PvPTargetType.Satellite || type == PvPTargetType.Rocket)
                {
                    return;
                }
                if (type == PvPTargetType.Cruiser)
                    Debug.Log("CRUISER DESTROYED LEFT");

                deadBuildables_left[type].AddDeadBuildable((int)(difficultyDestructionScoreMultiplier * ((float)value)));
                SynchedServerData.Instance.CalculateScoresOfLeftPlayer();
                if (type == PvPTargetType.Cruiser)
                {
                    GameOver = true;
                }
            }
        }

        public static void AddPlayedTime_Left(PvPTargetType type, float dt)
        {
            if (!GameOver)
            {
                deadBuildables_left?[type]?.AddPlayedTime(dt);
            }
        }


        public static void AddDeadBuildable_Right(PvPTargetType type, int value)
        {
            if (!GameOver)
            {
                if (type == PvPTargetType.Satellite || type == PvPTargetType.Rocket)
                {
                    return;
                }
                if (type == PvPTargetType.Cruiser)
                    Debug.Log("CRUISER DESTROYED RIGHT");
                deadBuildables_right[type].AddDeadBuildable((int)(difficultyDestructionScoreMultiplier * ((float)value)));
                SynchedServerData.Instance.CalculateScoresOfRightPlayer();
                //Debug.Log("" + (int)(difficultyDestructionScoreMultiplier*((float)value)) + " added");
                if (type == PvPTargetType.Cruiser)
                {
                    GameOver = true;
                }
            }
        }

        public static void AddPlayedTime_Right(PvPTargetType type, float dt)
        {
            if (!GameOver)
            {
                deadBuildables_right?[type]?.AddPlayedTime(dt);
            }
        }
        public void RegisterAIOfLeftPlayer()
        {
            IPvPArtificialIntelligence ai_LeftPlayer = pvpBattleHelper.CreateAI(playerACruiser, playerBCruiser, UnityEngine.Random.Range(0, 40));
            //     IPvPArtificialIntelligence ai_RightPlayer = pvpBattleHelper.CreateAI(playerBCruiser, playerACruiser, 1 current level num);
            _gameEndMonitor.RegisterAIOfLeftPlayer(ai_LeftPlayer);
        }

        public void RegisterAIOfRightPlayer()
        {
            IPvPArtificialIntelligence ai_RightPlayer = pvpBattleHelper.CreateAI(playerBCruiser, playerACruiser, UnityEngine.Random.Range(20, 40));
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
    }
}

