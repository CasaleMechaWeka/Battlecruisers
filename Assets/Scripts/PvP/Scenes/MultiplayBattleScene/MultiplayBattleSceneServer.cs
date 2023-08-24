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
            m_NetcodeHooks.OnNetworkSpawnHook += OnNetworkSpawn;
            m_NetcodeHooks.OnNetworkDespawnHook += OnNetworkDespawn;
        }

        private void Start()
        {

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

            if (Time.time > 300f)
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
            }
        }
        void OnClientEntered()
        {
            if (m_clients.Count == MaxConnectedPlayers)
            {
                GetComponent<PvPBattleSceneGodServer>().Initialise();
            }
        }

        // IEnumerator iLoadPvPPlayerManager(ulong clientID)
        // {
        //     yield return null;
        //     var pvpPlayerManager = Instantiate(m_PvPPlayerManagerPrefab);
        //     pvpPlayerManager.NetworkObject.SpawnWithOwnership(clientID, false);
        // }

        void OnClientExit()
        {
        }
        void OnNetworkSpawn()
        {
            if (!NetworkManager.Singleton.IsServer)
            {
                enabled = false;
                return;
            }
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += OnLoadEventCompleted;
            NetworkManager.Singleton.SceneManager.OnSynchronizeComplete += OnSynchronizeComplete;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;
            onClientEntered += OnClientEntered;
            onClientExit += OnClientExit;
        }

        void OnNetworkDespawn()
        {
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted -= OnLoadEventCompleted;
            NetworkManager.Singleton.SceneManager.OnSynchronizeComplete -= OnSynchronizeComplete;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnect;
            onClientEntered -= OnClientEntered;
            onClientExit -= OnClientExit;
        }

        void OnDestroy()
        {
            if (m_NetcodeHooks)
            {
                m_NetcodeHooks.OnNetworkSpawnHook -= OnNetworkSpawn;
                m_NetcodeHooks.OnNetworkDespawnHook -= OnNetworkDespawn;
            }
        }


        void OnLoadEventCompleted(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
        {

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
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }
            onClientExit?.Invoke();
        }


        // this is a Late Join scenario
        void OnSynchronizeComplete(ulong clientId)
        {
            m_clients.Add(clientId);
            isConnected = true;
            onClientEntered?.Invoke();
        }


    }
}
