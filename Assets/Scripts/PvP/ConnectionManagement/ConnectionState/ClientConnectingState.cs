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
        private bool m_HasConnectedSuccessfully = false;

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
            m_HasConnectedSuccessfully = false; // Reset on each new connection attempt

            if (MatchmakingScreenController.Instance != null)
                MatchmakingScreenController.Instance.SetMMStatus(MatchmakingScreenController.MMStatus.CONNECTING);

            _ = ConnectClientAsync();
        }

        public override void OnClientConnected(ulong _)
        {
            m_HasConnectedSuccessfully = true;
            Debug.Log("PVP: CLIENT connected, switching to ClientConnectedState");
            m_ConnectionManager.ChangeState(m_ConnectionManager.m_ClientConnected);
        }

        public override void OnClientDisconnect(ulong clientId)
        {
            Debug.LogWarning($"PVP: CLIENT connection rejected/failed (clientId={clientId}, WasConnected={m_HasConnectedSuccessfully})");

            if (!m_HasConnectedSuccessfully)
            {
                // Never successfully connected - this is a connection rejection
                // Try next lobby or host our own
                Debug.Log("PVP: Connection rejected before match started - retrying with next lobby");
                _ = HandleConnectionRejectionAsync();
            }
            else
            {
                // Was connected, then disconnected during match - don't auto-retry
                Debug.LogError("PVP: Disconnected after successful connection - not auto-retrying");
                m_ConnectionManager.ChangeState(m_ConnectionManager.m_Offline);
            }
        }

        public override void OnTransportFailure()
        {
            Debug.LogError("PVP: CLIENT transport layer failed");
            StartingClientFailedAsync();
        }

        private async Task HandleConnectionRejectionAsync()
        {
            // Leave the lobby we tried to join (HOST rejected us or join code expired)
            if (m_LobbyServiceFacade.CurrentUnityLobby != null)
            {
                string lobbyId = m_LobbyServiceFacade.CurrentUnityLobby.Id;
                Debug.Log($"PVP: Leaving lobby after rejection (LobbyId={lobbyId})");
                try
                {
                    await m_LobbyServiceFacade.LeaveLobbyAsync(lobbyId);
                }
                catch (System.Exception ex)
                {
                    Debug.LogWarning($"PVP: Failed to leave lobby after rejection: {ex.Message}");
                }
            }

            // Change to offline state
            m_ConnectionManager.ChangeState(m_ConnectionManager.m_Offline);

            // Retry matchmaking - try next lobby or host our own
            if (BattleCruisers.Network.Multiplay.Scenes.PvPBootManager.Instance != null)
            {
                Debug.Log("PVP: CONNECTION REJECTED - retrying TryJoinLobby to find another match or host our own");
                await Task.Delay(500); // Brief delay to ensure cleanup completes
                await BattleCruisers.Network.Multiplay.Scenes.PvPBootManager.Instance.TryJoinLobby();
            }
            else
            {
                Debug.LogError("PVP: Cannot retry - PvPBootManager.Instance is null");
            }
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
                Debug.LogWarning($"PVP: CLIENT failed to connect (RelayCode={m_LocalLobby.RelayJoinCode}, Error={e.Message})");

                // Failed before connection - this is a relay/join code issue (expected during matchmaking)
                // Retry with next lobby or host our own
                Debug.Log("PVP: Connection setup failed - retrying with next lobby or hosting");
                await HandleConnectionRejectionAsync();
            }
        }
    }
}

