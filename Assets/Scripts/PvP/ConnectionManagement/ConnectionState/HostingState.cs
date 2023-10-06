using BattleCruisers.Network.Multiplay.UnityServices.Lobbies;
using BattleCruisers.Network.Multiplay.Infrastructure;
using Unity.Multiplayer.Samples.Utilities;
using Unity.Multiplayer.Samples.BossRoom;
using Unity.Netcode;

using UnityEngine;
using VContainer;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;
using BattleCruisers.UI.ScreensScene.Multiplay.ArenaScreen;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene;
using System.Threading.Tasks;

namespace BattleCruisers.Network.Multiplay.ConnectionManagement
{
    class HostingState : OnlineState
    {
        [Inject]
        LobbyServiceFacade m_LobbyServiceFacade;
        [Inject]
        LocalLobby m_LocalLobby;
        [Inject]
        IPublisher<ConnectionEventMessage> m_ConnectionEventPublisher;

        // used in ApprovalCheck. This is intended as a bit of light protection against DOS attacks that rely on sending silly big buffers of garbage.
        const int k_MaxConnectPayload = 1024;

        public override void Enter()
        {
            if (m_LobbyServiceFacade.CurrentUnityLobby != null)
            {
                m_LobbyServiceFacade.BeginTracking();
                MatchmakingScreenController.Instance.SetMMString(MatchmakingScreenController.MMStatus.LOOKING_VICTIM);
            }
        }

        public override void LeaveLobby()
        {
            base.LeaveLobby();
            m_LobbyServiceFacade.EndTracking();
        }
        public override async void UpdateIsReady()
        {
            base.UpdateIsReady();
            m_LocalLobby.IsReady = "1";
            await m_LobbyServiceFacade.UpdateLobbyDataAsync(m_LocalLobby.GetDataForUnityServices());
        }


        public override void Exit()
        {
            SessionManager<SessionPlayerData>.Instance.OnServerEnded();
        }

        public override void OnClientConnected(ulong clientId)
        {
            if (clientId != m_ConnectionManager.NetworkManager.LocalClientId && m_ConnectionManager.NetworkManager.ConnectedClientsIds.Count == 2)
            {
                MatchmakingScreenController.Instance.SetFoundVictimString();
                MatchmakingScreenController.Instance.fleeButton.SetActive(true);
                MatchmakingScreenController.Instance.vsAIButton.SetActive(false);
            }
            if (clientId == m_ConnectionManager.NetworkManager.LocalClientId && m_ConnectionManager.NetworkManager.ConnectedClientsIds.Count == 1)
            {
                NetworkManager.Singleton.SceneManager.LoadScene("PvPBattleScene", UnityEngine.SceneManagement.LoadSceneMode.Single);
            }
        }

        public override void OnClientDisconnect(ulong clientId)
        {
        }

        public override void OnUserRequestedShutdown()
        {
            var reason = JsonUtility.ToJson(ConnectStatus.HostEndedSession);
            for (var i = m_ConnectionManager.NetworkManager.ConnectedClientsIds.Count - 1; i >= 0; i--)
            {
                var id = m_ConnectionManager.NetworkManager.ConnectedClientsIds[i];
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
            var connectionData = request.Payload;
            var clientId = request.ClientNetworkId;
            var payload = System.Text.Encoding.UTF8.GetString(connectionData);
            var connectionPayload = JsonUtility.FromJson<ConnectionPayload>(payload);
            connectionPayload.playerNetworkId = clientId;
            var gameReturnStatus = GetConnectStatus(connectionPayload);
            if (gameReturnStatus == ConnectStatus.Success)
            {
                if (clientId != m_ConnectionManager.NetworkManager.LocalClientId)
                {

                    // Player A
                    SynchedServerData.Instance.playerAPrefabName.Value = MatchmakingScreenController.Instance.playerAPrefabName;
                    SynchedServerData.Instance.playerAClientNetworkId.Value = MatchmakingScreenController.Instance.playerAClientNetworkId;
                    SynchedServerData.Instance.playerAName.Value = MatchmakingScreenController.Instance.playerAName;
                    SynchedServerData.Instance.playerAScore.Value = MatchmakingScreenController.Instance.playerAScore;
                    SynchedServerData.Instance.captainAPrefabName.Value = MatchmakingScreenController.Instance.captainAPrefabName;
                    SynchedServerData.Instance.playerARating.Value = MatchmakingScreenController.Instance.playerRating;
                    PvPBattleSceneGodTunnel._playerACruiserName = MatchmakingScreenController.Instance.playerAPrefabName;
                    PvPBattleSceneGodTunnel._playerACruiserVal = PvPBattleSceneGodTunnel.cruiser_scores[MatchmakingScreenController.Instance.playerAPrefabName];

                    // Player B
                    SynchedServerData.Instance.playerBPrefabName.Value = connectionPayload.playerHullPrefabName;
                    SynchedServerData.Instance.playerBClientNetworkId.Value = clientId;
                    SynchedServerData.Instance.playerBName.Value = connectionPayload.playerName;
                    SynchedServerData.Instance.playerBScore.Value = connectionPayload.playerScore;
                    SynchedServerData.Instance.captainBPrefabName.Value = connectionPayload.playerCaptainPrefabName;
                    SynchedServerData.Instance.playerBRating.Value = connectionPayload.playerRating;
                    PvPBattleSceneGodTunnel._playerBCruiserName = connectionPayload.playerHullPrefabName;
                    PvPBattleSceneGodTunnel._playerBCruiserVal = PvPBattleSceneGodTunnel.cruiser_scores[connectionPayload.playerHullPrefabName];
                }

                response.Approved = true;
                response.Pending = false;
                response.CreatePlayerObject = true;
                return;
            }
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

