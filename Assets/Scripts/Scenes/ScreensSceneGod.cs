using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Settings;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.Music;
using BattleCruisers.UI.ScreensScene;
using BattleCruisers.UI.ScreensScene.HomeScreen;
using BattleCruisers.UI.ScreensScene.LevelsScreen;
using BattleCruisers.UI.ScreensScene.LoadoutScreen;
using BattleCruisers.UI.ScreensScene.PostBattleScreen;
using BattleCruisers.UI.ScreensScene.SettingsScreen;
using BattleCruisers.UI.ScreensScene.TrashScreen;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Fetchers.Cache;
using BattleCruisers.Utils.Fetchers.Sprites;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using NSubstitute;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using Common = UnityCommon.PlatformAbstractions.Time;

namespace BattleCruisers.Scenes
{
    public class ScreensSceneGod : MonoBehaviour, IScreensSceneGod
	{
		private IPrefabFactory _prefabFactory;
		private ScreenController _currentScreen;
        private IApplicationModel _applicationModel;
        private IDataProvider _dataProvider;
		private IGameModel _gameModel;
        private ISceneNavigator _sceneNavigator;
        private IMusicPlayer _musicPlayer;
        private ISingleSoundPlayer _soundPlayer;

        public HomeScreenController homeScreen;
		public LevelsScreenController levelsScreen;
		public PostBattleScreenController postBattleScreen;
		public LoadoutScreenController loadoutScreen;
        public SettingsScreenController settingsScreen;
        public TrashScreenController trashScreen;
        public TrashTalkDataList trashDataList;
        [SerializeField]
        private AudioSource _uiAudioSource;

        public bool goToPostBattleScreen = false;

		async void Start()
		{
            Helper.AssertIsNotNull(homeScreen, levelsScreen, postBattleScreen, loadoutScreen, settingsScreen, trashScreen, trashDataList, _uiAudioSource);

            IPrefabCacheFactory prefabCacheFactory = new PrefabCacheFactory();
            IPrefabCache prefabCache = await prefabCacheFactory.CreatePrefabCacheAsync(new PrefabFetcher());
            _prefabFactory = new PrefabFactory(prefabCache);
            trashDataList.Initialise();

            _applicationModel = ApplicationModelProvider.ApplicationModel;
			_dataProvider = _applicationModel.DataProvider;
			_gameModel = _dataProvider.GameModel;
            _sceneNavigator = LandingSceneGod.SceneNavigator;
            _musicPlayer = LandingSceneGod.MusicPlayer;
            _soundPlayer
                = new SingleSoundPlayer(
                    new SoundFetcher(),
                    new AudioSourceBC(_uiAudioSource));

            // TEMP  For showing PostBattleScreen :)
            if (goToPostBattleScreen)
            {
                _gameModel.LastBattleResult = new BattleResult(1, wasVictory: true);
                //_gameModel.LastBattleResult = new BattleResult(1, wasVictory: false);
                _applicationModel.ShowPostBattleScreen = true;
                //_applicationModel.IsTutorial = true;
            }


            // TEMP  For when not coming from LandingScene :)
            if (_musicPlayer == null)
            {
                _musicPlayer = Substitute.For<IMusicPlayer>();
                _sceneNavigator = Substitute.For<ISceneNavigator>();
            }

            SpriteFetcher spriteFetcher = new SpriteFetcher();
            IDifficultySpritesProvider difficultySpritesProvider = new DifficultySpritesProvider(spriteFetcher);
            homeScreen.Initialise(_soundPlayer, this, _dataProvider);
            settingsScreen.Initialise(_soundPlayer, this, _dataProvider.SettingsManager, _musicPlayer);
            trashScreen.Initialise(_soundPlayer, this, _applicationModel, _prefabFactory, spriteFetcher, trashDataList);

            if (_applicationModel.ShowPostBattleScreen)
            {
				_applicationModel.ShowPostBattleScreen = false;
    
                await GoToPostBattleScreenAsync(difficultySpritesProvider);
            }
            else
            {
                GoToHomeScreen();
            }

            // After potentially initialising post battle screen, because that can modify the data model.
            await InitialiseLevelsScreenAsync(difficultySpritesProvider);
            loadoutScreen.Initialise(_soundPlayer, this, _dataProvider, _prefabFactory);

            // TEMP  Go to specific screen :)
            //GoToSettingsScreen();
            // FELIX TEMP
            GoToLevelsScreen();
            //GoToLoadoutScreen();
            //GoToTrashScreen(levelNum: 1);

            _sceneNavigator.SceneLoaded(SceneNames.SCREENS_SCENE);
            Common.TimeBC.Instance.TimeScale = 1;
        }
        
