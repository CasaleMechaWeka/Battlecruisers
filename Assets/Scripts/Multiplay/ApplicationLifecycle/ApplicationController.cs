using BattleCruisers.Network.Multiplay.ConnectionManagement;
using BattleCruisers.Network.Multiplay.Infrastructure;
using BattleCruisers.Network.Multiplay.UnityServices.Lobbies;
using BattleCruisers.Network.Multiplay.Utils;
using BattleCruisers.Network.Multiplay.Gameplay.GameState;
using BattleCruisers.Network.Multiplay.ApplicationLifecycle.Messages;
using BattleCruisers.Network.Multiplay.Gameplay.Messages;
using BattleCruisers.Network.Multiplay.UnityServices.Auth;
using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using BattleCruisers.Network.Multiplay.UnityServices;
using Unity.Services.Authentication;

namespace BattleCruisers.Network.Multiplay.ApplicationLifecycle
{
    public class ApplicationController : LifetimeScope
    {
        [SerializeField] UpdateRunner m_UpdateRunner;
        [SerializeField] ConnectionManager m_ConnectionManager;
        [SerializeField] NetworkManager m_NetworkManager;

        LocalLobby m_LocalLobby;
        LobbyServiceFacade m_LobbyServiceFacade;

        IDisposable m_Subscriptions;

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            builder.RegisterComponent(m_UpdateRunner);
            builder.RegisterComponent(m_ConnectionManager);
            builder.RegisterComponent(m_NetworkManager);

            builder.Register<LocalLobbyUser>(Lifetime.Singleton);
            builder.Register<LocalLobby>(Lifetime.Singleton);
            builder.Register<ProfileManager>(Lifetime.Singleton);
            builder.Register<PersistentGameState>(Lifetime.Singleton);

            builder.RegisterInstance(new MessageChannel<QuitApplicationMessage>()).AsImplementedInterfaces();
            builder.RegisterInstance(new MessageChannel<UnityServiceErrorMessage>()).AsImplementedInterfaces();
            builder.RegisterInstance(new MessageChannel<ConnectStatus>()).AsImplementedInterfaces();
            builder.RegisterInstance(new MessageChannel<DoorStateChangedEventMessage>()).AsImplementedInterfaces();


            builder.RegisterComponent(new NetworkedMessageChannel<LifeStateChangedEventMessage>()).AsImplementedInterfaces();
            builder.RegisterComponent(new NetworkedMessageChannel<ConnectionEventMessage>()).AsImplementedInterfaces();

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            builder.RegisterComponent(new NetworkedMessageChannel<CheatUsedMessage>()).AsImplementedInterfaces();
#endif

            builder.RegisterInstance(new MessageChannel<ReconnectMessage>()).AsImplementedInterfaces();
            builder.RegisterInstance(new BufferedMessageChannel<LobbyListFetchedMessage>()).AsImplementedInterfaces();
            builder.Register<AuthenticationServiceFacade>(Lifetime.Singleton);
            builder.RegisterEntryPoint<LobbyServiceFacade>(Lifetime.Singleton).AsSelf();

        }


        private void Start()
        {
            m_LocalLobby = Container.Resolve<LocalLobby>();
            m_LobbyServiceFacade = Container.Resolve<LobbyServiceFacade>();

            var quitApplicationSub = Container.Resolve<ISubscriber<QuitApplicationMessage>>();
            var subHandles = new DisposableGroup();
            subHandles.Add(quitApplicationSub.Subscribe(QuitGame));
            m_Subscriptions = subHandles;

            Application.wantsToQuit += OnWantToQuit;
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(m_UpdateRunner.gameObject);
            // Application.targetFrameRate = 120;
            //SceneManager.LoadScene("MainMenu");
        }
        protected override void OnDestroy()
        {
            m_Subscriptions?.Dispose();
            m_LobbyServiceFacade?.EndTracking();
            base.OnDestroy();
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
            var canQuit = string.IsNullOrEmpty(m_LocalLobby?.LobbyID);
            if (!canQuit)
            {
                StartCoroutine(LeaveBeforeQuit());
            }
            return canQuit;
        }


        public void DestroyNetworkObject()
        {
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

