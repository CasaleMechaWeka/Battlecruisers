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

namespace BattleCruisers.UI.ScreensScene.SkirmishScreen
{
    public class DropdownResult<TItem>
    {
        public bool WasRandom { get; }
        public TItem Result { get; }

        public DropdownResult(bool wasRandom, TItem result)
        {
            WasRandom = wasRandom;
            Result = result;
        }
    }

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
            ILocTable screensSceneStrings,
            IPrefabFactory prefabFactory)
        {
            base.Initialise(screensSceneGod);

            Helper.AssertIsNotNull(battleButton, homeButton, difficultyDropdown, strategyDropdown, cruiserDropdown);
            Helper.AssertIsNotNull(applicationModel, soundPlayer, commonStrings, screensSceneStrings, prefabFactory);

            _applicationModel = applicationModel;
            _random = RandomGenerator.Instance;
            _randomDropdownEntry = screensSceneStrings.GetString("UI/SkirmishScreen/RandomDropdownEntry");

            battleButton.Initialise(soundPlayer, Battle, this);
            homeButton.Initialise(soundPlayer, Home, this);
            difficultyDropdown.Initialise(FindDefaultDifficulty(), commonStrings);
            InitialiseStrategyDropdown(commonStrings);
            InitialiseCruiserDropdown(prefabFactory);
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

        private void InitialiseStrategyDropdown(ILocTable commonStrings)
        {
            _strategies = (StrategyType[])Enum.GetValues(typeof(StrategyType));
            string initialValue = _randomDropdownEntry;
            IList<string> strategyStrings = new List<string>();

            foreach (StrategyType strategy in _strategies)
            {
                string key = EnumKeyCreator.CreateKey(strategy);
                string strategyString = commonStrings.GetString(key);
                strategyStrings.Add(strategyString);

                if (Skirmish != null
                    && !Skirmish.WasRandomStrategy
                    && Skirmish.AIStrategy == strategy)
                {
                    initialValue = strategyString;
                }
            }

            strategyStrings.Insert(0, _randomDropdownEntry);
            strategyDropdown.Initialise(strategyStrings, initialValue);
        }

        private void InitialiseCruiserDropdown(IPrefabFactory prefabFactory)
        {
            string initialValue = _randomDropdownEntry;
            IList<string> hullNames = new List<string>();

            foreach (HullKey hull in StaticPrefabKeys.Hulls.AllKeysExplicit)
            {
                // Use cruiser prefab name, as this has been localised
                ICruiser cruiser = prefabFactory.GetCruiserPrefab(hull);
                hullNames.Add(cruiser.Name);

                if (Skirmish != null
                    && !Skirmish.WasRandomCruiser
                    && hull.Equals(Skirmish.AICruiser))
                {
                    initialValue = cruiser.Name;
                }
            }

            hullNames.Insert(0, _randomDropdownEntry);
            cruiserDropdown.Initialise(hullNames, initialValue);
        }

        public void Battle()
        {
            _applicationModel.Mode = GameMode.Skirmish;
            SaveSkirmishSettings();
            Logging.Log(Tags.SKIRMISH_SCREEN, Skirmish);
            _screensSceneGod.LoadBattleScene();
        }

        private DropdownResult<StrategyType> GetSelectedStrategy()
        {
            // First entry is "Random"
            int adjustedIndex = strategyDropdown.SelectedIndex - 1;
            bool wasRandom = adjustedIndex < 0;
            StrategyType strategy;

            if (wasRandom)
            {
                Logging.Log(Tags.SKIRMISH_SCREEN, $"Choosing random strategy!");
                strategy = RandomGenerator.Instance.RandomItem(_strategies);
            }
            else
            {
                Assert.IsTrue(adjustedIndex < _strategies.Length);
                strategy = _strategies[adjustedIndex];
            }

            return new DropdownResult<StrategyType>(wasRandom, strategy);
        }

        private DropdownResult<HullKey> GetSelectedCruiser()
        {
            // First entry is "Random"
            int adjustedIndex = cruiserDropdown.SelectedIndex - 1;
            bool wasRandom = adjustedIndex < 0;
            HullKey cruiserKey;

            if (wasRandom)
            {
                Logging.Log(Tags.SKIRMISH_SCREEN, $"Choosing random cruiser!");
                cruiserKey = RandomGenerator.Instance.RandomItem(StaticPrefabKeys.Hulls.AllKeysExplicit);
            }
            else
            {
                Assert.IsTrue(adjustedIndex < StaticPrefabKeys.Hulls.AllKeysExplicit.Count);
                cruiserKey = StaticPrefabKeys.Hulls.AllKeysExplicit[adjustedIndex];
            }

            return new DropdownResult<HullKey>(wasRandom, cruiserKey);
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

            DropdownResult<HullKey> cruiserResult = GetSelectedCruiser();
            DropdownResult<StrategyType> strategyResult = GetSelectedStrategy();

            _applicationModel.DataProvider.GameModel.Skirmish
                = new SkirmishModel(
                    difficultyDropdown.Difficulty,
                    cruiserResult.WasRandom,
                    cruiserResult.Result,
                    strategyResult.WasRandom,
                    strategyResult.Result,
                    backgroundLevelNum,
                    skyMaterialName);
            _applicationModel.DataProvider.SaveGame();
        }
    }
}