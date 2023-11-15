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
using BattleCruisers.UI.ScreensScene.Multiplay.ArenaScreen;
using BattleCruisers.Network.Multiplay.ApplicationLifecycle;
using BattleCruisers.Network.Multiplay.Gameplay.UI;
using BattleCruisers.Network.Multiplay.Infrastructure;
using UnityEngine;
using BattleCruisers.Scenes;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;

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
        const string k_MainMenuSceneName = "ScreensScene";

        public override void Enter()
        {
#pragma warning disable 4014
            m_LobbyServiceFacade.EndTracking();
#pragma warning restore 4014

            if (MatchmakingScreenController.Instance != null)
            {
                MatchmakingScreenController.Instance.FailedMatchmaking();
            }                
            else
            {
                if (GameObject.Find("ApplicationController") != null)
                    GameObject.Find("ApplicationController").GetComponent<ApplicationController>().DestroyNetworkObject();

                if (GameObject.Find("ConnectionManager") != null)
                    GameObject.Find("ConnectionManager").GetComponent<ConnectionManager>().DestroyNetworkObject();

                if (GameObject.Find("PopupPanelManager") != null)
                    GameObject.Find("PopupPanelManager").GetComponent<PopupManager>().DestroyNetworkObject();

                if (GameObject.Find("UIMessageManager") != null)
                    GameObject.Find("UIMessageManager").GetComponent<ConnectionStatusMessageUIManager>().DestroyNetworkObject();

                if (GameObject.Find("UpdateRunner") != null)
                    GameObject.Find("UpdateRunner").GetComponent<UpdateRunner>().DestroyNetworkObject();

                if (GameObject.Find("NetworkManager") != null)
                    GameObject.Find("NetworkManager").GetComponent<BCNetworkManager>().DestroyNetworkObject();
                LandingSceneGod.SceneNavigator.GoToScene(PvPSceneNames.SCREENS_SCENE, true);
            }
        }

        public override void Exit() { }

        public override void StartClientIP(string playerName, string ipaddress, int port)
        {
            var connectionMethod = new ConnectionMethodIP(ipaddress, (ushort)port, m_ConnectionManager, m_ProfileManager, playerName);
            m_ConnectionManager.m_ClientReconnecting.Configure(connectionMethod);
            m_ConnectionManager.ChangeState(m_ConnectionManager.m_ClientConnecting.Configure(connectionMethod));
        }

        public override void StartClientLobby(string playerName)
        {
            var connectionMethod = new ConnectionMethodLobby(m_LobbyServiceFacade, m_LocalLobby, m_ConnectionManager, m_ProfileManager, playerName);
            m_ConnectionManager.m_ClientReconnecting.Configure(connectionMethod);
            m_ConnectionManager.ChangeState(m_ConnectionManager.m_ClientConnecting.Configure(connectionMethod));
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

