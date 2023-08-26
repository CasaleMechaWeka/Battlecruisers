using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Multiplayer.Samples.Utilities;
using UnityEngine.SceneManagement;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;
using BattleCruisers.Network.Multiplay.Matchplay.Server;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene
{
    [RequireComponent(typeof(NetcodeHooks))]
    public class MultiplayBattleSceneServer : MonoBehaviour
    {
        [SerializeField]
        NetcodeHooks m_NetcodeHooks;

        [SerializeField]
        PvPPlayerManager m_PvPPlayerManagerPrefab;

        List<ulong> m_clients = new List<ulong>();

        Action onClientEntered;
        Action onClientExit;
        const int MaxConnectedPlayers = 2;
        private bool isConnected = false;
        private void Awake()
        {
        }

        private void Start()
        {
            if (!NetworkManager.Singleton.IsServer)
            {
                enabled = false;
                return;
            }
            NetworkManager.Singleton.SceneManager.OnSceneEvent += SceneManager_OnSceneEvent;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;
            onClientEntered += OnClientEntered;
            onClientExit += OnClientExit;
        }

        private void Update()
        {
            if (isConnected && NetworkManager.Singleton.ConnectedClients.Count == 0)
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
            }

            if (!isConnected && Time.time > 180f)
            {
#if UNITY_EDITOR
                NetworkManager.Singleton.Shutdown();
                UnityEditor.EditorApplication.isPlaying = false;
#else
                    NetworkManager.Singleton.Shutdown();
                    Application.Quit();
#endif
            }
        }


        private void SceneManager_OnSceneEvent(SceneEvent sceneEvent)
        {
            switch(sceneEvent.SceneEventType)
            {
                case SceneEventType.Load:
          
                    break;
                case SceneEventType.Unload:
             
                    break;
                case SceneEventType.LoadComplete:
             
                    break;
                case SceneEventType.UnloadComplete:
           
                    break;
                case SceneEventType.LoadEventCompleted:

                    break;
                case SceneEventType.UnloadEventCompleted:
       
                    break;
                case SceneEventType.SynchronizeComplete:
                    OnSynchronizeComplete(sceneEvent.ClientId);
                    break;
            }
        }

        void OnClientEntered()
        {
            if (m_clients.Count == MaxConnectedPlayers)
            {
                GetComponent<PvPBattleSceneGodServer>().Initialise();
            }
        }
        void OnClientExit()
        {
        }

        void OnDestroy()
        {
            NetworkManager.Singleton.SceneManager.OnSceneEvent -= SceneManager_OnSceneEvent;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnect;
            onClientEntered -= OnClientEntered;
            onClientExit -= OnClientExit;
        }


        void OnSynchronizeComplete(ulong clientID)
        {
            m_clients.Add(clientID);
            isConnected = true;
            onClientEntered?.Invoke();
        }

        void OnClientDisconnect(ulong clientId)
        {
            m_clients.Remove(clientId);
            if (SynchedServerData.Instance.playerAClientNetworkId.Value == clientId)
            {
                PvPBattleSceneGodServer.Instance.RegisterAIOfLeftPlayer();
            }
            else if (SynchedServerData.Instance.playerBClientNetworkId.Value == clientId)
            {
                PvPBattleSceneGodServer.Instance.RegisterAIOfRightPlayer();
            }

            if (m_clients.Count == 0)
            {
#if UNITY_EDITOR
                NetworkManager.Singleton.Shutdown();
                UnityEditor.EditorApplication.isPlaying = false;
#else
                NetworkManager.Singleton.Shutdown();
                Application.Quit();
#endif
            }
            onClientExit?.Invoke();
        }
    }
}
