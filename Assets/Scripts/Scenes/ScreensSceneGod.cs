using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.UI.ScreensScene;
using BattleCruisers.UI.ScreensScene.LevelsScreen;
using BattleCruisers.UI.ScreensScene.LoadoutScreen;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace BattleCruisers.Scenes
{
	public class ScreensSceneGod : MonoBehaviour, IScreensSceneGod
	{
		private PrefabFactory _prefabFactory;
		private ScreenController _currentScreen;
		private IDataProvider _dataProvider;
		private IGameModel _gameModel;
        private ISpriteProvider _spriteProvider;

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


            // TEMP  For showing PostBattleScreen :)
            _gameModel.LastBattleResult = new BattleResult(1, true);
			ApplicationModel.ShowPostBattleScreen = true;


            BattleResult lastBattleResult = _dataProvider.GameModel.LastBattleResult;
            int lastPlayedLevel = lastBattleResult != null ? lastBattleResult.LevelNum : 0;

            levelsScreen.Initialise(this, _dataProvider.Levels, _dataProvider.NumOfLevelsUnlocked, lastPlayedLevel);
			homeScreen.Initialise(this, _gameModel.LastBattleResult, _dataProvider.Levels.Count);
            loadoutScreen.Initialise(this, _dataProvider, _prefabFactory, _spriteProvider);
            settingsScreen.Initialise(this, _dataProvider.SettingsManager);


            if (ApplicationModel.ShowPostBattleScreen)
            {
                ApplicationModel.ShowPostBattleScreen = false;
                postBattleScreen.Initialise(_gameModel.LastBattleResult, this, _dataProvider.NumOfLevelsUnlocked, _spriteProvider);
                GoToScreen(postBattleScreen);
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
		
		public void GoToLevelsScreen()
		{
			GoToScreen(levelsScreen);
		}

		public void GoToHomeScreen()
		{
			GoToScreen(homeScreen);
		}

		public void GoToLoadoutScreen()
		{
			GoToScreen(loadoutScreen);
		}

        public void GoToSettingsScreen()
        {
            GoToScreen(settingsScreen);
        }

		public void LoadLevel(int levelNum)
		{
			Assert.IsTrue(levelNum <= _dataProvider.NumOfLevelsUnlocked);

			ApplicationModel.SelectedLevel = levelNum;
			SceneManager.LoadScene(SceneNames.BATTLE_SCENE);
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
