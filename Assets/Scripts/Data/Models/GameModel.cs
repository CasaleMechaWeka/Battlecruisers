using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.ScreensScene.ShopScreen;
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
    public class GameModel : IGameModel, IVoyageModel, IPlayerModel
    {

        // Voyage properties - These are for ROGUE voyages, currently unused.

        private int _voyageNumber;
        public int VoyageNumber
        {
            get => _voyageNumber;
            set => _voyageNumber = value;
        }

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

        // Player properties - for determining ROGUE player strength

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

        private long _credits;
        public long Credits
        {
            get => _credits;
            set => _credits = value;
        }

        private long _coins;
        public long Coins
        {
            get => _coins;
            set => _coins = value;
        }

        private int _totalVoyages;
        public int TotalVoyages
        {
            get => _totalVoyages;
            set => _totalVoyages = value;
        }

        private float _battleWinScore;
        public float BattleWinScore
        {
            get => _battleWinScore;
            set { _battleWinScore = value; if (_battleWinScore < 0) _battleWinScore = 0; }
        }

        public List<Arena> _arenas;
        public List<Arena> Arenas
        {
            get => _arenas;
            set => _arenas = value;
        }
        public Dictionary<String, int> _gameConfigs;
        public Dictionary<String, int> GameConfigs
        {
            get => _gameConfigs;
            set => _gameConfigs = value;
        }

        public List<IAPData> _iaps;
        public List<IAPData> IAPs
        {
            get => _iaps;
            set => _iaps = value;
        }

        public List<HeckleData> _outstandingHeckleTransactions;
        public List<HeckleData> OutstandingHeckleTransactions
        {
            get => _outstandingHeckleTransactions;
            set => _outstandingHeckleTransactions = value;
        }

        public List<CaptainData> _outstandingCaptainTransactions;
        public List<CaptainData> OutstandingCaptainTransactions
        {
            get => _outstandingCaptainTransactions;
            set => _outstandingCaptainTransactions = value;
        }

        public List<BodykitData> _outstandingBodykitTransactions;
        public List<BodykitData> OutstandingBodykitTransactions
        {
            get => _outstandingBodykitTransactions;
            set => _outstandingBodykitTransactions = value;
        }

        public List<VariantData> _outstandingVariantTransactions;
        public List<VariantData> OutstandingVariantTransactions
        {
            get => _outstandingVariantTransactions;
            set => _outstandingVariantTransactions = value;
        }

        private int _creditsChange;
        public int CreditsChange
        {
            get => _creditsChange;
            set => _creditsChange = value;
        }

        private int _coinsChange;
        public int CoinsChange
        {
            get => _coinsChange;
            set => _coinsChange = value;
        }

        public bool _hasSyncdShop;
        public bool HasSyncdShop
        {
            get => _hasSyncdShop;
            set => _hasSyncdShop = value;
        }

        private int _gameMap;
        public int GameMap
        {
            get => _gameMap;
            set => _gameMap = value;
        }

        private string _queueName;
        public string QueueName
        {
            get => _queueName;
            set => _queueName = value;
        }

        // MinCPuCores and MinCPuFreq are used for setting which devices are allowed to host in pvp
        private int minCPUCores;
        public int MinCPUCores
        {
            get => minCPUCores;
            set => minCPUCores = value;
        }

        private int minCPUFreq;
        public int MinCPUFreq
        {
            get => minCPUFreq;
            set => minCPUFreq = value;
        }

        private int maxLatency;
        public int MaxLatency
        {
            get => maxLatency;
            set => maxLatency = value;
        }

        private string _playerName;
        public String PlayerName
        {
            get => _playerName;
            set => _playerName = value;
        }

        public List<int> PurchasedExos { get => _purchasedExos; }
        public List<int> PurchasedHeckles { get => _purchasedHeckles; }
        public List<int> PurchasedBodykits { get => _purchasedBodykits; }
        public List<int> PurchasedVariants { get => _purchasedVariants; }

        private List<int> _purchasedExos;

        private List<int> _purchasedHeckles;

        private List<int> _purchasedBodykits;

        private List<int> _purchasedVariants;


        private bool _isDoneMigration;
        public bool IsDoneMigration
        {
            get => _isDoneMigration;
            set => _isDoneMigration = value;
        }

        /*        private int _rankData;
                public int RankData
                {
                    get => _rankData;
                    set => _rankData = value;
                }*/
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
        private List<CompletedLevel> _completedSideQuests;
        private List<int> _completedSideQuestIDs;

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
        private int _selectedPvPLevel;

        [SerializeField]
        private int _selectedSideQuestID;

        [SerializeField]
        private SkirmishModel _skirmish;

        [SerializeField]
        private CoinBattleModel _coinBattle;

        [SerializeField]
        private SideQuestData _sideQuest;

        [SerializeField]
        private HotkeysModel _hotkeys;
        public HotkeysModel Hotkeys => _hotkeys;

        [SerializeField]
        private int _version;
        public int Version
        {
            get => _version;
            set => _version = value;
        }

        public int NumOfLevelsCompleted => _completedLevels.Count;
        public int NumOfSideQuestsCompleted
        {
            get
            {
                if (_completedSideQuests == null)
                    return 0;
                else
                    return _completedSideQuests.Count;
            }
        }
        public int ID_Bodykit_AIbot { get; set; }
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
                if (value <= 0) { _selectedLevel = 1; return; }            //Assert.IsTrue(value > 0);
                if (value > StaticData.NUM_OF_LEVELS) { _selectedLevel = StaticData.NUM_OF_LEVELS; return; }     // Assert.IsTrue(value <= StaticData.NUM_OF_LEVELS);
                _selectedLevel = value;
            }
        }

        public int SelectedPvPLevel
        {
            get { return _selectedPvPLevel; }
            set
            {

                if (value < 0) { _selectedPvPLevel = 0; return; }
                if (value >= StaticData.NUM_OF_PvPLEVELS) { _selectedPvPLevel = StaticData.NUM_OF_PvPLEVELS - 1; return; }
                _selectedPvPLevel = value;
            }
        }

        public int SelectedSideQuestID
        {
            get { return _selectedSideQuestID; }
            set
            {

                if (value < 0) { _selectedSideQuestID = 0; return; }
                if (value >= StaticData.NUM_OF_SIDEQUESTS) { _selectedSideQuestID = StaticData.NUM_OF_SIDEQUESTS - 1; return; }
                _selectedSideQuestID = value;
            }
        }

        public SkirmishModel Skirmish
        {
            get { return _skirmish; }
            set { _skirmish = value; }
        }

        public CoinBattleModel CoinBattle
        {
            get { return _coinBattle; }
            set { _coinBattle = value; }
        }

        public SideQuestData SideQuest
        {
            get { return _sideQuest; }
            set { _sideQuest = value; }
        }

        public ReadOnlyCollection<HullKey> UnlockedHulls { get; }
        public ReadOnlyCollection<BuildingKey> UnlockedBuildings { get; }
        public ReadOnlyCollection<UnitKey> UnlockedUnits { get; }
        public ReadOnlyCollection<CompletedLevel> CompletedLevels { get; }
        public ReadOnlyCollection<CompletedLevel> CompletedSideQuests { get; }
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
            _completedSideQuests = new List<CompletedLevel>();
            CompletedLevels = _completedLevels.AsReadOnly();
            CompletedSideQuests = _completedSideQuests.AsReadOnly();

            NewHulls = new NewItems<HullKey>();
            NewBuildings = new NewItems<BuildingKey>();
            NewUnits = new NewItems<UnitKey>();

            Settings = new SettingsModel();
            _hotkeys = new HotkeysModel();
            _selectedLevel = UNSET_SELECTED_LEVEL;
            _skirmish = null;
            _sideQuest = null;

            _iaps = new List<IAPData> {
                    new IAPData(iapType: 0, iapNameKeyBase: "Coins100Name", iapDescriptionKeybase: "Coins100Description", iapIconName: "Coins100Pack", 0.99f, 100),
                    new IAPData(iapType: 0, iapNameKeyBase: "Coins500Name", iapDescriptionKeybase: "Coins500Description", iapIconName: "Coins500Pack", 1.99f, 500),
                    new IAPData(iapType: 0, iapNameKeyBase: "Coins1000Name", iapDescriptionKeybase: "Coins1000Description", iapIconName: "Coins1000Pack", 2.99f, 1000),
                    new IAPData(iapType: 0, iapNameKeyBase: "Coins5000Name", iapDescriptionKeybase: "Coins5000Description", iapIconName: "Coins5000Pack", 3.99f, 5000)
            };

            _purchasedExos = new List<int> { 0 };
            _purchasedHeckles = new List<int>();
            _purchasedBodykits = new List<int>();
            _purchasedVariants = new List<int>();
            _isDoneMigration = false;

            _playerName = "Charlie";
            _coins = 50;
            _credits = 0;
            _gameMap = 0;

            ID_Bodykit_AIbot = -1;
            _arenas = new List<Arena>
            {
                new Arena(),
                new Arena("PracticeWreckyards", prizecredits: 100),
                new Arena("OzPenitentiary", prizecoins:1),
                new Arena("SanFranciscoFightClub", costcoins:1, prizecoins:3, prizecredits: 500),
                new Arena("UACBattleNight", costcredits:100, prizecredits: 500),
                new Arena("NuclearDome", costcoins:3, prizecoins:4,prizecredits:400, prizenukes: 1, consolationnukes: 1),
                new Arena("UACArena", costcredits:1500, prizecredits: 400),
                new Arena("RioBattlesport", costcoins:10, prizecoins:15, prizecredits:2000, consolationcredits:2000),
                new Arena("UACUltimate", costcoins: 10000, prizecredits:20000),
                new Arena("MercenaryOne", costcoins:50, prizecredits:50000, prizenukes: 1)
            };

            _queueName = "bc-1vs1-queue";

            _gameConfigs = new Dictionary<string, int>() { { "scoredivider", 10 },
                { "creditdivider", 100 },
                { "coin1threshold", 1000 },
                { "coin2threshold", 2000 },
                { "coin3threshold", 3000 },
                { "coin4threshold", 4000 },
                { "coin5threshold", 5000 },
                { "creditmax", 1250 }
            };

            _battleWinScore = 0;
        }

        public GameModel(
            bool hasSyncdShop,
            bool hasAttemptedTutorial,
            long lifetimeDestructionScore,
            long bestDestructionScore,
            Loadout playerLoadout,
            BattleResult lastBattleResult,
            List<HullKey> unlockedHulls,
            List<BuildingKey> unlockedBuildings,
            List<UnitKey> unlockedUnits)
            : this()
        {
            HasSyncdShop = hasSyncdShop;
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
                                            { "HasAttemptedTutorial", HasAttemptedTutorial }
                                          };
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

        public void AddCompletedSideQuest(CompletedLevel completedSideQuest)
        {
            Assert.IsTrue(completedSideQuest.LevelNum <= StaticData.NUM_OF_SIDEQUESTS,
            "SideQuestID out of expected range: " + (completedSideQuest.LevelNum).ToString() + " | expected: <=" + (StaticData.NUM_OF_SIDEQUESTS).ToString());
            Assert.IsTrue(completedSideQuest.LevelNum >= 0);

            // First time SideQuest has been completed
            if (_completedSideQuests == null)
                _completedSideQuests = new List<CompletedLevel> { completedSideQuest };
            else if (!IsSideQuestCompleted(completedSideQuest.LevelNum))
            {
                _completedSideQuests.Add(completedSideQuest);
                if (_completedSideQuestIDs == null)
                    _completedSideQuestIDs = new List<int> { completedSideQuest.LevelNum };
                else
                    _completedSideQuestIDs.Add(completedSideQuest.LevelNum);
            }
            else
            {
                // SideQuest has been completed before
                CompletedLevel currentSideQuest = _completedSideQuests.FirstOrDefault(sideQuest => sideQuest.LevelNum == completedSideQuest.LevelNum);

                if (completedSideQuest.HardestDifficulty > currentSideQuest.HardestDifficulty)
                    currentSideQuest.HardestDifficulty = completedSideQuest.HardestDifficulty;
            }
        }

        public bool IsSideQuestCompleted(int sideQuestID)
        {
            if (_completedSideQuestIDs == null)
                return false;
            else
                return _completedSideQuestIDs.Contains(sideQuestID);
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

        public bool IsUnitUnlocked(UnitKey unitKey)
        {
            return UnlockedUnits.Contains(unitKey);
        }

        public bool IsBuildingUnlocked(BuildingKey buildingKey)
        {
            return UnlockedBuildings.Contains(buildingKey);
        }

        public void AddExo(int index)
        {
            if (_purchasedExos != null)
            {
                if (!_purchasedExos.Contains(index))
                    _purchasedExos.Add(index);
            }
            else
            {
                _purchasedExos = new List<int>() { index };
            }
        }
        public void RemoveExo(int id)
        {
            _purchasedExos.RemoveAll(x => x == id);
        }
        public void AddHeckle(int index)
        {
            if (_purchasedHeckles != null)
            {
                if (!_purchasedHeckles.Contains(index))
                    _purchasedHeckles.Add(index);
            }
            else
            {
                _purchasedHeckles = new List<int>() { index };
            }
        }
        public void RemoveHeckle(int id)
        {
            _purchasedHeckles.RemoveAll(x => x == id);
        }
        public void AddBodykit(int index)
        {
            if (_purchasedBodykits != null)
            {
                if (!_purchasedBodykits.Contains(index))
                    _purchasedBodykits.Add(index);
            }
            else
            {
                _purchasedBodykits = new List<int>() { index };
            }
        }
        public void RemoveBodykit(int id)
        {
            _purchasedBodykits.RemoveAll(x => x == id);
        }
        public void AddVariant(int index)
        {
            if (_purchasedVariants != null)
            {
                if (!_purchasedVariants.Contains(index))
                    _purchasedVariants.Add(index);
            }
            else
            {
                _purchasedVariants = new List<int>() { index };
            }
        }
        public void RemoveVariant(int id)
        {
            _purchasedVariants.RemoveAll(x => x == id);
        }
    }
}
