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
using BattleCruisers.Utils.Localisation;

namespace BattleCruisers.UI.ScreensScene.SkirmishScreen
{
    public class SkirmishScreenController : ScreenController
    {
        private IApplicationModel _applicationModel;
        private IRandomGenerator _random;
        private StrategyType[] _strategies;

        private ISkirmishModel Skirmish => _applicationModel.DataProvider.GameModel.Skirmish;

        private string _randomDropdownEntry;

        public CanvasGroupButton battleButton, homeButton;
        public DifficultyDropdown difficultyDropdown;
        public StringDropdown strategyDropdown, cruiserDropdown;

        public void Initialise(
            IScreensSceneGod screensSceneGod, 
            IApplicationModel applicationModel,
            ISingleSoundPlayer soundPlayer,
            ILocTable commonStrings,
            ILocTable screensSceneStrings)
        {
            base.Initialise(screensSceneGod);

            Helper.AssertIsNotNull(battleButton, homeButton, difficultyDropdown, strategyDropdown, cruiserDropdown);
            Helper.AssertIsNotNull(applicationModel, soundPlayer, commonStrings, screensSceneStrings);

            _applicationModel = applicationModel;
            _random = RandomGenerator.Instance;
            _randomDropdownEntry = screensSceneStrings.GetString("UI/SkirmishScreen/RandomDropdownEntry");

            battleButton.Initialise(soundPlayer, Battle, this);
            homeButton.Initialise(soundPlayer, Home, this);
            difficultyDropdown.Initialise(FindDefaultDifficulty(), commonStrings);
            InitialiseStrategyDropdown();
            InitialiseCruiserDropdown();
        }

        private Difficulty FindDefaultDifficulty()
        {
            if (Skirmish != null)
            {
                return Skirmish.Difficulty;
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
            strategyStrings.Insert(0, _randomDropdownEntry);
            strategyDropdown.Initialise(strategyStrings, FindDefaultStrategy());
        }

        private string FindDefaultStrategy()
        {
            if (Skirmish != null)
            {
                return Skirmish.AIStrategy.ToString();
            }
            else
            {
                return _randomDropdownEntry;
            }
        }

        private void InitialiseCruiserDropdown()
        {
            // FELIX  Initialise prefab, to get localised name
            // FELIX  Remove IPrefabKey.PrefabName
            IList<string> hullNames
                = StaticPrefabKeys.Hulls.AllKeys
                    .Select(key => key.PrefabName)
                    .ToList();
            hullNames.Insert(0, _randomDropdownEntry);
            cruiserDropdown.Initialise(hullNames, FindDefaultCruiser());
        }

        private string FindDefaultCruiser()
        {
            if (Skirmish != null)
            {
                return Skirmish.AICruiser.PrefabName;
            }
            else
            {
                return _randomDropdownEntry;
            }
        }

        public void Battle()
        {
            _applicationModel.Mode = GameMode.Skirmish;
            SaveSkirmishSettings();
            Logging.Log(Tags.SKIRMISH_SCREEN, Skirmish);
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
            int backgroundLevelNum = _random.Range(1, StaticData.NUM_OF_LEVELS);
            string skyMaterialName = _random.RandomItem(SkyMaterials.All);


            _applicationModel.DataProvider.GameModel.Skirmish
                = new SkirmishModel(
                    difficultyDropdown.Difficulty,
                    GetSelectedCruiser(),
                    GetSelectedStrategy(),
                    backgroundLevelNum,
                    skyMaterialName);
            _applicationModel.DataProvider.SaveGame();
        }
    }
}