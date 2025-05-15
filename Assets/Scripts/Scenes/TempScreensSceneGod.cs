using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Settings;
using BattleCruisers.Data.Static;
using BattleCruisers.PostBattleScreen;
using BattleCruisers.UI.Music;
using BattleCruisers.UI.ScreensScene;
using BattleCruisers.UI.ScreensScene.ChooseDifficultyScreen;
using BattleCruisers.UI.ScreensScene.HomeScreen;
using BattleCruisers.UI.ScreensScene.LevelsScreen;
using BattleCruisers.UI.ScreensScene.LoadoutScreen;
using BattleCruisers.UI.ScreensScene.PostBattleScreen;
using BattleCruisers.UI.ScreensScene.SettingsScreen;
using BattleCruisers.UI.ScreensScene.TrashScreen;
using BattleCruisers.UI.Sound.AudioSources;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Fetchers.Cache;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using BattleCruisers.UI.ScreensScene.BattleHubScreen;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.Scenes.BattleScene;
using BattleCruisers.Tutorial;
using BattleCruisers.UI.BattleScene.Navigation;

namespace BattleCruisers.Scenes
{
    public class TempScreensSceneGod : MonoBehaviour, IScreensSceneGod
    {
        private ScreenController _currentScreen;
        private GameModel _gameModel;
        private IMusicPlayer _musicPlayer;
        private SingleSoundPlayer _soundPlayer;
        private bool _isPlaying;
        private readonly IBattleSceneHelper _battleSceneHelper;
        private ITutorialProvider _tutorialProvider;
        private BattleSceneGodComponents components;
        private NavigationPermitters navigationPermitters;
        private IBattleSceneHelper helper;

        public HomeScreenController homeScreen;
        public LevelsScreenController levelsScreen;
        public PostBattleScreenController postBattleScreen;
        public InfiniteLoadoutScreenController loadoutScreen;
        public SettingsScreenController settingsScreen;
        public BattleHubScreensController hubScreen;
        public TrashScreenController trashScreen;
        public TrashTalkDataList levelTrashDataList, sideQuestTrashDataList;
        public ChooseDifficultyScreenController chooseDifficultyScreen;
        public SkirmishScreenController skirmishScreen;
        public AdvertisingBannerScrollingText AdvertisingBanner;
        public FullScreenAdverts fullScreenads;
        public ShopPanelScreenController shopPanelScreen;

        public Animator thankYouPlane;
        [SerializeField]
        private AudioSource _uiAudioSource;

        [Header("For testing the post battle screen")]
        public bool goToPostBattleScreen = false;
        [Header("For testing the levels screen")]
        public bool testLevelsScreen = false;
        [Range(1, 25)]
        public int numOfLevelsUnlocked = 1;
        [Header("For testing the trash talk screen")]
        public bool testTrashTalkScreen = false;
        [Header("For testing the settings screen")]
        public bool testSettingsScreen = false;
        [Header("For testing the choose difficulty screen")]
        public bool testDifficultyScreen = false;
        [Header("For testing the skirmish screen")]
        public bool testSkirmishScreen = false;
        [Header("For testing the loadout screen")]
        public bool testLoadoutScreen = false;
        [Header("For testing Shop screen")]
        public bool testShopScreen = false;
        public DestructionRanker ranker;
        [SerializeField]
        private Sprite[] difficultyIndicators;

