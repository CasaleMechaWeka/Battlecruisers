using System.Threading.Tasks;
using UnityEngine;
using System;
using BattleCruisers.Network.Multiplay.UnityServices.Lobbies;
using BattleCruisers.UI.ScreensScene.Multiplay.ArenaScreen;
using BattleCruisers.UI.ScreensScene.BattleHubScreen;
using BattleCruisers.Scenes;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Network.Multiplay.Infrastructure;
using BattleCruisers.Network.Multiplay.Utils;

namespace BattleCruisers.Network.Multiplay.ConnectionManagement
{
    class ClientConnectingState : OnlineState
    {
        protected LobbyServiceFacade m_LobbyServiceFacade;
        protected LocalLobby m_LocalLobby;
        ConnectionMethodBase m_ConnectionMethod;

        public ClientConnectingState(
            LobbyServiceFacade lobbyServiceFacade,
            LocalLobby localLobby,
            ConnectionManager connectionManager,
            IPublisher<ConnectStatus> connectStatusPublisher)
            : base(connectionManager, connectStatusPublisher)
        {
            m_LobbyServiceFacade = lobbyServiceFacade;
            m_LocalLobby = localLobby;
        }

        public ClientConnectingState Configure(ConnectionMethodBase baseConnectionMethod)
        {
            m_ConnectionMethod = baseConnectionMethod;
            return this;
        }
        public override void Enter()
        {
            if (MatchmakingScreenController.Instance != null)
                MatchmakingScreenController.Instance.SetMMStatus(MatchmakingScreenController.MMStatus.CONNECTING);

            _ = ConnectClientAsync();
        }

        public override void OnClientConnected(ulong _)
        {
            Debug.Log("PVP: CLIENT connected, switching to ClientConnectedState");
            m_ConnectionManager.ChangeState(m_ConnectionManager.m_ClientConnected);
        }

        public override void OnClientDisconnect(ulong clientId)
        {
            Debug.LogError($"PVP: CLIENT connection rejected/failed (clientId={clientId})");
            StartingClientFailedAsync();
        }

        public override void OnTransportFailure()
        {
            Debug.LogError("PVP: CLIENT transport layer failed");
            StartingClientFailedAsync();
        }

        protected void StartingClientFailedAsync()
        {
            m_ConnectionManager.ChangeState(m_ConnectionManager.m_Offline);
        }
        internal async Task ConnectClientAsync()
        {
            try
            {
                Debug.Log("PVP: CLIENT ConnectClientAsync - before SetupClientConnectionAsync");
                await m_ConnectionMethod.SetupClientConnectionAsync();
                Debug.Log("PVP: CLIENT ConnectClientAsync - after SetupClientConnectionAsync");

                if (DynamicPrefabLoadingUtilities.HashOfDynamicPrefabGUIDs == -1)
                {
                    Debug.Log("PVP: CLIENT - before DynamicPrefabLoadingUtilities.Init");
                    DynamicPrefabLoadingUtilities.Init(m_ConnectionManager.NetworkManager);
                    Debug.Log("PVP: CLIENT - after DynamicPrefabLoadingUtilities.Init");
                }

                Debug.Log($"PVP: CLIENT relay configured (RelayCode={m_LocalLobby.RelayJoinCode}, Private={ArenaSelectPanelScreenController.PrivateMatch}) - starting NetworkManager");

                if (!m_ConnectionManager.NetworkManager.StartClient())
                {
                    throw new System.Exception("NetworkManager StartClient failed");
                }
                Debug.Log($"PVP: CLIENT connecting to HOST (RelayCode={m_LocalLobby.RelayJoinCode})");
            }
            catch (Exception e)
            {
                Debug.LogError($"PVP: CLIENT failed to connect (RelayCode={m_LocalLobby.RelayJoinCode}, Error={e.Message})");

                if (LandingSceneGod.Instance != null)
                {
                    LandingSceneGod.Instance.messagebox.ShowMessage(LocTableCache.CommonTable.GetString("NetworkError"));
                }
                else if (PrivateMatchmakingController.Instance != null && PrivateMatchmakingController.Instance.messageBox != null)
                {
                    Debug.Log("PVP: CLIENT showing connection error in PrivateMatchmakingController");
                    PrivateMatchmakingController.Instance.ShowBadInternetMessageBox();
                }
                else
                {
                    Debug.LogError("PVP: CLIENT no message box available for error display");
                }

                StartingClientFailedAsync();
                throw;
            }
        }
    }
}

