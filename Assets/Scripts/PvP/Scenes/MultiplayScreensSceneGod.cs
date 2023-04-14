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
using VContainer.Unity;
using BattleCruisers.Network.Multiplay.Gameplay.Configuration;
using BattleCruisers.Network.Multiplay.ConnectionManagement;
using BattleCruisers.Network.Multiplay.Infrastructure;
using Random = UnityEngine.Random;

namespace BattleCruisers.Network.Multiplay.Scenes
{
    public class MultiplayScreensSceneGod : GameStateBehaviour, IMultiplayScreensSceneGod
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

        // screens
        public MultiplayScreenController multiplayScreen;
        public MatchmakingScreenController matchmakingScreen;

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




        // private void HostIPRequest(string ip, string port)
        // {

        //     int.TryParse(port, out var portNum);
        //     if (portNum <= 0)
        //     {
        //         portNum = k_DefaultPort;
        //     }
        //     ip = string.IsNullOrEmpty(ip) ? k_DefaultIP : ip;
        //     m_ConnectionManager.StartHostIp(m_localTestName, ip, portNum);

        // }


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
                field: QueryFilter.FieldOptions.S1, // S2 = "GameMap"
                op: QueryFilter.OpOptions.EQ,
                value: m_ConnectionManager.Manager.User.Data.userGamePreferences.ToSceneName),
            // Example "skill" range filter (skill is a custom numeric field in this example)
            new QueryFilter(
                field: QueryFilter.FieldOptions.N1, // N1 = "Skill"
                op: QueryFilter.OpOptions.GT,
                value: "0"),
            new QueryFilter(
                field: QueryFilter.FieldOptions.N2, // N2 = "Rank"
                op: QueryFilter.OpOptions.LT,
                value: "51"),

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
                        m_LobbyServiceFacade.BeginTracking();
                        await m_ConnectionManager.StartMatchmaking(m_LocalLobby.LobbyID);
                    }
                    Debug.Log($"Joined Lobby {lobbyJoinAttemp.Lobby.Name} ({lobbyJoinAttemp.Lobby.Id})");
                }

            }
            else
            {
                var lobbyData = new Dictionary<string, DataObject>()
                {
                    ["GameMap"] = new DataObject(DataObject.VisibilityOptions.Public, m_ConnectionManager.Manager.User.Data.userGamePreferences.ToSceneName, DataObject.IndexOptions.S1),
                    ["Skill"] = new DataObject(DataObject.VisibilityOptions.Public, "33", DataObject.IndexOptions.N1),
                    ["Rank"] = new DataObject(DataObject.VisibilityOptions.Public, "22", DataObject.IndexOptions.N2)
                };



                var lobbyCreationAttemp = await m_LobbyServiceFacade.TryCreateLobbyAsync(m_NameGenerationData.GenerateName(), m_ConnectionManager.MaxConnectedPlayers, isPrivate: false, m_LocalUser.GetDataForUnityServices(), lobbyData);
                if (lobbyCreationAttemp.Success)
                {
                    m_LocalUser.IsHost = true;
                    m_LobbyServiceFacade.SetRemoteLobby(lobbyCreationAttemp.Lobby);
                    if (m_LobbyServiceFacade.CurrentUnityLobby != null)
                    {
                        m_LobbyServiceFacade.BeginTracking();
                    }
                    Debug.Log($"Created new Lobby {lobbyCreationAttemp.Lobby.Name} ({lobbyCreationAttemp.Lobby.Id})");

                }



            }
            // if (lobbyQuickJoinAttemp.Success)
            // {
            //     Debug.Log("Joined to Lobby Name is " + lobbyQuickJoinAttemp.Lobby.Name);
            //     m_LobbyServiceFacade.SetRemoteLobby(lobbyQuickJoinAttemp.Lobby);
            //     if (m_LobbyServiceFacade.CurrentUnityLobby != null)
            //     {
            //         m_LobbyServiceFacade.BeginTracking();
            //         await m_ConnectionManager.StartMatchmaking(m_LocalLobby.LobbyID);
            //     }

            // }
            // else
            // {
            //     var lobbyCreationAttemp = await m_LobbyServiceFacade.TryCreateLobbyAsync(k_DefaultLobbyName, m_ConnectionManager.MaxConnectedPlayers, isPrivate: false);
            //     if (lobbyCreationAttemp.Success)
            //     {
            //         Debug.Log("Craeted Lobby Name is " + lobbyCreationAttemp.Lobby.Name);
            //         m_LocalUser.IsHost = true;
            //         m_LobbyServiceFacade.SetRemoteLobby(lobbyCreationAttemp.Lobby);
            //         if (m_LobbyServiceFacade.CurrentUnityLobby != null)
            //         {
            //             m_LobbyServiceFacade.BeginTracking();
            //         }
            //     }
            // }

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

            if (string.IsNullOrEmpty(Application.cloudProjectId))
            {
                OnSignInFailed();
                return;
            }

            TrySignIn();
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
                Debug.Log("Profile =" + profile.ToString());
                if (profile.Length > 0)
                {
                    try
                    {
                        unityAuthenticationInitOptions.SetProfile(profile);
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e.ToString());
                    }
                }
                await m_AuthServiceFacade.InitializeAndSignInAsync(unityAuthenticationInitOptions);
                OnAuthSignIn();
                m_ProfileManager.onProfileChanged += OnProfileChanged;
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
            Helper.AssertIsNotNull(multiplayScreen, _uiAudioSource, trashDataList);
            Logging.Log(Tags.Multiplay_SCREENS_SCENE_GOD, "START");

            ILocTable commonStrings = await LocTableFactory.Instance.LoadCommonTableAsync();
            ILocTable storyStrings = await LocTableFactory.Instance.LoadStoryTableAsync();
            ILocTable screensSceneStrings = await LocTableFactory.Instance.LoadScreensSceneTableAsync();
            IPrefabCacheFactory prefabCacheFactory = new PrefabCacheFactory(commonStrings);

            Logging.Log(Tags.Multiplay_SCREENS_SCENE_GOD, "Pre prefab cache load");
            IPrefabCache prefabCache = await prefabCacheFactory.CreatePrefabCacheAsync(new PrefabFetcher());
            Logging.Log(Tags.Multiplay_SCREENS_SCENE_GOD, "After prefab cache load");


            ISceneNavigator sceneNavigator = LandingSceneGod.SceneNavigator;
            _applicationModel = ApplicationModelProvider.ApplicationModel;
            _dataProvider = _applicationModel.DataProvider;
            _gameModel = _dataProvider.GameModel;
            _sceneNavigator = LandingSceneGod.SceneNavigator;
            _musicPlayer = LandingSceneGod.MusicPlayer;
            _soundPlayer
            = new SingleSoundPlayer(
                new SoundFetcher(),
                new EffectVolumeAudioSource(
                    new AudioSourceBC(_uiAudioSource),
                    _dataProvider.SettingsManager, 1));

            _prefabFactory = new PrefabFactory(prefabCache, _dataProvider.SettingsManager, commonStrings);
            trashDataList.Initialise(storyStrings);


            // TEMP  For when not coming from LandingScene :)
            if (_musicPlayer == null)
            {
                _musicPlayer = Substitute.For<IMusicPlayer>();
                _sceneNavigator = Substitute.For<ISceneNavigator>();
            }


            SpriteFetcher spriteFetcher = new SpriteFetcher();
            IDifficultySpritesProvider difficultySpritesProvider = new DifficultySpritesProvider(spriteFetcher);

            INextLevelHelper nextLevelHelper = new NextLevelHelper(_applicationModel);

            multiplayScreen.Initialise(this, _soundPlayer, _dataProvider);
            matchmakingScreen.Initialise(this, _soundPlayer, _dataProvider);


            multiplayScreen.gameObject.SetActive(true);
            matchmakingScreen.gameObject.SetActive(false);

            _currentScreen = multiplayScreen;


            // Temp only because I am starting the scene without a previous choose level scene
            if (sceneNavigator == null)
            {
                _applicationModel.SelectedLevel = defaultLevel;
                sceneNavigator = Substitute.For<ISceneNavigator>();
            }

            sceneNavigator.SceneLoaded(SceneNames.MULTIPLAY_SCREENS_SCENE);



            // cheat code for local test
            k_DefaultLobbyName = m_NameGenerationData.GenerateName();

            Debug.Log("m_localLobbyUser Name" + m_LocalUser.DisplayName);
        }


        /*        void OnEnable()
                {
                    LandingSceneGod.SceneNavigator.SceneLoaded(SceneNames.MULTIPLAY_SCREENS_SCENE);
                }*/

        /*     private async Task InitialiseMultiplayScreensScreenAsync()
             {

             }*/

        public void GotoMatchmakingScreen()
        {
            if (AuthenticationService.Instance.IsAuthorized)
            {
                GoToScreen(matchmakingScreen);
                StartCoroutine(iStartPvP());
            }
        }

        public void GotoSettingScreen()
        {

        }

        public void GotoShopScreen()
        {

        }

        public void LoadMainMenuScene()
        {
            _sceneNavigator.GoToScene(SceneNames.SCREENS_SCENE, true);
            StartCoroutine(iDestoryAllNetworkObjects());
        }

        // public void LoadMatchmakingScene()
        // {
        //     /*  _sceneNavigator.GoToScene(SceneNames.MATCHMAKING_SCREENS_SCENE, true);*/
        //     SceneLoaderWrapper.Instance.LoadScene(SceneNames.MATCHMAKING_SCREENS_SCENE, false);
        // }


        IEnumerator iDestoryAllNetworkObjects()
        {
            yield return new WaitForEndOfFrame();
            GameObject[] gos = (GameObject[])GameObject.FindObjectsOfType(typeof(GameObject));
            foreach (GameObject go in gos)
            {
                if (go && go.transform.parent == null)
                {
                    go.gameObject.BroadcastMessage("DestroyNetworkObject", SendMessageOptions.DontRequireReceiver);
                }
            }
        }

        private void GoToScreen(ScreenController destinationScreen, bool playDefaultMusic = true)
        {

            Logging.Log(Tags.Multiplay_SCREENS_SCENE_GOD, $"START  current: {_currentScreen}  destination: {destinationScreen}");

            Assert.AreNotEqual(_currentScreen, destinationScreen);

            if (_currentScreen != null)
            {
                _currentScreen.OnDismissing();
                _currentScreen.gameObject.SetActive(false);
                _soundPlayer.PlaySoundAsync(SoundKeys.UI.ScreenChange);
            }

            _currentScreen = destinationScreen;
            _currentScreen.gameObject.SetActive(true);
            _currentScreen.OnPresenting(activationParameter: null);

            if (playDefaultMusic)
            {
                _musicPlayer.PlayScreensSceneMusic();
            }
        }

    }
}

