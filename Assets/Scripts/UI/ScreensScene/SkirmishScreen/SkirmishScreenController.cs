using BattleCruisers.Data;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Settings;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Static;
using BattleCruisers.Data.Static.Strategies.Helper;
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
            difficultyDropdown.Initialise(FindDefaultDifficulty());
            InitialiseStrategyDropdown();
            InitialiseCruiserDropdown();
        }

        private Difficulty FindDefaultDifficulty()
        {
            if (_applicationModel.Skirmish != null)
            {
                return _applicationModel.Skirmish.Difficulty;
            }
            else
            {
                return _applicationModel.DataProvider.SettingsManager.AIDifficulty;
            }
        }

        private void InitialiseStrategyDropdown()
        {
            _strategies = (StrategyType[])Enum.GetValues(typeof(StrategyType));
            IList<string> strategyStrings
                = _strategies
                    .Select(strategy => strategy.ToString())
                    .ToList();
            strategyStrings.Insert(0, RANDOM);
            strategyDropdown.Initialise(strategyStrings, FindDefaultStrategy());
        }

        private string FindDefaultStrategy()
        {
            if (_applicationModel.Skirmish != null)
            {
                return _applicationModel.Skirmish.AIStrategy.ToString();
            }
            else
            {
                return RANDOM;
            }
        }

        private void InitialiseCruiserDropdown()
        {
            IList<string> hullNames
                = StaticPrefabKeys.Hulls.AllKeys
                    .Select(key => key.PrefabName)
                    .ToList();
            hullNames.Insert(0, RANDOM);
            cruiserDropdown.Initialise(hullNames, FindDefaultCruiser());
        }

        private string FindDefaultCruiser()
        {
            if (_applicationModel.Skirmish != null)
            {
                return _applicationModel.Skirmish.AICruiser.PrefabName;
            }
            else
            {
                return RANDOM;
            }
        }

        public void Battle()
        {
            _applicationModel.Mode = GameMode.Skirmish;
            SaveSkirmishSettings();
            Logging.Log(Tags.SKIRMISH_SCREEN, _applicationModel.Skirmish);
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
                Logging.Log(Tags.SKIRMISH_SCREEN, $"Choosing random strategy!");
                return RandomGenerator.Instance.RandomItem(_strategies);
            }
        }

        private HullKey GetSelectedCruiser()
        {
            string cruiserString = cruiserDropdown.SelectedValue;
            HullKey cruiserKey = StaticPrefabKeys.Hulls.AllKeysExplicit.FirstOrDefault(key => key.PrefabName == cruiserString);
            
            if (cruiserKey != null)
            {
                return cruiserKey;
            }
            else
            {
                Logging.Log(Tags.SKIRMISH_SCREEN, $"Choosing random cruiser!");
                return RandomGenerator.Instance.RandomItem(StaticPrefabKeys.Hulls.AllKeysExplicit);
            }
        }

        public override void Cancel()
        {
            Home();
        }

        public void Home()
        {
            SaveSkirmishSettings();
            _screensSceneGod.GoToHomeScreen();
        }


        private void SaveSkirmishSettings()
        {
            _applicationModel.Skirmish
                = new SkirmishModel(
                    difficultyDropdown.Difficulty,
                    GetSelectedCruiser(),
                    GetSelectedStrategy());
        }
    }
}