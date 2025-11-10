using System.Collections;
using System.Threading.Tasks;
using BattleCruisers.Network.Multiplay.Infrastructure;
using BattleCruisers.Network.Multiplay.UnityServices.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.ConnectionManagement
{
    class ClientReconnectingState : ClientConnectingState
    {
        IPublisher<ReconnectMessage> m_ReconnectMessagePublisher;
        Coroutine m_ReconnectCoroutine;
        string m_LobbyCode = "";
        int m_NbAttempts;
        const float k_TimeBetweenAttempts = 5;

        public ClientReconnectingState(
            IPublisher<ReconnectMessage> reconnectMessagePublisher,
            LobbyServiceFacade lobbyServiceFacade,
            LocalLobby localLobby,
            ConnectionManager connectionManager,
            IPublisher<ConnectStatus> connectStatusPublisher)
             : base(lobbyServiceFacade, localLobby, connectionManager, connectStatusPublisher)
        {
            m_ReconnectMessagePublisher = reconnectMessagePublisher;
        }

        public override void Enter()
        {
            m_NbAttempts = 0;
            m_LobbyCode = m_LobbyServiceFacade.CurrentUnityLobby != null ? m_LobbyServiceFacade.CurrentUnityLobby.LobbyCode : "";
            m_ReconnectCoroutine = m_ConnectionManager.StartCoroutine(ReconnectCoroutine());

        }

        public override void Exit()
        {
            if (m_ReconnectCoroutine != null)
            {
                m_ConnectionManager.StopCoroutine(m_ReconnectCoroutine);
                m_ReconnectCoroutine = null;
            }
            m_ReconnectMessagePublisher.Publish(new ReconnectMessage(m_ConnectionManager.NbReconnectAttempts, m_ConnectionManager.NbReconnectAttempts));

        }

        public override void OnClientConnected(ulong _)
        {
            m_ConnectionManager.ChangeState(m_ConnectionManager.m_ClientConnected);

        }

        public override void OnClientDisconnect(ulong _)
        {
            string disconnectReason = m_ConnectionManager.NetworkManager.DisconnectReason;
            if (m_NbAttempts < m_ConnectionManager.NbReconnectAttempts)
            {
                if (string.IsNullOrEmpty(disconnectReason))
                {
                    m_ReconnectCoroutine = m_ConnectionManager.StartCoroutine(ReconnectCoroutine());
                }
                else
                {
                    ConnectStatus connectStatus = JsonUtility.FromJson<ConnectStatus>(disconnectReason);
                    switch (connectStatus)
                    {
                        case ConnectStatus.UserRequestedDisconnect:
                        case ConnectStatus.HostEndedSession:
                        case ConnectStatus.ServerFull:
                        case ConnectStatus.IncompatibleBuildType:
                            m_ConnectionManager.ChangeState(m_ConnectionManager.m_Offline);
                            break;
                        default:
                            m_ReconnectCoroutine = m_ConnectionManager.StartCoroutine(ReconnectCoroutine());
                            break;

                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(disconnectReason))
                {
                    ConnectStatus connectStatus = JsonUtility.FromJson<ConnectStatus>(disconnectReason);
                }
                m_ConnectionManager.ChangeState(m_ConnectionManager.m_Offline);
            }
        }

        IEnumerator ReconnectCoroutine()
        {

            // If not on first attempt, wait some time before trying again, so that if the issue causing the disconnect
            // is temporary, it has time to fix itself before we try again. Here we are using a simple fixed cooldown
            // but we could want to use exponential backoff instead, to wait a longer time between each failed attempt.
            // See https://en.wikipedia.org/wiki/Exponential_backoff


            if (m_NbAttempts > 0)
            {
                yield return new WaitForSeconds(k_TimeBetweenAttempts);
            }

            Debug.Log($"PVP: CLIENT lost connection to HOST (LobbyCode={m_LobbyCode}) - attempting reconnect");
            m_ConnectionManager.NetworkManager.Shutdown();

            yield return new WaitWhile(() => m_ConnectionManager.NetworkManager.ShutdownInProgress);
            Debug.Log($"PVP: CLIENT reconnect attempt {m_NbAttempts + 1}/{m_ConnectionManager.NbReconnectAttempts} (LobbyID={m_LocalLobby?.LobbyID})");
            m_ReconnectMessagePublisher.Publish(new ReconnectMessage(m_NbAttempts, m_ConnectionManager.NbReconnectAttempts));
            m_NbAttempts++;
            if (!string.IsNullOrEmpty(m_LobbyCode))
            {
                // When using Lobby with Relay, if a user is disconnected from the Relay server, the server will notify
                // the Lobby service and mark the user as disconnected, but will not remove them from the lobby. They
                // then have some time to attempt to reconnect (defined by the "Disconnect removal time" parameter on
                // the dashboard), after which they will be removed from the lobby completely.
                // See https://docs.unity.com/lobby/reconnect-to-lobby.html
                Task<Lobby> reconnectingToLobby = m_LobbyServiceFacade.ReconnectToLobbyAsync(m_LocalLobby?.LobbyID);
                yield return new WaitUntil(() => reconnectingToLobby.IsCompleted);

                if (!reconnectingToLobby.IsFaulted && reconnectingToLobby.Result != null)
                {
                    Task connectingToRelay = ConnectClientAsync();
                    yield return new WaitUntil(() => connectingToRelay.IsCompleted);
                }
                else
                {
                    Debug.LogWarning($"PVP: CLIENT reconnect failed (LobbyID={m_LocalLobby?.LobbyID}, Attempt={m_NbAttempts}/{m_ConnectionManager.NbReconnectAttempts})");
                    OnClientDisconnect(0);
                }
            }
            else
            {
                Task connectingClient = ConnectClientAsync();
                yield return new WaitUntil(() => connectingClient.IsCompleted);
            }
        }
    }
}

