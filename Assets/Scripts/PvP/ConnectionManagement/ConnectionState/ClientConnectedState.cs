using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using BattleCruisers.Network.Multiplay.UnityServices.Lobbies;
using BattleCruisers.UI.ScreensScene.Multiplay.ArenaScreen;

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
            MatchmakingScreenController.Instance.SetFoudVictimString();
            /*            MatchmakingScreenController.Instance.fleeButton.SetActive(false);
                        MatchmakingScreenController.Instance.vsAIButton.SetActive(false);*/
        }

        public override void Exit() { }
        public override void OnClientDisconnect(ulong clientId)
        {

        }

        public override void OnUserRequestedShutdown()
        {
            m_ConnectStatusPublisher.Publish(ConnectStatus.UserRequestedDisconnect);
            m_ConnectionManager.ChangeState(m_ConnectionManager.m_Offline);
        }
    }
}

