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
                   new HeckleData("Heckle000", owned: true, id: 0), new HeckleData("Heckle001",owned: true,id: 1), new HeckleData("Heckle002",owned : true, id: 2), new HeckleData("Heckle003",id: 3), new HeckleData("Heckle004",id: 4),
                   new HeckleData("Heckle005",id: 5), new HeckleData("Heckle006",id: 6), new HeckleData("Heckle007",id: 7), new HeckleData("Heckle008",id: 8), new HeckleData("Heckle009",id: 9),
                   new HeckleData("Heckle010",id:10), new HeckleData("Heckle011",id: 11), new HeckleData("Heckle012",id: 12), new HeckleData("Heckle013",id: 13), new HeckleData("Heckle014",id: 14),
                   new HeckleData("Heckle015",id: 15), new HeckleData("Heckle016",id: 16), new HeckleData("Heckle017",id: 17), new HeckleData("Heckle018",id: 18), new HeckleData("Heckle019",id: 19),
                   new HeckleData("Heckle020",id: 20), new HeckleData("Heckle021",id: 21), new HeckleData("Heckle022",id: 22), new HeckleData("Heckle023",id: 23), new HeckleData("Heckle024",id: 24),
                   new HeckleData("Heckle025",id: 25), new HeckleData("Heckle026",id: 26), new HeckleData("Heckle027",id: 27), new HeckleData("Heckle028",id: 28), new HeckleData("Heckle029",id: 29),
                   new HeckleData("Heckle030",id: 30), new HeckleData("Heckle031",id: 31), new HeckleData("Heckle032",id: 32), new HeckleData("Heckle033",id: 33), new HeckleData("Heckle034",id: 34),
                   new HeckleData("Heckle035",id: 35), new HeckleData("Heckle036",id: 36), new HeckleData("Heckle037",id: 37), new HeckleData("Heckle038",id: 38), new HeckleData("Heckle039",id: 39),
                   new HeckleData("Heckle040",id: 40), new HeckleData("Heckle041",id: 41), new HeckleData("Heckle042",id: 42), new HeckleData("Heckle043",id: 43), new HeckleData("Heckle044",id: 44),
                   new HeckleData("Heckle045",id: 45), new HeckleData("Heckle046",id: 46), new HeckleData("Heckle047",id: 47), new HeckleData("Heckle048",id: 48), new HeckleData("Heckle049",id: 49),
                   new HeckleData("Heckle050",id: 50), new HeckleData("Heckle051",id: 51), new HeckleData("Heckle052",id: 52), new HeckleData("Heckle053",id: 53), new HeckleData("Heckle054",id: 54),
                   new HeckleData("Heckle055",id: 55), new HeckleData("Heckle056",id: 56), new HeckleData("Heckle057",id: 57), new HeckleData("Heckle058",id: 58), new HeckleData("Heckle059",id: 59),
                   new HeckleData("Heckle060",id: 60), new HeckleData("Heckle061",id: 61), new HeckleData("Heckle062",id: 62), new HeckleData("Heckle063",id: 63), new HeckleData("Heckle064",id: 64),
                   new HeckleData("Heckle065",id: 65), new HeckleData("Heckle066",id: 66), new HeckleData("Heckle067",id: 67), new HeckleData("Heckle068",id: 68), new HeckleData("Heckle069",id: 69),
                   new HeckleData("Heckle070",id: 70), new HeckleData("Heckle071",id: 71), new HeckleData("Heckle072",id: 72), new HeckleData("Heckle073",id: 73), new HeckleData("Heckle074",id: 74),
                   new HeckleData("Heckle075",id: 75), new HeckleData("Heckle076",id: 76), new HeckleData("Heckle077",id: 77), new HeckleData("Heckle078",id: 78), new HeckleData("Heckle079",id: 79),
                   new HeckleData("Heckle080",id: 80), new HeckleData("Heckle081",id: 81), new HeckleData("Heckle082",id: 82), new HeckleData("Heckle083",id: 83), new HeckleData("Heckle084",id: 84),
                   new HeckleData("Heckle085",id: 85), new HeckleData("Heckle086",id: 86), new HeckleData("Heckle087",id: 87), new HeckleData("Heckle088",id: 88), new HeckleData("Heckle089",id: 89),
                   new HeckleData("Heckle090",id: 90), new HeckleData("Heckle091",id: 91), new HeckleData("Heckle092",id: 92), new HeckleData("Heckle093",id: 93), new HeckleData("Heckle094",id: 94),
                   new HeckleData("Heckle095",id: 95), new HeckleData("Heckle096",id: 96), new HeckleData("Heckle097",id: 97), new HeckleData("Heckle098",id: 98), new HeckleData("Heckle099",id: 99),
                   new HeckleData("Heckle100",id: 100), new HeckleData("Heckle101",id: 101), new HeckleData("Heckle102",id: 102), new HeckleData("Heckle103",id: 103), new HeckleData("Heckle104",id: 104),
                   new HeckleData("Heckle105",id: 105), new HeckleData("Heckle106",id: 106), new HeckleData("Heckle107",id: 107), new HeckleData("Heckle108",id: 108), new HeckleData("Heckle109",id: 109),
                   new HeckleData("Heckle110",id: 110), new HeckleData("Heckle111",id: 111), new HeckleData("Heckle112",id: 112), new HeckleData("Heckle113",id: 113), new HeckleData("Heckle114",id: 114),
                   new HeckleData("Heckle115",id: 115), new HeckleData("Heckle116",id: 116), new HeckleData("Heckle117",id: 117), new HeckleData("Heckle118",id: 118), new HeckleData("Heckle119",id: 119),
                   new HeckleData("Heckle120",id: 120), new HeckleData("Heckle121",id: 121), new HeckleData("Heckle122",id: 122), new HeckleData("Heckle123",id: 123), new HeckleData("Heckle124",id: 124),
                   new HeckleData("Heckle125",id: 125), new HeckleData("Heckle126",id: 126), new HeckleData("Heckle127",id: 127), new HeckleData("Heckle128",id: 128), new HeckleData("Heckle129",id: 129),
                   new HeckleData("Heckle130",id: 130), new HeckleData("Heckle131",id: 131), new HeckleData("Heckle132",id: 132), new HeckleData("Heckle133",id: 133), new HeckleData("Heckle134",id: 134),
                   new HeckleData("Heckle135",id: 135), new HeckleData("Heckle136",id: 136), new HeckleData("Heckle137",id: 137), new HeckleData("Heckle138",id: 138), new HeckleData("Heckle139",id: 139),
                   new HeckleData("Heckle140",id: 140), new HeckleData("Heckle141",id: 141), new HeckleData("Heckle142",id: 142), new HeckleData("Heckle143",id: 143), new HeckleData("Heckle144",id: 144),
                   new HeckleData("Heckle145",id: 145), new HeckleData("Heckle146",id: 146), new HeckleData("Heckle147",id: 147), new HeckleData("Heckle148",id: 148), new HeckleData("Heckle149",id: 149),
                   new HeckleData("Heckle150",id: 150), new HeckleData("Heckle151",id: 151), new HeckleData("Heckle152",id: 152), new HeckleData("Heckle153",id: 153), new HeckleData("Heckle154",id: 154),
                   new HeckleData("Heckle155",id: 155), new HeckleData("Heckle156",id: 156), new HeckleData("Heckle157",id: 157), new HeckleData("Heckle158",id: 158), new HeckleData("Heckle159",id: 159),
                   new HeckleData("Heckle160",id: 160), new HeckleData("Heckle161",id: 161), new HeckleData("Heckle162",id: 162), new HeckleData("Heckle163",id: 163), new HeckleData("Heckle164",id: 164),
                   new HeckleData("Heckle165",id: 165), new HeckleData("Heckle165",id: 165), new HeckleData("Heckle167",id: 167), new HeckleData("Heckle168",id: 168), new HeckleData("Heckle169",id: 169),
                   new HeckleData("Heckle170",id: 170), new HeckleData("Heckle171",id: 171), new HeckleData("Heckle172",id: 172), new HeckleData("Heckle173",id: 173), new HeckleData("Heckle174",id: 174),
                   new HeckleData("Heckle175",id: 175), new HeckleData("Heckle176",id: 176), new HeckleData("Heckle177",id: 177), new HeckleData("Heckle178",id: 178), new HeckleData("Heckle179",id: 179),
                   new HeckleData("Heckle180",id: 180), new HeckleData("Heckle181",id: 181), new HeckleData("Heckle182",id: 182), new HeckleData("Heckle183",id: 183), new HeckleData("Heckle184",id: 184),
                   new HeckleData("Heckle185",id: 185), new HeckleData("Heckle186",id: 186), new HeckleData("Heckle187",id: 187), new HeckleData("Heckle188",id: 188), new HeckleData("Heckle189",id: 189),
                   new HeckleData("Heckle190",id: 190), new HeckleData("Heckle191",id: 191), new HeckleData("Heckle192",id: 192), new HeckleData("Heckle193",id: 193), new HeckleData("Heckle194",id: 194),
                   new HeckleData("Heckle195",id: 195), new HeckleData("Heckle196",id: 196), new HeckleData("Heckle197",id: 197), new HeckleData("Heckle198",id: 198), new HeckleData("Heckle199",id: 199),
                   new HeckleData("Heckle200",id: 200), new HeckleData("Heckle201",id: 201), new HeckleData("Heckle202",id: 202), new HeckleData("Heckle203",id: 203), new HeckleData("Heckle204",id: 204),
                   new HeckleData("Heckle205",id: 205), new HeckleData("Heckle206",id: 206), new HeckleData("Heckle207",id: 207), new HeckleData("Heckle208",id: 208), new HeckleData("Heckle209",id: 209),
                   new HeckleData("Heckle210",id: 210), new HeckleData("Heckle211",id: 211), new HeckleData("Heckle212",id: 212), new HeckleData("Heckle213",id: 213), new HeckleData("Heckle214",id: 214),
                   new HeckleData("Heckle215",id: 215), new HeckleData("Heckle216",id: 216), new HeckleData("Heckle217",id: 217), new HeckleData("Heckle218",id: 218), new HeckleData("Heckle219",id: 219),
                   new HeckleData("Heckle220",id: 220), new HeckleData("Heckle221",id: 221), new HeckleData("Heckle222",id: 222), new HeckleData("Heckle223",id: 223), new HeckleData("Heckle224",id: 224),
                   new HeckleData("Heckle225",id: 225), new HeckleData("Heckle226",id: 226), new HeckleData("Heckle227",id: 227), new HeckleData("Heckle228",id: 228), new HeckleData("Heckle229",id: 229),
                   new HeckleData("Heckle230",id: 230), new HeckleData("Heckle231",id: 231), new HeckleData("Heckle232",id: 232), new HeckleData("Heckle233",id: 233), new HeckleData("Heckle234",id: 234),
                   new HeckleData("Heckle235",id: 235), new HeckleData("Heckle236",id: 236), new HeckleData("Heckle237",id: 237), new HeckleData("Heckle238",id: 238), new HeckleData("Heckle239",id: 239),
                   new HeckleData("Heckle240",id: 240), new HeckleData("Heckle241",id: 241), new HeckleData("Heckle242",id: 242), new HeckleData("Heckle243",id: 243), new HeckleData("Heckle244",id: 244),
                   new HeckleData("Heckle245",id: 245), new HeckleData("Heckle246",id: 246), new HeckleData("Heckle247",id: 247), new HeckleData("Heckle248",id: 248), new HeckleData("Heckle249",id: 249),
                   new HeckleData("Heckle250",id: 250), new HeckleData("Heckle251",id: 251), new HeckleData("Heckle252",id: 252), new HeckleData("Heckle253",id: 253), new HeckleData("Heckle254",id: 254),
                   new HeckleData("Heckle255",id: 255), new HeckleData("Heckle256",id: 256), new HeckleData("Heckle257",id: 257), new HeckleData("Heckle258",id: 258), new HeckleData("Heckle259",id: 259),
                   new HeckleData("Heckle260",id: 260), new HeckleData("Heckle261",id: 261), new HeckleData("Heckle262",id: 262), new HeckleData("Heckle263",id: 263), new HeckleData("Heckle264",id: 264),
                   new HeckleData("Heckle265",id: 265), new HeckleData("Heckle266",id: 266), new HeckleData("Heckle267",id: 267), new HeckleData("Heckle268",id: 268), new HeckleData("Heckle269",id: 269),
                   new HeckleData("Heckle270",id: 270), new HeckleData("Heckle271",id: 271), new HeckleData("Heckle272",id: 272), new HeckleData("Heckle273",id: 273), new HeckleData("Heckle274",id: 274),
                   new HeckleData("Heckle035",id: 275), new HeckleData("Heckle276",id: 276), new HeckleData("Heckle277",id: 277), new HeckleData("Heckle278",id: 278), new HeckleData("Heckle279",id: 279),
                };

            _captains = new List<CaptainData> {
                    new CaptainData(nameBase:"CaptainExo000",descriptionBase: "CaptainDescription000",owned : true, id: 0),new CaptainData(nameBase:"CaptainExo001",descriptionBase: "CaptainDescription001",id: 1),new CaptainData(nameBase : "CaptainExo002",descriptionBase : "CaptainDescription002",id: 2),new CaptainData(nameBase : "CaptainExo003",descriptionBase : "CaptainDescription003", id: 3),new CaptainData(nameBase : "CaptainExo004",descriptionBase: "CaptainDescription004",id: 4),
                    new CaptainData(nameBase:"CaptainExo005",descriptionBase: "CaptainDescription005",id: 5),new CaptainData(nameBase:"CaptainExo006",descriptionBase: "CaptainDescription006",id: 6),new CaptainData(nameBase:"CaptainExo007",descriptionBase: "CaptainDescription007",id: 7),new CaptainData(nameBase : "CaptainExo008",descriptionBase: "CaptainDescription008",id: 8),new CaptainData(nameBase : "CaptainExo009",descriptionBase: "CaptainDescription009",id: 9),
                    new CaptainData(nameBase:"CaptainExo010",descriptionBase: "CaptainDescription010",id: 10),new CaptainData(nameBase:"CaptainExo011",descriptionBase: "CaptainDescription011",id: 11),new CaptainData(nameBase:"CaptainExo012",descriptionBase : "CaptainDescription012",id: 12),new CaptainData(nameBase : "CaptainExo013",descriptionBase: "CaptainDescription013",id: 13),new CaptainData(nameBase : "CaptainExo014",descriptionBase: "CaptainDescription014",id: 14),
                    new CaptainData(nameBase:"CaptainExo015",descriptionBase: "CaptainDescription015",id: 15),new CaptainData(nameBase:"CaptainExo016",descriptionBase: "CaptainDescription016",id: 16),new CaptainData(nameBase:"CaptainExo017",descriptionBase: "CaptainDescription017",id: 17),new CaptainData(nameBase : "CaptainExo018",descriptionBase: "CaptainDescription018",id: 18),new CaptainData(nameBase : "CaptainExo019",descriptionBase: "CaptainDescription019",id: 19),
                    new CaptainData(nameBase:"CaptainExo020",descriptionBase: "CaptainDescription020",id: 20),new CaptainData(nameBase:"CaptainExo021",descriptionBase: "CaptainDescription021",id: 21),new CaptainData(nameBase:"CaptainExo022",descriptionBase: "CaptainDescription022",id: 22),new CaptainData(nameBase : "CaptainExo023",descriptionBase: "CaptainDescription023",id: 23),new CaptainData(nameBase : "CaptainExo024",descriptionBase: "CaptainDescription024",id: 24),
                    new CaptainData(nameBase:"CaptainExo025",descriptionBase: "CaptainDescription025",id: 25),new CaptainData(nameBase:"CaptainExo026",descriptionBase: "CaptainDescription026",id: 26),new CaptainData(nameBase:"CaptainExo027",descriptionBase: "CaptainDescription027",id: 27),new CaptainData(nameBase : "CaptainExo028",descriptionBase: "CaptainDescription028",id: 28),new CaptainData(nameBase : "CaptainExo029",descriptionBase: "CaptainDescription029",id: 29),
                    new CaptainData(nameBase:"CaptainExo030",descriptionBase: "CaptainDescription030",id: 30),new CaptainData(nameBase:"CaptainExo031",descriptionBase: "CaptainDescription031",id: 31),new CaptainData(nameBase:"CaptainExo032",descriptionBase: "CaptainDescription032",id: 32),new CaptainData(nameBase : "CaptainExo033",descriptionBase: "CaptainDescription033",id: 33),new CaptainData(nameBase : "CaptainExo034",descriptionBase: "CaptainDescription034",id: 34),
                    new CaptainData(nameBase:"CaptainExo035",descriptionBase: "CaptainDescription035",id: 35),new CaptainData(nameBase:"CaptainExo036",descriptionBase: "CaptainDescription036",id: 36),new CaptainData(nameBase:"CaptainExo037",descriptionBase: "CaptainDescription037",id: 37),new CaptainData(nameBase : "CaptainExo038",descriptionBase: "CaptainDescription038",id: 38),new CaptainData(nameBase : "CaptainExo039",descriptionBase: "CaptainDescription039",id: 39),new CaptainData(nameBase : "CaptainExo040",descriptionBase: "CaptainDescription040",id: 40)
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
