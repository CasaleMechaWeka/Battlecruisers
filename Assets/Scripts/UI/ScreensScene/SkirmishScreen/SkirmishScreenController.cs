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
            _random = RandomGenerator.Instance;

            battleButton.Initialise(soundPlayer, Battle, this);
            homeButton.Initialise(soundPlayer, Home, this);
            difficultyDropdown.Initialise(FindDefaultDifficulty());
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
            strategyStrings.Insert(0, RANDOM);
            strategyDropdown.Initialise(strategyStrings, FindDefaultStrategy());
        }

        private string FindDefaultStrategy()
        {
            if (Skirmish != null
                && !Skirmish.WasRandomStrategy)
            {
                return Skirmish.AIStrategy.ToString();
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
            if (Skirmish != null
                && !Skirmish.WasRandomAICruiser)
            {
                return Skirmish.AICruiser.PrefabName;
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
            Logging.Log(Tags.SKIRMISH_SCREEN, Skirmish);
            _screensSceneGod.LoadBattleScene();
        }

        private DropdownResult<StrategyType> GetSelectedStrategy()
        {
            string strategyString = strategyDropdown.SelectedValue;
            bool wasRandom = !Enum.TryParse(strategyString, out StrategyType strategy);

            if (wasRandom)
            {
                Logging.Log(Tags.SKIRMISH_SCREEN, $"Choosing random strategy!");
                strategy = RandomGenerator.Instance.RandomItem(_strategies);
            }

            return new DropdownResult<StrategyType>(wasRandom, strategy);
        }

        private DropdownResult<HullKey> GetSelectedCruiser()
        {
            string cruiserString = cruiserDropdown.SelectedValue;
            HullKey cruiserKey = StaticPrefabKeys.Hulls.AllKeysExplicit.FirstOrDefault(key => key.PrefabName == cruiserString);
            bool wasRandom = cruiserKey == null;

            if (wasRandom)
            {
                Logging.Log(Tags.SKIRMISH_SCREEN, $"Choosing random cruiser!");
                cruiserKey = RandomGenerator.Instance.RandomItem(StaticPrefabKeys.Hulls.AllKeysExplicit);
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
                    // FELIX :P
                    false,
                    null,
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