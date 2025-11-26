using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using BattleCruisers.Network.Multiplay.Utils;
using BattleCruisers.Network.Multiplay.Infrastructure;
using BattleCruisers.Network.Multiplay.Matchplay.Client;
using BattleCruisers.Data.Static;
using BattleCruisers.Network.Multiplay.UnityServices.Lobbies;

namespace BattleCruisers.Network.Multiplay.ConnectionManagement
{
    public enum ConnectStatus
    {
        Undefined,
        Success,                  //client successfully connected. This may also be a successful reconnect.
        ServerFull,               //can't join, server is already at capacity.
        LoggedInAgain,            //logged in on a separate client, causing this one to be kicked out.
        UserRequestedDisconnect,  //Intentional Disconnect triggered by the user.
        GenericDisconnect,        //server disconnected, but no specific reason given.
        Reconnecting,             //client lost connection and is attempting to reconnect.
        IncompatibleBuildType,    //client build type is incompatible with server.
        HostEndedSession,         //host intentionally ended the session.
        StartHostFailed,          // server failed to bind
        StartClientFailed         // failed to connect to server and/or invalid network endpoint
    }

    public struct ReconnectMessage
    {
        public int CurrentAttempt;
        public int MaxAttempt;

        public ReconnectMessage(int currentAttempt, int maxAttempt)
        {
            CurrentAttempt = currentAttempt;
            MaxAttempt = maxAttempt;
        }
    }

    [Serializable]
    public class ConnectionPayload
    {
        public string playerId;
        public string playerName;
        public int playerHullID;
        public long playerScore;
        public ulong playerNetworkId;
        public string playerCaptainPrefabName;
        public int playerGameMap;
        public float playerRating;
        public int playerBodykit;
        public int playerBounty;
        //    public string playerSelectedVariants;
        //        public bool isDebug;
    }

    /// <summary>
    /// This state machine handles connection through the NetworkManager. It is responsible for listening to
    /// NetworkManger callbacks and other outside calls and redirecting them to the current ConnectionState object.
    /// </summary>
    public class ConnectionManager : MonoBehaviour, INetworkObject
    {
        ConnectionState m_CurrentState;

        public NetworkManager NetworkManager { get; private set; }

        [SerializeField]
        int m_NbReconnectAttempts = 2;

        public int NbReconnectAttempts => m_NbReconnectAttempts;

        public int MaxConnectedPlayers = 2;

        // Latency gets assigned from Remote Config values now
        private static int latencyLimit;
        public static int LatencyLimit
        {
            get { return latencyLimit; }
            set { latencyLimit = value; }
        }

        internal OfflineState m_Offline;
        internal ClientConnectingState m_ClientConnecting;
        internal ClientConnectedState m_ClientConnected;
        internal ClientReconnectingState m_ClientReconnecting;
        internal StartingHostState m_StartingHost;
        internal HostingState m_Hosting;

