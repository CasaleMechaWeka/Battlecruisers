using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using BattleCruisers.Network.Multiplay.UnityServices.Lobbies;
namespace BattleCruisers.Network.Multiplay.ConnectionManagement
{
    class ClientConnectedState : ConnectionState
    {

        [Inject]
        protected LobbyServiceFacade m_lobbyServiceFacade;

        public override void Enter()
        {
            if (m_lobbyServiceFacade.CurrentUnityLobby != null)
            {
                m_lobbyServiceFacade.BeginTracking();
            }
        }

        public override void Exit() { }
        public override void OnClientDisconnect(ulong clientId)
        {
            var disconnectReason = m_ConnectionManager.NetworkManager.DisconnectReason;
            if (string.IsNullOrEmpty(disconnectReason))
            {
                m_ConnectStatusPublisher.Publish(ConnectStatus.Reconnecting);
                m_ConnectionManager.ChangeState(m_ConnectionManager.m_ClientReconnecting);
            }
            else
            {
                var connectStatus = JsonUtility.FromJson<ConnectStatus>(disconnectReason);
                m_ConnectStatusPublisher.Publish(connectStatus);
                m_ConnectionManager.ChangeState(m_ConnectionManager.m_Offline);
            }
        }


        public override void OnUserRequestedShutdown()
        {
            m_ConnectStatusPublisher.Publish(ConnectStatus.UserRequestedDisconnect);
            m_ConnectionManager.ChangeState(m_ConnectionManager.m_Offline);
        }
    }
}

