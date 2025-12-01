using BattleCruisers.Network.Multiplay.ConnectionManagement;
using BattleCruisers.Network.Multiplay.Infrastructure;
using BattleCruisers.Network.Multiplay.UnityServices.Lobbies;
using BattleCruisers.Network.Multiplay.Utils;
using BattleCruisers.Network.Multiplay.ApplicationLifecycle.Messages;
using BattleCruisers.Network.Multiplay.UnityServices.Auth;
using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;
using BattleCruisers.Network.Multiplay.UnityServices;
using BattleCruisers.Network.Multiplay.Scenes;
using BattleCruisers.Network.Multiplay.Gameplay.UI;
using BattleCruisers.Network.Multiplay.GamePlay.UI;

namespace BattleCruisers.Network.Multiplay.ApplicationLifecycle
{
    public class ApplicationController : MonoBehaviour, INetworkObject
    {
        [SerializeField] UpdateRunner updateRunnerPrefab;
        [SerializeField] NetworkManager networkManagerPrefab;
        [SerializeField] ConnectionManager connectionManagerPrefab;

        UpdateRunner m_UpdateRunner;
        NetworkManager m_NetworkManager;
        ConnectionManager m_ConnectionManager;

        LocalLobby m_LocalLobby;
        LobbyServiceFacade m_LobbyServiceFacade;

        IDisposable m_Subscriptions;

        LocalLobbyUser localLobbyUser;
        ProfileManager profileManager;

        MessageChannel<QuitApplicationMessage> quitChannel;
        MessageChannel<UnityServiceErrorMessage> unityServiceErrorChannel;
        MessageChannel<ConnectStatus> connectionStatusChannel;


        MessageChannel<ReconnectMessage> reconnectChannel;
        BufferedMessageChannel<LobbyListFetchedMessage> lobbyListChannel;

        AuthenticationServiceFacade authenticationServiceFacade;
        public static ApplicationController Instance;

        public NetworkManager NetworkManager => m_NetworkManager;

        private bool m_ServicesInitialised = false;
        public void InitialiseServices()
        {
            if (m_ServicesInitialised)
            {
                return;
            }

            if (m_UpdateRunner == null)
            {
                m_UpdateRunner = Instantiate(updateRunnerPrefab);
                m_UpdateRunner.gameObject.name = "UpdateRunner";
            }

            if (m_NetworkManager == null)
            {
                m_NetworkManager = Instantiate(networkManagerPrefab);
                m_NetworkManager.gameObject.name = "NetworkManager";
            }

            if (m_ConnectionManager == null)
            {
                m_ConnectionManager = Instantiate(connectionManagerPrefab);
                m_ConnectionManager.gameObject.name = "ConnectionManager";
            }

            localLobbyUser = new LocalLobbyUser();
            m_LocalLobby = new LocalLobby();
            profileManager = new ProfileManager();

            quitChannel = new MessageChannel<QuitApplicationMessage>();
            unityServiceErrorChannel = new MessageChannel<UnityServiceErrorMessage>();
            connectionStatusChannel = new MessageChannel<ConnectStatus>();

            reconnectChannel = new MessageChannel<ReconnectMessage>();
            lobbyListChannel = new BufferedMessageChannel<LobbyListFetchedMessage>();

            authenticationServiceFacade = new AuthenticationServiceFacade(unityServiceErrorChannel);

            ConnectionStatusMessageUIManager connectionStatusMessageUIManager = new GameObject("UIMessageManager")
            .AddComponent<ConnectionStatusMessageUIManager>();
            connectionStatusMessageUIManager.Initialize(
            connectionStatusChannel,
            reconnectChannel);
            DontDestroyOnLoad(connectionStatusMessageUIManager.gameObject);

            UnityServicesUIHandler unityServicesUIHandler = new GameObject("UIServiceMessageManager")
            .AddComponent<UnityServicesUIHandler>();
            unityServicesUIHandler.SendMessage("Initialize", unityServiceErrorChannel, SendMessageOptions.DontRequireReceiver);

            m_LobbyServiceFacade = new LobbyServiceFacade(
            m_UpdateRunner,
            m_LocalLobby,
            localLobbyUser,
            unityServiceErrorChannel,
            lobbyListChannel);

            m_ConnectionManager.Initialise(m_NetworkManager,
            m_LobbyServiceFacade,
            profileManager,
            connectionStatusChannel,
            m_LocalLobby,
            reconnectChannel);

            DontDestroyOnLoad(m_ConnectionManager);

            PvPBootManager bootManager = new PvPBootManager(
            localLobbyUser,
            m_LocalLobby,
            m_LobbyServiceFacade,
            m_ConnectionManager,
            authenticationServiceFacade);

            DisposableGroup subHandles = new DisposableGroup();
            subHandles.Add(quitChannel.Subscribe(QuitGame));
            m_Subscriptions = subHandles;

            Application.wantsToQuit += OnWantToQuit;
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(m_UpdateRunner.gameObject);

            m_ServicesInitialised = true;
        }
        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        void OnDestroy()
        {
            Application.wantsToQuit -= OnWantToQuit;

            if (Instance == this)
            {
                Instance = null;
            }
        }

