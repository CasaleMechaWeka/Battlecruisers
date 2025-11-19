using System;
using BattleCruisers.Network.Multiplay.UnityServices.Lobbies;
using UnityEngine;
using Unity.Netcode;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;
using BattleCruisers.UI.ScreensScene.Multiplay.ArenaScreen;
using BattleCruisers.UI.ScreensScene.BattleHubScreen;
using BattleCruisers.Scenes;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Network.Multiplay.Infrastructure;
using BattleCruisers.Network.Multiplay.Scenes;
using BattleCruisers.Network.Multiplay.Utils;
using BattleCruisers.Data;

namespace BattleCruisers.Network.Multiplay.ConnectionManagement
{
    /// <summary>
    /// Connection state corresponding to a host starting up. Starts the host when entering the state. If successful,
    /// transitions to the Hosting state, if not, transitions back to the Offline state.
    /// </summary>
    class StartingHostState : OnlineState
    {
        LocalLobby m_LocalLobby;
        ConnectionMethodBase m_ConnectionMethod;

        public StartingHostState(
            LocalLobby localLobby,
            ConnectionManager connectionManager,
            IPublisher<ConnectStatus> connectStatusPublisher)
            : base(connectionManager, connectStatusPublisher)
        {
            m_LocalLobby = localLobby;
        }

        public StartingHostState Configure(ConnectionMethodBase baseConnectionMethod)
        {
            m_ConnectionMethod = baseConnectionMethod;
            return this;
        }

        public override void Enter()
        {
            if (MatchmakingScreenController.Instance != null)
                MatchmakingScreenController.Instance.SetMMStatus(MatchmakingScreenController.MMStatus.CONNECTING);
            StartHost();
        }

        public override void Exit() { }

        public override void OnClientDisconnect(ulong clientId)
        {
            if (clientId == m_ConnectionManager.NetworkManager.LocalClientId)
                StartHostFailed();
        }

        void StartHostFailed()
        {
            if (m_ConnectionManager == null)
                return;
            m_ConnectionManager.ChangeState(m_ConnectionManager.m_Offline);
        }
        public override void OnServerStarted()
        {
            m_ConnectionManager.ChangeState(m_ConnectionManager.m_Hosting);
        }
        public override void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
        {
            byte[] connectionData = request.Payload;
            ulong clientId = request.ClientNetworkId;
            SynchedServerData m_SynchedServerData = GameObject.Instantiate(Resources.Load<SynchedServerData>("SynchedServerData"));
            m_SynchedServerData.GetComponent<NetworkObject>().Spawn();

            m_SynchedServerData.map.Value = (Map)DataProvider.GameModel.GameMap;
            Debug.Log($"PVP: HOST synced map (Map={m_SynchedServerData.map.Value}, Arena={DataProvider.GameModel.GameMap})");

            if (clientId == m_ConnectionManager.NetworkManager.LocalClientId)
            {
                string payload = System.Text.Encoding.UTF8.GetString(connectionData);
                ConnectionPayload connectionPayload = JsonUtility.FromJson<ConnectionPayload>(payload);
                connectionPayload.playerNetworkId = clientId;

                PvPBootManager.Instance.playerAPrefabName = connectionPayload.playerHullPrefabName;
                PvPBootManager.Instance.playerAClientNetworkId = clientId;
                PvPBootManager.Instance.playerAName = connectionPayload.playerName;
                PvPBootManager.Instance.playerAScore = connectionPayload.playerScore;
                PvPBootManager.Instance.captainAPrefabName = connectionPayload.playerCaptainPrefabName;
                PvPBootManager.Instance.playerRating = connectionPayload.playerRating;
                PvPBootManager.Instance.playerABodykit = connectionPayload.playerBodykit;
                PvPBootManager.Instance.playerABounty = connectionPayload.playerBounty;
                //        MatchmakingScreenController.Instance.playerASelectedVariants = connectionPayload.playerSelectedVariants;
            }
            response.Approved = true;
            response.Pending = false;
            response.CreatePlayerObject = true;
        }
        async void StartHost()
        {
            try
            {
                Debug.Log("PVP: HOST StartHost - before SetupHostConnectionAsync");
                await m_ConnectionMethod.SetupHostConnectionAsync();
                Debug.Log($"PVP: HOST relay configured, Private={ArenaSelectPanelScreenController.PrivateMatch}) - preparing NetworkManager");
                if (DynamicPrefabLoadingUtilities.HashOfDynamicPrefabGUIDs == -1)
                {
                    Debug.Log("PVP: HOST - before DynamicPrefabLoadingUtilities.Init");
                    DynamicPrefabLoadingUtilities.Init(m_ConnectionManager.NetworkManager);
                    Debug.Log("PVP: HOST - after DynamicPrefabLoadingUtilities.Init");
                }
                m_ConnectionManager.NetworkManager.NetworkConfig.EnableSceneManagement = true;
                Debug.Log($"PVP: HOST starting NetworkManager (SceneManagement=true, Private={ArenaSelectPanelScreenController.PrivateMatch})");
                if (!m_ConnectionManager.NetworkManager.StartHost())
                {
                    OnClientDisconnect(m_ConnectionManager.NetworkManager.LocalClientId);
                }
                else
                {
                    Debug.Log("PVP: HOST NetworkManager.StartHost returned true");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"PVP: HOST failed to start (Error={e.Message})");
                if (LandingSceneGod.Instance != null)
                {
                    LandingSceneGod.Instance.messagebox.ShowMessage(LocTableCache.CommonTable.GetString("NetworkError"));
                }
                else if (PrivateMatchmakingController.Instance != null && PrivateMatchmakingController.Instance.messageBox != null)
                {
                    Debug.Log("PVP: HOST showing connection error in PrivateMatchmakingController");
                    PrivateMatchmakingController.Instance.ShowBadInternetMessageBox();
                }
                else
                {
                    Debug.LogError("PVP: HOST no message box available for error display");
                }
                StartHostFailed();
            }
        }
    }
}
