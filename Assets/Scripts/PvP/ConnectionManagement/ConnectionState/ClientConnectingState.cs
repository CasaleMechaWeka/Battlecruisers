using System.Threading.Tasks;
using UnityEngine;
using VContainer;
using System;
using BattleCruisers.Network.Multiplay.UnityServices.Lobbies;
using BattleCruisers.UI.ScreensScene.Multiplay.ArenaScreen;
using BattleCruisers.Scenes;
using BattleCruisers.Utils.Localisation;

namespace BattleCruisers.Network.Multiplay.ConnectionManagement
{
    class ClientConnectingState : OnlineState
    {
        [Inject]
        protected LobbyServiceFacade m_LobbyServiceFacade;
        [Inject]
        protected LocalLobby m_LocalLobby;
        ConnectionMethodBase m_ConnectionMethod;

        public ClientConnectingState Configure(ConnectionMethodBase baseConnectionMethod)
        {
            m_ConnectionMethod = baseConnectionMethod;
            return this;
        }

        public override void Enter()
        {
#pragma warning disable 4014
            MatchmakingScreenController.Instance.SetMMStatus(MatchmakingScreenController.MMStatus.CONNECTING);
            ConnectClientAsync();
#pragma warning restore 4014            
        }

        public override void Exit() { }
        public override void OnClientConnected(ulong _)
        {
            m_ConnectionManager.ChangeState(m_ConnectionManager.m_ClientConnected);
        }

        public override void OnClientDisconnect(ulong _)
        {
            StartingClientFailedAsync();
        }

        protected void StartingClientFailedAsync()
        {
            m_ConnectionManager.ChangeState(m_ConnectionManager.m_Offline);
        }

        internal async Task ConnectClientAsync()
        {
            try
            {
                await m_ConnectionMethod.SetupClientConnectionAsync();
                if (!m_ConnectionManager.NetworkManager.StartClient())
                {
                    throw new System.Exception("NetworkManager StartClient failed");
                }
            }
            catch (Exception e)
            {
                Debug.Log("Error ---> connecting client, see following exception");
                Debug.Log(e.Message);
                LandingSceneGod.Instance.messagebox.ShowMessage(LocTableCache.CommonTable.GetString("NetworkError"));
                /*                switch (e.Message)
                                {
                                    case "Latency":
                                        LandingSceneGod.Instance.messagebox.ShowMessage("Sorry, but your network seems to be not good enough for 1v1 Showdown.");
                                        break;
                                    default:
                                        LandingSceneGod.Instance.messagebox.ShowMessage("Sorry, but detected unknown network error, try again later.");
                                        break;
                                }*/
                StartingClientFailedAsync();
                throw;
            }
        }
    }
}