        async void Start()
        {
            //Screen.SetResolution(Math.Max(600, Screen.currentResolution.width), Math.Max(400, Screen.currentResolution.height), FullScreenMode.Windowed);
            Helper.AssertIsNotNull(homeScreen, levelsScreen, postBattleScreen, loadoutScreen, settingsScreen, trashScreen, chooseDifficultyScreen, skirmishScreen, levelTrashDataList, sideQuestTrashDataList, _uiAudioSource);
            Logging.Log(Tags.SCREENS_SCENE_GOD, "START");


            Logging.Log(Tags.SCREENS_SCENE_GOD, "Pre prefab cache load");
            await PrefabCache.CreatePrefabCacheAsync();
            Logging.Log(Tags.SCREENS_SCENE_GOD, "After prefab cache load");

            _gameModel = DataProvider.GameModel;
            _musicPlayer = LandingSceneGod.MusicPlayer;
            _soundPlayer
                = new SingleSoundPlayer(
                    new EffectVolumeAudioSource(
                        new AudioSourceBC(_uiAudioSource), 1));

            levelTrashDataList.Initialise();
            sideQuestTrashDataList.Initialise();

            // TEMP  For showing PostBattleScreen :)
            if (goToPostBattleScreen)
            {
                _gameModel.LastBattleResult = new BattleResult(1, wasVictory: true);
                //_gameModel.LastBattleResult = new BattleResult(1, wasVictory: false);
                ApplicationModel.ShowPostBattleScreen = true;
                //ApplicationModel.IsTutorial = true;
            }

            // TEMP  For when not coming from LandingScene :)
            if (_musicPlayer == null)
            {
                _musicPlayer = Substitute.For<IMusicPlayer>();
            }

            homeScreen.Initialise(this, _soundPlayer);
            hubScreen.Initialise(this, _soundPlayer);
            settingsScreen.Initialise(this, _soundPlayer, DataProvider.SettingsManager, DataProvider.GameModel.Hotkeys);
            trashScreen.Initialise(this, _soundPlayer, levelTrashDataList, sideQuestTrashDataList, _musicPlayer);
            chooseDifficultyScreen.Initialise(this, _soundPlayer, DataProvider.SettingsManager);
            skirmishScreen.Initialise(this, _soundPlayer);
            shopPanelScreen.Initialise(this, _soundPlayer);

            if (ApplicationModel.ShowPostBattleScreen)
            {
                ApplicationModel.ShowPostBattleScreen = false;

                Logging.Log(Tags.SCREENS_SCENE_GOD, "Pre go to post battle screen");
                await GoToPostBattleScreenAsync();
                fullScreenads.OpenAdvert();//<Aaron> Loads full screen adds after player win a battle
                Logging.Log(Tags.SCREENS_SCENE_GOD, "After go to post battle screen");
            }
            else if (levelToShowCutscene == 0)
            {
                GoToHomeScreen();
            }
            else
            {
                GoToTrashScreen(levelToShowCutscene);
            }

            // After potentially initialising post battle screen, because that can modify the data model.
            Logging.Log(Tags.SCREENS_SCENE_GOD, "Pre initialise levels screen");
            await InitialiseLevelsScreenAsync();
            Logging.Log(Tags.SCREENS_SCENE_GOD, "After initialise levels screen");
            loadoutScreen.Initialise(this, _soundPlayer);

            // TEMP  Go to specific screen :)
            //GoToLoadoutScreen();

            if (testSettingsScreen)
            {
                GoToSettingsScreen();
            }
            else if (testLevelsScreen)
            {
                GoToLevelsScreen();
            }
            else if (testTrashTalkScreen)
            {
                GoToTrashScreen(levelNum: 1);
            }
            else if (testDifficultyScreen)
            {
                GoToChooseDifficultyScreen();
            }
            else if (testSkirmishScreen)
            {
                GoToSkirmishScreen();
            }
            else if (testLoadoutScreen)
            {
                GoToLoadoutScreen();
            }
            else if (testShopScreen)
            {
                GotoShopScreen();
            }

            ranker.DisplayRank(_gameModel.LifetimeDestructionScore);

            SceneNavigator.SceneLoaded(SceneNames.SCREENS_SCENE);

            if (_gameModel.PremiumEdition)
            {
                thankYouPlane.SetTrigger("Play");
                _isPlaying = true;
            }

            Logging.Log(Tags.SCREENS_SCENE_GOD, "END");
        }


        private void OnEnable()
        {
            //LandingSceneGod.SceneNavigator.SceneLoaded(SceneNames.SCREENS_SCENE);
        }
        private async Task GoToPostBattleScreenAsync()
        {
            Assert.IsFalse(postBattleScreen.IsInitialised, "Should only ever navigate (and hence initialise) once");
            await postBattleScreen.InitialiseAsync(this, _soundPlayer, _musicPlayer, difficultyIndicators, levelTrashDataList, sideQuestTrashDataList);

            GoToScreen(postBattleScreen, playDefaultMusic: false);
        }

        public void GoToHomeScreen()
        {
            GoToScreen(homeScreen);
            AdvertisingBanner.startAdvert();
            fullScreenads.OpenAdvert();//<Aaron> To test the full screen ads
        }

        public void GoToLevelsScreen()
        {
            GoToScreen(levelsScreen);
        }


        public void GotoHubScreen()
        {
            GoToScreen(hubScreen);
        }

        public void GoToBodykitsShop()
        {
            GotoShopScreen();
            shopPanelScreen.InitialiseBodykits();
        }

        public void GotoShopScreen()
        {
            GoToScreen(shopPanelScreen);
        }

