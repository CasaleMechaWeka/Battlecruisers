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
using BattleCruisers.UI.ScreensScene.BattleHubScreen;
using BattleCruisers.UI.ScreensScene.SettingsScreen;
using BattleCruisers.UI.ScreensScene.SkirmishScreen;
using BattleCruisers.UI.ScreensScene.TrashScreen;
using BattleCruisers.UI.Sound.AudioSources;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Fetchers.Cache;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
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
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene;

namespace BattleCruisers.Scenes
{
    public class ScreensSceneGod : MonoBehaviour, IScreensSceneGod
    {
        public IPrefabFactory _prefabFactory;
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
        public TrashTalkDataList levelTrashDataList;
        public TrashTalkDataList sideQuestTrashDataList;
        public ChooseDifficultyScreenController chooseDifficultyScreen;
        public SkirmishScreenController skirmishScreen;
        public AdvertisingBannerScrollingText AdvertisingBanner;
        public FullScreenAdverts fullScreenads;
        public ShopPanelScreenController shopPanelScreen;
        public BlackMarketScreenController blackMarketScreen;
        public MessageBox messageBox;
        public MessageBoxBig messageBoxBig;
        public CanvasGroupButton premiumEditionButton;
        public GameObject processingPanel;
        public GameObject environmentArt;
        public GameObject homeScreenArt;
        public IAPPremiumConfirmation premiumConfirmationScreen;

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
        public string requiredVer; // App version from Cloud;
        private static bool IsFirstTimeLoad = true;
        IPrefabCache _prefabCache;
        [SerializeField]
        private Sprite[] difficultyIndicators;

