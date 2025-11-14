using BattleCruisers.Network.Multiplay.UnityServices.Lobbies;
using BattleCruisers.Network.Multiplay.Utils;
using BattleCruisers.UI.ScreensScene.Multiplay.ArenaScreen;
using BattleCruisers.Network.Multiplay.ApplicationLifecycle;
using BattleCruisers.Network.Multiplay.Gameplay.UI;
using BattleCruisers.Network.Multiplay.Infrastructure;
using UnityEngine;
using BattleCruisers.Utils;
using BattleCruisers.UI.ScreensScene.BattleHubScreen;

namespace BattleCruisers.Network.Multiplay.ConnectionManagement
{
    class OfflineState : ConnectionState
    {
        LobbyServiceFacade m_LobbyServiceFacade;
        ProfileManager m_ProfileManager;
        LocalLobby m_LocalLobby;

        public OfflineState(
            LobbyServiceFacade lobbyServiceFacade,
            ProfileManager profileManager,
            LocalLobby localLobby,
            ConnectionManager connectionManager,
            IPublisher<ConnectStatus> connectStatusPublisher)
            : base(connectionManager, connectStatusPublisher)
        {
            m_LobbyServiceFacade = lobbyServiceFacade;
            m_ProfileManager = profileManager;
            m_LocalLobby = localLobby;
        }
        public override void Enter()
        {
            m_LobbyServiceFacade.EndTracking();

            if (MatchmakingScreenController.Instance != null)
            {
                Debug.Log("PVP: Connection failed, calling MatchmakingScreen.FailedMatchmaking");
                MatchmakingScreenController.Instance.FailedMatchmaking();
            }
            else if (PrivateMatchmakingController.Instance != null)
            {
                Debug.Log("PVP: Connection failed, calling PrivateMatchmaking.FailedMatchmaking");
                PrivateMatchmakingController.Instance.FailedMatchmaking();
            }
            else
            {
                Debug.LogWarning("PVP: No matchmaking controller found, cleaning up network objects and unloading PVP scenes");
                if (GameObject.Find("ApplicationController") != null)
                    GameObject.Find("ApplicationController").GetComponent<ApplicationController>().DestroyNetworkObject();

                if (GameObject.Find("ConnectionManager") != null)
                    GameObject.Find("ConnectionManager").GetComponent<ConnectionManager>().DestroyNetworkObject();

                if (GameObject.Find("PopupPanelManager") != null)
                    GameObject.Find("PopupPanelManager").GetComponent<PopupManager>().DestroyNetworkObject();

                if (GameObject.Find("UIMessageManager") != null)
                    GameObject.Find("UIMessageManager").GetComponent<ConnectionStatusMessageUIManager>().DestroyNetworkObject();

                if (GameObject.Find("UpdateRunner") != null)
                    GameObject.Find("UpdateRunner").GetComponent<UpdateRunner>().DestroyNetworkObject();

                if (GameObject.Find("NetworkManager") != null)
                    GameObject.Find("NetworkManager").GetComponent<BCNetworkManager>().DestroyNetworkObject();

                if (UnityEngine.SceneManagement.SceneManager.GetSceneByName(SceneNames.PvP_INITIALIZE_SCENE).isLoaded)
                {
                    Debug.Log("PVP: OfflineState unloading PvPInitializeScene");
                    UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(SceneNames.PvP_INITIALIZE_SCENE);
                }
                if (UnityEngine.SceneManagement.SceneManager.GetSceneByName(SceneNames.PRIVATE_PVP_INITIALIZER_SCENE).isLoaded)
                {
                    Debug.Log("PVP: OfflineState unloading PrivatePVPInitializer");
                    UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(SceneNames.PRIVATE_PVP_INITIALIZER_SCENE);
                }
                if (UnityEngine.SceneManagement.SceneManager.GetSceneByName("PvPBattleScene").isLoaded)
                {
                    Debug.Log("PVP: OfflineState unloading PvPBattleScene");
                    UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("PvPBattleScene");
                }
            }
        }

        public override void StartClientIP(string playerName, string ipaddress, int port)
        {
            ConnectionMethodIP connectionMethod = new ConnectionMethodIP(ipaddress, (ushort)port, m_ConnectionManager, m_ProfileManager, playerName);
            m_ConnectionManager.m_ClientReconnecting.Configure(connectionMethod);
            m_ConnectionManager.ChangeState(m_ConnectionManager.m_ClientConnecting.Configure(connectionMethod));
        }

        public override void StartClientLobby(string playerName)
        {
            Debug.Log("PVP: OfflineState.StartClientLobby - creating ConnectionMethodLobby");
            ConnectionMethodLobby connectionMethod = new ConnectionMethodLobby(m_LobbyServiceFacade, m_LocalLobby, m_ConnectionManager, m_ProfileManager, playerName);
            Debug.Log("PVP: OfflineState.StartClientLobby - configuring ClientReconnecting");
            m_ConnectionManager.m_ClientReconnecting.Configure(connectionMethod);
            Debug.Log("PVP: OfflineState.StartClientLobby - changing state to ClientConnecting");
            m_ConnectionManager.ChangeState(m_ConnectionManager.m_ClientConnecting.Configure(connectionMethod));
            Debug.Log("PVP: OfflineState.StartClientLobby - state changed");
        }

        public override void StartHostIP(string playerName, string ipaddress, int port)
        {
            ConnectionMethodIP connectionMethod = new ConnectionMethodIP(ipaddress, (ushort)port, m_ConnectionManager, m_ProfileManager, playerName);
            m_ConnectionManager.ChangeState(m_ConnectionManager.m_StartingHost.Configure(connectionMethod));
        }

        public override void StartHostLobby(string playerName)
        {
            Debug.Log("PVP: OfflineState.StartHostLobby - creating ConnectionMethodLobby");
            ConnectionMethodLobby connectionMethod = new ConnectionMethodLobby(m_LobbyServiceFacade, m_LocalLobby, m_ConnectionManager, m_ProfileManager, playerName);
            Debug.Log("PVP: OfflineState.StartHostLobby - changing state to StartingHost");
            m_ConnectionManager.ChangeState(m_ConnectionManager.m_StartingHost.Configure(connectionMethod));
            Debug.Log("PVP: OfflineState.StartHostLobby - state changed");
        }
    }
}
