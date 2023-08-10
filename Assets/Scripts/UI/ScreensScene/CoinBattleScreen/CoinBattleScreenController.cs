using BattleCruisers.Cruisers;
using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Settings;
using BattleCruisers.Data.Static;
using BattleCruisers.Data.Static.Strategies.Helper;
using BattleCruisers.Scenes;
using BattleCruisers.UI.ScreensScene.SettingsScreen;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Localisation;
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.CoinBattleScreen
{
    public class CoinBattleScreenController : ScreenController
    {
        private IApplicationModel _applicationModel;
        private IRandomGenerator _random;
        private ICoinBattleModel CoinBattle => _applicationModel.DataProvider.GameModel.CoinBattle;

        //public CanvasGroupButton battleButton; //homeButton;

        public void Initialise(
            IScreensSceneGod screensSceneGod,
            IApplicationModel applicationModel,
            ISingleSoundPlayer soundPlayer,
            //ILocTable commonStrings,
            //ILocTable screensSceneStrings,
            IPrefabFactory prefabFactory)
        {
            base.Initialise(screensSceneGod);

            //Helper.AssertIsNotNull(battleButton);
            Helper.AssertIsNotNull(applicationModel, soundPlayer, prefabFactory);

            _applicationModel = applicationModel;

            //battleButton.Initialise(soundPlayer, BattleButtonClicked, this);
            //homeButton.Initialise(soundPlayer, Home, this);
        }

        public void BattleButtonClicked()
        {
            Invoke("Battle", 0.5f);
        }

        public void Battle()
        {
            _applicationModel.Mode = GameMode.CoinBattle;
            SaveCoinBattleSettings();

            int maxLevel = _applicationModel.DataProvider.GameModel.NumOfLevelsCompleted; //might need null or not-0 check?
            int levelIndex = UnityEngine.Random.Range(2, maxLevel);
            _screensSceneGod.GoToTrashScreen(levelIndex);
        }

        private void SaveCoinBattleSettings()
        {
            _applicationModel.DataProvider.GameModel.CoinBattle
                = new CoinBattleModel(
                    _applicationModel.DataProvider.SettingsManager.AIDifficulty,
                    _applicationModel.DataProvider.GameModel.PlayerLoadout.Hull
                    );
            _applicationModel.DataProvider.SaveGame();
        }

        public void Home()
        {
            _screensSceneGod.GotoHubScreen();
        }
    }
}
