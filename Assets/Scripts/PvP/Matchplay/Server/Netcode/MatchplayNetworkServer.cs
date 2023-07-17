using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine.SceneManagement;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;
using BattleCruisers.Network.Multiplay.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.Server
{
    public class MatchplayNetworkServer : IDisposable
    {
        public Action<Matchplayer> OnServerPlayerSpawned;
        public Action<Matchplayer> OnServerPlayerDespawned;

        public Action<UserData> OnPlayerLeft;
        public Action<UserData> OnPlayerJoined;

        NetworkManager m_NetworkManager;
        public int PlayerCount => m_NetworkManager.ConnectedClients.Count;
        SynchedServerData m_SynchedServerData;
        bool m_InitializedServer;

        const int k_MaxConnectPayload = 1024;

        Dictionary<string, UserData> m_clientData = new Dictionary<string, UserData>();
        Dictionary<ulong, string> m_NetworkIdToAuth = new Dictionary<ulong, string>();

        public MatchplayNetworkServer(NetworkManager networkManager)
        {
            m_NetworkManager = networkManager;
            m_NetworkManager.NetworkConfig.ConnectionApproval = true;
            // Here, we keep ForceSamePrefabs disabled. This will allow us to dynamically add network prefabs to Netcode
            // for GameObject after establishing a connection.
            m_NetworkManager.NetworkConfig.ForceSamePrefabs = false;
            m_NetworkManager.ConnectionApprovalCallback += ApprovalCheck;
            m_NetworkManager.OnServerStarted += OnNetworkReady;
            DynamicPrefabLoadingUtilities.Init(m_NetworkManager);
        }


        public bool OpenConnection(string ip, int port, GameInfo startingGameInfo)
        {
            var unityTransport = m_NetworkManager.gameObject.GetComponent<UnityTransport>();
            m_NetworkManager.NetworkConfig.NetworkTransport = unityTransport;
            unityTransport.SetConnectionData(ip, (ushort)port);
            Debug.Log($"Starting server at {ip}:{port}\nWith: {startingGameInfo}");

            return m_NetworkManager.StartServer();
        }


        public async Task<SynchedServerData> ConfigureServer(GameInfo startingGameInfo)
        {

            m_NetworkManager.SceneManager.LoadScene(startingGameInfo.ToSceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
            var localNetworkedSceneLoaded = false;
            m_NetworkManager.SceneManager.OnLoadComplete += CreateAndSetSynchedServerData;
            void CreateAndSetSynchedServerData(ulong clientId, string sceneName, LoadSceneMode sceneMode)
            {
                if (clientId != m_NetworkManager.LocalClientId)
                    return;
                localNetworkedSceneLoaded = true;
                m_NetworkManager.SceneManager.OnLoadComplete -= CreateAndSetSynchedServerData;
            }

            var waitTask = WaitUntilSceneLoaded();
            async Task WaitUntilSceneLoaded()
            {
                while (!localNetworkedSceneLoaded)
                    await Task.Delay(50);
            }
            if (await Task.WhenAny(waitTask, Task.Delay(50000)) != waitTask)
            {
                Debug.LogWarning($"Timed out waiting for Server Scene Loading: Not able to Load Scene");
                return null;
            }

            m_SynchedServerData = GameObject.Instantiate(Resources.Load<SynchedServerData>("SynchedServerData"));
            m_SynchedServerData.GetComponent<NetworkObject>().Spawn();

            m_SynchedServerData.map.Value = startingGameInfo.map;
            m_SynchedServerData.gameMode.Value = startingGameInfo.gameMode;
            m_SynchedServerData.gameQueue.Value = startingGameInfo.gameQueue;

            Debug.Log($"Synched Server Values: {m_SynchedServerData.map.Value} - {m_SynchedServerData.gameMode.Value} - {m_SynchedServerData.gameQueue.Value}", m_SynchedServerData.gameObject);
            return m_SynchedServerData;
        }


        void OnNetworkReady()
        {
            m_NetworkManager.OnClientDisconnectCallback += OnClientDisconnect;
        }

        void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
        {
            if (request.Payload.Length > k_MaxConnectPayload)
            {
                response.Approved = false;
                response.CreatePlayerObject = false;
                response.Position = null;
                response.Rotation = null;
                response.Pending = false;

                Debug.LogError($"Connection payload was too big! : {request.Payload.Length} / {k_MaxConnectPayload}");
                return;
            }


            var payload = System.Text.Encoding.UTF8.GetString(request.Payload);
            var userData = JsonUtility.FromJson<UserData>(payload);
            userData.networkId = request.ClientNetworkId;
            if (m_clientData.Count == 0)
            {
                m_SynchedServerData.playerAPrefabName.Value = userData.hullPrefabName;
                m_SynchedServerData.playerAClientNetworkId.Value = userData.networkId;
            }
            else if (m_clientData.Count == 1)
            {
                m_SynchedServerData.playerBPrefabName.Value = userData.hullPrefabName;
                m_SynchedServerData.playerBClientNetworkId.Value = userData.networkId;
            }

            Debug.Log($"Host ApprovalCheck: connecting client: ({request.ClientNetworkId}) - {userData}");

            if (m_clientData.ContainsKey(userData.userAuthId))
            {
                ulong oldClientId = m_clientData[userData.userAuthId].networkId;
                Debug.Log($"Duplicate ID Found : {userData.userAuthId}, Disconnecting Old User");
                SendClientDisconnected(request.ClientNetworkId, MatchplayConnectStatus.LoggedInAgain);
                WaitToDisconnect(oldClientId);
            }
            SendClientConnected(request.ClientNetworkId, MatchplayConnectStatus.Success);
            m_NetworkIdToAuth[request.ClientNetworkId] = userData.userAuthId;
            m_clientData[userData.userAuthId] = userData;
            OnPlayerJoined?.Invoke(userData);


            response.Approved = true;
            response.CreatePlayerObject = true;
            response.Position = Vector3.zero;
            response.Rotation = Quaternion.identity;
            response.Pending = false;
            response.CreatePlayerObject = false;

            // var schedular = TaskScheduler.FromCurrentSynchronizationContext();
            // Task.Factory.StartNew(
            //     async () => await SetupPlayerPrefab(request.ClientNetworkId, userData.userName),
            //     System.Threading.CancellationToken.None,
            //     TaskCreationOptions.None, schedular
            // );
        }

        private void OnClientDisconnect(ulong networkId)
        {
            SendClientDisconnected(networkId, MatchplayConnectStatus.GenericDisconnect);
            if (m_NetworkIdToAuth.TryGetValue(networkId, out var authId))
            {
                m_NetworkIdToAuth?.Remove(networkId);
                OnPlayerLeft?.Invoke(m_clientData[authId]);

                if (m_clientData[authId].networkId == networkId)
                {
                    m_clientData.Remove(authId);
                }
            }
/*            var matchPlayerIntance = GetNetworkedMatchPlayer(networkId);
            OnServerPlayerDespawned?.Invoke(matchPlayerIntance);*/
        }


        async Task SetupPlayerPrefab(ulong networkId, string playerName)
        {
            NetworkObject playerNetworkObject;
            do
            {
                playerNetworkObject = m_NetworkManager.SpawnManager.GetPlayerNetworkObject(networkId);
                await Task.Delay(100);
            } while (playerNetworkObject == null);

            var networkedMatchPlayer = GetNetworkedMatchPlayer(networkId);
            networkedMatchPlayer.PlayerName.Value = playerName;
            // networkedMatchPlayer.PlayerColor.Value = Customization.IDToColor(networkId);
            OnServerPlayerSpawned?.Invoke(networkedMatchPlayer);
        }


        Matchplayer GetNetworkedMatchPlayer(ulong networkId)
        {
            var networkObject = m_NetworkManager.SpawnManager.GetPlayerNetworkObject(networkId);
            return networkObject.GetComponent<Matchplayer>();
        }

        async void WaitToDisconnect(ulong networkId)
        {
            await Task.Delay(500);
            m_NetworkManager.DisconnectClient(networkId);
        }

        void SendClientConnected(ulong networkId, MatchplayConnectStatus status)
        {
            var writer = new FastBufferWriter(sizeof(MatchplayConnectStatus), Unity.Collections.Allocator.Temp);
            writer.WriteValueSafe(status);
            Debug.Log($"Send Network Client Connected to : {networkId}");
            MatchplayNetworkMessenger.SendMessageTo(NetworkMessage.LocalClientConnected, networkId, writer);
        }


        void SendClientDisconnected(ulong networkId, MatchplayConnectStatus status)
        {
            var writer = new FastBufferWriter(sizeof(MatchplayConnectStatus), Unity.Collections.Allocator.Temp);
            writer.WriteValueSafe(status);
            Debug.Log($"Send networkClient Disconnected to : {networkId}");
            MatchplayNetworkMessenger.SendMessageTo(NetworkMessage.LocalClientDisconnected, networkId, writer);
        }


        public void Dispose()
        {
            if (m_NetworkManager == null)
            {
                return;
            }
            m_NetworkManager.ConnectionApprovalCallback -= ApprovalCheck;
            m_NetworkManager.OnClientDisconnectCallback -= OnClientDisconnect;
            m_NetworkManager.OnServerStarted -= OnNetworkReady;
            if (m_NetworkManager.IsListening)
            {
                m_NetworkManager.Shutdown();
            }
        }
    }
}

