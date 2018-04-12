using System.Collections;
using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.UI.ScreensScene;
using BattleCruisers.UI.ScreensScene.LevelsScreen;
using BattleCruisers.UI.ScreensScene.LoadoutScreen;
using BattleCruisers.UI.ScreensScene.PostBattleScreen;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes
{
    public class ScreensSceneGod : MonoBehaviour, IScreensSceneGod
	{
		private PrefabFactory _prefabFactory;
		private ScreenController _currentScreen;
		private IDataProvider _dataProvider;
		private IGameModel _gameModel;
        private ISpriteProvider _spriteProvider;
        private ISceneNavigator _sceneNavigator;

		public HomeScreenController homeScreen;
		public LevelsScreenController levelsScreen;
		public PostBattleScreenController postBattleScreen;
		public LoadoutScreenController loadoutScreen;
        public SettingsScreenController settingsScreen;

		void Start()
		{
            Helper.AssertIsNotNull(homeScreen, levelsScreen, postBattleScreen, loadoutScreen, settingsScreen);

			_prefabFactory = new PrefabFactory(new PrefabFetcher());
			_dataProvider = ApplicationModel.DataProvider;
			_gameModel = _dataProvider.GameModel;
            _spriteProvider = new SpriteProvider(new SpriteFetcher());
            _sceneNavigator = LandingSceneGod.SceneNavigator;


            // TEMP  For showing PostBattleScreen :)
   //         _gameModel.LastBattleResult = new BattleResult(1, true);
			//ApplicationModel.ShowPostBattleScreen = true;


			homeScreen.Initialise(this, _gameModel.LastBattleResult, _dataProvider.Levels.Count);
            settingsScreen.Initialise(this, _dataProvider.SettingsManager);


            if (ApplicationModel.ShowPostBattleScreen)
            {
				ApplicationModel.ShowPostBattleScreen = false;
    
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
            postBattleScreen.Initialise(this, _dataProvider, _prefabFactory, _spriteProvider);

            GoToScreen(postBattleScreen);
        }

		public void GoToLevelsScreen()
		{
            // Laziliy initalise, because post battle screen can change the number of levels unlocked
            if (!levelsScreen.IsInitialised)
            {
                BattleResult lastBattleResult = _dataProvider.GameModel.LastBattleResult;
                int lastPlayedLevel = lastBattleResult != null ? lastBattleResult.LevelNum : 0;

                levelsScreen.Initialise(this, _dataProvider.Levels, _dataProvider.LockedInfo.NumOfLevelsUnlocked, lastPlayedLevel);
            }

			GoToScreen(levelsScreen);
		}

		public void GoToHomeScreen()
		{
			GoToScreen(homeScreen);
		}

		public void GoToLoadoutScreen()
		{
            // Laziliy initalise, because post battle screen can change the loadout
            if (!loadoutScreen.IsInitialised)
            {
                // TEMP  For starting ScreensScene without previous LandingScene.
                // So I can test the ScreensScene without having to go through
                // the LandingScene each time :P
				IEnumerator initialiseLoadout = loadoutScreen.Initialise(this, _dataProvider, _prefabFactory, _spriteProvider);

                if (LandingSceneGod.LoadingScreen != null)
                {
					StartCoroutine(LandingSceneGod.LoadingScreen.PerformLongOperation(initialiseLoadout));
                }
                else
                {
                    StartCoroutine(initialiseLoadout);
                }
            }

			GoToScreen(loadoutScreen);
		}

        public void GoToSettingsScreen()
        {
            GoToScreen(settingsScreen);
        }

		public void LoadLevel(int levelNum)
		{
            Assert.IsTrue(levelNum <= _dataProvider.LockedInfo.NumOfLevelsUnlocked);

			ApplicationModel.SelectedLevel = levelNum;
            _sceneNavigator.GoToScene(SceneNames.BATTLE_SCENE);
		}

		private void GoToScreen(ScreenController destinationScreen)
		{
			Assert.AreNotEqual(_currentScreen, destinationScreen);

			if (_currentScreen != null)
			{
				_currentScreen.gameObject.SetActive(false);
			}

			_currentScreen = destinationScreen;
			_currentScreen.gameObject.SetActive(true);
		}
	}
}
