using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Data.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models
{
    [Serializable]
    public class GameModel : IPvPGameModel, IPvPVoyageModel, IPvPPlayerModel
    {
        private int _voyageNumber;
        public int VoyageNumber
        {
            get => _voyageNumber;
            set => _voyageNumber = value;
        }

        // Voyage properties

        private int _legNumber;
        public int LegNumber
        {
            get => _legNumber;
            set => _legNumber = value;
        }

        private int _battleNumber;
        public int BattleNumber
        {
            get => _battleNumber;
            set => _battleNumber = value;
        }

        private int _battlesWon;
        public int BattlesWon
        {
            get => _battlesWon;
            set => _battlesWon = value;
        }

        private bool _voyageInProgress;
        public bool VoyageInProgress
        {
            get => _voyageInProgress;
            set => _voyageInProgress = value;
        }

        // Player properties


        private int _totalUpgrades;
        public int TotalUpgrades
        {
            get => _totalUpgrades;
            set => _totalUpgrades = value;
        }


        private int _totalPerks;
        public int TotalPerks
        {
            get => _totalPerks;
            set => _totalPerks = value;
        }


        private int _totalBuildables;
        public int TotalBuildables
        {
            get => _totalBuildables;
            set => _totalBuildables = value;
        }


        private int _playerBounty;
        public int PlayerBounty
        {
            get => _playerBounty;
            set => _playerBounty = value;
        }


        private int _playerLevel;
        public int PlayerLevel
        {
            get => _playerLevel;
            set => _playerLevel = value;
        }


        private int _currentLuck;
        public int CurrentLuck
        {
            get => _currentLuck;
            set => _currentLuck = value;
        }


        private int _baseLuck;
        public int BaseLuck
        {
            get => _baseLuck;
            set => _baseLuck = value;
        }


        private int _extraDrones;
        public int ExtraDrones
        {
            get => _extraDrones;
            set => _extraDrones = value;
        }


        private int _currentHP;
        public int CurrentHP
        {
            get => _currentHP;
            set => _currentHP = value;
        }


        private int _maxHP;

        public int MaxHP
        {
            get => _maxHP;
            set => _maxHP = value;
        }


        private int _credits;
        public int Credits
        {
            get => _credits;
            set => _credits = value;
        }


        private int _totalVoyages;
        public int TotalVoyages
        {
            get => _totalVoyages;
            set => _totalVoyages = value;
        }

        // Pre-Rogue stuff

        public class ModelVersion
        {
            public const int PreShowHelpLabel = 0;
            public const int WithShowHelpLabel = 1;
            public const int RemovedShowHelpLabel = 2;// Voyage properties

            private int _stageNumber;
            public int StageNumber;


        }

        [SerializeField]
        private bool _hasAttemptedTutorial;

        [SerializeField]
        private PvPLoadout _playerLoadout;

        [SerializeField]
        private PvPBattleResult _lastBattleResult;

        [SerializeField]
        private List<PvPHullKey> _unlockedHulls;

        [SerializeField]
        private List<PvPBuildingKey> _unlockedBuildings;

        [SerializeField]
        private List<PvPUnitKey> _unlockedUnits;

        [SerializeField]
        private List<PvPCompletedLevel> _completedLevels;

        [SerializeField]
        private long _lifetimeDestructionScore;
        public long LifetimeDestructionScore
        {
            get => _lifetimeDestructionScore;
            set => _lifetimeDestructionScore = value;
        }

        private bool _PremiumEdition;
        public bool PremiumEdition
        {
            get => _PremiumEdition;
            set => _PremiumEdition = value;
        }

        [SerializeField]
        private long _bestDestructionScore;
        public long BestDestructionScore
        {
            get => _bestDestructionScore;
            set => _bestDestructionScore = value;
        }

        [SerializeField]
        private SettingsModel _settings;

        [SerializeField]
        private int _selectedLevel;

        [SerializeField]
        private PvPSkirmishModel _skirmish;

        [SerializeField]
        private PvPHotkeysModel _hotkeys;
        public PvPHotkeysModel Hotkeys => _hotkeys;

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

        public PvPLoadout PlayerLoadout
        {
            get { return _playerLoadout; }
            set { _playerLoadout = value; }
        }

        public PvPBattleResult LastBattleResult
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

        public PvPSkirmishModel Skirmish
        {
            get { return _skirmish; }
            set { _skirmish = value; }
        }

        public ReadOnlyCollection<PvPHullKey> UnlockedHulls { get; }
        public ReadOnlyCollection<PvPBuildingKey> UnlockedBuildings { get; }
        public ReadOnlyCollection<PvPUnitKey> UnlockedUnits { get; }
        public ReadOnlyCollection<PvPCompletedLevel> CompletedLevels { get; }

        public PvPNewItems<PvPHullKey> NewHulls { get; set; }
        public PvPNewItems<PvPBuildingKey> NewBuildings { get; set; }
        public PvPNewItems<PvPUnitKey> NewUnits { get; set; }

        public const int UNSET_SELECTED_LEVEL = -1;

        public GameModel()
        {
            _unlockedHulls = new List<PvPHullKey>();
            UnlockedHulls = _unlockedHulls.AsReadOnly();

            _unlockedBuildings = new List<PvPBuildingKey>();
            UnlockedBuildings = _unlockedBuildings.AsReadOnly();

            _unlockedUnits = new List<PvPUnitKey>();
            UnlockedUnits = _unlockedUnits.AsReadOnly();

            _completedLevels = new List<PvPCompletedLevel>();
            CompletedLevels = _completedLevels.AsReadOnly();

            NewHulls = new PvPNewItems<PvPHullKey>();
            NewBuildings = new PvPNewItems<PvPBuildingKey>();
            NewUnits = new PvPNewItems<PvPUnitKey>();

            Settings = new SettingsModel();
            _hotkeys = new PvPHotkeysModel();
            _selectedLevel = UNSET_SELECTED_LEVEL;
            _skirmish = null;
        }

        public GameModel(
            bool hasAttemptedTutorial,
            long lifetimeDestructionScore,
            long bestDestructionScore,
            PvPLoadout playerLoadout,
            PvPBattleResult lastBattleResult,
            List<PvPHullKey> unlockedHulls,
            List<PvPBuildingKey> unlockedBuildings,
            List<PvPUnitKey> unlockedUnits)
            : this()
        {
            HasAttemptedTutorial = hasAttemptedTutorial;
            LifetimeDestructionScore = lifetimeDestructionScore;
            BestDestructionScore = bestDestructionScore;
            PlayerLoadout = playerLoadout;
            LastBattleResult = lastBattleResult;

            _unlockedHulls.AddRange(unlockedHulls);
            _unlockedBuildings.AddRange(unlockedBuildings);
            _unlockedUnits.AddRange(unlockedUnits);
        }

        public Dictionary<string, object> Analytics(string gameModeString, string type, bool lastSkirmishResult)
        {
            int campaignDifficulty = 0;
            if (Settings != null) { campaignDifficulty = (int)Settings.AIDifficulty; }
            int skirmishDifficulty = 0;
            if (_skirmish != null) { campaignDifficulty = (int)_skirmish.Difficulty; }

            int levelNumber = 0;
            bool wasVictory = false;
            if (LastBattleResult != null)
            {
                levelNumber = LastBattleResult.LevelNum;
                wasVictory = LastBattleResult.WasVictory;
            }
            return new Dictionary<string, object>() { { "gameMode", gameModeString },
                                            { "Analytics_Type", type },
                                            { "selectedLevel", SelectedLevel },
                                            { "campaign_Difficulty", campaignDifficulty },
                                            { "lastCampaign_Level", levelNumber },
                                            { "lastCampaign_Result", wasVictory },
                                            { "lastSkirmish_Result", lastSkirmishResult },
                                            { "lastSkirmish_Difficulty", skirmishDifficulty },
                                            { "LifetimeDestructionScore", LifetimeDestructionScore },
                                            { "BestDestructionScore", BestDestructionScore },
                                            { "HasAttemptedTutorial", HasAttemptedTutorial },
                                          };
        }

        public void AddUnlockedHull(PvPHullKey hull)
        {
            if (!_unlockedHulls.Contains(hull))
            {
                _unlockedHulls.Add(hull);
                NewHulls.AddItem(hull);
            }
        }

        public void AddUnlockedBuilding(PvPBuildingKey building)
        {
            if (!_unlockedBuildings.Contains(building))
            {
                _unlockedBuildings.Add(building);
                NewBuildings.AddItem(building);
            }
        }

        public void AddUnlockedUnit(PvPUnitKey unit)
        {
            if (!_unlockedUnits.Contains(unit))
            {
                _unlockedUnits.Add(unit);
                NewUnits.AddItem(unit);
            }
        }

        public void AddCompletedLevel(PvPCompletedLevel completedLevel)
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
                PvPCompletedLevel currentLevel = _completedLevels[completedLevel.LevelNum - 1];

                if (completedLevel.HardestDifficulty > currentLevel.HardestDifficulty)
                {
                    currentLevel.HardestDifficulty = completedLevel.HardestDifficulty;
                }
            }
        }

        public IList<PvPBuildingKey> GetUnlockedBuildings(PvPBuildingCategory buildingCategory)
        {
            return _unlockedBuildings.Where(buildingKey => buildingKey.BuildingCategory == buildingCategory).ToList();
        }

        public IList<PvPUnitKey> GetUnlockedUnits(PvPUnitCategory unitCategory)
        {
            return _unlockedUnits.Where(unitKey => unitKey.UnitCategory == unitCategory).ToList();
        }

        [OnDeserialized()]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            // For backwards compatability, when this class did not have these fields
            if (NewHulls == null)
            {
                NewHulls = new PvPNewItems<PvPHullKey>();
            }
            if (NewBuildings == null)
            {
                NewBuildings = new PvPNewItems<PvPBuildingKey>();
            }
            if (NewUnits == null)
            {
                NewUnits = new PvPNewItems<PvPUnitKey>();
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
                _hotkeys = new PvPHotkeysModel();
            }

            if (_version != ModelVersion.RemovedShowHelpLabel)
            {
                _version = ModelVersion.RemovedShowHelpLabel;
            }
        }

        public override bool Equals(object obj)
        {
            GameModel other = obj as GameModel;

            return other != null
                && other.HasAttemptedTutorial == HasAttemptedTutorial
                && other.NumOfLevelsCompleted == NumOfLevelsCompleted
                && other.SelectedLevel == SelectedLevel
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

        public bool IsUnitUnlocked(PvPUnitKey unitKey)
        {
            return UnlockedUnits.Contains(unitKey);
        }

        public bool IsBuildingUnlocked(PvPBuildingKey buildingKey)
        {
            return UnlockedBuildings.Contains(buildingKey);
        }
    }
}
