using System.Collections;
using System.Collections.Generic;
using BattleCruisers.Data;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Threading;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Navigation;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Scenes.BattleScene;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers.Cache;
using UnityEngine;
using UnityEngine.Assertions;
using Unity.Multiplayer.Samples.Utilities;
using Unity.Netcode;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene
{

    [RequireComponent(typeof(NetcodeHooks))]
    public class PvPBattleSceneGodServer : MonoBehaviour
    {
        private PvPBattleSceneGodComponentsServer components;
        [SerializeField]
        NetcodeHooks m_NetcodeHooks;

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
        private async void Start()
        {

            ILocTable commonStrings = await LocTableFactory.Instance.LoadCommonTableAsync();
            ILocTable storyStrings = await LocTableFactory.Instance.LoadStoryTableAsync();
            IPvPPrefabCacheFactory prefabCacheFactory = new PvPPrefabCacheFactory(commonStrings);
            IPvPPrefabFetcher prefabFetcher = new PvPPrefabFetcher();
            IPvPPrefabCache prefabCache = await prefabCacheFactory.CreatePrefabCacheAsync(prefabFetcher);
            IPvPPrefabFactory prefabFactory = new PvPPrefabFactory(prefabCache, null, commonStrings);



            // components = GetComponent<PvPBattleSceneGodComponentsServer>();
            // Assert.IsNotNull(components);

            // components.Initialise(applicationModel.DataProvider.SettingsManager);
            // components.UpdaterProvider.SwitchableUpdater.Enabled = false;

            // IPvPBattleSceneHelper pvpBattleHelper = CreatePvPBattleHelper(applicationModel, prefabFetcher, prefabFactory, components.Deferrer, navigationPermitters, storyStrings);

            // IPvPLevel currentLevel = pvpBattleHelper.GetPvPLevel();

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

