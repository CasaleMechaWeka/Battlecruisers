using System.Threading.Tasks;
using UnityEngine;
using Unity.Multiplayer.Samples.Utilities;
using VContainer;
using System;
using BattleCruisers.Network.Multiplay.UnityServices.Lobbies;
using BattleCruisers.UI.ScreensScene.Multiplay.ArenaScreen;

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
            MatchmakingScreenController.Instance.NotFound();
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
                if (!m_ConnectionManager.NetworkManager.StartClient())
                {                
                    throw new System.Exception("NetworkManager StartClient failed");
                }
              //  SceneLoaderWrapper.Instance.AddOnSceneEventCallback();
            }
            catch (Exception e)
            {
                Debug.LogError("Error connecting client, see following exception");
                Debug.LogException(e);
                StartingClientFailedAsync();
                throw;
            }
        }

    }
}

