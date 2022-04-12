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
        private IList<HullKey> _unlockedHulls;
        private IRandomGenerator _random;
        private StrategyType[] _strategies;

        private ISkirmishModel Skirmish => _applicationModel.DataProvider.GameModel.Skirmish;

        private string _randomDropdownEntry;

        public CanvasGroupButton battleButton, homeButton;
        public DifficultyDropdown difficultyDropdown;
        public StringDropdown strategyDropdown, playerCruiserDropdown, aiCruiserDropdown;

        public void Initialise(
            IScreensSceneGod screensSceneGod, 
            IApplicationModel applicationModel,
            ISingleSoundPlayer soundPlayer,
            ILocTable commonStrings,
            ILocTable screensSceneStrings,
            IPrefabFactory prefabFactory)
        {
            base.Initialise(screensSceneGod);

            Helper.AssertIsNotNull(battleButton, homeButton, difficultyDropdown, strategyDropdown, playerCruiserDropdown, aiCruiserDropdown);
            Helper.AssertIsNotNull(applicationModel, soundPlayer, commonStrings, screensSceneStrings, prefabFactory);

            _applicationModel = applicationModel;
            _unlockedHulls = applicationModel.DataProvider.GameModel.UnlockedHulls;
            _random = RandomGenerator.Instance;
            _randomDropdownEntry = screensSceneStrings.GetString("UI/SkirmishScreen/RandomDropdownEntry");

            battleButton.Initialise(soundPlayer, BattleButtonClicked, this);
            homeButton.Initialise(soundPlayer, Home, this);
            difficultyDropdown.Initialise(FindDefaultDifficulty(), commonStrings);
            InitialiseStrategyDropdown(commonStrings);
            InitialiseCruiserDropdown(playerCruiserDropdown, prefabFactory, FindDefaultPlayerCruiser(prefabFactory));
            InitialiseCruiserDropdown(aiCruiserDropdown, prefabFactory, FindDefaultAICruiser(prefabFactory));
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

        private void InitialiseCruiserDropdown(StringDropdown dropdown, IPrefabFactory prefabFactory, string defaultCruiser)
        {
            IList<string> hullNames = new List<string>();

            foreach (HullKey hull in _unlockedHulls)
            {
                // Use cruiser prefab name, as this has been localised
                ICruiser cruiser = prefabFactory.GetCruiserPrefab(hull);
                hullNames.Add(cruiser.Name);
            }

            hullNames.Insert(0, _randomDropdownEntry);
            dropdown.Initialise(hullNames, defaultCruiser);
        }

        private string FindDefaultPlayerCruiser(IPrefabFactory prefabFactory)
        {
            if (Skirmish != null
                && !Skirmish.WasRandomPlayerCruiser)
            {
                ICruiser cruiser = prefabFactory.GetCruiserPrefab(Skirmish.PlayerCruiser);
                return cruiser.Name;
            }
            else
            {
                return _randomDropdownEntry;
            }
        }

        private string FindDefaultAICruiser(IPrefabFactory prefabFactory)
        {
            if (Skirmish != null
                && !Skirmish.WasRandomAICruiser)
            {
                ICruiser cruiser = prefabFactory.GetCruiserPrefab(Skirmish.AICruiser);
                return cruiser.Name;
            }
            else
            {
                return _randomDropdownEntry;
            }
        }

        public void BattleButtonClicked()
        {
            Invoke("Battle", 0.5f);
        }
        public void Battle()
        {
            _applicationModel.Mode = GameMode.Skirmish;
            SaveSkirmishSettings();
            Logging.Log(Tags.SKIRMISH_SCREEN, Skirmish);
            _screensSceneGod.LoadBattleScene();
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

            DropdownResult<HullKey> playerCruiserResult = GetSelectedCruiser(playerCruiserDropdown);
            DropdownResult<HullKey> aiCruiserResult = GetSelectedCruiser(aiCruiserDropdown);
            DropdownResult<StrategyType> strategyResult = GetSelectedStrategy();

            _applicationModel.DataProvider.GameModel.Skirmish
                = new SkirmishModel(
                    difficultyDropdown.Difficulty,
                    playerCruiserResult.WasRandom,
                    playerCruiserResult.Result,
                    aiCruiserResult.WasRandom,
                    aiCruiserResult.Result,
                    strategyResult.WasRandom,
                    strategyResult.Result,
                    backgroundLevelNum);
            _applicationModel.DataProvider.SaveGame();
        }

        private DropdownResult<HullKey> GetSelectedCruiser(StringDropdown cruiserDropdown)
        {
            // First entry is "Random"
            int adjustedIndex = cruiserDropdown.SelectedIndex - 1;
            bool wasRandom = adjustedIndex < 0;
            HullKey cruiserKey;

            if (wasRandom)
            {
                Logging.Log(Tags.SKIRMISH_SCREEN, $"Choosing random cruiser!");
                cruiserKey = RandomGenerator.Instance.RandomItem(_unlockedHulls);
            }
            else
            {
                Assert.IsTrue(adjustedIndex < _unlockedHulls.Count);
                cruiserKey = _unlockedHulls[adjustedIndex];
            }

            return new DropdownResult<HullKey>(wasRandom, cruiserKey);
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
    }
}