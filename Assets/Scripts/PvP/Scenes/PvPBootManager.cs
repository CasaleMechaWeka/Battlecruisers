using BattleCruisers.Data;
using BattleCruisers.Scenes;
using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading.Tasks;
using UnityEngine;
using NSubstitute;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.UI.ScreensScene.Multiplay.ArenaScreen;
using BattleCruisers.UI.ScreensScene;
using BattleCruisers.Data.Models;
using BattleCruisers.UI.Music;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils.Fetchers.Cache;
using BattleCruisers.UI.Sound.AudioSources;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using BattleCruisers.UI.ScreensScene.TrashScreen;
using BattleCruisers.Utils.Fetchers.Sprites;
using BattleCruisers.Data.Helpers;
using BattleCruisers.Data.Static;
using UnityEngine.Assertions;
using VContainer;
using BattleCruisers.Network.Multiplay.UnityServices.Auth;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using BattleCruisers.Network.Multiplay.Utils;
using Unity.Services.Authentication;
using BattleCruisers.Network.Multiplay.UnityServices.Lobbies;
using Unity.Multiplayer.Samples.Utilities;
using BattleCruisers.Network.Multiplay.Gameplay.GameState;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;
using VContainer.Unity;
using BattleCruisers.Network.Multiplay.Gameplay.Configuration;
using BattleCruisers.Network.Multiplay.ConnectionManagement;
using BattleCruisers.Network.Multiplay.Infrastructure;
using Random = UnityEngine.Random;
using BattleCruisers.Network.Multiplay.ApplicationLifecycle;
using BattleCruisers.Network.Multiplay.Gameplay.UI;

namespace BattleCruisers.Network.Multiplay.Scenes
{
    public class PvPBootManager : GameStateBehaviour
    {

        public override GameState ActiveState { get { return GameState.MultiplayScreenScene; } }

        // networking settings

        [SerializeField]
        string m_IP;
        [SerializeField]
        string m_Port;
        [SerializeField]
        NameGenerationData m_NameGenerationData;


        //// ------------------------------------  Local Test  -------------------------------------  //////
        public bool m_LocalLaunchMode = true;
        private string k_DefaultLobbyName;
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        public int defaultLevel;

        const int k_DefaultPort = 7777;
        const string k_DefaultIP = "127.0.0.1";


        private IPrefabFactory _prefabFactory;
        private ScreenController _currentScreen;
        private IApplicationModel _applicationModel;
        private IDataProvider _dataProvider;
        private IGameModel _gameModel;
        private ISceneNavigator _sceneNavigator;
        private IMusicPlayer _musicPlayer;
        private ISingleSoundPlayer _soundPlayer;


        public TrashTalkDataList trashDataList;
        [SerializeField]
        private AudioSource _uiAudioSource;

        [Inject] AuthenticationServiceFacade m_AuthServiceFacade;
        [Inject] LocalLobbyUser m_LocalUser;
        [Inject] LocalLobby m_LocalLobby;
        [Inject] LobbyServiceFacade m_LobbyServiceFacade;
        [Inject] LobbyAPIInterface m_LobbyAPIInterface;
        [Inject] ProfileManager m_ProfileManager;

        [Inject] ConnectionManager m_ConnectionManager;
        [Inject] AuthenticationServiceFacade m_AuthenticationServiceFacade;
        ISubscriber<ConnectStatus> m_ConnectStatusSubscriber;

        [Inject]
        void InjectDependencies(ISubscriber<ConnectStatus> connectStatusSubscriber)
        {
            m_ConnectStatusSubscriber = connectStatusSubscriber;
            m_ConnectStatusSubscriber.Subscribe(OnConnectStatusMessage);
        }


        void OnConnectStatusMessage(ConnectStatus connectStatus)
        {

        }


        private void JoinWithIP(string ip, string port)
        {
            int.TryParse(port, out var portNum);
            if (portNum <= 0)
            {
                portNum = k_DefaultPort;
            }
            ip = string.IsNullOrEmpty(ip) ? k_DefaultIP : ip;
            m_ConnectionManager.StartClientIp(k_DefaultLobbyName, ip, portNum);
        }

        private void JoinWithLobby()
        {

#pragma warning disable 4014
            JoinWithLobbyRequest();
#pragma warning restore 4014
        }

