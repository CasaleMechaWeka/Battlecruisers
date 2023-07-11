using System;
using UnityEngine;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.Client
{
    public class MatchplayNetworkClient : IDisposable
    {
        public event Action<MatchplayConnectStatus> OnLocalConnection;
        public event Action<MatchplayConnectStatus> OnLocalDisconnection;

        const int k_TimeoutDuration = 10;
        NetworkManager m_NetworkManager;
        DisconnectReason DisconnectReason { get; } = new DisconnectReason();

        public MatchplayNetworkClient()
        {
            m_NetworkManager = NetworkManager.Singleton;
            m_NetworkManager.OnClientDisconnectCallback += RemoteDisconnect;
        }


        // public void StartClient(string ipaddress, int port)
        // {
        //     var unityTransport = m_NetworkManager.gameObject.GetComponent<UnityTransport>();
        //     unityTransport.SetConnectionData(ipaddress, (ushort)port);
        //     ConnectClient();
        // }

        public void DisconnectClient()
        {
            DisconnectReason.SetDisconnectReason(MatchplayConnectStatus.UserRequestedDisconnect);
            NetworkShutdown();
        }

        // void ConnectClient()
        // {
        //     var userData = ClientSingleton.Instance.Manager.User.Data;
        //     var payload = JsonUtility.ToJson(userData);
        //     var payloadBytes = System.Text.Encoding.UTF8.GetBytes(payload);


        //     m_NetworkManager.NetworkConfig.ConnectionData = payloadBytes;
        //     m_NetworkManager.NetworkConfig.ClientConnectionBufferTimeout = k_TimeoutDuration;

        //     if (m_NetworkManager.StartClient())
        //     {
        //         Debug.Log("Starting Client!");
        //         MatchplayNetworkMessenger.RegisterListener(NetworkMessage.LocalClientConnected, ReceiveLocalClientConnectStatus);
        //         MatchplayNetworkMessenger.RegisterListener(NetworkMessage.LocalClientDisconnected, ReceiveLocalClientDisconnectStatus);
        //     }
        //     else
        //     {
        //         Debug.LogWarning($"Could not Start Client!");
        //         OnLocalDisconnection?.Invoke(MatchplayConnectStatus.Undefined);
        //     }
        // }

        void ReceiveLocalClientConnectStatus(ulong clientId, FastBufferReader reader)
        {
            reader.ReadValueSafe(out MatchplayConnectStatus status);
            Debug.Log("ReceiveLocalClientConnectStatus: " + status);

            if (status != MatchplayConnectStatus.Success)
            {
                DisconnectReason.SetDisconnectReason(status);
            }
            OnLocalConnection?.Invoke(status);
        }

        void ReceiveLocalClientDisconnectStatus(ulong clientId, FastBufferReader reader)
        {
            reader.ReadValueSafe(out MatchplayConnectStatus status);
            Debug.Log("ReceiveLocalClientDisconnectStatus: " + status);
            DisconnectReason.SetDisconnectReason(status);
        }

        void RemoteDisconnect(ulong clientId)
        {
            Debug.Log($"Got Client Disconnect callback for {clientId}");
            if (clientId == m_NetworkManager.LocalClientId)
                return;
            NetworkShutdown();
        }

        void NetworkShutdown()
        {
            if (SceneManager.GetActiveScene().name != PvPSceneNames.SCREENS_SCENE)
            {
                SceneManager.LoadScene(PvPSceneNames.SCREENS_SCENE);
            }

            if (m_NetworkManager.IsConnectedClient)
                m_NetworkManager.Shutdown(true);
            OnLocalDisconnection?.Invoke(DisconnectReason.Reason);
            MatchplayNetworkMessenger.UnRegisterListener(NetworkMessage.LocalClientConnected);
            MatchplayNetworkMessenger.UnRegisterListener(NetworkMessage.LocalClientDisconnected);
        }

        public void Dispose()
        {
            if (m_NetworkManager != null && m_NetworkManager.CustomMessagingManager != null)
            {
                m_NetworkManager.OnClientDisconnectCallback -= RemoteDisconnect;
            }
        }
    }
}

