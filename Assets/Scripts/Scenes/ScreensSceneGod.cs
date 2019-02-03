using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Settings;
using BattleCruisers.UI.Music;
using BattleCruisers.UI.ScreensScene;
using BattleCruisers.UI.ScreensScene.HomeScreen;
using BattleCruisers.UI.ScreensScene.LevelsScreen;
using BattleCruisers.UI.ScreensScene.LoadoutScreen;
using BattleCruisers.UI.ScreensScene.PostBattleScreen;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using NSubstitute;
using System.Collections;
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

		public HomeScreenController homeScreen;
		public LevelsScreenController levelsScreen;
		public PostBattleScreenController postBattleScreen;
		public LoadoutScreenController loadoutScreen;
        public SettingsScreenController settingsScreen;

		void Start()
		{
            Helper.AssertIsNotNull(homeScreen, levelsScreen, postBattleScreen, loadoutScreen, settingsScreen);

			_prefabFactory = new PrefabFactory(new PrefabFetcher());
            _applicationModel = ApplicationModelProvider.ApplicationModel;
			_dataProvider = _applicationModel.DataProvider;
			_gameModel = _dataProvider.GameModel;
            _sceneNavigator = LandingSceneGod.SceneNavigator;
            _musicPlayer = LandingSceneGod.MusicPlayer;


            // TEMP  For showing PostBattleScreen :)
            //_gameModel.LastBattleResult = new BattleResult(1, wasVictory: true);
            ////_gameModel.LastBattleResult = new BattleResult(1, wasVictory: false);
            //_applicationModel.ShowPostBattleScreen = true;


            // TEMP  For when not coming from LandingScene :)
            if (_musicPlayer == null)
            {
                _musicPlayer = Substitute.For<IMusicPlayer>();
            }


            _musicPlayer.PlayScreensSceneMusic();
            homeScreen.Initialise(this, _gameModel, _dataProvider.Levels.Count);
            settingsScreen.Initialise(this, _dataProvider.SettingsManager);


            if (_applicationModel.ShowPostBattleScreen)
            {
				_applicationModel.ShowPostBattleScreen = false;
    
                GoToPostBattleScreen();
            }
            else
            {
                GoToHomeScreen();
            }


            // TEMP  Go to specific screen :)
            //GoToSettingsScreen();
            //GoToLevelsScreen();
            //GoToLoadoutScreen();
        }
        
        private void GoToPostBattleScreen()
        {
            Assert.IsFalse(postBattleScreen.IsInitialised, "Should only ever navigate (and hence initialise) once");
            postBattleScreen.Initialise(this, _dataProvider, _prefabFactory, _musicPlayer);

            GoToScreen(postBattleScreen, playDefaultMusic: false);
        }

		public void GoToLevelsScreen()
		{
            // Laziliy initalise, because post battle screen can change the number of levels unlocked
            if (!levelsScreen.IsInitialised)
            {
                BattleResult lastBattleResult = _dataProvider.GameModel.LastBattleResult;
                int lastPlayedLevel = lastBattleResult != null ? lastBattleResult.LevelNum : 0;

                IList<LevelInfo> levels = CreateLevelInfo(_dataProvider.Levels, _dataProvider.GameModel.CompletedLevels);

                levelsScreen.Initialise(this, levels, _dataProvider.LockedInfo.NumOfLevelsUnlocked, lastPlayedLevel);
            }

			GoToScreen(levelsScreen);
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

        public void GoToHomeScreen()
		{
			GoToScreen(homeScreen);
		}

		public void GoToLoadoutScreen()
		{
            StartCoroutine(GotToLoadoutScreenAsync());
        }

        private IEnumerator GotToLoadoutScreenAsync()
        {
            // Laziliy initalise, because post battle screen can change the loadout
            if (!loadoutScreen.IsInitialised)
            {
                // TEMP  For starting ScreensScene without previous LandingScene.
                // So I can test the ScreensScene without having to go through
                // the LandingScene each time :P
                // => Should be able to remove if els, and just keep if content
                IEnumerator initialiseLoadout = loadoutScreen.Initialise(this, _dataProvider, _prefabFactory);

                if (LandingSceneGod.LoadingScreen != null)
                {
                    yield return StartCoroutine(LandingSceneGod.LoadingScreen.PerformLongOperation(initialiseLoadout));
                }
                else
                {
                    yield return StartCoroutine(initialiseLoadout);
                }
            }

            yield return GoToScreenAsync(loadoutScreen);
        }

        public void GoToSettingsScreen()
        {
            GoToScreen(settingsScreen);
        }

		public void LoadLevel(int levelNum)
		{
            Assert.IsTrue(
                levelNum <= _dataProvider.LockedInfo.NumOfLevelsUnlocked, 
                "levelNum: " + levelNum + " should be <= than number of levels unlocked: " + _dataProvider.LockedInfo.NumOfLevelsUnlocked);

			_applicationModel.SelectedLevel = levelNum;
            _sceneNavigator.GoToScene(SceneNames.BATTLE_SCENE);
		}

        private IEnumerator GoToScreenAsync(ScreenController destinationScreen)
        {
            yield return null;
            GoToScreen(destinationScreen);
        }

		private void GoToScreen(ScreenController destinationScreen, bool playDefaultMusic = true)
		{
			Assert.AreNotEqual(_currentScreen, destinationScreen);

			if (_currentScreen != null)
			{
                _currentScreen.OnDismissing();
				_currentScreen.gameObject.SetActive(false);
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
