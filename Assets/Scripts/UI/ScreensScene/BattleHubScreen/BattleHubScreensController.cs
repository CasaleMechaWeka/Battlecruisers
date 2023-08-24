using BattleCruisers.Data;
using BattleCruisers.Data.Helpers;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Static;
using BattleCruisers.Scenes;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.UI.ScreensScene.LoadoutScreen;
using BattleCruisers.Utils.Fetchers;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using BattleCruisers.UI.ScreensScene.CoinBattleScreen;
using Unity.Services.Authentication;

namespace BattleCruisers.UI.ScreensScene.BattleHubScreen
{
    public class BattleHubScreensController : ScreenController
    {
        private BattleResult _lastBattleResult;
        private INextLevelHelper _nextLevelHelper;
        private ScreenController _currentScreen;
        private IPrefabFactory _prefabFactory;
        private ISingleSoundPlayer _soundPlayer;
        private IDataProvider _dataProvider;
        private IApplicationModel _applicationModel;


        public CanvasGroupButton homeButton, battleHubButton, loadoutButton, shopButton, leaderboardButton, profileButton;
        public GameObject coins;

        public BattlePanelScreenController battlePanel;
        public InfiniteLoadoutScreenController loadoutPanel;
        //    public ShopPanelScreenController shopPanel;
        public LeaderboardPanelScreenController leaderboardPanel;
        public ProfilePanelScreenController profilePanel;
        public ArenaSelectPanelScreenController arenaSelectPanel;
        public CoinBattleScreenController coinBattleController;

        public PlayerInfoPanelController playerInfoPanelController;
        public CanvasGroupButton continueButton, levelsButton, skirmishButton, battleButton;

        public Text titleOfBattleButton;

        public GameObject serverStatusPanel;
        public GameObject offlinePlayOnly;
        public GameObject battle1vAI;

        public void Initialise(
            IScreensSceneGod screensSceneGod,
            ISingleSoundPlayer soundPlayer,
            IPrefabFactory prefabFactory,
            IDataProvider dataProvider,
            IApplicationModel applicationModel,
            INextLevelHelper nextLevelHelper)
        {
            base.Initialise(screensSceneGod);

            Helper.AssertIsNotNull(dataProvider, nextLevelHelper);
            Helper.AssertIsNotNull(homeButton, battleHubButton, loadoutButton, shopButton, leaderboardButton, profileButton);

            _lastBattleResult = dataProvider.GameModel.LastBattleResult;
            _nextLevelHelper = nextLevelHelper;
            _soundPlayer = soundPlayer;
            _dataProvider = dataProvider;
            _applicationModel = applicationModel;
            _prefabFactory = prefabFactory;

            homeButton.Initialise(_soundPlayer, GoHome);
            battleHubButton.Initialise(_soundPlayer, OpenBattleHub);
            loadoutButton.Initialise(_soundPlayer, OpenLoadout);
            shopButton.Initialise(_soundPlayer, OpenShop);
            leaderboardButton.Initialise(_soundPlayer, OpenLeaderboard);
            profileButton.Initialise(_soundPlayer, OpenProfile);

            continueButton.Initialise(_soundPlayer, Continue);
            levelsButton.Initialise(_soundPlayer, GoToLevelsScreen);
            skirmishButton.Initialise(_soundPlayer, GoToSkirmishScreen);
            battleButton.Initialise(_soundPlayer, GotoPvPMode);

            battlePanel.Initialise(screensSceneGod, _soundPlayer, prefabFactory, dataProvider, nextLevelHelper);
            leaderboardPanel.Initialise(screensSceneGod, _soundPlayer, prefabFactory, dataProvider, nextLevelHelper);
            profilePanel.Initialise(screensSceneGod, _soundPlayer, prefabFactory, dataProvider, nextLevelHelper);
            arenaSelectPanel.Initialise(screensSceneGod, _soundPlayer, prefabFactory, dataProvider, nextLevelHelper);

            coinBattleController.Initialise(screensSceneGod, _applicationModel, _soundPlayer, prefabFactory);
            playerInfoPanelController.UpdateInfo(_dataProvider, _prefabFactory);
        }

        private void GoHome()
        {
            if(_currentScreen == arenaSelectPanel)
            {
                OpenBattleHub();
            }
            else
            {
                playerInfoPanelController.gameObject.SetActive(false);
                _screensSceneGod.GoToHomeScreen();
            }
        }
        private void UnselectAll()
        {
            battleHubButton.IsSelected = false;
            loadoutButton.IsSelected = false;
            shopButton.IsSelected = false;
            leaderboardButton.IsSelected = false;
            profileButton.IsSelected = false;
        }
        private void OpenBattleHub()
        {
            playerInfoPanelController.gameObject.SetActive(true);
            ScreensSceneGod.Instance.cameraOfCaptains.SetActive(false);
            ScreensSceneGod.Instance.cameraOfCharacter.SetActive(false);
            GoToScreen(battlePanel);
            UnselectAll();
        }