        private async Task GoToPostBattleScreenAsync(IDifficultySpritesProvider difficultySpritesProvider)
        {
            Assert.IsFalse(postBattleScreen.IsInitialised, "Should only ever navigate (and hence initialise) once");
            await postBattleScreen.InitialiseAsync(_soundPlayer, this, _applicationModel, _prefabFactory, _musicPlayer, difficultySpritesProvider);

            GoToScreen(postBattleScreen, playDefaultMusic: false);
        }

        public void GoToHomeScreen()
        {
            GoToScreen(homeScreen);
        }

        public void GoToLevelsScreen()
        {
            GoToScreen(levelsScreen);
        }

        private async Task InitialiseLevelsScreenAsync(IDifficultySpritesProvider difficultySpritesProvider)
        {
            BattleResult lastBattleResult = _dataProvider.GameModel.LastBattleResult;
            int lastPlayedLevel = lastBattleResult != null ? lastBattleResult.LevelNum : 0;

            IList<LevelInfo> levels = CreateLevelInfo(_dataProvider.Levels, _dataProvider.GameModel.CompletedLevels);

            await levelsScreen.InitialiseAsync(
                _soundPlayer, 
                this, 
                levels, 
                _dataProvider.LockedInfo.NumOfLevelsUnlocked, 
                lastPlayedLevel, 
                difficultySpritesProvider, 
                trashDataList);
        }

        private IList<LevelInfo> CreateLevelInfo(IList<ILevel> staticLevels, IList<CompletedLevel> completedLevels)
        {
            IList<LevelInfo> levels = new List<LevelInfo>();

            for (int i = 0; i < staticLevels.Count; ++i)
            {
                ILevel staticLevel = staticLevels[i];
                CompletedLevel completedLevel = completedLevels.ElementAtOrDefault(i);
                Difficulty? completedDifficulty = null;

                if (completedLevel != null)
                {
                    completedDifficulty = completedLevel.HardestDifficulty;
                }

                levels.Add(new LevelInfo(staticLevel.Num, staticLevel.Name, completedDifficulty));
            }

            return levels;
        }

		public void GoToLoadoutScreen()
		{
            GoToScreen(loadoutScreen);
        }

        public void GoToSettingsScreen()
        {
            GoToScreen(settingsScreen);
        }

		public void GoToTrashScreen(int levelNum)
		{
            Assert.IsTrue(
                levelNum <= _dataProvider.LockedInfo.NumOfLevelsUnlocked, 
                "levelNum: " + levelNum + " should be <= than number of levels unlocked: " + _dataProvider.LockedInfo.NumOfLevelsUnlocked);

			_applicationModel.SelectedLevel = levelNum;

            if (_applicationModel.IsTutorial)
            {
                LoadBattleScene();
            }
            else
            {
                GoToScreen(trashScreen);
            }
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

        public void LoadBattleScene()
        { 
            _sceneNavigator.GoToScene(SceneNames.BATTLE_SCENE);
            CleanUp();
        }

        private void CleanUp()
        {
            loadoutScreen.DisposeManagedState();
        }

        void Update()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                _currentScreen?.Cancel();
            }
        }
    }
}