        private void onMatchmakingFailed()
        {
            DestroyAllNetworkObjects();
        }

        private void onMatchmakingStarted()
        {

        }

        public async void DestroyAllNetworkObjects()
        {
            await Task.Delay(10);
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
        }

        private async Task JoinWithLobbyRequest()
        {
            bool playerIsAuthorized = await m_AuthenticationServiceFacade.EnsurePlayerIsAuthorized();

            if (!playerIsAuthorized)
            {
                return;
            }

            List<QueryFilter> mFilters = new List<QueryFilter>()
            {
            // Let's search for games with open slots (AvailableSlots greater than 0)
            new QueryFilter(
                field: QueryFilter.FieldOptions.AvailableSlots,
                op: QueryFilter.OpOptions.GT,
                value: "0"),
            new QueryFilter(
                field: QueryFilter.FieldOptions.S1, // S1 = "GameMap"
                op: QueryFilter.OpOptions.EQ,
                value: m_ConnectionManager.Manager.User.Data.userGamePreferences.ToSceneName),
            // Example "Score" range filter (Score is a custom numeric field in this example)
            new QueryFilter(
                field: QueryFilter.FieldOptions.N1, // N1 = "Battle Win Score"
                op: QueryFilter.OpOptions.EQ,
                value: ApplicationModelProvider.ApplicationModel.DataProvider.GameModel.BattleWinScore.ToString()),
/*            new QueryFilter(
                field: QueryFilter.FieldOptions.N2, // N2 = "Rank"
                op: QueryFilter.OpOptions.EQ,
                value: "0"),*/
            };


            List<QueryOrder> mOrders = new List<QueryOrder>
        {
            new QueryOrder(true, QueryOrder.FieldOptions.AvailableSlots),
            new QueryOrder(false, QueryOrder.FieldOptions.Created),
            new QueryOrder(false, QueryOrder.FieldOptions.Name),
        };



            QueryResponse response = await m_LobbyServiceFacade.QueryLobbyListAsync(mFilters, mOrders);

            List<Lobby> foundLobbies = response.Results;

            if (foundLobbies.Any())
            {
                Debug.Log("Found Lobbies :\n" + JsonConvert.SerializeObject(foundLobbies));

                var randomLobby = foundLobbies[Random.Range(0, foundLobbies.Count)];

                var lobbyJoinAttemp = await m_LobbyServiceFacade.TryJoinLobbyAsync(lobbyId: randomLobby.Id, null);

                if (lobbyJoinAttemp.Success)
                {
                    m_LobbyServiceFacade.SetRemoteLobby(lobbyJoinAttemp.Lobby);
                    if (m_LobbyServiceFacade.CurrentUnityLobby != null)
                    {
                        Debug.Log($"Joined Lobby {lobbyJoinAttemp.Lobby.Name} ({lobbyJoinAttemp.Lobby.Id})");
                        m_LobbyServiceFacade.BeginTracking();
                    }
                }
            }
            else
            {
                var lobbyData = new Dictionary<string, DataObject>()
                {
                    ["GameMap"] = new DataObject(DataObject.VisibilityOptions.Public, m_ConnectionManager.Manager.User.Data.userGamePreferences.ToSceneName, DataObject.IndexOptions.S1),
                    ["Score"] = new DataObject(DataObject.VisibilityOptions.Public, ApplicationModelProvider.ApplicationModel.DataProvider.GameModel.BattleWinScore.ToString(), DataObject.IndexOptions.N1),
                    //   ["Rank"] = new DataObject(DataObject.VisibilityOptions.Public, CalculateRank(ApplicationModelProvider.ApplicationModel.DataProvider.GameModel.LifetimeDestructionScore).ToString(), DataObject.IndexOptions.N2)
                };
                var lobbyCreationAttemp = await m_LobbyServiceFacade.TryCreateLobbyAsync(m_NameGenerationData.GenerateName(), m_ConnectionManager.MaxConnectedPlayers, isPrivate: false, m_LocalUser.GetDataForUnityServices(), lobbyData);
                if (lobbyCreationAttemp.Success)
                {
                    m_LocalUser.IsHost = true;
                    m_LobbyServiceFacade.SetRemoteLobby(lobbyCreationAttemp.Lobby);
                    if (m_LobbyServiceFacade.CurrentUnityLobby != null)
                    {
                        Debug.Log($"Created new Lobby {lobbyCreationAttemp.Lobby.Name} ({lobbyCreationAttemp.Lobby.Id})");
                        m_LobbyServiceFacade.BeginTracking();
                    }
                }
            }
        }


