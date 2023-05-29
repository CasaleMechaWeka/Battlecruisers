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
using UnityEngine;
using UnityEngine.Assertions;
using Unity.Multiplayer.Samples.Utilities;
using Unity.Netcode;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene
{
    [RequireComponent(typeof(NetcodeHooks))]
    public class PvPBattleSceneGodClient : MonoBehaviour
    {
        public PvPCameraInitialiser cameraInitialiser;

        private IApplicationModel applicationModel;
        private IDataProvider dataProvider;
        public PvPFactoryProvider factoryProvider;
        private PvPBattleSceneGodComponents components;
        private PvPNavigationPermitters navigationPermitters;
        private IPvPCameraComponents cameraComponents;
        private PvPCruiser playerCruiser;
        private PvPCruiser enemyCruiser;
        private IPvPBattleSceneHelper pvpBattleHelper;
        private IPvPLevel currentLevel;
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

            ILocTable commonStrings = await LocTableFactory.Instance.LoadCommonTableAsync();
            ILocTable storyStrings = await LocTableFactory.Instance.LoadStoryTableAsync();
            IPvPPrefabCacheFactory prefabCacheFactory = new PvPPrefabCacheFactory(commonStrings);
            IPvPPrefabFetcher prefabFetcher = new PvPPrefabFetcher();
            IPvPPrefabCache prefabCache = await prefabCacheFactory.CreatePrefabCacheAsync(prefabFetcher);
            IPvPPrefabFactory prefabFactory = new PvPPrefabFactory(prefabCache, dataProvider.SettingsManager, commonStrings);
            IPvPSpriteProvider spriteProvider = new PvPSpriteProvider(new PvPSpriteFetcher());
            // navigationPermitters = new PvPNavigationPermitters();



            components = GetComponent<PvPBattleSceneGodComponents>();
            Assert.IsNotNull(components);
            components.Initialise_Client(applicationModel.DataProvider.SettingsManager);
            components.UpdaterProvider.SwitchableUpdater.Enabled = false;

            pvpBattleHelper = CreatePvPBattleHelper(applicationModel, prefabFetcher, prefabFactory, null, navigationPermitters, storyStrings);
            factoryProvider = new PvPFactoryProvider(components, prefabFactory, spriteProvider);
            factoryProvider.Initialise();

            currentLevel = pvpBattleHelper.GetPvPLevel();


            // components.CloudInitialiser.Initialise(currentLevel.SkyMaterialName, components.UpdaterProvider.VerySlowUpdater,);

        }

        private async void InitialiseAsync()
        {
            PvPHelper.AssertIsNotNull(playerCruiser, enemyCruiser);


            cameraComponents = cameraInitialiser.Initialise(
                dataProvider.SettingsManager,
                playerCruiser,
                enemyCruiser,
                components.UpdaterProvider.SwitchableUpdater,
                factoryProvider.Sound.UISoundPlayer
            );

            IPvPPrefabContainer<PvPBackgroundImageStats> backgroundStats = await pvpBattleHelper.GetBackgroundStatsAsync(currentLevel.Num);
            components.CloudInitialiser.Initialise(currentLevel.SkyMaterialName, components.UpdaterProvider.VerySlowUpdater, cameraComponents.MainCamera.Aspect, backgroundStats);
            await components.SkyboxInitialiser.InitialiseAsync(cameraComponents.Skybox, currentLevel);
            cameraComponents.CameraFocuser.FocusOnPlayerCruiser();
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

