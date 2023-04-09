using System.Threading.Tasks;
using UnityEngine;
using Unity.Multiplayer.Samples.Utilities;
using VContainer;
using System;
using BattleCruisers.Network.Multiplay.UnityServices.Lobbies;
using BattleCruisers.Network.Multiplay.Matchplay.Networking;
using Unity.Netcode;

namespace BattleCruisers.Network.Multiplay.ConnectionManagement
{
    class ClientConnectingState : OnlineState
    {
        [Inject]
        protected LobbyServiceFacade m_LobbyServiceFacade;
        [Inject]
        protected LocalLobby m_LocalLobby;
        ConnectionMethodBase m_ConnectionMethod;

        const int k_TimeoutDuration = 10;


        public event Action<MatchplayConnectStatus> OnLocalConnection;
        public event Action<MatchplayConnectStatus> OnLocalDisconnection;

        DisconnectReason disconnectReason { get; } = new DisconnectReason();

        public ClientConnectingState Configure(ConnectionMethodBase baseConnectionMethod)
        {
            m_ConnectionMethod = baseConnectionMethod;
            return this;
        }

        public override void Enter()
        {
#pragma warning disable 4014
            ConnectClientAsync();
#pragma warning restore 4014

        }

        public override void Exit() { }
        public override void OnClientConnected(ulong _)
        {
            m_ConnectStatusPublisher.Publish(ConnectStatus.Success);
            m_ConnectionManager.ChangeState(m_ConnectionManager.m_ClientConnected);
        }

        public override void OnClientDisconnect(ulong _)
        {
            StartingClientFailedAsync();
        }

        protected void StartingClientFailedAsync()
        {
            var disconnectReason = m_ConnectionManager.NetworkManager.DisconnectReason;
            if (string.IsNullOrEmpty(disconnectReason))
            {
                m_ConnectStatusPublisher.Publish(ConnectStatus.StartClientFailed);
            }
            else
            {
                var connectStatus = JsonUtility.FromJson<ConnectStatus>(disconnectReason);
                m_ConnectStatusPublisher.Publish(connectStatus);
            }
            m_ConnectionManager.ChangeState(m_ConnectionManager.m_Offline);
        }

        internal async Task ConnectClientAsync()
        {
            try
            {
                await m_ConnectionMethod.SetupClientConnectionAsync();

                var userData = m_ConnectionManager.Manager.User.Data;
                var payload = JsonUtility.ToJson(userData);
                var payloadBytes = System.Text.Encoding.UTF8.GetBytes(payload);

                m_ConnectionManager.NetworkManager.NetworkConfig.ConnectionData = payloadBytes;
                m_ConnectionManager.NetworkManager.NetworkConfig.ClientConnectionBufferTimeout = k_TimeoutDuration;

                if (!m_ConnectionManager.NetworkManager.StartClient())
                {
                    throw new System.Exception("NetworkManager StartClient failed");
                }
                else
                {
                    MatchplayNetworkMessenger.RegisterListener(NetworkMessage.LocalClientConnected,
                                  ReceiveLocalClientConnectStatus);
                    MatchplayNetworkMessenger.RegisterListener(NetworkMessage.LocalClientDisconnected,
                        ReceiveLocalClientDisconnectStatus);
                }
                SceneLoaderWrapper.Instance.AddOnSceneEventCallback();
            }
            catch (Exception e)
            {
                Debug.LogError("Error connecting client, see following exception");
                Debug.LogException(e);
                StartingClientFailedAsync();
                throw;
            }
        }


        void ReceiveLocalClientConnectStatus(ulong clientId, FastBufferReader reader)
        {
            reader.ReadValueSafe(out MatchplayConnectStatus status);
            Debug.Log("ReceiveLocalClientConnectStatus: " + status);

            //this indicates a game level failure, rather than a network failure. See note in ServerGameNetPortal.
            if (status != MatchplayConnectStatus.Success)
                disconnectReason.SetDisconnectReason(status);

            OnLocalConnection?.Invoke(status);
        }

        void ReceiveLocalClientDisconnectStatus(ulong clientId, FastBufferReader reader)
        {
            reader.ReadValueSafe(out MatchplayConnectStatus status);
            Debug.Log("ReceiveLocalClientDisconnectStatus: " + status);
            disconnectReason.SetDisconnectReason(status);
        }

    }
}

