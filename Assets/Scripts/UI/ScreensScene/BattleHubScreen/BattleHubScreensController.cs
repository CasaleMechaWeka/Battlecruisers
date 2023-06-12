using System;
using System.Collections.Generic;
using BattleCruisers.Data;
using BattleCruisers.Data.Helpers;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Static;
using BattleCruisers.Scenes;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.UI.ScreensScene.LoadoutScreen;
using BattleCruisers.UI.ScreensScene.HomeScreen.Buttons;
using BattleCruisers.UI.ScreensScene.HomeScreen;
using BattleCruisers.Utils.Fetchers;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.BattleHubScreen
{
    public class BattleHubScreensController : ScreenController
    {
        private BattleResult _lastBattleResult;
        private INextLevelHelper _nextLevelHelper;
        private ScreenController _currentScreen;
        private ISingleSoundPlayer _soundPlayer;


        public CanvasGroupButton homeButton, battleHubButton, loadoutButton, shopButton, leaderboardButton, profileButton;

        public BattlePanelScreenController battlePanel;
        public InfiniteLoadoutScreenController loadoutPanel;
        public ShopPanelScreenController shopPanel;
        public LeaderboardPanelScreenController leaderboardPanel;
        public ProfilePanelScreenController profilePanel;

        public CanvasGroupButton continueButton, levelsButton, skirmishButton;

        public void Initialise(
            IScreensSceneGod screensSceneGod,
            ISingleSoundPlayer soundPlayer,
            IPrefabFactory prefabFactory,
            IDataProvider dataProvider,
            INextLevelHelper nextLevelHelper)
        {
            base.Initialise(screensSceneGod);

            Helper.AssertIsNotNull(dataProvider, nextLevelHelper);
            Helper.AssertIsNotNull(homeButton, battleHubButton, loadoutButton, shopButton, leaderboardButton, profileButton);

            _lastBattleResult = dataProvider.GameModel.LastBattleResult;
            _nextLevelHelper = nextLevelHelper;
            _soundPlayer = soundPlayer;

            homeButton.Initialise(_soundPlayer, GoHome);
            battleHubButton.Initialise(_soundPlayer, OpenBattleHub);
            loadoutButton.Initialise(_soundPlayer, OpenLoadout);
            shopButton.Initialise(_soundPlayer, OpenShop);
            leaderboardButton.Initialise(_soundPlayer, OpenLeaderboard);
            profileButton.Initialise(_soundPlayer, OpenProfile);

            continueButton.Initialise(_soundPlayer, Continue);
            levelsButton.Initialise(_soundPlayer, GoToLevelsScreen);
            skirmishButton.Initialise(_soundPlayer, GoToSkirmishScreen);

            battlePanel.Initialise(screensSceneGod, _soundPlayer, prefabFactory, dataProvider, nextLevelHelper);
            leaderboardPanel.Initialise(screensSceneGod, _soundPlayer, prefabFactory, dataProvider, nextLevelHelper);
            profilePanel.Initialise(screensSceneGod, _soundPlayer, prefabFactory, dataProvider, nextLevelHelper);
            shopPanel.Initialise(screensSceneGod, _soundPlayer, prefabFactory, dataProvider, nextLevelHelper);


        }



        private void GoHome()
        {
            _screensSceneGod.GoToHomeScreen();
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
            GoToScreen(battlePanel);
            UnselectAll();

        }

        private void OpenLoadout()
        {
            _screensSceneGod.GoToLoadoutScreen();

            UnselectAll();

        }
        private void OpenShop()
        {
            //GoToScreen(shopPanel);
            _screensSceneGod.GotoShopScreen();

            UnselectAll();

        }

        private void OpenLeaderboard()
        {
            GoToScreen(leaderboardPanel);

            UnselectAll();

        }


        private void OpenProfile()
        {
            GoToScreen(profilePanel);

            UnselectAll();

        }

        public void Continue()
        {
            Assert.IsNotNull(_lastBattleResult);

            int nextLevelToPlay = _nextLevelHelper.FindNextLevel();
            _screensSceneGod.GoToTrashScreen(nextLevelToPlay);
        }

        public void GoToLevelsScreen()
        {
            _screensSceneGod.GoToLevelsScreen();
        }

        // public void GoToMultiplayScreen()
        // {
        //     _screensSceneGod.LoadMultiplayScene();
        // }


        public void GoToSkirmishScreen()
        {
            _screensSceneGod.GoToSkirmishScreen();
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