        private void OpenLoadout()
        {
            playerInfoPanelController.gameObject.SetActive(false);
            _screensSceneGod.GoToLoadoutScreen();
            UnselectAll();
        }
        private void OpenShop()
        {
            //  GoToScreen(shopPanel);
            playerInfoPanelController.gameObject.SetActive(true);
            ProfilePanelScreenController.Instance?.captainsPanel?.RemoveAllCaptainsFromRenderCamera();
            ProfilePanelScreenController.Instance?.captainsPanel?.gameObject.SetActive(false);
            _screensSceneGod.GotoShopScreen();
            UnselectAll();
        }

        private void OpenLeaderboard()
        {
            playerInfoPanelController.gameObject.SetActive(true);
            ScreensSceneGod.Instance.cameraOfCaptains.SetActive(false);
            ScreensSceneGod.Instance.cameraOfCharacter.SetActive(false);
            GoToScreen(leaderboardPanel);
            UnselectAll();
        }


        public void OpenProfile()
        {
            // Profile button is available in the shop, so we need to be able to turn that off when entering profile (otherwise shop gets in the way)
            if (ScreensSceneGod.Instance.shopPanelScreen.gameObject.activeInHierarchy)
            {
                ScreensSceneGod.Instance.characterOfBlackmarket.SetActive(false);
                ScreensSceneGod.Instance.characterOfShop.SetActive(false);
                ScreensSceneGod.Instance.characterOfCharlie.SetActive(false);
                ScreensSceneGod.Instance.homeScreenArt.SetActive(false);
                ScreensSceneGod.Instance.environmentArt.SetActive(false);
                ScreensSceneGod.Instance.GotoHubScreen();
            }

            playerInfoPanelController.gameObject.SetActive(true);
            ScreensSceneGod.Instance.cameraOfCaptains.SetActive(true);
            ScreensSceneGod.Instance.cameraOfCharacter.SetActive(false);
            GoToScreen(profilePanel);
            UnselectAll();
        }

        public void Continue()
        {
            Assert.IsNotNull(_lastBattleResult);
            playerInfoPanelController.gameObject.SetActive(false);
            int nextLevelToPlay = _nextLevelHelper.FindNextLevel();
            _screensSceneGod.GoToTrashScreen(nextLevelToPlay);
        }

        public void GoToLevelsScreen()
        {
            playerInfoPanelController.gameObject.SetActive(false);
            _screensSceneGod.GoToLevelsScreen();
        }

        // public void GoToMultiplayScreen()
        // {
        //     _screensSceneGod.LoadMultiplayScene();
        // }


        public void GoToSkirmishScreen()
        {
            playerInfoPanelController.gameObject.SetActive(false);
            _screensSceneGod.GoToSkirmishScreen();
        }

        public void GotoPvPMode()
        {
            if(ScreensSceneGod.Instance.serverStatus && AuthenticationService.Instance.IsSignedIn)
            {
                playerInfoPanelController.gameObject.SetActive(false);
                GoToScreen(arenaSelectPanel);
            }
            else
            {
                coinBattleController.BattleButtonClicked();
            }

            /*            if (await LandingSceneGod.CheckForInternetConnection() && AuthenticationService.Instance.IsSignedIn)
                        {
                            GoToScreen(arenaSelectPanel);
                        }
                        else
                        {
                        
                        }*/
        }



        private void GoToScreen(ScreenController destinationScreen, bool playDefaultMusic = true)
        {

            Logging.Log(Tags.SCREENS_SCENE_GOD, $"START  current: {_currentScreen}  destination: {destinationScreen}");

            if (_currentScreen == destinationScreen)
                return;

            if (_currentScreen != null)
            {
                _currentScreen.OnDismissing();
                _currentScreen.gameObject.SetActive(false);
                _soundPlayer.PlaySoundAsync(SoundKeys.UI.ScreenChange);
            }

            _currentScreen = destinationScreen;
            _currentScreen.gameObject.SetActive(true);
            _currentScreen.OnPresenting(activationParameter: null);

            // if (playDefaultMusic)
            // {
            //     _musicPlayer.PlayScreensSceneMusic();
            // }
        }

        public override void OnPresenting(object activationParameter)
        {
            base.OnPresenting(activationParameter);
            OpenBattleHub();
            battleHubButton.OnPointerClick(null);
        }
    }
}
