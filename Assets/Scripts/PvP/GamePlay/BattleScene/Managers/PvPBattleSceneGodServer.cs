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
using BattleCruisers.Network.Multiplay.Matchplay.Shared;
using BattleCruisers.Utils.Localisation;
using UnityEngine;
using UnityEngine.Assertions;
using Unity.Multiplayer.Samples.Utilities;
using Unity.Netcode;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones;
using System;
using BattleCruisers.Cruisers.Construction;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Construction;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Time;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Timers;
using BattleCruisers.UI.BattleScene;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene
{

    [RequireComponent(typeof(NetcodeHooks))]
    public class PvPBattleSceneGodServer : MonoBehaviour
    {
        private IApplicationModel applicationModel;
        private IDataProvider dataProvider;
        private PvPBattleSceneGodComponents components;
        public PvPFactoryProvider factoryProvider;
        private PvPCruiser playerACruiser;
        private PvPCruiser playerBCruiser;
        private PvPPopulationLimitAnnouncer _populationLimitAnnouncer;
        [SerializeField]
        NetcodeHooks m_NetcodeHooks;

        // Hold reference to avoid garbage collection
#pragma warning disable CS0414  // Variable is assigned but never used
        private PvPDroneManagerMonitor droneManagerMonitor;
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
            IPvPPrefabFactory prefabFactory = new PvPPrefabFactory(prefabCache, null, commonStrings);
            IPvPSpriteProvider spriteProvider = new PvPSpriteProvider(new PvPSpriteFetcher());


            components = GetComponent<PvPBattleSceneGodComponents>();
            Assert.IsNotNull(components);
            components.Initialise_Server();
            components.UpdaterProvider.SwitchableUpdater.Enabled = false;
            IPvPBattleSceneHelper pvpBattleHelper = CreatePvPBattleHelper(applicationModel, prefabFetcher, prefabFactory, components.Deferrer, storyStrings);
            IPvPUserChosenTargetManager playerACruiserUserChosenTargetManager = new PvPUserChosenTargetManager();
            IPvPUserChosenTargetHelper playerBCruiseruserChosenTargetHelper = pvpBattleHelper.CreateUserChosenTargetHelper(
                playerACruiserUserChosenTargetManager
                /* playerACruiser.FactoryProvider.Sound.PrioritisedSoundPlayer,
                components.TargetIndicator */);
            IPvPUserChosenTargetManager playerBCruiserUserChosenTargetManager = new PvPUserChosenTargetManager();
            factoryProvider = new PvPFactoryProvider(components, prefabFactory, spriteProvider);
            factoryProvider.Initialise();
            IPvPCruiserFactory cruiserFactory = new PvPCruiserFactory(factoryProvider, pvpBattleHelper, applicationModel /*, uiManager */);
            playerACruiser = await cruiserFactory.CreatePlayerACruiser();
            playerBCruiser = await cruiserFactory.CreatePlayerBCruiser();

            cruiserFactory.InitialisePlayerACruiser(playerACruiser, playerBCruiser /*, cameraComponents.CameraFocuser*/, playerACruiserUserChosenTargetManager);
            cruiserFactory.InitialisePlayerBCruiser(playerBCruiser, playerACruiser, playerBCruiserUserChosenTargetManager /*, playerBCruiseruserChosenTargetHelper*/);

            // IPvPLevel currentLevel = pvpBattleHelper.GetPvPLevel();

            droneManagerMonitor = new PvPDroneManagerMonitor(playerACruiser.DroneManager, components.Deferrer);
            droneManagerMonitor.IdleDronesStarted += _droneManagerMonitor_IdleDronesStarted;
            droneManagerMonitor.IdleDronesEnded += _droneManagerMonitor_IdleDronesEnded;

            IPvPTime time = PvPTimeBC.Instance;
            _populationLimitAnnouncer = CreatePopulationLimitAnnouncer(playerACruiser);

            components.UpdaterProvider.SwitchableUpdater.Enabled = true;
        }


        private PvPPopulationLimitAnnouncer CreatePopulationLimitAnnouncer(PvPCruiser playerCruiser)
        {
            return
                new PvPPopulationLimitAnnouncer(
                    playerCruiser,
                    playerCruiser.PopulationLimitMonitor
                    );
        }
        private void _droneManagerMonitor_IdleDronesStarted(object sender, EventArgs e)
        {
            playerACruiser.pvp_IdleDronesStarted.Value = true;
        }

        private void _droneManagerMonitor_IdleDronesEnded(object sender, EventArgs e)
        {
            playerACruiser.pvp_IdleDronesEnded.Value = false;
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