        async void Start()
        {
            if (Instance == null)
                Instance = this;

            //Screen.SetResolution(Math.Max(600, Screen.currentResolution.width), Math.Max(400, Screen.currentResolution.height), FullScreenMode.Windowed);
            Helper.AssertIsNotNull(homeScreen, levelsScreen, postBattleScreen, loadoutScreen, settingsScreen, hubScreen, trashScreen, chooseDifficultyScreen, skirmishScreen, levelTrashDataList, sideQuestTrashDataList, _uiAudioSource);
            Helper.AssertIsNotNull(characterOfBlackmarket, characterOfShop, ContainerCaptain);
            Helper.AssertIsNotNull(premiumEditionButton);
            Logging.Log(Tags.SCREENS_SCENE_GOD, "START");

            DestroyAllNetworkObjects();
            _applicationModel = ApplicationModelProvider.ApplicationModel;
            _dataProvider = _applicationModel.DataProvider;
            _gameModel = _dataProvider.GameModel;
            //components = GetComponent<ScreensSceneGodCompoments>();

            Task<bool> checkInternetConnection = LandingSceneGod.CheckForInternetConnection();
            ILocTable commonStrings = LandingSceneGod.Instance.commonStrings;
            ILocTable screensSceneStrings = LandingSceneGod.Instance.screenSceneStrings;
            Task<ILocTable> loadStoryStrings = LocTableFactory.Instance.LoadStoryTableAsync();

            IPrefabCacheFactory prefabCacheFactory = new PrefabCacheFactory(commonStrings, _dataProvider);
            IPrefabFetcher prefabFetcher = new PrefabFetcher(); // Must be added before the Initialize call

            Logging.Log(Tags.SCREENS_SCENE_GOD, "Pre prefab cache load");
            Task<IPrefabCache> loadPrefabCache = prefabCacheFactory.CreatePrefabCacheAsync(new PrefabFetcher());
            Logging.Log(Tags.SCREENS_SCENE_GOD, "After prefab cache load");

            premiumEditionButton.gameObject.SetActive(false);

            // Interacting with Cloud
            bool IsInternetAccessable = false;
            if (IsFirstTimeLoad)
            {
                IsInternetAccessable = LandingSceneGod.Instance.InternetConnectivity.Value;
            }
            else
            {
                IsInternetAccessable = await checkInternetConnection;
            }

            if (IsInternetAccessable && AuthenticationService.Instance.IsSignedIn)
            {
                try
                {
                    Task refreshEcoConfig = _dataProvider.RefreshEconomyConfiguration();
                    if (IsFirstTimeLoad)
                    {
                        await _dataProvider.CloudLoad();
                        IsFirstTimeLoad = false;
                    }

                    await refreshEcoConfig;
                    await _dataProvider.ApplyRemoteConfig();

                    // local transactions syncing:
                    if (_dataProvider.GameModel.OutstandingCaptainTransactions != null &&
                        _dataProvider.GameModel.OutstandingCaptainTransactions.Count > 0 ||
                        _dataProvider.GameModel.OutstandingHeckleTransactions != null &&
                        _dataProvider.GameModel.OutstandingHeckleTransactions.Count > 0 ||
                        _dataProvider.GameModel.OutstandingBodykitTransactions != null &&
                        _dataProvider.GameModel.OutstandingBodykitTransactions.Count > 0 ||
                        _dataProvider.GameModel.OutstandingVariantTransactions != null &&
                        _dataProvider.GameModel.OutstandingVariantTransactions.Count > 0 ||
                        _dataProvider.GameModel.CoinsChange > 0 ||
                        _dataProvider.GameModel.CreditsChange > 0)
                    {
                        Debug.Log("Processing offline shop purchases and currency changes.");
                        await _dataProvider.ProcessOfflineTransactions();
                        PlayerInfoPanelController.Instance.UpdateInfo(_dataProvider, _prefabFactory);
                    }

                    // version check
                    string currentVersion = Application.version;
                    requiredVer = _dataProvider.GetPVPVersion();
                    Debug.Log("Application Version: " + currentVersion);
                    Debug.Log("DataProvider Version: " + requiredVer);

#if DISABLE_MATCHMAKING
                    // When matchmaking is disabled, show offline-only UI regardless of version or server status
                    hubScreen.serverStatusPanel.SetActive(false);
                    hubScreen.offlinePlayOnly.SetActive(true);
                    hubScreen.battle1vAI.SetActive(true);
                    hubScreen.updateForPVP.SetActive(false);
                    hubScreen.titleOfBattleButton.gameObject.GetComponent<LocalizeStringEvent>().SetEntry("BattleBots");
                    Debug.Log("Matchmaking disabled by compile flag.");
#else
                    if (requiredVer != "EDITOR" && VersionToInt(Application.version) < VersionToInt(requiredVer))
                    {
                        // set status panel values, prompt update
                        hubScreen.serverStatusPanel.SetActive(false);
                        hubScreen.offlinePlayOnly.SetActive(false);
                        hubScreen.battle1vAI.SetActive(false);
                        hubScreen.updateForPVP.SetActive(true);
                        hubScreen.titleOfBattleButton.gameObject.GetComponent<LocalizeStringEvent>().SetEntry("BattleBots");
                        Debug.Log("PvP version mismatch, an update will be required to play online.");
                    }
                    else
                    {
                        // set pvp status in Battle Hub
                        serverStatus = _dataProvider.RefreshPVPServerStatus();
                        if (serverStatus)
                        {
                            // server available
                            hubScreen.serverStatusPanel.SetActive(false);
                            hubScreen.titleOfBattleButton.gameObject.GetComponent<LocalizeStringEvent>().SetTable("Common");
                            hubScreen.titleOfBattleButton.gameObject.GetComponent<LocalizeStringEvent>().SetEntry("BattleOnline");
                            hubScreen.battle1vAI.SetActive(false);
                            hubScreen.offlinePlayOnly.SetActive(false);
                            hubScreen.updateForPVP.SetActive(false);
                            Debug.Log("PVP Server Available.");
                        }
                        else
                        {
                            // server NOT available
                            hubScreen.serverStatusPanel.SetActive(true);
                            hubScreen.battle1vAI.SetActive(true);
                            hubScreen.offlinePlayOnly.SetActive(false);
                            hubScreen.updateForPVP.SetActive(false);
                            hubScreen.titleOfBattleButton.gameObject.GetComponent<LocalizeStringEvent>().SetTable("Common");
                            hubScreen.titleOfBattleButton.gameObject.GetComponent<LocalizeStringEvent>().SetEntry("CoinBattleDescription");
                            Debug.Log("PVP Server Unavailable.");
                        }
                    }
#endif
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
                hubScreen.offlinePlayOnly.SetActive(true);
                hubScreen.battle1vAI.SetActive(true);
                hubScreen.updateForPVP.SetActive(false);
                hubScreen.titleOfBattleButton.gameObject.GetComponent<LocalizeStringEvent>().SetEntry("BattleBots");
                Debug.Log("Offline, can't find out status of PVP Server.");

                // Shop should not load until after a first remote config sync
                if (!_dataProvider.GameModel.HasSyncdShop)
                {
                    hubScreen.shopButton.gameObject.SetActive(false);
                    hubScreen.leaderboardButton.gameObject.SetActive(false);
                }
                else
                { // just in case it ever matters?
                    hubScreen.shopButton.gameObject.SetActive(true);
                    hubScreen.leaderboardButton.gameObject.SetActive(true);
                }

                // if not Internet Connection or Sign in, we will use local data.
            }

            Task handlePlayerPrefs = HandlePlayerPrefs();

            _sceneNavigator = LandingSceneGod.SceneNavigator;
            _musicPlayer = LandingSceneGod.MusicPlayer;
            if (this == null)
                return;
            _uiAudioSource = GetComponent<AudioSource>();
            _soundPlayer
                = new SingleSoundPlayer(
                    new SoundFetcher(),
                    new EffectVolumeAudioSource(
                        new AudioSourceBC(_uiAudioSource),
                        _dataProvider.SettingsManager, 1));


            ILocTable storyStrings = await loadStoryStrings;
            levelTrashDataList.Initialise(storyStrings);
            sideQuestTrashDataList.Initialise(storyStrings);

            homeScreen.Initialise(this, _soundPlayer, _dataProvider);
            settingsScreen.Initialise(this, _soundPlayer, _dataProvider.SettingsManager, _dataProvider.GameModel.Hotkeys, commonStrings, screensSceneStrings);
            chooseDifficultyScreen.Initialise(this, _soundPlayer, _dataProvider.SettingsManager);

            // TEMP  For when not coming from LandingScene :)
            if (_musicPlayer == null)
            {
                _musicPlayer = Substitute.For<IMusicPlayer>();
                _sceneNavigator = Substitute.For<ISceneNavigator>();
            }

            messageBox.gameObject.SetActive(true);
            messageBox.Initialize(_dataProvider, _soundPlayer);
            messageBox.HideMessage();
            messageBoxBig.gameObject.SetActive(true);
            messageBoxBig.Initialize(_dataProvider, _soundPlayer);
            messageBoxBig.HideMessage();

#if !PREMIUM_EDITION
            if (!_gameModel.PremiumEdition && IsInternetAccessable && AuthenticationService.Instance.IsSignedIn)
            {
                premiumEditionButton.Initialise(_soundPlayer, ShowPremiumEditionIAP);
                premiumEditionButton.gameObject.SetActive(true);
            }
#endif

            characterOfShop.SetActive(false);
            characterOfBlackmarket.SetActive(false);
            processingPanel.SetActive(false);

            processingPanel.GetComponentInChildren<Text>().text = screensSceneStrings.GetString("Processing");
            Debug.Log(_applicationModel.Mode);

            _applicationModel.DataProvider.GameModel.ID_Bodykit_AIbot = -1;

            _prefabCache = await loadPrefabCache;

            _prefabFactory = new PrefabFactory(_prefabCache, _dataProvider.SettingsManager, commonStrings);
            _isPlaying = false;

            // TEMP  For showing PostBattleScreen :)
            if (goToPostBattleScreen)
            {
                _gameModel.LastBattleResult = new BattleResult(1, wasVictory: true);
                //_gameModel.LastBattleResult = new BattleResult(1, wasVictory: false);
                _applicationModel.ShowPostBattleScreen = true;
                //_applicationModel.IsTutorial = true;
            }

            ShowCharlieOnMainMenu();

            hubScreen.Initialise(this, _soundPlayer, _prefabFactory, _dataProvider, _applicationModel);
            trashScreen.Initialise(this, _soundPlayer, _applicationModel, _prefabFactory, levelTrashDataList, sideQuestTrashDataList, _musicPlayer, commonStrings, storyStrings);
            Camera captainsCamera = cameraOfCaptains.GetComponent<Camera>();
            if (captainsCamera != null)
            {
                trashScreen.SetCamera(captainsCamera);
            }
            chooseDifficultyScreen.Initialise(this, _soundPlayer, _dataProvider.SettingsManager);
            skirmishScreen.Initialise(this, _applicationModel, _soundPlayer, commonStrings, screensSceneStrings, _prefabFactory);
            shopPanelScreen.Initialise(this, _soundPlayer, _prefabFactory, _dataProvider, IsInternetAccessable);
            blackMarketScreen.Initialise(this, _soundPlayer, _prefabFactory, _dataProvider);
            captainSelectorPanel.Initialize(_soundPlayer, _prefabFactory, _dataProvider);

            _applicationModel.DataProvider.SaveGame();

            if (_applicationModel.ShowPostBattleScreen)
            {
                _applicationModel.ShowPostBattleScreen = false;
                Logging.Log(Tags.SCREENS_SCENE_GOD, "Pre go to post battle screen");
                await GoToPostBattleScreenAsync(screensSceneStrings);
#if !THIRD_PARTY_PUBLISHER
                fullScreenads.OpenAdvert();//<Aaron> Loads full screen ads after player win a battle
#endif
                Logging.Log(Tags.SCREENS_SCENE_GOD, "After go to post battle screen");
            }
            else if (_applicationModel.Mode == GameMode.CoinBattle)
            {
                _applicationModel.ShowPostBattleScreen = false;
                //_applicationModel.Mode = GameMode.Campaign;
#if !THIRD_PARTY_PUBLISHER
                //PlayAdvertisementMusic();
                //fullScreenads.OpenAdvert();//<Aaron> Loads full screen ads after player win a battle
#endif
                if (LandingSceneGod.Instance.coinBattleLevelNum == -1)
                    GotoHubScreen();
                else
                    GoToTrashScreen(LandingSceneGod.Instance.coinBattleLevelNum);
            }
            else if (_applicationModel.Mode == GameMode.PvP_1VS1)
            {
                _applicationModel.ShowPostBattleScreen = false;
                //_applicationModel.Mode = GameMode.Campaign;
#if !THIRD_PARTY_PUBLISHER
                fullScreenads.OpenAdvert();//<Aaron> Loads full screen ads after player win a battle
#endif
                GotoHubScreen();
            }
            else if (levelToShowCutscene == 0)
                GoToHomeScreen();
            else
                GoToTrashScreen(levelToShowCutscene);

            // After potentially initialising post battle screen, because that can modify the data model.
            Logging.Log(Tags.SCREENS_SCENE_GOD, "Pre initialise levels screen");
            await InitialiseLevelsScreenAsync();
            Logging.Log(Tags.SCREENS_SCENE_GOD, "After initialise levels screen");
            loadoutScreen.GetComponent<InfiniteLoadoutScreenController>()._bodykitDetails.Initialise(_dataProvider, _prefabFactory, _soundPlayer, commonStrings);
            loadoutScreen.GetComponent<InfiniteLoadoutScreenController>()._buildingDetails.Initialize(_dataProvider, _prefabFactory, _soundPlayer, commonStrings);
            loadoutScreen.GetComponent<InfiniteLoadoutScreenController>()._unitDetails.Initialize(_dataProvider, _prefabFactory, _soundPlayer, commonStrings);
            loadoutScreen.Initialise(this, _soundPlayer, _dataProvider, _prefabFactory);

            // TEMP  Go to specific screen :)
            //GoToLoadoutScreen();

            if (testSettingsScreen)
                GoToSettingsScreen();
            else if (testLevelsScreen)
                GoToLevelsScreen();
            else if (testTrashTalkScreen)
                GoToTrashScreen(levelNum: 1);
            else if (testDifficultyScreen)
                GoToChooseDifficultyScreen();
            else if (testSkirmishScreen)
                GoToSkirmishScreen();
            else if (testLoadoutScreen)
                GoToLoadoutScreen();

            ranker.DisplayRank(_gameModel.LifetimeDestructionScore);

            if (_gameModel.PremiumEdition)
            {
                thankYouPlane.SetTrigger("Play");
                _isPlaying = true;
            }

            _sceneNavigator.SceneLoaded(SceneNames.SCREENS_SCENE);

            Logging.Log(Tags.SCREENS_SCENE_GOD, "END");

            await handlePlayerPrefs;
            PvPBattleSceneGodTunnel.isCost = false;
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
                Destroy(charlie.gameObject);
                charlie = null;
            }
            CaptainExo charliePrefab = _prefabFactory.GetCaptainExo(_gameModel.PlayerLoadout.CurrentCaptain);
            charlie = Instantiate(charliePrefab, ContainerCaptain);
            charlie.gameObject.transform.localScale = Vector3.one * 1; //0.5f
            characterOfCharlie = charlie.gameObject;
            cameraOfCharacter.SetActive(true);
            cameraOfCaptains.SetActive(false);
        }
        private async Task GoToPostBattleScreenAsync(ILocTable screensSceneStrings)
        {
            Assert.IsFalse(postBattleScreen.IsInitialised, "Should only ever navigate (and hence initialise) once");
            await postBattleScreen.InitialiseAsync(this, _soundPlayer, _applicationModel, _prefabFactory, _musicPlayer, difficultyIndicators, levelTrashDataList, sideQuestTrashDataList, screensSceneStrings);
            //--->CODE CHANGED BY ANUJ
            if (_applicationModel.Mode == GameMode.PvP_1VS1)
            {
                GoToScreen(hubScreen);
            }
            else
            {
                GoToScreen(postBattleScreen, playDefaultMusic: false);
            }
            //<---
        }

