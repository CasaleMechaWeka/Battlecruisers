using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Data;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Scenes.BattleScene;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using UnityEngine;
using UnityEngine.Assertions;
using Unity.Multiplayer.Samples.Utilities;
using Unity.Netcode;
using System;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Construction;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene;
using BattleCruisers.Data.Settings;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;
using BattleCruisers.Network.Multiplay.Gameplay.Configuration;
using BattleCruisers.Buildables;
using BattleCruisers.Utils.PlatformAbstractions.Time;
using System.Collections.Generic;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using BattleCruisers.Scenes.BattleScene;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene
{
    [RequireComponent(typeof(NetcodeHooks))]
    public class PvPBattleSceneGodServer : MonoBehaviour
    {
        private static IPvPGameEndMonitor _gameEndMonitor;
        public PvPBattleSceneGodTunnel _battleSceneGodTunnel;
        private PvPBattleSceneGodComponents components;
        private PvPCruiser playerACruiser;
        private PvPCruiser playerBCruiser;
        private PvPPopulationLimitAnnouncer _populationLimitAnnouncerA;
        private PvPPopulationLimitAnnouncer _populationLimitAnnouncerB;
        private static float difficultyDestructionScoreMultiplier;
        private static bool GameOver;
        private IPvPBattleSceneHelper pvpBattleHelper;
        public IUserChosenTargetManager playerACruiserUserChosenTargetManager;
        public IUserChosenTargetManager playerBCruiserUserChosenTargetManager;

        public static Dictionary<TargetType, DeadBuildableCounter> deadBuildables_left;
        public static Dictionary<TargetType, DeadBuildableCounter> deadBuildables_right;
        public static Sprite enemyCruiserSprite;
        public static string enemyCruiserName;

        public static Sprite playerBCruiserSprite;
        public static string playerBCruiserName;

        public IUserChosenTargetHelper playerBCruiseruserChosenTargetHelper;
        public IUserChosenTargetHelper playerACruiseruserChosenTargetHelper;

        private bool isInitializingServer = false;

        [SerializeField]
        NetcodeHooks m_NetcodeHooks;

        // Hold reference to avoid garbage collection
#pragma warning disable CS0414  // Variable is assigned but never used
        private DroneManagerMonitor droneManagerMonitorA;
        private DroneManagerMonitor droneManagerMonitorB;
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
            _ = PvPPrefabCache.CreatePvPPrefabCacheAsync();
            components = GetComponent<PvPBattleSceneGodComponents>();
            Assert.IsNotNull(components);
            components.Initialise();
            PvPFactoryProvider.Setup(components);
            if (NetworkManager.Singleton.IsHost)
                PvPPrefabFactory.CreatePools();

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
                            Initialise();
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
        private void Initialise()
        {
            if (isInitializingServer)
                return;
            isInitializingServer = true;
            _Initialise();
        }

        private void _Initialise()
        {
            _battleSceneGodTunnel = GetComponent<PvPBattleSceneGodTunnel>();
            components.UpdaterProvider.SwitchableUpdater.Enabled = false;
            pvpBattleHelper = new PvPBattleHelper();

            playerACruiserUserChosenTargetManager = new UserChosenTargetManager();
            playerACruiseruserChosenTargetHelper = pvpBattleHelper.CreateUserChosenTargetHelper(
                playerACruiserUserChosenTargetManager);

            playerBCruiserUserChosenTargetManager = new UserChosenTargetManager();
            playerBCruiseruserChosenTargetHelper = pvpBattleHelper.CreateUserChosenTargetHelper(
                playerBCruiserUserChosenTargetManager);

            PvPFactoryProvider.Initialise();
            GetComponent<PvPBattleSceneGodClient>().StaticInitialiseAsync_Host();
            _Initialise_Rest();
        }
        public void _Initialise_Rest()
        {
            IPvPCruiserFactory cruiserFactory = new PvPCruiserFactory(pvpBattleHelper /*, uiManager */);
            //await Task.Delay(500);
            playerACruiser = cruiserFactory.CreatePlayerACruiser(Team.LEFT);
            //await Task.Delay(500);
            playerBCruiser = cruiserFactory.CreatePlayerBCruiser(Team.RIGHT);
            cruiserFactory.InitialisePlayerACruiser(playerACruiser, playerBCruiser, playerACruiserUserChosenTargetManager);
            cruiserFactory.InitialisePlayerBCruiser(playerBCruiser, playerACruiser, playerBCruiserUserChosenTargetManager);

            enemyCruiserSprite = playerACruiser.Sprite;
            enemyCruiserName = playerACruiser.stringKeyBase;

            playerBCruiserSprite = playerBCruiser.Sprite;
            playerBCruiserName = playerBCruiser.stringKeyBase;

            droneManagerMonitorA = new DroneManagerMonitor(playerACruiser.DroneManager, components.Deferrer);
            droneManagerMonitorA = new DroneManagerMonitor(playerACruiser.DroneManager, components.Deferrer);
            droneManagerMonitorA.IdleDronesStarted += _droneManagerMonitorA_IdleDronesStarted;
            droneManagerMonitorA.IdleDronesEnded += _droneManagerMonitorA_IdleDronesEnded;
            droneManagerMonitorA.DroneNumIncreased += _droneManagerMonitorA_DroneNumIncreased;

            droneManagerMonitorB = new DroneManagerMonitor(playerBCruiser.DroneManager, components.Deferrer);
            droneManagerMonitorB.IdleDronesStarted += _droneManagerMonitorB_IdleDronesStarted;
            droneManagerMonitorB.IdleDronesEnded += _droneManagerMonitorB_IdleDronesEnded;
            droneManagerMonitorB.DroneNumIncreased += _droneManagerMonitorB_DroneNumIncreased;

            ITime time = TimeBC.Instance;
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
                            AnalyticsService.Instance.CustomData("Battle", DataProvider.GameModel.Analytics(ApplicationModel.Mode.ToString(), logName, ApplicationModel.UserWonSkirmish));
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

            deadBuildables_left = new Dictionary<TargetType, DeadBuildableCounter>();
            deadBuildables_left.Add(TargetType.Aircraft, new DeadBuildableCounter());
            deadBuildables_left.Add(TargetType.Ships, new DeadBuildableCounter());
            deadBuildables_left.Add(TargetType.Cruiser, new DeadBuildableCounter());
            deadBuildables_left.Add(TargetType.Buildings, new DeadBuildableCounter());
            deadBuildables_left.Add(TargetType.PlayedTime, new DeadBuildableCounter());


            deadBuildables_right = new Dictionary<TargetType, DeadBuildableCounter>();
            deadBuildables_right.Add(TargetType.Aircraft, new DeadBuildableCounter());
            deadBuildables_right.Add(TargetType.Ships, new DeadBuildableCounter());
            deadBuildables_right.Add(TargetType.Cruiser, new DeadBuildableCounter());
            deadBuildables_right.Add(TargetType.Buildings, new DeadBuildableCounter());
            deadBuildables_right.Add(TargetType.PlayedTime, new DeadBuildableCounter());


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
        }

        public static void AddDeadBuildable_Left(TargetType type, int value)
        {
            if (!GameOver)
            {
                if (type == TargetType.Satellite || type == TargetType.Rocket)
                {
                    return;
                }
                if (type == TargetType.Cruiser)
                    Debug.Log("CRUISER DESTROYED LEFT");

                deadBuildables_left[type].AddDeadBuildable((int)(difficultyDestructionScoreMultiplier * ((float)value)));
                SynchedServerData.Instance.CalculateScoresOfLeftPlayer();
                if (type == TargetType.Cruiser)
                {
                    GameOver = true;
                }
            }
        }

        public static void AddPlayedTime_Left(TargetType type, float dt)
        {
            if (!GameOver)
            {
                deadBuildables_left?[type]?.AddPlayedTime(dt);
            }
        }

        public static void AddDeadBuildable_Right(TargetType type, int value)
        {
            if (!GameOver)
            {
                if (type == TargetType.Satellite || type == TargetType.Rocket)
                {
                    return;
                }
                if (type == TargetType.Cruiser)
                    Debug.Log("CRUISER DESTROYED RIGHT");
                deadBuildables_right[type].AddDeadBuildable((int)(difficultyDestructionScoreMultiplier * ((float)value)));
                SynchedServerData.Instance.CalculateScoresOfRightPlayer();
                //Debug.Log("" + (int)(difficultyDestructionScoreMultiplier*((float)value)) + " added");
                if (type == TargetType.Cruiser)
                {
                    GameOver = true;
                }
            }
        }

        public static void AddPlayedTime_Right(TargetType type, float dt)
        {
            if (!GameOver)
            {
                deadBuildables_right?[type]?.AddPlayedTime(dt);
            }
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
    }
}