        private IEnumerator LeaveBeforeQuit()
        {
            // We want to quit anyways, so if anything happens while trying to leave the Lobby, log the exception then carry on
            try
            {
                m_LobbyServiceFacade.EndTracking();
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
            yield return null;
            Application.Quit();
        }

        private bool OnWantToQuit()
        {
            bool canQuit = string.IsNullOrEmpty(m_LocalLobby?.LobbyID);
            if (!canQuit)
            {
                StartCoroutine(LeaveBeforeQuit());
            }
            return canQuit;
        }
        public void ReinitialiseServicesForFLEE()
        {
            Debug.Log("PVP: ApplicationController.ReinitialiseServicesForFLEE - lightweight reset (matching organic flow)");

            // Shutdown NetworkManager if listening, but DON'T destroy it
            if (m_NetworkManager != null)
            {
                Debug.Log($"PVP: NetworkManager state - IsListening={m_NetworkManager.IsListening}, IsServer={m_NetworkManager.IsServer}, IsClient={m_NetworkManager.IsClient}");

                if (m_NetworkManager.IsListening)
                {
                    Debug.Log("PVP: Calling NetworkManager.Shutdown() (keeping instance alive)");
                    m_NetworkManager.Shutdown();
                    Debug.Log($"PVP: After Shutdown - IsListening={m_NetworkManager.IsListening}, IsServer={m_NetworkManager.IsServer}, IsClient={m_NetworkManager.IsClient}");
                }
            }

            // Reset ConnectionManager to offline state
            if (m_ConnectionManager != null)
            {
                Debug.Log("PVP: Resetting ConnectionManager to OfflineState");
                m_ConnectionManager.ResetToOffline();
            }

            // Clear static caches - these are safe to clear and necessary for clean state
            Debug.Log("PVP: Clearing static caches");
            DynamicPrefabLoadingUtilities.Init(m_NetworkManager);
            Matchplay.MultiplayBattleScene.Utils.Fetchers.PvPPrefabCache.Clear();
            Matchplay.MultiplayBattleScene.Utils.Factories.PvPFactoryProvider.Clear();
            Matchplay.Shared.SynchedServerData.ClearInstance();

            Debug.Log("PVP: FLEE reset complete - NetworkManager instance preserved");
        }
        public void DestroyNetworkObject()
        {
            Application.wantsToQuit -= OnWantToQuit;
            m_Subscriptions?.Dispose();
            m_LobbyServiceFacade?.EndTracking();
            Destroy(gameObject);
        }

        private void QuitGame(QuitApplicationMessage msg)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}