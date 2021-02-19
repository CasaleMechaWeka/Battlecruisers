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
        private StrategyType[] _strategies;

        private const string RANDOM = "Random";

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
            _strategies = (StrategyType[])Enum.GetValues(typeof(StrategyType));
            IList<string> strategyStrings
                = _strategies
                    .Select(strategy => strategy.ToString())
                    .ToList();
            strategyStrings.Insert(0, RANDOM);
            strategyDropdown.Initialise(strategyStrings, RANDOM);
        }

        private void InitialiseCruiserDropdown()
        {
            IList<string> hullNames
                = StaticPrefabKeys.Hulls.AllKeys
                    .Select(key => key.PrefabName)
                    .ToList();
            hullNames.Insert(0, RANDOM);
            // FELIX  Want to use last used skirmish settings => Don't wipe skirmish settings in post battle!
            cruiserDropdown.Initialise(hullNames, RANDOM);
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
            bool result = Enum.TryParse(strategyString, out StrategyType strategy);

            if (result)
            {
                return strategy;
            }
            else
            {
                return RandomGenerator.Instance.RandomItem(_strategies);
            }
        }

        private IPrefabKey GetSelectedCruiser()
        {
            string cruiserString = cruiserDropdown.SelectedValue;
            IPrefabKey cruiserKey = StaticPrefabKeys.Hulls.AllKeys.FirstOrDefault(key => key.PrefabName == cruiserString);
            
            if (cruiserKey != null)
            {
                return cruiserKey;
            }
            else
            {
                return RandomGenerator.Instance.RandomItem(StaticPrefabKeys.Hulls.AllKeys);
            }
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