        public void GotoBlackMarketScreen()
        {
            //
        }

        private async Task InitialiseLevelsScreenAsync()
        {
            IList<LevelInfo> levels = CreateLevelInfo(StaticData.Levels, DataProvider.GameModel.CompletedLevels);

            await levelsScreen.InitialiseAsync(
                this,
                _soundPlayer,
                levels,
                testLevelsScreen ? numOfLevelsUnlocked : DataProvider.LockedInfo.NumOfLevelsUnlocked,
                difficultyIndicators,
                levelTrashDataList);
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

                levels.Add(new LevelInfo(staticLevel.Num, completedDifficulty));
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

        private static int levelToShowCutscene = 0;
        public void GoToTrashScreen(int levelNum)
        {
            AdvertisingBanner.stopAdvert();
            Logging.Log(Tags.SCREENS_SCENE_GOD, $"Game mode: {ApplicationModel.Mode}  levelNum: {levelNum}");
            Assert.IsTrue(
                levelNum <= DataProvider.LockedInfo.NumOfLevelsUnlocked,
                "levelNum: " + levelNum + " should be <= than number of levels unlocked: " + DataProvider.LockedInfo.NumOfLevelsUnlocked);

            ApplicationModel.SelectedLevel = levelNum;

            if (ApplicationModel.Mode == GameMode.Campaign)
            {
                if (LevelStages.STAGE_STARTS.Contains(levelNum - 1) && levelToShowCutscene != levelNum)
                {
                    levelToShowCutscene = levelNum;
                    //GoToScreen(trashScreen, playDefaultMusic: false);
                    SceneNavigator.GoToScene(SceneNames.STAGE_INTERSTITIAL_SCENE, true);
                }
                else
                {
                    levelToShowCutscene = 0;
                    //_musicPlayer.PlayTrashMusic();
                    GoToScreen(trashScreen, playDefaultMusic: false);
                    //_musicPlayer.PlayTrashMusic();

                }

            }
            else
            {
                LoadBattleScene();
            }
        }

        public void GoStraightToTrashScreen(int levelNum)
        {
            AdvertisingBanner.stopAdvert();
            Logging.Log(Tags.SCREENS_SCENE_GOD, $"Game mode: {ApplicationModel.Mode}  levelNum: {levelNum}");
            Assert.IsTrue(
                levelNum <= DataProvider.LockedInfo.NumOfLevelsUnlocked,
                "levelNum: " + levelNum + " should be <= than number of levels unlocked: " + DataProvider.LockedInfo.NumOfLevelsUnlocked);

            ApplicationModel.SelectedLevel = levelNum;

            if (ApplicationModel.Mode == GameMode.Campaign)
            {

                levelToShowCutscene = 0;
                GoToScreen(trashScreen, playDefaultMusic: false);
            }
            else
            {
                LoadBattleScene();
            }
        }

        public void GoToChooseDifficultyScreen()
        {
            GoToScreen(chooseDifficultyScreen);
        }

        public void GoToSkirmishScreen()
        {
            GoToScreen(skirmishScreen);
        }

        private void GoToScreen(ScreenController destinationScreen, bool playDefaultMusic = true)
        {
            AdvertisingBanner.stopAdvert();

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

        public void LoadBattleSceneSideQuest(int sideQuestID)
        {
            ApplicationModel.SelectedSideQuestID = sideQuestID;
            ApplicationModel.Mode = GameMode.SideQuest;
            AdvertisingBanner.stopAdvert();
            SceneNavigator.GoToScene(SceneNames.BATTLE_SCENE, true);
            CleanUp();
        }

        public void LoadBattleScene()
        {
            AdvertisingBanner.stopAdvert();
            SceneNavigator.GoToScene(SceneNames.BATTLE_SCENE, true);
            CleanUp();
        }

        public void LoadCreditsScene()
        {
            SceneNavigator.GoToScene(SceneNames.CREDITS_SCENE, true);
            CleanUp();
        }

        public void LoadCutsceneScene()
        {
            SceneNavigator.GoToScene(SceneNames.CUTSCENE_SCENE, true);
            CleanUp();
        }

        public void LoadPvPBattleScene()
        {
            AdvertisingBanner.stopAdvert();
            //   _sceneNavigator.GoToScene(SceneNames.MULTIPLAY_SCREENS_SCENE, true);
            CleanUp();
        }

        public void LoadBattle1v1Mode()
        {

        }

        public void ShowNewsPanel()
        {
            throw new Exception("ShowNewsPanel method is not implemented");
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

            if (_gameModel != null)
            {
                if (_gameModel.PremiumEdition)
                {
                    if (!_isPlaying)
                    {
                        thankYouPlane.SetTrigger("Play");
                    }
                }
            }
        }
        public void SomeMethodUsingBattleSceneHelper()
        {
            if (_battleSceneHelper != null)
            {
                // Here you can use methods from IBattleSceneHelper
                ILevel currentLevel = helper.GetLevel();

                // Example usage:
                Debug.Log($"Current level: {currentLevel}");
            }
            else
            {
                Debug.LogWarning("BattleSceneHelper is not set!");
            }
        }

        public void GoToSideQuestTrashScreen(int sideQuestLevelNum, int firstLevelOfStage = -1)
        {
            // Implementation similar to GoToTrashScreen method, but for side quest levels
            AdvertisingBanner.stopAdvert();
            Logging.Log(Tags.SCREENS_SCENE_GOD, $"Game mode: {ApplicationModel.Mode}  levelNum: {sideQuestLevelNum}");
            Assert.IsTrue(
                sideQuestLevelNum <= DataProvider.LockedInfo.NumOfLevelsUnlocked,
                "levelNum: " + sideQuestLevelNum + " should be <= than number of levels unlocked: " + DataProvider.LockedInfo.NumOfLevelsUnlocked);

            ApplicationModel.SelectedLevel = sideQuestLevelNum;

            if (ApplicationModel.Mode == GameMode.Campaign)
            {
                if (LevelStages.STAGE_STARTS.Contains(sideQuestLevelNum - 1) && levelToShowCutscene != sideQuestLevelNum)
                {
                    levelToShowCutscene = sideQuestLevelNum;
                    DataProvider.GameModel.ID_Bodykit_AIbot = -1;
                    DataProvider.SaveGame();
                    SceneNavigator.GoToScene(SceneNames.STAGE_INTERSTITIAL_SCENE, true);
                }
                else
                {
                    levelToShowCutscene = 0;
                    DataProvider.GameModel.ID_Bodykit_AIbot = -1;
                    DataProvider.SaveGame();
                    GoToScreen(trashScreen, playDefaultMusic: false);
                }
            }
            else if (ApplicationModel.Mode == GameMode.CoinBattle)
            {
                levelToShowCutscene = 0;
                // Random bodykits for AIBot
                ILevel level = StaticData.Levels[sideQuestLevelNum - 1];
                DataProvider.GameModel.ID_Bodykit_AIbot = UnityEngine.Random.Range(0, 5) == 2 ? GetRandomBodykitForAI(GetHullType(level.Hull.PrefabName)) : -1;
                DataProvider.SaveGame();
                GoToScreen(trashScreen, playDefaultMusic: false);
            }
            else
            {
                LoadBattleScene();
            }
        }
        private int GetRandomBodykitForAI(HullType hullType)
        {
            int id_bodykit = -1;
            if (hullType != HullType.None)
            {
                List<int> bodykits = new List<int>();
                for (int i = 0; i < /*12*/ StaticData.Bodykits.Count; i++)
                {
                    if (PrefabFactory.GetBodykit(StaticPrefabKeys.BodyKits.GetBodykitKey(i)).cruiserType == hullType)
                    {
                        bodykits.Add(i);
                    }
                }
                if (bodykits.Count == 0)
                    id_bodykit = -1;
                else
                    id_bodykit = bodykits[UnityEngine.Random.Range(0, bodykits.Count)];
            }
            return id_bodykit;
        }
        private HullType GetHullType(string hullName)
        {
            switch (hullName)
            {
                case "Trident":
                    return HullType.Trident;
                case "BlackRig":
                    return HullType.BlackRig;
                case "Bullshark":
                    return HullType.Bullshark;
                case "Eagle":
                    return HullType.Eagle;
                case "Goatherd":
                    return HullType.Goatherd;
                case "Hammerhead":
                    return HullType.Hammerhead;
                case "Longbow":
                    return HullType.Longbow;
                case "Megalodon":
                    return HullType.Megalodon;
                case "Raptor":
                    return HullType.Raptor;
                case "Rickshaw":
                    return HullType.Rickshaw;
                case "Rockjaw":
                    return HullType.Rockjaw;
                case "TasDevil":
                    return HullType.TasDevil;
                case "Yeti":
                    return HullType.Yeti;
            }
            return HullType.None;
        }
    }
}
