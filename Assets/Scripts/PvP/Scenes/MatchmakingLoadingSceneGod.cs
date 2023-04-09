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
using BattleCruisers.Network.Multiplay.Gameplay.Configuration;
using BattleCruisers.Network.Multiplay.ConnectionManagement;
using BattleCruisers.Network.Multiplay.Infrastructure;


namespace BattleCruisers.Network.Multiplay.Scenes
{
    public class MatchmakingLoadingSceneGod : GameStateBehaviour
    {

        public override GameState ActiveState { get { return GameState.MatchmakingLoadingScene; } }

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

        [Inject] ConnectionManager m_ConnectionManager;

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

        async void Start()
        {
            Helper.AssertIsNotNull(_uiAudioSource, trashDataList);
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
        }

    }
}

