using BattleCruisers.Data;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Settings;
using BattleCruisers.Data.Skirmishes;
using BattleCruisers.Data.Static;
using BattleCruisers.Data.Static.Strategies;
using BattleCruisers.Scenes;
using BattleCruisers.UI.ScreensScene.SettingsScreen;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleCruisers.UI.ScreensScene.SkirmishScreen
{
    public class SkirmishScreenController : ScreenController
    {
        private IApplicationModel _applicationModel;

        public CanvasGroupButton battleButton, homeButton;
        public DifficultyDropdown difficultyDropdown;
        public StringDropdown strategyDropdown, cruiserDropdown;

        public void Initialise(
            IScreensSceneGod screensSceneGod, 
            IApplicationModel applicationModel,
            ISingleSoundPlayer soundPlayer)
        {
            base.Initialise(screensSceneGod);

            Helper.AssertIsNotNull(battleButton, homeButton, difficultyDropdown, strategyDropdown, cruiserDropdown);
            Helper.AssertIsNotNull(applicationModel, soundPlayer);

            _applicationModel = applicationModel;

            battleButton.Initialise(soundPlayer, Battle, this);
            homeButton.Initialise(soundPlayer, Home, this);
            difficultyDropdown.Initialise(applicationModel.DataProvider.GameModel.Settings.AIDifficulty);
            InitialiseStrategyDropdown();
            InitialiseCruiserDropdown();
        }

        private void InitialiseStrategyDropdown()
        {
            StrategyType[] strategies = (StrategyType[])Enum.GetValues(typeof(StrategyType));
            IList<string> startegyStrings
                = strategies
                    .Select(strategy => strategy.ToString())
                    .ToList();
            strategyDropdown.Initialise(startegyStrings, StrategyType.Balanced.ToString());
        }

        private void InitialiseCruiserDropdown()
        {
            IList<string> hullNames
                            = StaticPrefabKeys.Hulls.AllKeys
                                .Select(key => key.PrefabName)
                                .ToList();
            // FELIX  Want to use last used skirmish settings => Don't wipe skirmish settings in post battle!
            cruiserDropdown.Initialise(hullNames, StaticPrefabKeys.Hulls.AllKeys[0].PrefabName);
        }

        public void Battle()
        {
            _applicationModel.Mode = GameMode.Skirmish;
            _applicationModel.Skirmish
                = new Skirmish(
                    difficultyDropdown.Difficulty,
                    GetSelectedCruiser(),
                    GetSelectedStrategy());
            _screensSceneGod.LoadBattleScene();
        }

        private StrategyType GetSelectedStrategy()
        {
            string strategyString = strategyDropdown.SelectedValue;
            Enum.TryParse(strategyString, out StrategyType strategy);
            return strategy;
        }

        private IPrefabKey GetSelectedCruiser()
        {
            string cruiserString = cruiserDropdown.SelectedValue;
            IPrefabKey cruiserKey = StaticPrefabKeys.Hulls.AllKeys.FirstOrDefault(key => key.PrefabName == cruiserString);
            return cruiserKey ?? StaticPrefabKeys.Hulls.Bullshark;
        }

        public void Home()
        {
            _screensSceneGod.GoToHomeScreen();
        }

        public override void Cancel()
        {
            Home();
        }
    }
}