        public void Initialise(
            NetworkManager networkManager,
            LobbyServiceFacade lobbyServiceFacade,
            ProfileManager profileManager,
            IPublisher<ConnectStatus> connectStatusPublisher,
            LocalLobby localLobby,
            IPublisher<ReconnectMessage> reconnectMessagePublisher)
        {
            NetworkManager = networkManager;
            m_LobbyServiceFacade = lobbyServiceFacade;
            m_LocalLobby = localLobby;
            m_ProfileManager = profileManager;

            m_Offline = new OfflineState(lobbyServiceFacade,
                                         profileManager,
                                         localLobby,
                                         this,
                                         connectStatusPublisher);
            m_ClientConnecting = new ClientConnectingState(lobbyServiceFacade,
                                                           localLobby,
                                                           this,
                                                           connectStatusPublisher);
            m_ClientConnected = new ClientConnectedState(lobbyServiceFacade,
                                                         this,
                                                         connectStatusPublisher);
            m_ClientReconnecting = new ClientReconnectingState(reconnectMessagePublisher,
                                                               lobbyServiceFacade,
                                                               localLobby,
                                                               this,
                                                               connectStatusPublisher);
            m_StartingHost = new StartingHostState(localLobby,
                                                   this,
                                                   connectStatusPublisher);
            m_Hosting = new HostingState(lobbyServiceFacade,
                                         this,
                                         connectStatusPublisher);


#if UNITY_EDITOR
            LatencyLimit = 2000;
            Debug.Log("Running in editor mode, latency limit set to 2000. Remote Config latency limit would be " + StaticData.MaxLatency);
#else
            LatencyLimit = StaticData.MaxLatency;
            if (LatencyLimit == 0) // Just in case 
            {
                LatencyLimit = 300;
            }
            Debug.Log("Remote Config latency limit set to " + StaticData.MaxLatency);
#endif

            List<ConnectionState> states = new() { m_Offline, m_ClientConnecting, m_ClientConnected, m_ClientReconnecting, m_StartingHost, m_Hosting };

            m_CurrentState = m_Offline;
            // Here, we keep ForceSamePrefabs disabled. This will allow us to dynamically add network prefabs to Netcode
            // for GameObject after establishing a connection.
            //    NetworkManager.NetworkConfig.ForceSamePrefabs = false;

            // Unsubscribe first to prevent double registration if Initialise is called multiple times
            // SceneManager might be null on first initialization before StartClient/StartHost
            NetworkManager.OnClientConnectedCallback -= OnClientConnectedCallback;
            NetworkManager.OnClientDisconnectCallback -= OnClientDisconnectCallback;
            NetworkManager.OnServerStarted -= OnServerStarted;
            NetworkManager.ConnectionApprovalCallback -= ApprovalCheck;
            NetworkManager.OnTransportFailure -= OnTransportFailure;
            if (NetworkManager.SceneManager != null)
            {
                NetworkManager.SceneManager.OnLoad -= OnSceneLoad;
                NetworkManager.SceneManager.OnLoadEventCompleted -= OnSceneLoadEventCompleted;
                NetworkManager.SceneManager.OnLoadComplete -= OnSceneLoadComplete;
            }

            // Now subscribe
            NetworkManager.OnClientConnectedCallback += OnClientConnectedCallback;
            NetworkManager.OnClientDisconnectCallback += OnClientDisconnectCallback;
            NetworkManager.OnServerStarted += OnServerStarted;
            NetworkManager.ConnectionApprovalCallback += ApprovalCheck;
            NetworkManager.OnTransportFailure += OnTransportFailure;
            if (NetworkManager.SceneManager != null)
            {
                NetworkManager.SceneManager.OnLoad += OnSceneLoad;
                NetworkManager.SceneManager.OnLoadEventCompleted += OnSceneLoadEventCompleted;
                NetworkManager.SceneManager.OnLoadComplete += OnSceneLoadComplete;
            }

            //    m_GameManager = new ClientGameManager(m_ProfileManager.Profile);
            //    DynamicPrefabLoadingUtilities.Init(m_NetworkManager);
        }

        public ClientGameManager Manager
        {
            get
            {
                if (m_GameManager != null) return m_GameManager;
                Debug.LogError($"CilentGameManger is missing, did you run StartClient()?");
                return null;
            }
            set { m_GameManager = value; }
        }

        ClientGameManager m_GameManager;

        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void DestroyNetworkObject()
        {
            if (NetworkManager != null)
            {
                NetworkManager.OnClientConnectedCallback -= OnClientConnectedCallback;
                NetworkManager.OnClientDisconnectCallback -= OnClientDisconnectCallback;
                NetworkManager.OnServerStarted -= OnServerStarted;
                NetworkManager.ConnectionApprovalCallback -= ApprovalCheck;
                NetworkManager.OnTransportFailure -= OnTransportFailure;

                if (NetworkManager.SceneManager != null)
                {
                    NetworkManager.SceneManager.OnLoad -= OnSceneLoad;
                    NetworkManager.SceneManager.OnLoadEventCompleted -= OnSceneLoadEventCompleted;
                    NetworkManager.SceneManager.OnLoadComplete -= OnSceneLoadComplete;
                }
            }
            Destroy(gameObject);
        }

