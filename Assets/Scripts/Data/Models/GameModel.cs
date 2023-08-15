using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
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


        private List<int> _captainExoList;
        public List<int> CaptainExoList
        {
            get => _captainExoList;
            set => _captainExoList = value;
        }

        private List<int> _heckleList;
        public List<int> HeckleList
        {
            get => _heckleList;
            set => _heckleList = value;
        }

        public List<HeckleData> _heckles;
        public List<HeckleData> Heckles
        {
            get => _heckles;
            set => _heckles = value;
        }

        public List<CaptainData> _captains;
        public List<CaptainData> Captains
        {
            get => _captains;
            set => _captains = value;
        }

        public List<IAPData> _iaps;
        public List<IAPData> IAPs
        {
            get => _iaps;
            set => _iaps = value;
        }
        // Captain Logic

        [SerializeField]
        private CaptainExoKey _currentCaptain;
        public CaptainExoKey CurrentCaptain
        {
            get => _currentCaptain;
            set => _currentCaptain = value;
        }

        private string _playerName;
        public String PlayerName
        {
            get => _playerName;
            set => _playerName = value;
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
        private SkirmishModel _skirmish;

        [SerializeField]
        private CoinBattleModel _coinBattle;

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

        public int SelectedPvPLevel
        {
            get { return _selectedPvPLevel; }
            set
            {
                Assert.IsTrue(value > 0);
                Assert.IsTrue(value <= StaticData.NUM_OF_PvPLEVELS);
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

            _captainExoList = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
            _heckleList = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };

            _heckles = new List<HeckleData> {
                   new HeckleData("Heckle000", owned: true,id: 0), new HeckleData("Heckle001",owned: true,id: 1), new HeckleData("Heckle002",owned: true,id: 2), new HeckleData("Heckle003",owned: true,id: 3), new HeckleData("Heckle004",id: 4),
                   new HeckleData("Heckle005",id: 5), new HeckleData("Heckle006",owned: true,id: 6), new HeckleData("Heckle007",id: 7), new HeckleData("Heckle008",id: 8), new HeckleData("Heckle009",id: 9),
                   new HeckleData("Heckle010",id:10), new HeckleData("Heckle011",id: 11), new HeckleData("Heckle012",owned: true,id: 12), new HeckleData("Heckle013",id: 13), new HeckleData("Heckle014",id: 14),
                   new HeckleData("Heckle015",id: 15), new HeckleData("Heckle016",owned: true,id: 16), new HeckleData("Heckle017",id: 17), new HeckleData("Heckle018",owned: true,id: 18), new HeckleData("Heckle019",id: 19),
                   new HeckleData("Heckle020",id: 20), new HeckleData("Heckle021",owned: true,id: 21), new HeckleData("Heckle022",id: 22), new HeckleData("Heckle023",owned: true,id: 23), new HeckleData("Heckle024",id: 24),
                   new HeckleData("Heckle025",id: 25), new HeckleData("Heckle026",owned: true,id: 26), new HeckleData("Heckle027",id: 27), new HeckleData("Heckle028",owned: true,id: 28), new HeckleData("Heckle029",id: 29),
                   new HeckleData("Heckle030",id: 30), new HeckleData("Heckle031",owned: true,id: 31), new HeckleData("Heckle032",id: 32), new HeckleData("Heckle033",owned: true,id: 33), new HeckleData("Heckle034",id: 34),
                   new HeckleData("Heckle035",id: 35), new HeckleData("Heckle036",owned: true,id: 36), new HeckleData("Heckle037",id: 37), new HeckleData("Heckle038",owned: true,id: 38), new HeckleData("Heckle039",id: 39),
                };

            _captains = new List<CaptainData> {
                    new CaptainData(nameBase: "CaptainExo000", descriptionBase: "CaptainDescription000", owned: true, id: 0),new CaptainData(nameBase:"CaptainExo001",descriptionBase : "CaptainDescription001", id: 1),new CaptainData(nameBase : "CaptainExo002",descriptionBase : "CaptainDescription002", id: 2),new CaptainData(nameBase : "CaptainExo003",descriptionBase : "CaptainDescription003", id: 3),new CaptainData(nameBase : "CaptainExo004",descriptionBase: "CaptainDescription004",id: 4),
                    new CaptainData(nameBase:"CaptainExo005",descriptionBase: "CaptainDescription005",owned: true,id: 5),new CaptainData(nameBase:"CaptainExo006",descriptionBase: "CaptainDescription006",owned: true,id: 6),new CaptainData(nameBase:"CaptainExo007",descriptionBase: "CaptainDescription007",id: 7),new CaptainData(nameBase : "CaptainExo008",descriptionBase: "CaptainDescription008",id: 8),new CaptainData("CaptainExo009",descriptionBase: "CaptainDescription009",id: 9),
                    new CaptainData(nameBase:"CaptainExo010",descriptionBase: "CaptainDescription010",owned: true,id: 10),new CaptainData(nameBase:"CaptainExo011",descriptionBase : "CaptainDescription011", id: 11),new CaptainData(nameBase:"CaptainExo012",descriptionBase : "CaptainDescription012", id: 12),new CaptainData(nameBase : "CaptainExo013",descriptionBase: "CaptainDescription013",id: 13),new CaptainData(nameBase : "CaptainExo014",descriptionBase: "CaptainDescription014",id: 14),
                    new CaptainData(nameBase:"CaptainExo015",descriptionBase: "CaptainDescription015",owned: true,id: 15),new CaptainData(nameBase:"CaptainExo016",descriptionBase: "CaptainDescription016",owned: true,id: 16),new CaptainData(nameBase:"CaptainExo017",descriptionBase: "CaptainDescription017",owned : true, id: 17),new CaptainData(nameBase : "CaptainExo018",descriptionBase: "CaptainDescription018",owned: true,id: 18),new CaptainData(nameBase : "CaptainExo019",descriptionBase: "CaptainDescription019",id: 19),
                    new CaptainData(nameBase:"CaptainExo020",descriptionBase: "CaptainDescription020",owned: true,id: 20),new CaptainData(nameBase:"CaptainExo021",descriptionBase: "CaptainDescription021",owned: true,id: 21),new CaptainData(nameBase:"CaptainExo022",descriptionBase: "CaptainDescription022",owned : true, id: 22),new CaptainData(nameBase : "CaptainExo023",descriptionBase: "CaptainDescription023",owned: true,id: 23),new CaptainData(nameBase : "CaptainExo024",descriptionBase: "CaptainDescription024",id: 24),
                    new CaptainData(nameBase:"CaptainExo025",descriptionBase: "CaptainDescription025",owned: true,id: 25),new CaptainData(nameBase:"CaptainExo026",descriptionBase: "CaptainDescription026",owned: true,id: 26),new CaptainData(nameBase:"CaptainExo027",descriptionBase: "CaptainDescription027",owned : true, id: 27),new CaptainData(nameBase : "CaptainExo028",descriptionBase: "CaptainDescription028",owned: true,id: 28),new CaptainData(nameBase : "CaptainExo029",descriptionBase: "CaptainDescription029",id: 29),
                    new CaptainData(nameBase:"CaptainExo030",descriptionBase: "CaptainDescription030",id: 30),new CaptainData(nameBase:"CaptainExo031",descriptionBase: "CaptainDescription031",owned: true,id: 31),new CaptainData(nameBase:"CaptainExo032",descriptionBase: "CaptainDescription032",owned : true, id: 32),new CaptainData(nameBase : "CaptainExo033",descriptionBase: "CaptainDescription033",owned: true,id: 33),new CaptainData(nameBase : "CaptainExo034",descriptionBase: "CaptainDescription034",id: 34),
                    new CaptainData(nameBase:"CaptainExo035",descriptionBase: "CaptainDescription035",id: 35),new CaptainData(nameBase:"CaptainExo036",descriptionBase: "CaptainDescription036",owned: true,id: 36),new CaptainData(nameBase:"CaptainExo037",descriptionBase: "CaptainDescription037",owned : true, id: 37),new CaptainData(nameBase : "CaptainExo038",descriptionBase: "CaptainDescription038",owned: true,id: 38),new CaptainData(nameBase : "CaptainExo039",descriptionBase: "CaptainDescription039",id: 39),new CaptainData(nameBase : "CaptainExo040",descriptionBase: "CaptainDescription040",id: 40)
                };

            _iaps = new List<IAPData> {
                    new IAPData(iapType: 0, iapNameKeyBase: "Coins100Pack", iapDescriptionKeybase: "Coins100PackDescription", iapIconName: "Coins100Pack", 0.99f, 100),
                    new IAPData(iapType: 0, iapNameKeyBase: "Coins500Pack", iapDescriptionKeybase: "Coins500PackDescription", iapIconName: "Coins500Pack", 1.99f, 500),
                    new IAPData(iapType: 0, iapNameKeyBase: "Coins1000Pack", iapDescriptionKeybase: "Coins1000PackDescription", iapIconName: "Coins1000Pack", 2.99f, 1000),
                    new IAPData(iapType: 0, iapNameKeyBase: "Coins5000Pack", iapDescriptionKeybase: "Coins5000PackDescription", iapIconName: "Coins5000Pack", 3.99f, 5000),
            };

            _playerName = "Charlie";
            _coins = 50;
            _credits = 0;

/*            _rankData = 0;*/
        }

        public GameModel(
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

    }
}
