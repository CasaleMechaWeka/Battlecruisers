using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Settings;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.Loading;
using BattleCruisers.UI.Music;
using BattleCruisers.UI.ScreensScene;
using BattleCruisers.UI.ScreensScene.HomeScreen;
using BattleCruisers.UI.ScreensScene.LevelsScreen;
using BattleCruisers.UI.ScreensScene.LoadoutScreen;
using BattleCruisers.UI.ScreensScene.PostBattleScreen;
using BattleCruisers.UI.ScreensScene.SettingsScreen;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions;
using NSubstitute;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes
{
    public class ScreensSceneGod : MonoBehaviour, IScreensSceneGod
	{
		private PrefabFactory _prefabFactory;
		private ScreenController _currentScreen;
        private IApplicationModel _applicationModel;
        private IDataProvider _dataProvider;
		private IGameModel _gameModel;
        private ISceneNavigator _sceneNavigator;
        private IMusicPlayer _musicPlayer;
        private IHintProvider _hintProvider;
        private ISoundPlayer _soundPlayer;

        public HomeScreenController homeScreen;
		public LevelsScreenController levelsScreen;
		public PostBattleScreenController postBattleScreen;
		public LoadoutScreenController loadoutScreen;
        public SettingsScreenController settingsScreen;

		void Start()
		{
            Assert.raiseExceptions = true;
            Helper.AssertIsNotNull(homeScreen, levelsScreen, postBattleScreen, loadoutScreen, settingsScreen);

			_prefabFactory = new PrefabFactory(new PrefabFetcher());
            _applicationModel = ApplicationModelProvider.ApplicationModel;
			_dataProvider = _applicationModel.DataProvider;
			_gameModel = _dataProvider.GameModel;
            _sceneNavigator = LandingSceneGod.SceneNavigator;
            _musicPlayer = LandingSceneGod.MusicPlayer;
            HintProviders hintProviders = new HintProviders(RandomGenerator.Instance);
            _hintProvider = new CompositeHintProvider(hintProviders.BasicHints, hintProviders.AdvancedHints, _gameModel, RandomGenerator.Instance);
            _soundPlayer
                = new SoundPlayer(
                    new SoundFetcher(),
                    new AudioClipPlayer(),
                    new CameraBC(Camera.main));

            // TEMP  For showing PostBattleScreen :)
            //_gameModel.LastBattleResult = new BattleResult(1, wasVictory: true);
            ////_gameModel.LastBattleResult = new BattleResult(1, wasVictory: false);
            //_applicationModel.ShowPostBattleScreen = true;
            //_applicationModel.IsTutorial = true;


            // TEMP  For when not coming from LandingScene :)
            if (_musicPlayer == null)
            {
                _musicPlayer = Substitute.For<IMusicPlayer>();
            }


            _musicPlayer.PlayScreensSceneMusic();
            homeScreen.Initialise(_soundPlayer, this, _gameModel, _dataProvider.Levels.Count);
            settingsScreen.Initialise(_soundPlayer, this, _dataProvider.SettingsManager);


            if (_applicationModel.ShowPostBattleScreen)
            {
				_applicationModel.ShowPostBattleScreen = false;
    
                GoToPostBattleScreen();
            }
            else
            {
                GoToHomeScreen();
            }

            // After potentially initialising post battle screen, because that can modify the data model.
            InitialiseLevelsScreen();
            loadoutScreen.Initialise(_soundPlayer, this, _dataProvider, _prefabFactory);

            // TEMP  Go to specific screen :)
            //GoToSettingsScreen();
            //GoToLevelsScreen();
            // FELIX  TEMP
            GoToLoadoutScreen();
        }
        
        private void GoToPostBattleScreen()
        {
            Assert.IsFalse(postBattleScreen.IsInitialised, "Should only ever navigate (and hence initialise) once");
            postBattleScreen.Initialise(_soundPlayer, this, _applicationModel, _prefabFactory, _musicPlayer);

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

        private void InitialiseLevelsScreen()
        {
            BattleResult lastBattleResult = _dataProvider.GameModel.LastBattleResult;
            int lastPlayedLevel = lastBattleResult != null ? lastBattleResult.LevelNum : 0;

            IList<LevelInfo> levels = CreateLevelInfo(_dataProvider.Levels, _dataProvider.GameModel.CompletedLevels);

            levelsScreen.Initialise(_soundPlayer, this, levels, _dataProvider.LockedInfo.NumOfLevelsUnlocked, lastPlayedLevel);
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

		private void GoToScreen(ScreenController destinationScreen, bool playDefaultMusic = true)
		{
            Logging.Log(Tags.SCREENS_SCENE_GOD, $"START  current: {_currentScreen}  destination: {destinationScreen}");

            Assert.AreNotEqual(_currentScreen, destinationScreen);

			if (_currentScreen != null)
			{
                _currentScreen.OnDismissing();
				_currentScreen.gameObject.SetActive(false);
                _soundPlayer.PlaySound(SoundKeys.UI.ScreenChange);
			}

			_currentScreen = destinationScreen;
			_currentScreen.gameObject.SetActive(true);
            _currentScreen.OnPresenting(activationParameter: null);

            if (playDefaultMusic)
            {
                _musicPlayer.PlayScreensSceneMusic();
            }
        }

		public void LoadLevel(int levelNum)
		{
            Assert.IsTrue(
                levelNum <= _dataProvider.LockedInfo.NumOfLevelsUnlocked, 
                "levelNum: " + levelNum + " should be <= than number of levels unlocked: " + _dataProvider.LockedInfo.NumOfLevelsUnlocked);

			_applicationModel.SelectedLevel = levelNum;

            string hint = !_applicationModel.IsTutorial ? _hintProvider.GetHint() : null;
            _sceneNavigator.GoToScene(SceneNames.BATTLE_SCENE, hint);
		}
	}
}
