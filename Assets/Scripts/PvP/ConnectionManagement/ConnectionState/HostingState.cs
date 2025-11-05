using BattleCruisers.Network.Multiplay.UnityServices.Lobbies;
using BattleCruisers.Network.Multiplay.Infrastructure;
using Unity.Multiplayer.Samples.BossRoom;
using Unity.Netcode;

using UnityEngine;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;
using BattleCruisers.UI.ScreensScene.Multiplay.ArenaScreen;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene;
using BattleCruisers.UI.ScreensScene.BattleHubScreen;
using BattleCruisers.Network.Multiplay.Scenes;

namespace BattleCruisers.Network.Multiplay.ConnectionManagement
{
    class HostingState : OnlineState
    {
        LobbyServiceFacade m_LobbyServiceFacade;

        public HostingState(
            LobbyServiceFacade lobbyServiceFacade,
            ConnectionManager connectionManager,
            IPublisher<ConnectStatus> connectStatusPublisher)
            : base(connectionManager, connectStatusPublisher)
        {
            m_LobbyServiceFacade = lobbyServiceFacade;
        }

        // used in ApprovalCheck. This is intended as a bit of light protection against DOS attacks that rely on sending silly big buffers of garbage.
        const int k_MaxConnectPayload = 1024;

        public override void Enter()
        {
            if (m_LobbyServiceFacade.CurrentUnityLobby != null)
            {
                m_LobbyServiceFacade.BeginTracking();
                if (MatchmakingScreenController.Instance != null)
                    MatchmakingScreenController.Instance.SetMMStatus(MatchmakingScreenController.MMStatus.LOOKING_VICTIM);
            }
        }

        public override void LeaveLobby()
        {
            m_LobbyServiceFacade.EndTracking();
        }

        public override void LockLobby()
        {
            m_LobbyServiceFacade.LockLobby();
        }

        public override void Exit()
        {
            SessionManager<SessionPlayerData>.Instance.OnServerEnded();
        }

        public override void OnClientConnected(ulong clientId)
        {
            if (clientId == m_ConnectionManager.NetworkManager.LocalClientId
                && m_ConnectionManager.NetworkManager.ConnectedClientsIds.Count == 1)
            {
                Debug.Log($"PVP: HOST relay connected (LocalClientId={clientId}, ConnectedClients=1, Private={ArenaSelectPanelScreenController.PrivateMatch}) - loading scene");
                NetworkManager.Singleton.SceneManager.LoadScene("PvPBattleScene", UnityEngine.SceneManagement.LoadSceneMode.Single);
            }
            else if (clientId != m_ConnectionManager.NetworkManager.LocalClientId
                && m_ConnectionManager.NetworkManager.ConnectedClientsIds.Count == 2)
            {
                Debug.Log($"PVP: HOST CLIENT joined (clientId={clientId}, ConnectedClients=2, Private={ArenaSelectPanelScreenController.PrivateMatch}) - match ready");

                if (MatchmakingScreenController.Instance != null)
                {
                    MatchmakingScreenController.Instance.SetFoundVictimString();
                    MatchmakingScreenController.Instance.fleeButton.SetActive(false);
                    MatchmakingScreenController.Instance.vsAIButton.SetActive(false);
                }
            }
        }

        public override void OnClientDisconnect(ulong clientId)
        {
            Debug.Log($"PVP: HOST CLIENT disconnected (clientId={clientId}, RemainingClients={m_ConnectionManager.NetworkManager.ConnectedClientsIds.Count}, Private={ArenaSelectPanelScreenController.PrivateMatch})");
            PvPBattleSceneGodTunnel.OpponentQuit = true;
        }

        public override void OnUserRequestedShutdown()
        {
            string reason = JsonUtility.ToJson(ConnectStatus.HostEndedSession);
            for (int i = m_ConnectionManager.NetworkManager.ConnectedClientsIds.Count - 1; i >= 0; i--)
            {
                ulong id = m_ConnectionManager.NetworkManager.ConnectedClientsIds[i];
                if (id != m_ConnectionManager.NetworkManager.LocalClientId)
                {
                    m_ConnectionManager.NetworkManager.DisconnectClient(id, reason);
                }
            }
            m_ConnectionManager.ChangeState(m_ConnectionManager.m_Offline);
        }

