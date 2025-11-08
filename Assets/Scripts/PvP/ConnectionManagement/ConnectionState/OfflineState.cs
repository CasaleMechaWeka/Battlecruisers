using BattleCruisers.Network.Multiplay.UnityServices.Lobbies;
using BattleCruisers.Network.Multiplay.Utils;
using BattleCruisers.UI.ScreensScene.Multiplay.ArenaScreen;
using BattleCruisers.Network.Multiplay.ApplicationLifecycle;
using BattleCruisers.Network.Multiplay.Gameplay.UI;
using BattleCruisers.Network.Multiplay.Infrastructure;
using UnityEngine;
using BattleCruisers.Scenes;
using BattleCruisers.Utils;

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
#pragma warning disable 4014
            m_LobbyServiceFacade.EndTracking();
#pragma warning restore 4014

            if (MatchmakingScreenController.Instance != null)
            {
                Debug.Log("PVP: Connection failed, calling MatchmakingScreen.FailedMatchmaking");
                MatchmakingScreenController.Instance.FailedMatchmaking();
            }
            else if (BattleCruisers.UI.ScreensScene.BattleHubScreen.PrivateMatchmakingController.Instance != null)
            {
                Debug.Log("PVP: Connection failed, calling PrivateMatchmaking.FailedMatchmaking");
                BattleCruisers.UI.ScreensScene.BattleHubScreen.PrivateMatchmakingController.Instance.FailedMatchmaking();
            }
            else
            {
                Debug.LogWarning("PVP: No matchmaking controller found, falling back to ScreensScene");
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

                SceneNavigator.GoToScene(SceneNames.SCREENS_SCENE, true);
            }
        }
        public override void Exit() { }

        public override void StartClientIP(string playerName, string ipaddress, int port)
        {
            ConnectionMethodIP connectionMethod = new ConnectionMethodIP(ipaddress, (ushort)port, m_ConnectionManager, m_ProfileManager, playerName);
            m_ConnectionManager.m_ClientReconnecting.Configure(connectionMethod);
            m_ConnectionManager.ChangeState(m_ConnectionManager.m_ClientConnecting.Configure(connectionMethod));
        }

        public override void StartClientLobby(string playerName)
        {
            ConnectionMethodLobby connectionMethod = new ConnectionMethodLobby(m_LobbyServiceFacade, m_LocalLobby, m_ConnectionManager, m_ProfileManager, playerName);
            m_ConnectionManager.m_ClientReconnecting.Configure(connectionMethod);
            m_ConnectionManager.ChangeState(m_ConnectionManager.m_ClientConnecting.Configure(connectionMethod));
        }

        public override void StartHostIP(string playerName, string ipaddress, int port)
        {
            ConnectionMethodIP connectionMethod = new ConnectionMethodIP(ipaddress, (ushort)port, m_ConnectionManager, m_ProfileManager, playerName);
            m_ConnectionManager.ChangeState(m_ConnectionManager.m_StartingHost.Configure(connectionMethod));
        }

        public override void StartHostLobby(string playerName)
        {
            ConnectionMethodLobby connectionMethod = new ConnectionMethodLobby(m_LobbyServiceFacade, m_LocalLobby, m_ConnectionManager, m_ProfileManager, playerName);
            m_ConnectionManager.ChangeState(m_ConnectionManager.m_StartingHost.Configure(connectionMethod));
        }
    }
}

