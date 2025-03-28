using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Static;
using BattleCruisers.Scenes;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.UI.ScreensScene.LoadoutScreen;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using BattleCruisers.UI.ScreensScene.CoinBattleScreen;
using System.Linq;
using Unity.Services.Authentication;
using BattleCruisers.Utils.Localisation;
namespace BattleCruisers.UI.ScreensScene.BattleHubScreen
{
    public class BattleHubScreensController : ScreenController
    {
        private BattleResult _lastBattleResult;
        private ScreenController _currentScreen;
        private ISingleSoundPlayer _soundPlayer;

        public CanvasGroupButton homeButton, battleHubButton, loadoutButton, shopButton, leaderboardButton, profileButton, arenaBackButton;
        public GameObject coins;

        public ScreenController battlePanel;
        public InfiniteLoadoutScreenController loadoutPanel;
        //    public ShopPanelScreenController shopPanel;
        public LeaderboardPanelScreenController leaderboardPanel;
        public ProfilePanelScreenController profilePanel;
        public ArenaSelectPanelScreenController arenaSelectPanel;
        public CoinBattleScreenController coinBattleController;

        public PlayerInfoPanelController playerInfoPanelController;
        public CanvasGroupButton continueButton, levelsButton, skirmishButton, battleButton, coinBattleButton;

        public Text titleOfBattleButton;

        public GameObject serverStatusPanel;
        public GameObject offlinePlayOnly;
        public GameObject updateForPVP;
        public GameObject battle1vAI;
        public Text offlineOnlyText;
        public Text continueTitle;
        public Text continueSubtitle;
        public Text levelsTitle;
        public Text skirmishTitle;
        public Text openingSoonText;
        public void Initialise(
            IScreensSceneGod screensSceneGod,
            ISingleSoundPlayer soundPlayer)
        {
            base.Initialise(screensSceneGod);

            Helper.AssertIsNotNull(homeButton, battleHubButton, loadoutButton, shopButton, leaderboardButton, profileButton, arenaBackButton);

            _lastBattleResult = DataProvider.GameModel.LastBattleResult;
            _soundPlayer = soundPlayer;

            homeButton.Initialise(_soundPlayer, GoHome);
            battleHubButton.Initialise(_soundPlayer, OpenBattleHub);
            arenaBackButton.Initialise(_soundPlayer, OpenBattleHub);
            loadoutButton.Initialise(_soundPlayer, OpenLoadout);
            shopButton.Initialise(_soundPlayer, OpenShop);
            leaderboardButton.Initialise(_soundPlayer, OpenLeaderboard);
            profileButton.Initialise(_soundPlayer, OpenProfile);

            continueButton.Initialise(_soundPlayer, Continue);
            levelsButton.Initialise(_soundPlayer, GoToLevelsScreen);
            skirmishButton.Initialise(_soundPlayer, GoToSkirmishScreen);
            battleButton.Initialise(_soundPlayer, GotoPvPMode);
            coinBattleButton.Initialise(_soundPlayer, GotoCoinBattle);

            battlePanel.Initialise(screensSceneGod);
            leaderboardPanel.Initialise(screensSceneGod);
            profilePanel.Initialise(screensSceneGod, _soundPlayer);
            arenaSelectPanel.Initialise(screensSceneGod, _soundPlayer);

            coinBattleController.Initialise(screensSceneGod);
            playerInfoPanelController.UpdateInfo();

            continueTitle.text = LocTableCache.ScreensSceneTable.GetString("ContinueCampaign");
            continueSubtitle.text = LocTableCache.ScreensSceneTable.GetString("ContinueCampaignDescription");
            levelsTitle.text = LocTableCache.ScreensSceneTable.GetString("LevelSelect");
            skirmishTitle.text = LocTableCache.ScreensSceneTable.GetString("SkirmishMode");
            offlineOnlyText.text = LocTableCache.ScreensSceneTable.GetString("OfflineOnlySubtitle");
            openingSoonText.text = LocTableCache.ScreensSceneTable.GetString("ArenasOpenDecemberMessage");
        }

        private void GoHome()
        {
            if (_currentScreen == arenaSelectPanel)
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

            if (ApplicationModel.Mode != GameMode.PvP_1VS1)
            {
                if (ScreensSceneGod.Instance.cameraOfCaptains != null)
                    ScreensSceneGod.Instance.cameraOfCaptains.SetActive(false);
                if (ScreensSceneGod.Instance.cameraOfCharacter != null)
                    ScreensSceneGod.Instance.cameraOfCharacter.SetActive(false);
            }

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
            if (ProfilePanelScreenController.Instance?.captainsPanel != null)
                ProfilePanelScreenController.Instance?.captainsPanel?.gameObject.SetActive(false);
            _screensSceneGod.GotoShopScreen();
            UnselectAll();
        }

