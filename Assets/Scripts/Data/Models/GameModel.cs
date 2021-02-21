using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Data.Models
{
    [Serializable]
    public class GameModel : IGameModel
    {
        public class ModelVersion
        {
            public const int PreShowHelpLabel = 0;
            public const int WithShowHelpLabel = 1;
        }

        [SerializeField]
        private bool _hasAttemptedTutorial;

        [SerializeField]
        private Loadout _playerLoadout;

        [SerializeField]
        private BattleResult _lastBattleResult;

        [SerializeField]
        private List<HullKey> _unlockedHulls;

        [SerializeField]
        private List<BuildingKey> _unlockedBuildings;

        [SerializeField]
        private List<UnitKey> _unlockedUnits;

        [SerializeField]
        private List<CompletedLevel> _completedLevels;

        [SerializeField]
        private SettingsModel _settings;

        [SerializeField]
        private int _selectedLevel;

        [SerializeField]
        private SkirmishModel _skirmish;

        [SerializeField]
        private HotkeysModel _hotkeys;
        public HotkeysModel Hotkeys => _hotkeys;

        [SerializeField]
        private bool _showHelpLabels;

        [SerializeField]
        private int _version;
        public int Version
        {
            get => _version;
            set => _version = value;
        }

        public int NumOfLevelsCompleted => _completedLevels.Count;

        public bool HasAttemptedTutorial
        {
            get { return _hasAttemptedTutorial; }
            set { _hasAttemptedTutorial = value; }
        }

        public bool FirstNonTutorialBattle
            => HasAttemptedTutorial
                && LastBattleResult == null
                && NumOfLevelsCompleted == 0;

        public Loadout PlayerLoadout
        {
            get { return _playerLoadout; }
            set { _playerLoadout = value; }
        }

        public BattleResult LastBattleResult
        {
            get { return _lastBattleResult; }
            set { _lastBattleResult = value; }
        }

        public SettingsModel Settings
        {
            get { return _settings; }
            set { _settings = value; }
        }

        public int SelectedLevel
        {
            get { return _selectedLevel; }
            set
            {
                Assert.IsTrue(value > 0);
                Assert.IsTrue(value <= StaticData.NUM_OF_LEVELS);
                _selectedLevel = value;
            }
        }

        public SkirmishModel Skirmish
        {
            get { return _skirmish; }
            set { _skirmish = value; }
        }

        public bool ShowHelpLabels
        {
            get { return _showHelpLabels; }
            set { _showHelpLabels = value; }
        }

        public ReadOnlyCollection<HullKey> UnlockedHulls { get; }
        public ReadOnlyCollection<BuildingKey> UnlockedBuildings { get; }
        public ReadOnlyCollection<UnitKey> UnlockedUnits { get; }
        public ReadOnlyCollection<CompletedLevel> CompletedLevels { get; }

        public NewItems<HullKey> NewHulls { get; set; }
        public NewItems<BuildingKey> NewBuildings { get; set; }
        public NewItems<UnitKey> NewUnits { get; set; }

        public const int UNSET_SELECTED_LEVEL = -1;

        public GameModel()
        {
            _unlockedHulls = new List<HullKey>();
            UnlockedHulls = _unlockedHulls.AsReadOnly();

            _unlockedBuildings = new List<BuildingKey>();
            UnlockedBuildings = _unlockedBuildings.AsReadOnly();

            _unlockedUnits = new List<UnitKey>();
            UnlockedUnits = _unlockedUnits.AsReadOnly();

            _completedLevels = new List<CompletedLevel>();
            CompletedLevels = _completedLevels.AsReadOnly();

            NewHulls = new NewItems<HullKey>();
            NewBuildings = new NewItems<BuildingKey>();
            NewUnits = new NewItems<UnitKey>();

            Settings = new SettingsModel();
            _hotkeys = new HotkeysModel();
            _selectedLevel = UNSET_SELECTED_LEVEL;
            _skirmish = null;
            _showHelpLabels = true;
        }

        public GameModel(
            bool hasAttemptedTutorial,
            Loadout playerLoadout,
            BattleResult lastBattleResult,
            List<HullKey> unlockedHulls,
            List<BuildingKey> unlockedBuildings,
            List<UnitKey> unlockedUnits)
            : this()
        {
            HasAttemptedTutorial = hasAttemptedTutorial;
            PlayerLoadout = playerLoadout;
            LastBattleResult = lastBattleResult;

            _unlockedHulls.AddRange(unlockedHulls);
            _unlockedBuildings.AddRange(unlockedBuildings);
            _unlockedUnits.AddRange(unlockedUnits);
        }

        public void AddUnlockedHull(HullKey hull)
        {
            if (!_unlockedHulls.Contains(hull))
            {
                _unlockedHulls.Add(hull);
                NewHulls.AddItem(hull);
            }
        }

        public void AddUnlockedBuilding(BuildingKey building)
        {
            if (!_unlockedBuildings.Contains(building))
            {
                _unlockedBuildings.Add(building);
                NewBuildings.AddItem(building);
            }
        }

        public void AddUnlockedUnit(UnitKey unit)
        {
            if (!_unlockedUnits.Contains(unit))
            {
                _unlockedUnits.Add(unit);
                NewUnits.AddItem(unit);
            }
        }

        public void AddCompletedLevel(CompletedLevel completedLevel)
        {
            Assert.IsTrue(completedLevel.LevelNum <= _completedLevels.Count + 1, "Have not completed preceeding level :/");
            Assert.IsTrue(completedLevel.LevelNum > 0);

            if (completedLevel.LevelNum > _completedLevels.Count)
            {
                // First time level has been completed
                _completedLevels.Add(completedLevel);
            }
            else
            {
                // Level has been completed before
                CompletedLevel currentLevel = _completedLevels[completedLevel.LevelNum - 1];

                if (completedLevel.HardestDifficulty > currentLevel.HardestDifficulty)
                {
                    currentLevel.HardestDifficulty = completedLevel.HardestDifficulty;
                }
            }
        }

        public IList<BuildingKey> GetUnlockedBuildings(BuildingCategory buildingCategory)
        {
            return _unlockedBuildings.Where(buildingKey => buildingKey.BuildingCategory == buildingCategory).ToList();
        }

        public IList<UnitKey> GetUnlockedUnits(UnitCategory unitCategory)
        {
            return _unlockedUnits.Where(unitKey => unitKey.UnitCategory == unitCategory).ToList();
        }

        [OnDeserialized()]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            // For backwards compatability, when this class did not have these fields
            if (NewHulls == null)
            {
                NewHulls = new NewItems<HullKey>();
            }
            if (NewBuildings == null)
            {
                NewBuildings = new NewItems<BuildingKey>();
            }
            if (NewUnits == null)
            {
                NewUnits = new NewItems<UnitKey>();
            }
            if (Settings == null)
            {
                Settings = new SettingsModel();
            }
            if (_selectedLevel == default)
            {
                _selectedLevel = UNSET_SELECTED_LEVEL;
            }
            if (_hotkeys == null)
            {
                _hotkeys = new HotkeysModel();
            }

            // FELIX  Test this is the case :)
            // Ensure help label doesn't suddenly appear by default for existing users
            if (_version == ModelVersion.PreShowHelpLabel)
            {
                _showHelpLabels = false;
                _version = ModelVersion.WithShowHelpLabel;
            }
        }

        public override bool Equals(object obj)
        {
            GameModel other = obj as GameModel;

            return other != null
                && other.HasAttemptedTutorial == HasAttemptedTutorial
                && other.NumOfLevelsCompleted == NumOfLevelsCompleted
                && other.SelectedLevel == SelectedLevel
                && other.ShowHelpLabels == ShowHelpLabels
                && PlayerLoadout.SmartEquals(other.PlayerLoadout)
                && LastBattleResult.SmartEquals(other.LastBattleResult)
                && Settings.SmartEquals(other.Settings)
                && Hotkeys.SmartEquals(other.Hotkeys)
                && Skirmish.SmartEquals(other.Skirmish)
                && NewHulls.SmartEquals(other.NewHulls)
                && NewBuildings.SmartEquals(other.NewBuildings)
                && NewUnits.SmartEquals(other.NewUnits)
                && Enumerable.SequenceEqual(UnlockedHulls, other.UnlockedHulls)
                && Enumerable.SequenceEqual(UnlockedBuildings, other.UnlockedBuildings)
                && Enumerable.SequenceEqual(UnlockedUnits, other.UnlockedUnits)
                && Enumerable.SequenceEqual(CompletedLevels, other.CompletedLevels);
        }

        public override int GetHashCode()
        {
            return this.GetHashCode(
                HasAttemptedTutorial, 
                NumOfLevelsCompleted, 
                SelectedLevel,
                ShowHelpLabels,
                PlayerLoadout, 
                LastBattleResult, 
                Settings,
                Hotkeys,
                Skirmish,
                _unlockedHulls, 
                _unlockedUnits, 
                _unlockedBuildings, 
                _completedLevels,
                NewHulls,
                NewBuildings,
                NewUnits);
        }

        public bool IsUnitUnlocked(UnitKey unitKey)
        {
            return UnlockedUnits.Contains(unitKey);
        }

        public bool IsBuildingUnlocked(BuildingKey buildingKey)
        {
            return UnlockedBuildings.Contains(buildingKey);
        }
    }
}
