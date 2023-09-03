using System;
using BattleCruisers.Network.Multiplay.UnityServices.Lobbies;
using Unity.Multiplayer.Samples.BossRoom;
using BattleCruisers.Network.Multiplay.Infrastructure;
using UnityEngine;
using VContainer;
using Unity.Netcode;
using UnityEngine.Android;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;
using BattleCruisers.UI.ScreensScene.Multiplay.ArenaScreen;

namespace BattleCruisers.Network.Multiplay.ConnectionManagement
{
    /// <summary>
    /// Connection state corresponding to a host starting up. Starts the host when entering the state. If successful,
    /// transitions to the Hosting state, if not, transitions back to the Offline state.
    /// </summary>
    class StartingHostState : OnlineState
    {
        [Inject]
        LobbyServiceFacade m_LobbyServiceFacade;
        [Inject]
        LocalLobby m_LocalLobby;
        ConnectionMethodBase m_ConnectionMethod;

        public StartingHostState Configure(ConnectionMethodBase baseConnectionMethod)
        {
            m_ConnectionMethod = baseConnectionMethod;
            return this;
        }

        public override void Enter()
        {
            StartHost();
            MatchmakingScreenController.Instance.fleeButton.SetActive(true);
        }

        public override void Exit() { }

        public override void OnClientDisconnect(ulong clientId)
        {
            if (clientId == m_ConnectionManager.NetworkManager.LocalClientId)
            {
                StartHostFailed();
            }
        }

        void StartHostFailed()
        {
            m_ConnectStatusPublisher.Publish(ConnectStatus.StartHostFailed);
            m_ConnectionManager.ChangeState(m_ConnectionManager.m_Offline);
        }

        public override void OnServerStarted()
        {
            m_ConnectStatusPublisher.Publish(ConnectStatus.Success);
            m_ConnectionManager.ChangeState(m_ConnectionManager.m_Hosting);
        }

        public override void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
        {
            var connectionData = request.Payload;
            var clientId = request.ClientNetworkId;
            SynchedServerData m_SynchedServerData = GameObject.Instantiate(Resources.Load<SynchedServerData>("SynchedServerData"));
            m_SynchedServerData.GetComponent<NetworkObject>().Spawn();
            if (clientId == m_ConnectionManager.NetworkManager.LocalClientId)
            {
                var payload = System.Text.Encoding.UTF8.GetString(connectionData);
                var connectionPayload = JsonUtility.FromJson<ConnectionPayload>(payload);
                connectionPayload.playerNetworkId = clientId;

                MatchmakingScreenController.Instance.playerAPrefabName = connectionPayload.playerHullPrefabName;
                MatchmakingScreenController.Instance.playerAClientNetworkId = clientId;
                MatchmakingScreenController.Instance.playerAName = connectionPayload.playerName;
                MatchmakingScreenController.Instance.playerAScore = connectionPayload.playerScore;
                MatchmakingScreenController.Instance.captainAPrefabName = connectionPayload.playerCaptainPrefabName;
            }
            response.Approved = true;
            response.Pending = false;
            response.CreatePlayerObject = true;
        }

        async void StartHost()
        {
            try
            {
                await m_ConnectionMethod.SetupHostConnectionAsync();
                Debug.Log($"Created relay allocation with join code {m_LocalLobby.RelayJoinCode}");

                // NGO's StartHost launches everything
                if (!m_ConnectionManager.NetworkManager.StartHost())
                {
                    OnClientDisconnect(m_ConnectionManager.NetworkManager.LocalClientId);
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                StartHostFailed();
                //           throw;
            }
        }
    }
}