        /// <summary>
        /// This logic plugs into the "ConnectionApprovalResponse" exposed by Netcode.NetworkManager. It is run every time a client connects to us.
        /// The complementary logic that runs when the client starts its connection can be found in ClientConnectingState.
        /// </summary>
        /// <remarks>
        /// Multiple things can be done here, some asynchronously. For example, it could authenticate your user against an auth service like UGS' auth service. It can
        /// also send custom messages to connecting users before they receive their connection result (this is useful to set status messages client side
        /// when connection is refused, for example).
        /// Note on authentication: It's usually harder to justify having authentication in a client hosted game's connection approval. Since the host can't be trusted,
        /// clients shouldn't send it private authentication tokens you'd usually send to a dedicated server.
        /// </remarks>
        /// <param name="request"> The initial request contains, among other things, binary data passed into StartClient. In our case, this is the client's GUID,
        /// which is a unique identifier for their install of the game that persists across app restarts.
        ///  <param name="response"> Our response to the approval process. In case of connection refusal with custom return message, we delay using the Pending field.
        public override void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
        {
            ulong clientId = request.ClientNetworkId;
            byte[] connectionData = request.Payload;
            string payload = System.Text.Encoding.UTF8.GetString(connectionData);
            ConnectionPayload connectionPayload = JsonUtility.FromJson<ConnectionPayload>(payload);
            connectionPayload.playerNetworkId = clientId;

            Debug.Log($"PVP: HOST approval requested (clientId={clientId}, ConnectedClients={m_ConnectionManager.NetworkManager.ConnectedClientsIds.Count}/{m_ConnectionManager.MaxConnectedPlayers}, Hull={connectionPayload.playerHullPrefabName}, Name={connectionPayload.playerName})");

            ConnectStatus gameReturnStatus = GetConnectStatus(connectionPayload);

            if (gameReturnStatus == ConnectStatus.Success)
            {
                if (clientId != m_ConnectionManager.NetworkManager.LocalClientId)
                {
                    if (SynchedServerData.Instance == null)
                    {
                        Debug.LogError($"PVP: HOST rejecting CLIENT (clientId={clientId}, Reason=SynchedServerData.Instance is NULL)");
                        response.Approved = false;
                        response.Reason = JsonUtility.ToJson(ConnectStatus.GenericDisconnect);
                        return;
                    }

                    SynchedServerData.Instance.playerAPrefabName.Value = PvPBootManager.Instance.playerAPrefabName;
                    SynchedServerData.Instance.playerAClientNetworkId.Value = PvPBootManager.Instance.playerAClientNetworkId;
                    SynchedServerData.Instance.playerAName.Value = PvPBootManager.Instance.playerAName;
                    SynchedServerData.Instance.playerAScore.Value = PvPBootManager.Instance.playerAScore;
                    SynchedServerData.Instance.captainAPrefabName.Value = PvPBootManager.Instance.captainAPrefabName;
                    SynchedServerData.Instance.playerARating.Value = PvPBootManager.Instance.playerRating;
                    SynchedServerData.Instance.playerABodykit.Value = PvPBootManager.Instance.playerABodykit;
                    SynchedServerData.Instance.playerABounty.Value = PvPBootManager.Instance.playerABounty;
                    PvPBattleSceneGodTunnel._playerACruiserName = PvPBootManager.Instance.playerAPrefabName;
                    PvPBattleSceneGodTunnel._playerACruiserVal = PvPBattleSceneGodTunnel.cruiser_scores[PvPBootManager.Instance.playerAPrefabName];

                    SynchedServerData.Instance.playerBPrefabName.Value = connectionPayload.playerHullPrefabName;
                    SynchedServerData.Instance.playerBClientNetworkId.Value = clientId;
                    SynchedServerData.Instance.playerBName.Value = connectionPayload.playerName;
                    SynchedServerData.Instance.playerBScore.Value = connectionPayload.playerScore;
                    SynchedServerData.Instance.captainBPrefabName.Value = connectionPayload.playerCaptainPrefabName;
                    SynchedServerData.Instance.playerBRating.Value = connectionPayload.playerRating;
                    SynchedServerData.Instance.playerBBodykit.Value = connectionPayload.playerBodykit;
                    SynchedServerData.Instance.playerBBounty.Value = connectionPayload.playerBounty;
                    PvPBattleSceneGodTunnel._playerBCruiserName = connectionPayload.playerHullPrefabName;
                    PvPBattleSceneGodTunnel._playerBCruiserVal = PvPBattleSceneGodTunnel.cruiser_scores[connectionPayload.playerHullPrefabName];

                    Debug.Log($"PVP: HOST approving CLIENT (clientId={clientId}, HostHull={PvPBootManager.Instance.playerAPrefabName}, ClientHull={connectionPayload.playerHullPrefabName}) - syncing game state");
                }
                response.Approved = true;
                response.Pending = false;
                response.CreatePlayerObject = true;
                return;
            }

            Debug.LogWarning($"PVP: HOST rejecting CLIENT (clientId={clientId}, Reason={gameReturnStatus})");
            response.Approved = false;
        }

        ConnectStatus GetConnectStatus(ConnectionPayload connectionPayload)
        {
            if (m_ConnectionManager.NetworkManager.ConnectedClientsIds.Count >= m_ConnectionManager.MaxConnectedPlayers)
            {
                return ConnectStatus.ServerFull;
            }
            return ConnectStatus.Success;
        }
    }
}