        public void GoToHomeScreen()
        {
            homeScreen.gameObject.SetActive(true);
            ShowCharlieOnMainMenu();
            characterOfBlackmarket.SetActive(false);
            characterOfShop.SetActive(false);
            characterOfCharlie.SetActive(true);
            cameraOfCharacter.SetActive(true);
            cameraOfCaptains.SetActive(false);
            homeScreenArt.SetActive(true);
            environmentArt.SetActive(true);
            GoToScreen(homeScreen);

            if (_gameModel.PremiumEdition)
            {
                thankYouPlane.SetTrigger("Play");
                _isPlaying = true;
            }
        }

        public void GoToLevelsScreen()
        {
            GoToScreen(levelsScreen);
        }

        public void GotoHubScreen()
        {
            CanvasGroup AIv1ButtonCanvasGroup = hubScreen.coinBattleButton.GetComponent<CanvasGroup>(); // Made for bugfix
            AIv1ButtonCanvasGroup.interactable = true;
            AIv1ButtonCanvasGroup.blocksRaycasts = true;

            homeScreen.gameObject.SetActive(false);
            characterOfBlackmarket.SetActive(false);
            characterOfShop.SetActive(false);
            characterOfCharlie.SetActive(false);
            cameraOfCharacter.SetActive(false);
            cameraOfCaptains.SetActive(false);
            homeScreenArt.SetActive(false);
            environmentArt.SetActive(false);
            _musicPlayer.PlayScreensSceneMusic();
            fullScreenads.CloseAdvert();
            GoToScreen(hubScreen);
        }

