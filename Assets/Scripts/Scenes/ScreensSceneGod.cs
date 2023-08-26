using BattleCruisers.Data;
using BattleCruisers.Data.Helpers;
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
using BattleCruisers.UI.ScreensScene.Multiplay.ArenaScreen;
using BattleCruisers.UI.ScreensScene.BattleHubScreen;
using BattleCruisers.UI.ScreensScene.SettingsScreen;
using BattleCruisers.UI.ScreensScene.SkirmishScreen;
using BattleCruisers.UI.ScreensScene.TrashScreen;
using BattleCruisers.UI.Sound.AudioSources;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Fetchers.Cache;
using BattleCruisers.Utils.Fetchers.Sprites;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Localization;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.Data.Models.PrefabKeys;
using Unity.Services.Authentication;
using UnityEngine.Localization.Components;
using BattleCruisers.UI;
using BattleCruisers.Network.Multiplay.ApplicationLifecycle;
using BattleCruisers.Network.Multiplay.ConnectionManagement;
using BattleCruisers.Network.Multiplay.Gameplay.UI;
using BattleCruisers.Network.Multiplay.Infrastructure;
using UnityEngine.UI;

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
        private bool _isPlaying;

        public HomeScreenController homeScreen;
        public LevelsScreenController levelsScreen;
        public PostBattleScreenController postBattleScreen;
        public InfiniteLoadoutScreenController loadoutScreen;
        public SettingsScreenController settingsScreen;
        public BattleHubScreensController hubScreen;
        public TrashScreenController trashScreen;
        public TrashTalkDataList trashDataList;
        public ChooseDifficultyScreenController chooseDifficultyScreen;
        public SkirmishScreenController skirmishScreen;
        public AdvertisingBannerScrollingText AdvertisingBanner;
        public FullScreenAdverts fullScreenads;
        public ShopPanelScreenController shopPanelScreen;
        public BlackMarketScreenController blackMarketScreen;
        public MessageBox messageBox;
        public GameObject processingPanel;
        public GameObject environmentArt;
        public GameObject homeScreenArt;

        public Animator thankYouPlane;
        [SerializeField]
        private AudioSource _uiAudioSource;

        [Header("For testing the post battle screen")]
        public bool goToPostBattleScreen = false;
        [Header("For testing the levels screen")]
        public bool testLevelsScreen = false;
        [Range(1, 45)]
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
        public DestructionRanker ranker;

        [SerializeField]
        private CaptainSelectorPanel captainSelectorPanel;

        public GameObject characterOfShop, characterOfBlackmarket;
        public GameObject characterOfCharlie;
        public GameObject cameraOfCharacter;
        public GameObject cameraOfCaptains;
        public Transform ContainerCaptain;

        public static ScreensSceneGod Instance;
        private CaptainExo charlie;
        public bool serverStatus;

        async void Start()
        {

            //Screen.SetResolution(Math.Max(600, Screen.currentResolution.width), Math.Max(400, Screen.currentResolution.height), FullScreenMode.Windowed);
            Helper.AssertIsNotNull(homeScreen, levelsScreen, postBattleScreen, loadoutScreen, settingsScreen, hubScreen, trashScreen, chooseDifficultyScreen, skirmishScreen, trashDataList, _uiAudioSource);
            Helper.AssertIsNotNull(characterOfBlackmarket, characterOfShop, ContainerCaptain);
            Logging.Log(Tags.SCREENS_SCENE_GOD, "START");

            DestroyAllNetworkObjects();
            _applicationModel = ApplicationModelProvider.ApplicationModel;
            _dataProvider = _applicationModel.DataProvider;
            _gameModel = _dataProvider.GameModel;


            ILocTable commonStrings = await LocTableFactory.Instance.LoadCommonTableAsync();
            ILocTable storyStrings = await LocTableFactory.Instance.LoadStoryTableAsync();
            ILocTable screensSceneStrings = await LocTableFactory.Instance.LoadScreensSceneTableAsync();
            IPrefabCacheFactory prefabCacheFactory = new PrefabCacheFactory(commonStrings, _dataProvider);

            Logging.Log(Tags.SCREENS_SCENE_GOD, "Pre prefab cache load");
            IPrefabCache prefabCache = await prefabCacheFactory.CreatePrefabCacheAsync(new PrefabFetcher());
            Logging.Log(Tags.SCREENS_SCENE_GOD, "After prefab cache load");

            // Interacting with Cloud

            bool IsInternetAccessable = await LandingSceneGod.CheckForInternetConnection();
            if (IsInternetAccessable && AuthenticationService.Instance.IsSignedIn)
            {
                try
                {
                    await _dataProvider.LoadBCData();
                    if (_dataProvider.GameModel.NumOfLevelsCompleted >= 10)
                    {
                        // set pvp status in Battle Hub
                        serverStatus = await _dataProvider.RefreshPVPServerStatus();
                        if (serverStatus)
                        {
                            // server available
                            hubScreen.serverStatusPanel.SetActive(false);
                            hubScreen.titleOfBattleButton.gameObject.GetComponent<LocalizeStringEvent>().SetEntry("BattleOnline");
                            hubScreen.battle1vAI.SetActive(false);
                            hubScreen.offlinePlayOnly.SetActive(false);
                            Debug.Log("PVP Server Available.");
                        }
                        else
                        {
                            // server NOT available
                            hubScreen.serverStatusPanel.SetActive(true);
                            hubScreen.battle1vAI.SetActive(true);
                            hubScreen.offlinePlayOnly.SetActive(false);
                            hubScreen.titleOfBattleButton.gameObject.GetComponent<LocalizeStringEvent>().SetEntry("BattleBots");
                            Debug.Log("PVP Server Unavailable.");
                        }
                    }
                    else
                    {
                        hubScreen.serverStatusPanel.SetActive(false);
                        hubScreen.battle1vAI.SetActive(false);
                        hubScreen.offlinePlayOnly.SetActive(true);
                        hubScreen.titleOfBattleButton.gameObject.GetComponent<LocalizeStringEvent>().SetEntry("NotPassed10Levels");
                        hubScreen.battleButton.GetComponent<CanvasGroupButton>().Enabled = false;
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log(ex.Message);
                }
            }
            else
            {
                // turn off server status panel anyway, there is no server to be maintained:
                hubScreen.serverStatusPanel.SetActive(false);
                hubScreen.titleOfBattleButton.gameObject.GetComponent<LocalizeStringEvent>().SetEntry("BattleBots");
                Debug.Log("Offline, can't find out status of PVP Server.");

                // if not Internet Connection or Sign in, we will use local data.
            }

            var prefabFetcher = new PrefabFetcher(); // Must be added before the Initialize call


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
            _isPlaying = false;

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
            ShowCharlieOnMainMenu();

            SpriteFetcher spriteFetcher = new SpriteFetcher();
            IDifficultySpritesProvider difficultySpritesProvider = new DifficultySpritesProvider(spriteFetcher);
            INextLevelHelper nextLevelHelper = new NextLevelHelper(_applicationModel);
            homeScreen.Initialise(this, _soundPlayer, _dataProvider, nextLevelHelper);
            hubScreen.Initialise(this, _soundPlayer, _prefabFactory, _dataProvider, _applicationModel, nextLevelHelper);
            settingsScreen.Initialise(this, _soundPlayer, _dataProvider.SettingsManager, _dataProvider.GameModel.Hotkeys, commonStrings);
            trashScreen.Initialise(this, _soundPlayer, _applicationModel, _prefabFactory, spriteFetcher, trashDataList, _musicPlayer, commonStrings, storyStrings);
            chooseDifficultyScreen.Initialise(this, _soundPlayer, _dataProvider.SettingsManager);
            skirmishScreen.Initialise(this, _applicationModel, _soundPlayer, commonStrings, screensSceneStrings, _prefabFactory);
            shopPanelScreen.Initialise(this, _soundPlayer, _prefabFactory, _dataProvider, nextLevelHelper);
            blackMarketScreen.Initialise(this, _soundPlayer, _prefabFactory, _dataProvider, nextLevelHelper);
            captainSelectorPanel.Initialize(this, _soundPlayer, _prefabFactory, _dataProvider);
            messageBox.gameObject.SetActive(true);
            messageBox.Initialize(_dataProvider, _soundPlayer);
            messageBox.HideMessage();
            characterOfShop.SetActive(false);
            characterOfBlackmarket.SetActive(false);
            processingPanel.SetActive(false);

            processingPanel.GetComponentInChildren<Text>().text = screensSceneStrings.GetString("Processing");

            if (_applicationModel.ShowPostBattleScreen)
            {
                _applicationModel.ShowPostBattleScreen = false;

                Logging.Log(Tags.SCREENS_SCENE_GOD, "Pre go to post battle screen");
                await GoToPostBattleScreenAsync(difficultySpritesProvider, screensSceneStrings);
                fullScreenads.OpenAdvert();//<Aaron> Loads full screen ads after player win a battle
                Logging.Log(Tags.SCREENS_SCENE_GOD, "After go to post battle screen");
            }
            else if (_applicationModel.Mode == GameMode.CoinBattle)
            {
                _applicationModel.ShowPostBattleScreen = false;
                _applicationModel.Mode = GameMode.Campaign;
                fullScreenads.OpenAdvert();
                GotoHubScreen();
            }
            else if (_applicationModel.Mode == GameMode.PvP_1VS1)
            {
                _applicationModel.ShowPostBattleScreen = false;
                _applicationModel.Mode = GameMode.Campaign;
                fullScreenads.OpenAdvert();
                GotoHubScreen();
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
            await InitialiseLevelsScreenAsync(difficultySpritesProvider, nextLevelHelper);
            Logging.Log(Tags.SCREENS_SCENE_GOD, "After initialise levels screen");
            loadoutScreen.Initialise(this, _soundPlayer, _dataProvider, _prefabFactory);

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

            ranker.DisplayRank(_gameModel.LifetimeDestructionScore);

            if (_gameModel.PremiumEdition)
            {
                thankYouPlane.SetTrigger("Play");
                _isPlaying = true;
            }

            _sceneNavigator.SceneLoaded(SceneNames.SCREENS_SCENE);

            if (Instance == null)
                Instance = this;
            Logging.Log(Tags.SCREENS_SCENE_GOD, "END");
        }

        public async void DestroyAllNetworkObjects()
        {
            await Task.Delay(10);
            if (GameObject.Find("ApplicationController") != null)
                GameObject.Find("ApplicationController").GetComponent<ApplicationController>().DestroyNetworkObject();

            if (GameObject.Find("ConnectionManager") != null)
                GameObject.Find("ConnectionManager").GetComponent<ConnectionManager>().DestroyNetworkObject();

            if (GameObject.Find("PopupPanelManager") != null)
                GameObject.Find("PopupPanelManager").GetComponent<PopupManager>().DestroyNetworkObject();

            if (GameObject.Find("UIMessageManager") != null)
                GameObject.Find("UIMessageManager").GetComponent<ConnectionStatusMessageUIManager>().DestroyNetworkObject();

            if (GameObject.Find("UpdateRunner") != null)
                GameObject.Find("UpdateRunner").GetComponent<UpdateRunner>().DestroyNetworkObject();

            if (GameObject.Find("NetworkManager") != null)
                GameObject.Find("NetworkManager").GetComponent<BCNetworkManager>().DestroyNetworkObject();
        }

        void ShowCharlieOnMainMenu()
        {
            if (charlie is not null)
            {
                DestroyImmediate(charlie.gameObject);
                charlie = null;
            }

            charlie = Instantiate(_prefabFactory.GetCaptainExo(_gameModel.PlayerLoadout.CurrentCaptain), ContainerCaptain);
            charlie.gameObject.transform.localScale = Vector3.one * 0.5f;
            characterOfCharlie = charlie.gameObject;
            cameraOfCharacter.SetActive(true);
            cameraOfCaptains.SetActive(false);
        }

        private async Task GoToPostBattleScreenAsync(IDifficultySpritesProvider difficultySpritesProvider, ILocTable screensSceneStrings)
        {
            Assert.IsFalse(postBattleScreen.IsInitialised, "Should only ever navigate (and hence initialise) once");
            await postBattleScreen.InitialiseAsync(this, _soundPlayer, _applicationModel, _prefabFactory, _musicPlayer, difficultySpritesProvider, trashDataList, screensSceneStrings);

            GoToScreen(postBattleScreen, playDefaultMusic: false);
        }

        public void GoToHomeScreen()
        {
            ShowCharlieOnMainMenu();
            characterOfBlackmarket.SetActive(false);
            characterOfShop.SetActive(false);
            characterOfCharlie.SetActive(true);
            cameraOfCharacter.SetActive(true);
            cameraOfCaptains.SetActive(false);
            homeScreenArt.SetActive(true);
            environmentArt.SetActive(true);
            GoToScreen(homeScreen);
            AdvertisingBanner.startAdvert();
        }

        public void GoToLevelsScreen()
        {
            GoToScreen(levelsScreen);
        }


        public void GotoHubScreen()
        {
            characterOfBlackmarket.SetActive(false);
            characterOfShop.SetActive(false);
            characterOfCharlie.SetActive(false);
            cameraOfCharacter.SetActive(false);
            cameraOfCaptains.SetActive(false);
            homeScreenArt.SetActive(false);
            environmentArt.SetActive(false);
            GoToScreen(hubScreen);
        }

        public void GotoShopScreen()
        {
            characterOfBlackmarket.SetActive(false);
            characterOfShop.SetActive(true);
            characterOfCharlie.SetActive(false);
            cameraOfCharacter.SetActive(true);
            cameraOfCaptains.SetActive(true);
            GoToScreen(shopPanelScreen);
            shopPanelScreen.InitiaiseCaptains();
        }

        public void GotoBlackMarketScreen()
        {
            characterOfBlackmarket.SetActive(true);
            characterOfShop.SetActive(false);
            characterOfCharlie.SetActive(false);
            cameraOfCharacter.SetActive(true);
            cameraOfCaptains.SetActive(true);
            GoToScreen(blackMarketScreen);
            blackMarketScreen.InitialiseIAPs();
        }

        private async Task InitialiseLevelsScreenAsync(IDifficultySpritesProvider difficultySpritesProvider, INextLevelHelper nextLevelHelper)
        {
            IList<LevelInfo> levels = CreateLevelInfo(_dataProvider.Levels, _dataProvider.GameModel.CompletedLevels);

            await levelsScreen.InitialiseAsync(
                this,
                _soundPlayer,
                levels,
                testLevelsScreen ? numOfLevelsUnlocked : _dataProvider.LockedInfo.NumOfLevelsUnlocked,
                difficultySpritesProvider,
                trashDataList,
                nextLevelHelper);
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
            characterOfBlackmarket.SetActive(true);
            characterOfShop.SetActive(false);
            characterOfCharlie.SetActive(false);
            cameraOfCharacter.SetActive(false);
            cameraOfCaptains.SetActive(false);
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
            Logging.Log(Tags.SCREENS_SCENE_GOD, $"Game mode: {_applicationModel.Mode}  levelNum: {levelNum}");
            Assert.IsTrue(
                levelNum <= _dataProvider.LockedInfo.NumOfLevelsUnlocked,
                "levelNum: " + levelNum + " should be <= than number of levels unlocked: " + _dataProvider.LockedInfo.NumOfLevelsUnlocked);

            _applicationModel.SelectedLevel = levelNum;

            if (_applicationModel.Mode == GameMode.Campaign)
            {
                if (LevelStages.STAGE_STARTS.Contains(levelNum - 1) && levelToShowCutscene != levelNum)
                {
                    levelToShowCutscene = levelNum;
                    //GoToScreen(trashScreen, playDefaultMusic: false);
                    _sceneNavigator.GoToScene(SceneNames.STAGE_INTERSTITIAL_SCENE, true);
                }
                else
                {
                    levelToShowCutscene = 0;
                    //_musicPlayer.PlayTrashMusic();
                    GoToScreen(trashScreen, playDefaultMusic: false);
                    //_musicPlayer.PlayTrashMusic();

                }

            }
            else if (_applicationModel.Mode == GameMode.CoinBattle)
            {
                levelToShowCutscene = 0;
                GoToScreen(trashScreen, playDefaultMusic: false);
            }
            else
            {
                LoadBattleScene();
            }
        }

        public void GoStraightToTrashScreen(int levelNum)
        {
            AdvertisingBanner.stopAdvert();
            Logging.Log(Tags.SCREENS_SCENE_GOD, $"Game mode: {_applicationModel.Mode}  levelNum: {levelNum}");
            Assert.IsTrue(
                levelNum <= _dataProvider.LockedInfo.NumOfLevelsUnlocked,
                "levelNum: " + levelNum + " should be <= than number of levels unlocked: " + _dataProvider.LockedInfo.NumOfLevelsUnlocked);

            _applicationModel.SelectedLevel = levelNum;

            if (_applicationModel.Mode == GameMode.Campaign || _applicationModel.Mode == GameMode.CoinBattle)
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

        public void LoadBattleScene()
        {
            AdvertisingBanner.stopAdvert();
            _sceneNavigator.GoToScene(SceneNames.BATTLE_SCENE, true);
            CleanUp();
        }

        public void LoadCreditsScene()
        {
            _sceneNavigator.GoToScene(SceneNames.CREDITS_SCENE, true);
            CleanUp();
        }

        public void LoadCutsceneScene()
        {
            _sceneNavigator.GoToScene(SceneNames.CUTSCENE_SCENE, true);
            CleanUp();
        }

        public void LoadPvPBattleScene()
        {
            AdvertisingBanner.stopAdvert();
            _sceneNavigator.GoToScene(SceneNames.PvP_BOOT_SCENE, true);
            CleanUp();
        }

        public void LoadBattle1v1Mode()
        {

        }

        public CaptainExo GetCaptainExoData(CaptainExoKey key)
        {
            var prefabPath = key.PrefabPath;
            var prefab = Resources.Load<GameObject>(prefabPath);
            if (prefab == null)
            {
                Debug.LogError($"Cannot find prefab at path: {prefabPath}");
                return null;
            }

            var data = prefab.GetComponent<CaptainExo>();
            if (data == null)
            {
                Debug.LogError($"No CaptainExoData component attached to prefab at path: {prefabPath}");
            }

            return data;
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
    }
}