        private int CalculateRank(long score)
        {

            for (int i = 0; i <= StaticPrefabKeys.Ranks.AllRanks.Count; i++)
            {
                long x = 2500 + 2500 * i * i;
                //Debug.Log(x);
                if (score < x)
                {
                    return i;
                }
            }
            return StaticPrefabKeys.Ranks.AllRanks.Count;
        }

        IEnumerator iStartPvP()
        {
            yield return new WaitForEndOfFrame();

            if (m_LocalLaunchMode)
            {
                JoinWithIP("127.0.0.1", "7777");
            }
            else
            {
                // should be tested, still not confirmed.
                JoinWithLobby();
            }
        }

        protected override void Awake()
        {
            base.Awake();
            //   TrySignIn();
        }



        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
        }

        private async void TrySignIn()
        {
            try
            {
                var unityAuthenticationInitOptions = new InitializationOptions();
                var profile = m_ProfileManager.Profile;

                if (profile.Length > 0)
                {
                    try
                    {
                        /*{LocalProfileTool.LocalProfileSuffix}*/
                        unityAuthenticationInitOptions.SetProfile($"{profile}");

                    }
                    catch (Exception e)
                    {
                        Debug.Log(e.ToString());
                    }
                }
                await m_AuthServiceFacade.InitializeAndSignInAsync(unityAuthenticationInitOptions);
                OnAuthSignIn();
                m_ProfileManager.onProfileChanged += OnProfileChanged;
                m_ConnectionManager.Manager = new Matchplay.Client.ClientGameManager();
            }
            catch (Exception ex)
            {
                Debug.Log(ex.ToString());
                OnSignInFailed();
            }
        }

        private void OnAuthSignIn()
        {
            Debug.Log($"Signed in. Unity Player ID {AuthenticationService.Instance.PlayerId}");
            m_LocalUser.ID = AuthenticationService.Instance.PlayerId;
            m_LocalUser.DisplayName = m_NameGenerationData.GenerateName();
            // The local LobbyUser object will be hooked into UI before the LocalLobby is populated during lobby join, so the LocalLobby must know about it already when that happens.
            m_LocalLobby.AddUser(m_LocalUser);
        }


        private void OnSignInFailed()
        {

        }

        void OnDestroy()
        {
            m_ProfileManager.onProfileChanged -= OnProfileChanged;
            m_LobbyServiceFacade.OnMatchMakingFailed -= onMatchmakingFailed;
            m_LobbyServiceFacade.OnMatchMakingStarted -= onMatchmakingStarted;
        }

        async void OnProfileChanged()
        {
            await m_AuthServiceFacade.SwitchProfileAndReSignInAsync(m_ProfileManager.Profile);

            Debug.Log($"Signed in. Unity Player ID {AuthenticationService.Instance.PlayerId}");

            // Updating LocalUser and LocalLobby
            m_LocalLobby.RemoveUser(m_LocalUser);
            m_LocalUser.ID = AuthenticationService.Instance.PlayerId;
            m_LocalLobby.AddUser(m_LocalUser);
        }

        private async void Start()
        {
            Helper.AssertIsNotNull(_uiAudioSource, trashDataList);
            Logging.Log(Tags.Multiplay_SCREENS_SCENE_GOD, "START");

            ILocTable commonStrings = await LocTableFactory.Instance.LoadCommonTableAsync();
            ILocTable storyStrings = await LocTableFactory.Instance.LoadStoryTableAsync();

            trashDataList.Initialise(storyStrings);
            ITrashTalkData trashTalkData = await trashDataList.GetTrashTalkAsync(/*_gameModel.SelectedLevel*/1);
            MatchmakingScreenController.Instance.SetTraskTalkData(trashTalkData, commonStrings, storyStrings);

            // cheat code for local test
            k_DefaultLobbyName = m_NameGenerationData.GenerateName();

            m_LobbyServiceFacade.OnMatchMakingFailed += onMatchmakingFailed;
            m_LobbyServiceFacade.OnMatchMakingStarted += onMatchmakingStarted;

            StartCoroutine(iStartPvP());
        }
    }
}

