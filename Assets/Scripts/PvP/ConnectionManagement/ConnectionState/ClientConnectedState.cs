using UnityEngine;
using BattleCruisers.Network.Multiplay.UnityServices.Lobbies;
using BattleCruisers.UI.ScreensScene.Multiplay.ArenaScreen;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene;
using BattleCruisers.Data;
using BattleCruisers.Network.Multiplay.Infrastructure;

namespace BattleCruisers.Network.Multiplay.ConnectionManagement
{
    class ClientConnectedState : ConnectionState
    {

        protected LobbyServiceFacade m_lobbyServiceFacade;

        public ClientConnectedState(
            LobbyServiceFacade lobbyServiceFacade,
            ConnectionManager connectionManager,
            IPublisher<ConnectStatus> connectStatusPublisher)
        : base(connectionManager, connectStatusPublisher)
        {
            m_lobbyServiceFacade = lobbyServiceFacade;
        }
        public override void Enter()
        {
            Debug.Log($"PVP: CLIENT connected to HOST (LocalHull={DataProvider.GameModel.PlayerLoadout.Hull.PrefabName}) - waiting for game sync");
            if (m_lobbyServiceFacade.CurrentUnityLobby != null)
            {
                //    m_lobbyServiceFacade.BeginTracking();
                // m_lobbyServiceFacade.EndTracking();
            }

            if (MatchmakingScreenController.Instance != null)
            {
                MatchmakingScreenController.Instance.SetFoundVictimString();
                MatchmakingScreenController.Instance.fleeButton.SetActive(false);
                MatchmakingScreenController.Instance.vsAIButton.SetActive(false);
            }
            PvPBattleSceneGodTunnel._playerACruiserVal = 3500;  // in case of worse state
            PvPBattleSceneGodTunnel._playerBCruiserVal = 3500;
            PvPBattleSceneGodTunnel._playerBCruiserName = DataProvider.GameModel.PlayerLoadout.Hull.PrefabName;
        }

        public override void OnClientDisconnect(ulong clientId)
        {
            Debug.Log($"PVP: CLIENT HOST disconnected (clientId={clientId})");
            PvPBattleSceneGodTunnel.OpponentQuit = true;
        }

        public override void LeaveLobby()
        {
            base.LeaveLobby();
            m_lobbyServiceFacade.EndTracking();
        }
        public override void OnUserRequestedShutdown()
        {
            //    m_ConnectStatusPublisher.Publish(ConnectStatus.UserRequestedDisconnect);
            m_ConnectionManager.ChangeState(m_ConnectionManager.m_Offline);
        }
    }
}