        public void LeaveLobby()
        {
            m_CurrentState.LeaveLobby();
        }

        public void LockLobby()
        {
            m_CurrentState.LockLobby();
        }
        void OnDestroy()
        {
            if (NetworkManager != null)
            {
                NetworkManager.OnClientConnectedCallback -= OnClientConnectedCallback;
                NetworkManager.OnClientDisconnectCallback -= OnClientDisconnectCallback;
                NetworkManager.OnServerStarted -= OnServerStarted;
                NetworkManager.ConnectionApprovalCallback -= ApprovalCheck;
                NetworkManager.OnTransportFailure -= OnTransportFailure;

                if (NetworkManager.SceneManager != null)
                {
                    NetworkManager.SceneManager.OnLoad -= OnSceneLoad;
                    NetworkManager.SceneManager.OnLoadEventCompleted -= OnSceneLoadEventCompleted;
                    NetworkManager.SceneManager.OnLoadComplete -= OnSceneLoadComplete;
                }
            }
        }

        internal void ChangeState(ConnectionState nextState)
        {
            if (m_CurrentState != null)
                m_CurrentState.Exit();

            m_CurrentState = nextState;
            m_CurrentState.Enter();
        }

        void OnClientDisconnectCallback(ulong clientId)
        {
            m_CurrentState.OnClientDisconnect(clientId);
        }

        void OnClientConnectedCallback(ulong clientId)
        {
            m_CurrentState.OnClientConnected(clientId);
        }

        void OnServerStarted()
        {
            m_CurrentState.OnServerStarted();
        }

        void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
        {
            m_CurrentState.ApprovalCheck(request, response);
        }

        void OnTransportFailure()
        {
            m_CurrentState.OnTransportFailure();
        }

        void OnSceneLoad(ulong clientId, string sceneName, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode, UnityEngine.AsyncOperation asyncOperation)
        {
            // Scene load callbacks - kept minimal for production
        }

        void OnSceneLoadEventCompleted(string sceneName, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode, System.Collections.Generic.List<ulong> clientsCompleted, System.Collections.Generic.List<ulong> clientsTimedOut)
        {
            if (clientsTimedOut.Count > 0)
            {
                Debug.LogError($"PVP: Scene load timeout - {clientsTimedOut.Count} clients failed to load {sceneName}");
            }
        }

        void OnSceneLoadComplete(ulong clientId, string sceneName, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode)
        {
        }

        public void StartClientLobby(string playerName)
        {
            m_CurrentState.StartClientLobby(playerName);
        }

        public void StartClientIp(string playerName, string ipaddress, int port)
        {
            m_CurrentState.StartClientIP(playerName, ipaddress, port);
        }

        public void StartHostLobby(string playerName)
        {
            m_CurrentState.StartHostLobby(playerName);
        }

        public void StartHostIp(string playerName, string ipaddress, int port)
        {
            m_CurrentState.StartHostIP(playerName, ipaddress, port);
        }
        public async System.Threading.Tasks.Task SetupRelayForMatchmaking(string playerName)
        {
            ConnectionMethodBase connectionMethod = new ConnectionMethodLobby(
                m_LobbyServiceFacade,
                m_LocalLobby,
                this,
                m_ProfileManager,
                playerName);
            UnityEngine.Debug.Log("PVP: SetupRelayForMatchmaking - creating relay allocation (lobby will become discoverable)");
            await connectionMethod.SetupHostConnectionAsync();
            UnityEngine.Debug.Log("PVP: SetupRelayForMatchmaking - relay allocated, lobby updated with RelayJoinCode");
        }

        private LobbyServiceFacade m_LobbyServiceFacade;
        private LocalLobby m_LocalLobby;
        private ProfileManager m_ProfileManager;
    }
}
