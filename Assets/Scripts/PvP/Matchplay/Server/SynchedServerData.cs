using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Unity.Netcode;
using BattleCruisers.Network.Multiplay.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.Network.Multiplay.Matchplay.Shared
{
    public class SynchedServerData : NetworkBehaviour
    {
        [HideInInspector]
        public NetworkVariable<NetworkString> serverID = new NetworkVariable<NetworkString>();
        public NetworkVariable<Map> map = new NetworkVariable<Map>();
        public NetworkVariable<GameMode> gameMode = new NetworkVariable<GameMode>();
        public NetworkVariable<GameQueue> gameQueue = new NetworkVariable<GameQueue>();

        public NetworkVariable<NetworkString> playerAPrefabName = new NetworkVariable<NetworkString>();
        public NetworkVariable<ulong> playerAClientNetworkId = new NetworkVariable<ulong>();

        public NetworkVariable<NetworkString> playerBPrefabName = new NetworkVariable<NetworkString>();
        public NetworkVariable<ulong> playerBClientNetworkId = new NetworkVariable<ulong>();
        public Action OnNetworkSpawned;




        public float NetworkSpawnTimeoutSeconds { get; private set; } = 3000;
        int m_SynchronousSpawnAckCount = 0;
        float m_SynchronousSpawnTimeoutTimer;


        public override void OnNetworkSpawn()
        {
            OnNetworkSpawned?.Invoke();
        }
        void Start()
        {
            // DynamicPrefabLoadingUtilities.Init(NetworkManager.Singleton);
        }


        public Team GetTeam()
        {
            if (NetworkManager.Singleton.LocalClientId == playerAClientNetworkId.Value)
                return Team.LEFT;
            return Team.RIGHT;
        }

        public ulong LocalClientID
        {
            get
            {
                return NetworkManager.Singleton.LocalClientId;
            }
        }
        public static SynchedServerData Instance
        {
            get
            {
                if (sync_ServerData == null)
                {
                    sync_ServerData = FindObjectOfType<SynchedServerData>();
                }
                if (sync_ServerData == null)
                {
                    Debug.LogError("No ServerSingleton in scene, did you run this from the bootstrap scene?");
                    return null;
                }
                return sync_ServerData;
            }
        }



        static SynchedServerData sync_ServerData;


        public void TryPreLoadBuildablePrefab(PvPBuildingCategory category, string prefabName)
        {
            LoadBuildablePrefabServerRpc(category, prefabName);
        }


        public void ActivateNetworkObject(ulong networkObjectId)
        {
            ActivateNetworkObjectClientRpc(networkObjectId);
        }

        public void DeactivateNetworkObject(ulong networkObjectId)
        {
            DeactivateNetworkObjectClientRpc(networkObjectId);
        }
        /// <summary>
        /// This call attempts to spawn a prefab by it's addressable guid - it ensures that all the clients have loaded the prefab before spawning it,
        /// and if the clients fail to acknowledge that they've loaded a prefab - the spawn will fail.
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public async Task<bool> TrySpawnCruiserDynamicSynchronously(IPvPPrefabKey iKey, PvPPrefab iPrefab)
        {

            if (IsServer)
            {
                var assetGuid = new AddressableGUID()
                {
                    Value = iKey.PrefabPath
                };

                if (DynamicPrefabLoadingUtilities.IsPrefabLoadedOnAllClients(assetGuid))
                {
                    Debug.Log("Prefab is already loaded by all peers, we can spawn it immediately");
                    // var obj = Spawn(assetGuid);
                    return true;
                }

                m_SynchronousSpawnAckCount = 0;
                m_SynchronousSpawnTimeoutTimer = 0;

                Debug.Log("Loading dynamic prefab on the clients... ---> " + iKey.PrefabPath);
                LoadAddressableClientRpc(iKey.PrefabPath);



                //load the prefab on the server, so that any late-joiner will need to load that prefab also
                DynamicPrefabLoadingUtilities.LoadDynamicPrefab(assetGuid /*, m_InGameUI.ArtificialDelayMilliseconds*/, iPrefab);


                var requiredAcknowledgementsCount = IsHost ? NetworkManager.Singleton.ConnectedClients.Count - 1 :
                    NetworkManager.Singleton.ConnectedClients.Count;

                while (m_SynchronousSpawnTimeoutTimer < NetworkSpawnTimeoutSeconds)
                {
                    if (m_SynchronousSpawnAckCount >= requiredAcknowledgementsCount)
                    {
                        Debug.Log($"All clients have loaded the prefab in {m_SynchronousSpawnTimeoutTimer} seconds, spawning the prefab on the server...");
                        // var obj = Spawn(assetGuid);
                        return true;
                    }

                    m_SynchronousSpawnTimeoutTimer += Time.deltaTime;
                    await Task.Yield();
                }

                // left to the reader: you'll need to be reactive to clients failing to load -- you should either have
                // the offending client try again or disconnect it after a predetermined amount of failed attempts
                Debug.LogError("Failed to spawn dynamic prefab - timeout");
                return false;
            }

            return false;

        }


        [ServerRpc(RequireOwnership = false)]
        void LoadBuildablePrefabServerRpc(PvPBuildingCategory category, string prefabName, ServerRpcParams rpcParams = default)
        {
            PvPBuildingKey buildingKey = new PvPBuildingKey(category, prefabName);
            PvPPrefab iPrefab = PvPBattleSceneGodServer.Instance.factoryProvider.PrefabFactory.GetBuildingWrapperPrefab(buildingKey).UnityObject;
            TrySpawnCruiserDynamicSynchronously(buildingKey, iPrefab);
        }

        [ClientRpc]
        void LoadAddressableClientRpc(string prefabPath, ClientRpcParams rpcParams = default)
        {
            if (!IsHost)
            {
                Load(prefabPath);
            }

            void Load(string _prefabPath)
            {
                Debug.Log("Loading dynamic prefab on the client..." + _prefabPath);
                PvPPrefab iPrefab = PvPBattleSceneGodClient.Instance.factoryProvider.PrefabFactory.GetPrefab(_prefabPath);
                if (iPrefab == null)
                {
                    Debug.LogError("Prefab Not found in client");
                    return;
                }
                var assetGuid = new AddressableGUID() { Value = _prefabPath };
                DynamicPrefabLoadingUtilities.LoadDynamicPrefab(assetGuid, iPrefab /*, m_InGameUI.ArtificialDelayMilliseconds*/);
                Debug.Log("Client loaded dynamic prefab" + _prefabPath);

                AcknowledgeSuccessfulPrefabLoadServerRpc(assetGuid.GetHashCode());
            }
        }

        [ServerRpc(RequireOwnership = false)]
        void AcknowledgeSuccessfulPrefabLoadServerRpc(int prefabHash, ServerRpcParams rpcParams = default)
        {
            m_SynchronousSpawnAckCount++;
            Debug.Log($"Client acknowledged successful prefab load with hash: {prefabHash}");
            DynamicPrefabLoadingUtilities.RecordThatClientHasLoadedAPrefab(prefabHash,
                rpcParams.Receive.SenderClientId);
        }

        [ClientRpc]
        private void ActivateNetworkObjectClientRpc(ulong networkObjectId)
        {
            NetworkObject obj = PvPBattleSceneGodClient.Instance.GetNetworkObject(networkObjectId);
            obj.gameObject.SetActive(true);
            
        }

        [ClientRpc]
        private void DeactivateNetworkObjectClientRpc(ulong networkObjectId)
        {
            NetworkObject obj = PvPBattleSceneGodClient.Instance.GetNetworkObject(networkObjectId);
            obj.gameObject.SetActive(false);
            
        }
    }
}