        private void OpenLeaderboard()
        {
            playerInfoPanelController.gameObject.SetActive(true);
            ScreensSceneGod.Instance.cameraOfCaptains.SetActive(false);
            ScreensSceneGod.Instance.cameraOfCharacter.SetActive(false);
            leaderboardPanel.Initialise(_screensSceneGod);
            GoToScreen(leaderboardPanel);
            UnselectAll();
        }


        public void OpenProfile()
        {
            // Profile button is available in the shop, so we need to be able to turn that off when entering profile (otherwise shop gets in the way)
            if (ScreensSceneGod.Instance.shopPanelScreen.gameObject.activeInHierarchy || ScreensSceneGod.Instance.blackMarketScreen.gameObject.activeInHierarchy)
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
            Debug.Log(_lastBattleResult);
            if (_lastBattleResult == null)
            {
                playerInfoPanelController.gameObject.SetActive(false);
                ApplicationModel.Mode = GameMode.Campaign;
                _screensSceneGod.GoToTrashScreen(1);
            }
            else
            {
                Assert.IsNotNull(_lastBattleResult);
                playerInfoPanelController.gameObject.SetActive(false);
                ApplicationModel.Mode = GameMode.Campaign;
                int nextLevelToPlay = DataProvider.GameModel.NumOfLevelsCompleted < 31 ? DataProvider.GameModel.NumOfLevelsCompleted + 1 : 1;
                _screensSceneGod.GoToTrashScreen(nextLevelToPlay);
            }
        }

        public void GoToLevelsScreen()
        {
            playerInfoPanelController.gameObject.SetActive(false);
            ApplicationModel.Mode = GameMode.Campaign;
            _screensSceneGod.GoToLevelsScreen();
        }

        // public void GoToMultiplayScreen()
        // {
        //     _screensSceneGod.LoadMultiplayScene();
        // }


        public void GoToSkirmishScreen()
        {
            playerInfoPanelController.gameObject.SetActive(false);
            ApplicationModel.Mode = GameMode.Skirmish;
            _screensSceneGod.GoToSkirmishScreen();
        }

        public void GotoPvPMode()
        {
#if DISABLE_MATCHMAKING
            ApplicationModel.Mode = GameMode.CoinBattle;
            // Set UI elements for offline-only mode
            if (ScreensSceneGod.Instance != null)
            {
                ScreensSceneGod.Instance.hubScreen.serverStatusPanel.SetActive(false);
                ScreensSceneGod.Instance.hubScreen.offlinePlayOnly.SetActive(true);
                ScreensSceneGod.Instance.hubScreen.battle1vAI.SetActive(true);
                ScreensSceneGod.Instance.hubScreen.updateForPVP.SetActive(false);
            
                offlineOnlyText.gameObject.SetActive(false);
            }
            coinBattleController.Battle();
#else
            if (ScreensSceneGod.Instance.requiredVer != "EDITOR" && VersionToInt(Application.version) < VersionToInt(ScreensSceneGod.Instance.requiredVer))
            {
                // prompt update
                Debug.Log("Opening: market://details?id=" + Application.identifier);
                Application.OpenURL("market://details?id=" + Application.identifier);
            }
            else
            {
                if (ScreensSceneGod.Instance.serverStatus && AuthenticationService.Instance.IsSignedIn)
                {
                    //playerInfoPanelController.gameObject.SetActive(false);
                    GoToScreen(arenaSelectPanel);
                }
                else
                {
                    ApplicationModel.Mode = GameMode.CoinBattle;
                    coinBattleController.Battle();
                }
            }
#endif
        }

        public void GotoCoinBattle()
        {

            ApplicationModel.Mode = GameMode.CoinBattle;
            coinBattleController.Battle();
            CanvasGroup AIv1ButtonCanvasGroup = coinBattleButton.GetComponent<CanvasGroup>();
            AIv1ButtonCanvasGroup.blocksRaycasts = false;
            AIv1ButtonCanvasGroup.interactable = false;
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

        int VersionToInt(string version)
        {
            int seperatorCount = version.Count(c => c == '.');

            for (int i = 0; i < seperatorCount; i++)
                version = version.Remove(version.IndexOf('.'), 1);
            int version_num;
            bool result = int.TryParse(version, out version_num);
            if (result)
                return version_num;
            else return 0;
        }
    }
}
