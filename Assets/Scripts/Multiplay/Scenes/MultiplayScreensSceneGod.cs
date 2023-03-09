using BattleCruisers.Data;
using BattleCruisers.Scenes;
using System.Collections;
using System.Collections.Generic;
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

namespace BattleCruisers.Network.Multiplay.Scenes
{
    public class MultiplayScreensSceneGod : MonoBehaviour, IMultiplayScreensSceneGod
    {
        public MultiplayScreenController multiplayScreen;
        public MatchmakingScreenController matchmakingScreen;

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


        public void GotoMatchmakingScreen()
        {
            GoToScreen(matchmakingScreen);
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
        }


        private void GoToScreen(ScreenController destinationScreen, bool playDefaultMusic = true)
        {           

            Logging.Log(Tags.SCREENS_SCENE_GOD, $"START  current: {_currentScreen}  destination: {destinationScreen}");

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

