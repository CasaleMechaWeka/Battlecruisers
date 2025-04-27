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
using System.Linq;
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
        private IList<HullKey> _unlockedHulls;
        private List<HullKey> _playableHulls;   //these also include the boss hulls
        private StrategyType[] _strategies;

        private ISkirmishModel Skirmish => DataProvider.GameModel.Skirmish;

        private string _randomDropdownEntry;

        public CanvasGroupButton battleButton, homeButton;
        public DifficultyDropdown difficultyDropdown;
        public StringDropdown strategyDropdown, playerCruiserDropdown, aiCruiserDropdown;

        public void Initialise(
            IScreensSceneGod screensSceneGod,
            ISingleSoundPlayer soundPlayer)
        {
            base.Initialise(screensSceneGod);

            Helper.AssertIsNotNull(battleButton, homeButton, difficultyDropdown, strategyDropdown, playerCruiserDropdown, aiCruiserDropdown);
            Helper.AssertIsNotNull(soundPlayer);

            _unlockedHulls = DataProvider.GameModel.UnlockedHulls.ToList();
            _playableHulls = _unlockedHulls.ToList();

            if (DataProvider.GameModel.CompletedLevels.Count >= 15)
                _playableHulls.Add(StaticPrefabKeys.Hulls.ManOfWarBoss);
            if (DataProvider.GameModel.CompletedLevels.Count >= 31)
                _playableHulls.Add(StaticPrefabKeys.Hulls.HuntressBoss);
            if (DataProvider.GameModel.CompletedSideQuests != null
                && DataProvider.GameModel.CompletedSideQuests.ToArray().Select(sideQuest => sideQuest.LevelNum).Contains(23))
                _playableHulls.Add(StaticPrefabKeys.Hulls.FortressPrime);

            _randomDropdownEntry = LocTableCache.ScreensSceneTable.GetString("UI/SkirmishScreen/RandomDropdownEntry");

            battleButton.Initialise(soundPlayer, Battle, this);
            homeButton.Initialise(soundPlayer, Home, this);
            difficultyDropdown.Initialise(FindDefaultDifficulty());
            InitialiseStrategyDropdown();
            InitialiseCruiserDropdown(playerCruiserDropdown, FindDefaultPlayerCruiser());
            InitialiseCruiserDropdown(aiCruiserDropdown, FindDefaultAICruiser());
        }

        private Difficulty FindDefaultDifficulty()
        {
            if (Skirmish != null)
            {
                return Skirmish.Difficulty;
            }
            else
            {
                return DataProvider.SettingsManager.AIDifficulty;
            }
        }

        private void InitialiseStrategyDropdown()
        {
            _strategies = (StrategyType[])Enum.GetValues(typeof(StrategyType));
            string initialValue = _randomDropdownEntry;
            IList<string> strategyStrings = new List<string>();

            foreach (StrategyType strategy in _strategies)
            {
                string key = EnumKeyCreator.CreateKey(strategy);
                string strategyString = LocTableCache.CommonTable.GetString(key);
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

        private void InitialiseCruiserDropdown(StringDropdown dropdown, string defaultCruiser)
        {
            IList<string> hullNames = new List<string>();

            foreach (HullKey hull in _playableHulls)
            {
                // Use cruiser prefab name, as this has been localised
                ICruiser cruiser = PrefabFactory.GetCruiserPrefab(hull);
                hullNames.Add(cruiser.Name);
            }

            hullNames.Insert(0, _randomDropdownEntry);
            dropdown.Initialise(hullNames, defaultCruiser);
        }

        private string FindDefaultPlayerCruiser()
        {
            if (Skirmish != null
                && !Skirmish.WasRandomPlayerCruiser)
            {
                ICruiser cruiser = PrefabFactory.GetCruiserPrefab(Skirmish.PlayerCruiser);
                return cruiser.Name;
            }
            else
            {
                return _randomDropdownEntry;
            }
        }

        private string FindDefaultAICruiser()
        {
            if (Skirmish != null
                && !Skirmish.WasRandomAICruiser)
            {
                ICruiser cruiser = PrefabFactory.GetCruiserPrefab(Skirmish.AICruiser);
                return cruiser.Name;
            }
            else
            {
                return _randomDropdownEntry;
            }
        }

        public void Battle()
        {
            ApplicationModel.Mode = GameMode.Skirmish;
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
            _screensSceneGod.GotoHubScreen();
        }

        private void SaveSkirmishSettings()
        {
            int backgroundLevelNum = RandomGenerator.Range(1, StaticData.NUM_OF_LEVELS);

            DropdownResult<HullKey> playerCruiserResult = GetSelectedCruiser(playerCruiserDropdown);

            if (playerCruiserResult.Result != DataProvider.GameModel.PlayerLoadout.Hull)
                DataProvider.GameModel.PlayerLoadout.SelectedBodykit = -1;

            DropdownResult<HullKey> aiCruiserResult = GetSelectedCruiser(aiCruiserDropdown);
            DropdownResult<StrategyType> strategyResult = GetSelectedStrategy();

            DataProvider.GameModel.Skirmish
                = new SkirmishModel(
                    difficultyDropdown.Difficulty,
                    playerCruiserResult.WasRandom,
                    playerCruiserResult.Result,
                    aiCruiserResult.WasRandom,
                    aiCruiserResult.Result,
                    strategyResult.WasRandom,
                    strategyResult.Result,
                    backgroundLevelNum);
            DataProvider.SaveGame();
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
                cruiserKey = RandomGenerator.RandomItem(_unlockedHulls);
            }
            else
            {
                Assert.IsTrue(adjustedIndex < _playableHulls.Count);
                cruiserKey = _playableHulls[adjustedIndex];
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
                strategy = RandomGenerator.RandomItem(_strategies);
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