        public void GoToBodykitsShop()
        {
            GotoShopScreen();
            shopPanelScreen.BodykitButton_OnClick();
        }

        public void GotoShopScreen()
        {
            hubScreen.playerInfoPanelController.gameObject.SetActive(true);
            homeScreen.gameObject.SetActive(false);
            characterOfBlackmarket.SetActive(false);
            characterOfShop.SetActive(true);
            characterOfCharlie.SetActive(false);
            cameraOfCharacter.SetActive(true);
            cameraOfCaptains.SetActive(true);
            fullScreenads.CloseAdvert();
            GoToScreen(shopPanelScreen);
            shopPanelScreen.InitiaiseCaptains();
        }

        public void GotoBlackMarketScreen()
        {
            homeScreen.gameObject.SetActive(false);
            characterOfBlackmarket.SetActive(true);
            characterOfShop.SetActive(false);
            characterOfCharlie.SetActive(false);
            cameraOfCharacter.SetActive(true);
            cameraOfCaptains.SetActive(true);
            fullScreenads.CloseAdvert();
            GoToScreen(blackMarketScreen);
            blackMarketScreen.InitialiseIAPs();
            //AdvertisingBanner.startAdvert();
        }

        public void ShowPremiumEditionIAP()
        {
            Debug.Log("ShowPremiumEditionIAP");
            Assert.IsNotNull(premiumConfirmationScreen, "Premium confirmation screen is missing!");
            GotoHubScreen();
            GotoBlackMarketScreen();
            premiumConfirmationScreen.gameObject.SetActive(true);
        }

