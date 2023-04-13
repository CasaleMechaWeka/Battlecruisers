using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using BattleCruisers.Network.Multiplay.UnityServices.Lobbies;
using BattleCruisers.Network.Multiplay.Utils;
using Unity.Multiplayer.Samples.Utilities;
using VContainer;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;

namespace BattleCruisers.Network.Multiplay.ConnectionManagement
{
    class OfflineState : ConnectionState
    {
        [Inject]
        LobbyServiceFacade m_LobbyServiceFacade;
        [Inject]
        ProfileManager m_ProfileManager;
        [Inject]
        LocalLobby m_LocalLobby;
        [Inject]
        LocalLobbyUser m_LocalLobbyUser;
        const string k_MainMenuSceneName = "MainMenu";

        public override void Enter()
        {
            m_LobbyServiceFacade.EndTracking();
            m_ConnectionManager.NetworkManager.Shutdown();
            if (SceneManager.GetActiveScene().name != k_MainMenuSceneName)
            {
                SceneLoaderWrapper.Instance.LoadScene(k_MainMenuSceneName, useNetworkSceneManager: false);
            }
        }

        public override void Exit() { }

        public override void StartClientIP(string playerName, string ipaddress, int port)
        {
            var connectionMethod = new ConnectionMethodIP(ipaddress, (ushort)port, m_ConnectionManager, m_ProfileManager, playerName);
            m_ConnectionManager.m_ClientReconnecting.Configure(connectionMethod);
            m_ConnectionManager.ChangeState(m_ConnectionManager.m_ClientConnecting.Configure(connectionMethod));
        }

        public override async Task StartClientLobby(string playerName)
        {
            m_LocalLobbyUser.DisplayName = playerName;
            var connectionMethod = new ConnectionMethodLobby(m_LobbyServiceFacade, m_LocalLobby, m_ConnectionManager, m_ProfileManager, playerName);
            (bool success, Lobby lobby) = await connectionMethod.TryQuickJoinConnectionAsync();
            if (success)
            {
                m_LocalLobby.ApplyRemoteData(lobby);
                m_ConnectionManager.m_ClientReconnecting.Configure(connectionMethod);
                m_ConnectionManager.ChangeState(m_ConnectionManager.m_ClientConnecting.Configure(connectionMethod));
            }
            else
            {

            }

        }

        public override void StartHostIP(string playerName, string ipaddress, int port)
        {
            var connectionMethod = new ConnectionMethodIP(ipaddress, (ushort)port, m_ConnectionManager, m_ProfileManager, playerName);
            m_ConnectionManager.ChangeState(m_ConnectionManager.m_StartingHost.Configure(connectionMethod));
        }

        public override void StartHostLobby(string playerName)
        {
            var connectionMethod = new ConnectionMethodLobby(m_LobbyServiceFacade, m_LocalLobby, m_ConnectionManager, m_ProfileManager, playerName);
            m_ConnectionManager.ChangeState(m_ConnectionManager.m_StartingHost.Configure(connectionMethod));
        }
    }
}

