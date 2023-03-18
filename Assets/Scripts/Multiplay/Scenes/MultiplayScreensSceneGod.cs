using BattleCruisers.Data;
using BattleCruisers.Scenes;
using System;
using System.Collections;
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
using BattleCruisers.Network.Multiplay.Utils;
using Unity.Services.Authentication;
using BattleCruisers.Network.Multiplay.UnityServices.Lobbies;
using Unity.Multiplayer.Samples.Utilities;
using BattleCruisers.Network.Multiplay.Gameplay.GameState;
using VContainer.Unity;

namespace BattleCruisers.Network.Multiplay.Scenes
{
    public class MultiplayScreensSceneGod : GameStateBehaviour, IMultiplayScreensSceneGod
    {

        public override GameState ActiveState { get { return GameState.MultiplayScreenScene; } }
        public MultiplayScreenController multiplayScreen;
/*        public MatchmakingScreenController matchmakingScreen;*/

        public int defaultLevel;


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
        [Inject] ProfileManager m_ProfileManager;


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

            // Temp only because I am starting the scene without a previous choose level scene
            if (sceneNavigator == null)
            {
                _applicationModel.SelectedLevel = defaultLevel;
                sceneNavigator = Substitute.For<ISceneNavigator>();
            }          

            sceneNavigator.SceneLoaded(SceneNames.MULTIPLAY_SCREENS_SCENE);
        }


        void OnEnable()
        {
            LandingSceneGod.SceneNavigator.SceneLoaded(SceneNames.MULTIPLAY_SCREENS_SCENE);
        }

   /*     private async Task InitialiseMultiplayScreensScreenAsync()
        {
            
        }*/

/*        public void GotoMatchmakingScreen()
        {
            GoToScreen(matchmakingScreen);
        }*/

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

        public void LoadMatchmakingScene()
        {
            /*  _sceneNavigator.GoToScene(SceneNames.MATCHMAKING_SCREENS_SCENE, true);*/
            SceneLoaderWrapper.Instance.LoadScene(SceneNames.MATCHMAKING_SCREENS_SCENE, false);
        }


        IEnumerator iDestoryAllNetworkObjects()
        {            
            yield return new WaitForEndOfFrame();
            GameObject[] gos = (GameObject[]) GameObject.FindObjectsOfType(typeof(GameObject));
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

