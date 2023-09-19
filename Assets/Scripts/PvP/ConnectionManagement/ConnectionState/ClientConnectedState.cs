using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using BattleCruisers.Network.Multiplay.UnityServices.Lobbies;
using BattleCruisers.UI.ScreensScene.Multiplay.ArenaScreen;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene;
using BattleCruisers.Data;

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
                //    m_lobbyServiceFacade.BeginTracking();
                m_lobbyServiceFacade.EndTracking();
            }
            MatchmakingScreenController.Instance.SetFoundVictimString();
            MatchmakingScreenController.Instance.fleeButton.SetActive(true);
            MatchmakingScreenController.Instance.vsAIButton.SetActive(false);

            PvPBattleSceneGodTunnel._playerACruiserVal = 1;
            PvPBattleSceneGodTunnel._playerBCruiserVal = 1;
            PvPBattleSceneGodTunnel._playerBCruiserName = ApplicationModelProvider.ApplicationModel.DataProvider.GameModel.PlayerLoadout.Hull.PrefabName;
        }

        public override void Exit() { }
        public override void OnClientDisconnect(ulong clientId)
        {

        }

        public override void OnUserRequestedShutdown()
        {
            //    m_ConnectStatusPublisher.Publish(ConnectStatus.UserRequestedDisconnect);
            m_ConnectionManager.ChangeState(m_ConnectionManager.m_Offline);
        }
    }
}