        private async Task InitialiseLevelsScreenAsync()
        {
            IList<LevelInfo> levels = CreateLevelInfo(_dataProvider.Levels, _dataProvider.GameModel.CompletedLevels);

            await levelsScreen.InitialiseAsync(
                this,
                _soundPlayer,
                levels,
                testLevelsScreen ? numOfLevelsUnlocked : _dataProvider.LockedInfo.NumOfLevelsUnlocked,
                difficultyIndicators,
                levelTrashDataList,
                _dataProvider);
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
            homeScreen.gameObject.SetActive(false);
            characterOfBlackmarket.SetActive(true);
            characterOfShop.SetActive(false);
            characterOfCharlie.SetActive(false);
            cameraOfCharacter.SetActive(false);
            cameraOfCaptains.SetActive(false);
            fullScreenads.CloseAdvert();
            loadoutScreen.GetComponent<InfiniteLoadoutScreenController>()._bodykitDetails.CollectUnlockedBodykits();
            loadoutScreen.GetComponent<InfiniteLoadoutScreenController>().RefreshBodykitsUI();
            loadoutScreen.GetComponent<InfiniteLoadoutScreenController>()._buildingDetails.CollectUnlockedBuildingVariant();
            loadoutScreen.GetComponent<InfiniteLoadoutScreenController>()._unitDetails.CollectUnlockedUnitVariant();
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
            Assert.IsTrue(levelNum <= _dataProvider.LockedInfo.NumOfLevelsUnlocked,
                "levelNum: " + levelNum + " should be <= than number of levels unlocked: " + _dataProvider.LockedInfo.NumOfLevelsUnlocked);

            _applicationModel.SelectedLevel = levelNum;

            if (_applicationModel.Mode == GameMode.Campaign)
            {
                if (LevelStages.STAGE_STARTS.Contains(levelNum - 1) && levelToShowCutscene != levelNum)
                {
                    levelToShowCutscene = levelNum;
                    _applicationModel.DataProvider.GameModel.ID_Bodykit_AIbot = -1;
                    _applicationModel.DataProvider.SaveGame();
                    //GoToScreen(trashScreen, playDefaultMusic: false);
                    _sceneNavigator.GoToScene(SceneNames.STAGE_INTERSTITIAL_SCENE, true);
                }
                else
                {
                    levelToShowCutscene = 0;
                    _applicationModel.DataProvider.GameModel.ID_Bodykit_AIbot = -1;
                    _applicationModel.DataProvider.SaveGame();
                    //_musicPlayer.PlayTrashMusic();
                    GoToScreen(trashScreen, playDefaultMusic: false);
                    //_musicPlayer.PlayTrashMusic();
                }
            }
            else if (_applicationModel.Mode == GameMode.CoinBattle)
            {
                levelToShowCutscene = 0;
                // Random bodykits for AIBot
                ILevel level = _applicationModel.DataProvider.Levels[levelNum - 1];
                _applicationModel.DataProvider.GameModel.ID_Bodykit_AIbot = UnityEngine.Random.Range(0, 5) == 2 ? GetRandomBodykitForAI(GetHullType(level.Hull.PrefabName)) : -1;
                //_applicationModel.DataProvider.GameModel.ID_Bodykit_AIbot = GetRandomBodykitForAI(GetHullType(level.Hull.PrefabName));
                _applicationModel.DataProvider.SaveGame();
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
                for (int i = 0; i < /*12*/ _applicationModel.DataProvider.StaticData.Bodykits.Count; i++)
                {
                    if (_prefabFactory.GetBodykit(StaticPrefabKeys.BodyKits.GetBodykitKey(i)).cruiserType == hullType)
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
                case "Flea":
                    return HullType.Flea;
                case "Goatherd":
                    return HullType.Goatherd;
                case "Hammerhead":
                    return HullType.Hammerhead;
                case "Longbow":
                    return HullType.Longbow;
                case "Megalodon":
                    return HullType.Megalodon;
                case "Megalith":
                    return HullType.Megalith;
                case "Microlodon":
                    return HullType.Microlodon;
                case "Raptor":
                    return HullType.Raptor;
                case "Rickshaw":
                    return HullType.Rickshaw;
                case "Rockjaw":
                    return HullType.Rockjaw;
                case "Pistol":
                    return HullType.Pistol;
                case "Shepherd":
                    return HullType.Shepherd;
                case "TasDevil":
                    return HullType.TasDevil;
                case "Yeti":
                    return HullType.Yeti;
            }
            return HullType.None;
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


        public void GoToSideQuestTrashScreen(int sideQuestLevelNum, int firstLevelOfStage = -1)
        {
            // Implementation similar to GoToTrashScreen method, but for side quest levels
            AdvertisingBanner.stopAdvert();
            if (firstLevelOfStage > -1)
                _applicationModel.SelectedLevel = firstLevelOfStage + 1;
            _applicationModel.Mode = GameMode.SideQuest;

            _applicationModel.SelectedSideQuestID = sideQuestLevelNum;
            levelToShowCutscene = 0;
            _applicationModel.DataProvider.GameModel.ID_Bodykit_AIbot = -1;
            _applicationModel.DataProvider.SaveGame();
            GoToScreen(trashScreen, playDefaultMusic: false);
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
            //AdvertisingBanner.stopAdvert();

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
            _applicationModel.SelectedSideQuestID = sideQuestID;
            _applicationModel.Mode = GameMode.SideQuest;
            AdvertisingBanner.stopAdvert();
            _sceneNavigator.GoToScene(SceneNames.BATTLE_SCENE, true);
            CleanUp();
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

        public void ShowNewsPanel()
        {
            ILocTable screensSceneStrings = LandingSceneGod.Instance.screenSceneStrings;

            messageBoxBig.ShowMessage(screensSceneStrings.GetString("UpdateTitle"), screensSceneStrings.GetString("UpdateDescription"));
        }

        public void PlayAdvertisementMusic()
        {
            //if (_applicationModel.DataProvider.GameModel.Settings.ShowAds || !_applicationModel.DataProvider.GameModel.PremiumEdition)
            //{
            //    _musicPlayer.PlayAdsMusic();
            //}
            _musicPlayer.PlayAdsMusic();
        }

        public void PlayMusicCloseAdsButton()
        {
            if (_gameModel.LastBattleResult == null)
            {
                // If the player has exited the tutorial without progressing to the first level!
                _musicPlayer.PlayVictoryMusic();
                return;
            }
            //Only called via Unity Button event when clicking the close button on a FullScreenAd
            if (_gameModel.HasAttemptedTutorial && _gameModel.FirstNonTutorialBattle)
                _musicPlayer.PlayVictoryMusic();

            else if (_gameModel.LastBattleResult.WasVictory)
                _musicPlayer.PlayVictoryMusic();
            else if (!_gameModel.LastBattleResult.WasVictory)
                _musicPlayer.PlayDefeatMusic();
            else
                _musicPlayer.PlayScreensSceneMusic();
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
                Debug.LogError($"No CaptainExoData component attached to prefab at path: {prefabPath}");

            return data;
        }

        private void CleanUp()
        {
            loadoutScreen.DisposeManagedState();
        }

        void Update()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
                _currentScreen?.Cancel();

            if (_gameModel != null)
                if (_gameModel.PremiumEdition)
                    if (!_isPlaying)
                        thankYouPlane.SetTrigger("Play");
        }

        void OnApplicationQuit()
        {
            try
            {
                _applicationModel.DataProvider.SaveGame();
                _applicationModel.DataProvider.SyncCoinsToCloud();
                _applicationModel.DataProvider.SyncCreditsToCloud();

                // Save changes:
                _applicationModel.DataProvider.CloudSave();
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
        }

        int VersionToInt(string version)
        {
            int seperatorCount = version.Count(c => c == '.');

            for (int i = 0; i < seperatorCount; i++)
                version = version.Remove(version.IndexOf('.'), 1);

            return int.Parse(version);
        }

        async Task HandlePlayerPrefs()
        {
            bool isPlayed = PlayerPrefs.GetInt("PLAYED", 0) != 0;
            bool isSetPlayerName = PlayerPrefs.GetInt("SETNAME", 0) != 0;

            if (!isPlayed)
            {
                try
                {
                    await AuthenticationService.Instance.UpdatePlayerNameAsync(_dataProvider.GameModel.PlayerName + "#" + _dataProvider.GameModel.PlayerLoadout.CurrentCaptain.PrefabName);
                    PlayerPrefs.SetInt("SETNAME", 1);
                }
                catch
                {
                    PlayerPrefs.SetInt("SETNAME", 0);
                }
            }

            if (!isSetPlayerName)
            {
                try
                {
                    await AuthenticationService.Instance.UpdatePlayerNameAsync(_dataProvider.GameModel.PlayerName + "#" + _dataProvider.GameModel.PlayerLoadout.CurrentCaptain.PrefabName);
                    PlayerPrefs.SetInt("SETNAME", 1);
                }
                catch
                {
                    PlayerPrefs.SetInt("SETNAME", 0);
                }
            }
            PlayerPrefs.GetInt("PLAYED", 1);
        }
    }
}
