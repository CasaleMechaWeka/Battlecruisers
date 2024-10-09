using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using Buildings = BattleCruisers.Data.Static.StaticPrefabKeys.Buildings;
using Units = BattleCruisers.Data.Static.StaticPrefabKeys.Units;
using Hulls = BattleCruisers.Data.Static.StaticPrefabKeys.Hulls;
using Exos = BattleCruisers.Data.Static.StaticPrefabKeys.CaptainExos;
using BackgroundMusic = BattleCruisers.Data.Static.SoundKeys.Music.Background;
using BattleCruisers.Data.Static.LevelLoot;
using BattleCruisers.Data.Static.Strategies.Helper;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine.Assertions;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static.Strategies.Helper;

namespace BattleCruisers.Data.Static
{
    public class StaticData : IStaticData
    {
        private readonly IDictionary<BuildingKey, int> _buildingToUnlockedLevel;
        private readonly IDictionary<UnitKey, int> _unitToUnlockedLevel;
        private readonly IDictionary<HullKey, int> _hullToUnlockedLevel;

        private readonly IDictionary<BuildingKey, int> _buildingToCompletedSideQuest;
        private readonly IDictionary<UnitKey, int> _unitToCompletedSideQuest;
        private readonly IDictionary<HullKey, int> _hullToCompletedSideQuest;

        private readonly IList<BuildingKey> _allBuildings;
        private readonly IList<UnitKey> _allUnits;

        private const int MIN_AVAILABILITY_LEVEL_NUM = 2;
        public const int NUM_OF_LEVELS = 31;
        public const int NUM_OF_PvPLEVELS = 9;
        public const int NUM_OF_STANDARD_LEVELS = 31;
        public const int NUM_OF_LEVELS_IN_DEMO = 7;
        public const int NUM_OF_SIDEQUESTS = 30;


#if IS_DEMO
        public bool IsDemo => true;
#else
        public bool IsDemo => false;
#endif

#if UNITY_ASSERTIONS
        public bool HasAsserts => true;
#else
        public bool HasAsserts => false;
#endif

        public GameModel InitialGameModel { get; }
        public ReadOnlyCollection<ILevel> Levels { get; }
        public ReadOnlyCollection<ISideQuestData> SideQuests { get; }
        public ReadOnlyDictionary<Map, IPvPLevel> PvPLevels { get; }
        public ReadOnlyCollection<HullKey> HullKeys { get; }
        public ReadOnlyCollection<UnitKey> UnitKeys { get; }
        public ReadOnlyCollection<BuildingKey> BuildingKeys { get; }
        public ReadOnlyCollection<BuildingKey> AIBannedUltrakeys { get; }
        public int LastLevelWithLoot => 40;
        public ILevelStrategies Strategies { get; }
        public ILevelStrategies SideQuestStrategies { get; }
        public IPvPLevelStrategies PvPStrategies { get; }

        public StaticData()
        {
            HullKeys = AllHullKeys().AsReadOnly();

            _allBuildings = AllBuildingKeys();
            BuildingKeys = new ReadOnlyCollection<BuildingKey>(_allBuildings);

            _allUnits = AllUnitKeys();
            UnitKeys = new ReadOnlyCollection<UnitKey>(_allUnits);

            _buildingToUnlockedLevel = AssignBuildingUnlockLevel();
            _unitToUnlockedLevel = AssignUnitUnlockLevel();
            _hullToUnlockedLevel = AssignHullUnlockLevel();

            _buildingToCompletedSideQuest = AssignBuildingUnlockSideQuest();
            _unitToCompletedSideQuest = AssignUnitUnlockSideQuest();
            _hullToCompletedSideQuest = AssignHullUnlockSideQuest();

            Strategies = new LevelStrategies();
            SideQuestStrategies = new SideQuestStrategies();
            PvPStrategies = new PvPLevelStrategies();

            AIBannedUltrakeys = new ReadOnlyCollection<BuildingKey>(CreateAIBannedUltraKeys());

            InitialGameModel = CreateInitialGameModel();
            Levels = new ReadOnlyCollection<ILevel>(CreateLevels());
            PvPLevels = new ReadOnlyDictionary<Map, IPvPLevel>(CreatePvPLevels());
            SideQuests = new ReadOnlyCollection<ISideQuestData>(CreateSideQuests());
        }

        private List<HullKey> AllHullKeys()
        {
            // In order they are available to the user.  Means the loadout
            // screen order is nice :)
            return new List<HullKey>()
            {
                Hulls.Trident,
                Hulls.Bullshark,
                Hulls.Raptor,
                Hulls.Rockjaw,
                Hulls.Eagle,
                Hulls.Flea,
                Hulls.Goatherd,
                Hulls.Hammerhead,
                Hulls.Longbow,
                Hulls.Megalodon,
                Hulls.Megalith,
                Hulls.Microlodon,
                Hulls.BlackRig,
                Hulls.Pistol,
                Hulls.Rickshaw,
                Hulls.Shepherd,
                Hulls.TasDevil,
                Hulls.Yeti,
                Hulls.FortressPrime
            };
        }

        private List<BuildingKey> AllBuildingKeys()
        {
            // Buildings in a category (eg:  Factories) are in the order they 
            // become available to the user.  Means the loadout screen order is nice :)
            return new List<BuildingKey>()
            {
                // Factories
                Buildings.AirFactory,
                Buildings.NavalFactory,
                Buildings.DroneStation,
                Buildings.DroneStation4,
                Buildings.DroneStation6,
                Buildings.DroneStation8,

                // Tactical
                Buildings.ShieldGenerator,
                Buildings.LocalBooster,
                Buildings.ControlTower,
                Buildings.StealthGenerator,
                Buildings.SpySatelliteLauncher,
                Buildings.GrapheneBarrier,

                // Defence
                Buildings.AntiShipTurret,
                Buildings.AntiAirTurret,
                Buildings.Mortar,
                Buildings.SamSite,
                Buildings.TeslaCoil,
                Buildings.Coastguard,//new
                Buildings.FlakTurret,//new
                Buildings.CIWS,//new

                // Offence
                Buildings.Artillery,
                Buildings.Railgun,
                Buildings.RocketLauncher,
                Buildings.MLRS,
                Buildings.GatlingMortar,
                Buildings.IonCannon,
                Buildings.MissilePod,
                Buildings.Cannon,

                // Ultras
                Buildings.DeathstarLauncher,
                Buildings.NukeLauncher,
                Buildings.Ultralisk,
                Buildings.KamikazeSignal,
                Buildings.Broadsides,
                Buildings.NovaArtillery,//new
                Buildings.UltraCIWS,//new
                Buildings.GlobeShield,//new
                Buildings.Sledgehammer//new
			};
        }

        private IList<BuildingKey> CreateAIBannedUltraKeys()
        {
            return new List<BuildingKey>()
            {
                // Don't want AI to try and build a kamikaze signal as an ultra,
                // as it is only effective if there are a certain number of planes.
                // Simpler to make the AI only build ultras that are always effective.
                Buildings.KamikazeSignal,

                // Don't want AI to try and build an ultralisk as an ultra,
                // because it is only super effective if building something else
                // afterwards (other offensives, ultras, or units).  As the AI's
                // strategy may be to win with a fast ultra (after which the AI
                // may do nothing), again only let the AI build ultras that
                // are always effective.
                Buildings.Ultralisk
            };
        }

        private List<UnitKey> AllUnitKeys()
        {
            // Units in a category (eg:  Aircraft) are in the order they 
            // become available to the user.  Means the loadout screen order is nice :)
            return new List<UnitKey>()
            {
                // Aircraft
                Units.Bomber,
                Units.Gunship,
                Units.Fighter,
                Units.SteamCopter,
                Units.Broadsword,
                Units.StratBomber,
                Units.SpyPlane,
                Units.MissileFighter,

                // Ships
                Units.AttackBoat,
                Units.Frigate,
                Units.Destroyer,
                Units.SiegeDestroyer,
                Units.GunBoat,
                Units.ArchonBattleship,
                Units.AttackRIB,
                Units.GlassCannoneer,
                Units.RocketTurtle,
                Units.FlakTurtle
            };
        }

        /// <summary>
        /// Creates the initial game model.
        /// 
        /// NOTE:  Do NOT share key list objects between Loadout and GameModel, otherwise
        /// both will share the same list.  In that case if the Loadout deletes one of its
        /// buildings the building will also be deleted from the GameModel.
        /// </summary>
        private GameModel CreateInitialGameModel()
        {
            HullKey initialHull = GetInitialHull();
            // TEMP  For final game, don't add ALL the prefabs :D
            //Loadout playerLoadout = new Loadout(initialHull, AllBuildingKeys(), AllUnitKeys());

            Loadout playerLoadout = new Loadout(initialHull, GetInitialBuildings(), GetInitialUnits(), GetInitialbuildingLimit(), GetInitialUnitLimit());

            bool hasAttemptedTutorial = false;
            bool HasSyncdShop = false;

            GameModel game = new GameModel(
                HasSyncdShop,
                hasAttemptedTutorial,
                0,
                0,
                playerLoadout,
                lastBattleResult: null,
                // TEMP  Do not unlock all hulls & buildables at the game start :P
                unlockedHulls: new List<HullKey>() { initialHull },
                unlockedBuildings: GetInitialBuildings(),
                unlockedUnits: GetInitialUnits()
                );

            foreach (int i in playerLoadout.CurrentHeckles)
            {
                game._heckles[i].isOwned = true;
                game.AddHeckle(i);
            }

            return game;
            //unlockedHulls: AllHullKeys(),
            //unlockedBuildings: AllBuildingKeys(),
            //unlockedUnits: AllUnitKeys());
        }

        private Dictionary<BuildingCategory, List<BuildingKey>> GetInitialbuildingLimit()
        {
            List<BuildingKey> limit = GetInitialBuildings();
            List<BuildingKey> factories = new List<BuildingKey>();
            List<BuildingKey> defence = new List<BuildingKey>();
            List<BuildingKey> offense = new List<BuildingKey>();
            List<BuildingKey> tactical = new List<BuildingKey>();
            List<BuildingKey> Ultra = new List<BuildingKey>();
            foreach (BuildingKey key in limit)
            {
                switch (key.BuildingCategory)
                {
                    case BuildingCategory.Factory:
                        factories.Add(key);
                        break;
                    case BuildingCategory.Defence:
                        defence.Add(key);
                        break;
                    case BuildingCategory.Offence:
                        offense.Add(key);
                        break;
                    case BuildingCategory.Tactical:
                        tactical.Add(key);
                        break;
                    case BuildingCategory.Ultra:
                        Ultra.Add(key);
                        break;
                    default:
                        break;
                }
            }
            Dictionary<BuildingCategory, List<BuildingKey>> buildables = new()
            {
                { BuildingCategory.Factory, factories },
                { BuildingCategory.Defence, defence },
                { BuildingCategory.Offence, offense },
                { BuildingCategory.Tactical, tactical },
                { BuildingCategory.Ultra, Ultra }
            };
            return buildables;
        }

        private Dictionary<UnitCategory, List<UnitKey>> GetInitialUnitLimit()
        {
            List<UnitKey> units = GetInitialUnits();
            List<UnitKey> ships = new();
            List<UnitKey> aircraft = new();
            foreach (UnitKey unit in units)
            {
                if (unit.UnitCategory == UnitCategory.Naval)
                    ships.Add(unit);
                else if (unit.UnitCategory == UnitCategory.Aircraft)
                    aircraft.Add(unit);
                else
                    break;
            }
            Dictionary<UnitCategory, List<UnitKey>> unitlimit = new()
            {
                {UnitCategory.Naval, ships },
                {UnitCategory.Aircraft, aircraft }
            };
            return unitlimit;
        }
        private HullKey GetInitialHull()
        {
            return Hulls.Trident;
        }

        private List<BuildingKey> GetInitialBuildings()
        {
            return GetBuildingsUnlockedInLevel(levelFirstAvailableIn: 1).ToList();
        }

        private List<UnitKey> GetInitialUnits()
        {
            return GetUnitsUnlockedInLevel(levelFirstAvailableIn: 1).ToList();
        }

        private IList<ILevel> CreateLevels()
        {
            return new List<ILevel>()
            {
                // Set 1:  Raptor
                new Level(1, Hulls.Raptor, BackgroundMusic.Bobby, SkyMaterials.Morning, Exos.GetCaptainExoKey(1)),
                new Level(2, Hulls.Hammerhead, BackgroundMusic.Juggernaut, SkyMaterials.Purple, Exos.GetCaptainExoKey(2)),
                new Level(3, Hulls.Raptor, BackgroundMusic.Experimental, SkyMaterials.Dusk, Exos.GetCaptainExoKey(3)),
                
                // Set 2:  Bullshark
                new Level(4, Hulls.Rockjaw, BackgroundMusic.Nothing, SkyMaterials.Cold, Exos.GetCaptainExoKey(4)),
                new Level(5, Hulls.Bullshark, BackgroundMusic.Confusion, SkyMaterials.Midday, Exos.GetCaptainExoKey(5)),
                new Level(6, Hulls.Raptor, BackgroundMusic.Sleeper, SkyMaterials.Midnight, Exos.GetCaptainExoKey(6)),
                new Level(7, Hulls.Bullshark, BackgroundMusic.Bobby, SkyMaterials.Sunrise, Exos.GetCaptainExoKey(7)),

                // Set 3:  Rockjaw
                new Level(8, Hulls.Hammerhead, BackgroundMusic.Nothing, SkyMaterials.Cold, Exos.GetCaptainExoKey(8)),
                new Level(9, Hulls.Eagle, BackgroundMusic.Juggernaut, SkyMaterials.Morning, Exos.GetCaptainExoKey(9)),
                new Level(10, Hulls.Rockjaw, BackgroundMusic.Againagain, SkyMaterials.Purple, Exos.GetCaptainExoKey(10)),

                // Set 4:  Eagle
                new Level(11, Hulls.Longbow, BackgroundMusic.Sleeper, SkyMaterials.Midnight, Exos.GetCaptainExoKey(11)),
                new Level(12, Hulls.Bullshark, BackgroundMusic.Nothing, SkyMaterials.Midday, Exos.GetCaptainExoKey(12)),
                new Level(13, Hulls.Rockjaw, BackgroundMusic.Confusion, SkyMaterials.Dusk, Exos.GetCaptainExoKey(13)),
                new Level(14, Hulls.Eagle, BackgroundMusic.Bobby, SkyMaterials.Sunrise, Exos.GetCaptainExoKey(14)),
                new Level(15, Hulls.ManOfWarBoss, BackgroundMusic.Juggernaut, SkyMaterials.Midnight, Exos.GetCaptainExoKey(15)),

                // Set 5:  Hammerhead
                new Level(16, Hulls.Longbow, BackgroundMusic.Experimental, SkyMaterials.Morning, Exos.GetCaptainExoKey(16)),
                new Level(17, Hulls.Hammerhead, BackgroundMusic.Nothing, SkyMaterials.Midday, Exos.GetCaptainExoKey(17)),
                new Level(18, Hulls.Rickshaw, BackgroundMusic.Juggernaut, SkyMaterials.Dusk, Exos.GetCaptainExoKey(18)),

                // Set 6:  Longbow
                new Level(19, Hulls.Eagle, BackgroundMusic.Sleeper, SkyMaterials.Purple, Exos.GetCaptainExoKey(19)),
                new Level(20, Hulls.Rockjaw, BackgroundMusic.Againagain, SkyMaterials.Midnight, Exos.GetCaptainExoKey(20)),
                new Level(21, Hulls.Hammerhead, BackgroundMusic.Nothing, SkyMaterials.Cold, Exos.GetCaptainExoKey(21)),
                new Level(22, Hulls.Longbow, BackgroundMusic.Confusion, SkyMaterials.Sunrise, Exos.GetCaptainExoKey(22)),

                // Set 7:  Megalodon
                new Level(23, Hulls.Bullshark, BackgroundMusic.Bobby, SkyMaterials.Dusk, Exos.GetCaptainExoKey(23)),
                new Level(24, Hulls.Longbow, BackgroundMusic.Juggernaut, SkyMaterials.Midnight, Exos.GetCaptainExoKey(24)),
                new Level(25, Hulls.Raptor, BackgroundMusic.Nothing, SkyMaterials.Morning, Exos.GetCaptainExoKey(25)),
                new Level(26, Hulls.Megalodon, BackgroundMusic.Confusion, SkyMaterials.Midday, Exos.GetCaptainExoKey(26)),
				
			     // Set 8:  Huntress Prime
                new Level(27, Hulls.TasDevil, BackgroundMusic.Experimental, SkyMaterials.Purple, Exos.GetCaptainExoKey(27)),
                new Level(28, Hulls.BlackRig, BackgroundMusic.Juggernaut, SkyMaterials.Cold, Exos.GetCaptainExoKey(28)),
                new Level(29, Hulls.Rickshaw, BackgroundMusic.Againagain, SkyMaterials.Dusk, Exos.GetCaptainExoKey(29)),
                new Level(30, Hulls.Yeti, BackgroundMusic.Confusion, SkyMaterials.Midnight, Exos.GetCaptainExoKey(30)),
                new Level(31, Hulls.HuntressBoss, BackgroundMusic.Bobby, SkyMaterials.Sunrise, Exos.GetCaptainExoKey(31)), //HUNTRESS PRIME

                 // Set 9:  Secret Levels
                new Level(32, Hulls.Trident, BackgroundMusic.Experimental, SkyMaterials.Purple, Exos.GetCaptainExoKey(32)),
                new Level(33, Hulls.Raptor, BackgroundMusic.Juggernaut, SkyMaterials.Cold, Exos.GetCaptainExoKey(33)),
                new Level(34, Hulls.Bullshark, BackgroundMusic.Againagain, SkyMaterials.Dusk, Exos.GetCaptainExoKey(34)),
                new Level(35, Hulls.Rockjaw, BackgroundMusic.Confusion, SkyMaterials.Midnight, Exos.GetCaptainExoKey(35)),
                new Level(36, Hulls.Eagle, BackgroundMusic.Bobby, SkyMaterials.Sunrise, Exos.GetCaptainExoKey(36)),
                new Level(37, Hulls.Hammerhead, BackgroundMusic.Sleeper, SkyMaterials.Midday, Exos.GetCaptainExoKey(37)),
                new Level(38, Hulls.Longbow, BackgroundMusic.Nothing, SkyMaterials.Morning, Exos.GetCaptainExoKey(38)),
                new Level(39, Hulls.Megalodon, BackgroundMusic.Juggernaut, SkyMaterials.Sunrise, Exos.GetCaptainExoKey(39)),
                new Level(40, Hulls.TasDevil, BackgroundMusic.Againagain, SkyMaterials.Midnight, Exos.GetCaptainExoKey(40)) //TODO: Change to new boss broadsword
            };
        }

        private List<ISideQuestData> CreateSideQuests()
        {
            return new List<ISideQuestData>()
            {
                //Set 1: Original Secret Levels
                new SideQuestData(false, Exos.GetCaptainExoKey(32), 32, -1, Hulls.Trident, BackgroundMusic.Experimental, SkyMaterials.Purple, false, 0),
                new SideQuestData(false, Exos.GetCaptainExoKey(33), 32, 0, Hulls.Raptor, BackgroundMusic.Juggernaut, SkyMaterials.Cold, false, 1),
                new SideQuestData(false, Exos.GetCaptainExoKey(34), 32, 1, Hulls.Bullshark, BackgroundMusic.Againagain, SkyMaterials.Dusk, false, 2),
                new SideQuestData(false, Exos.GetCaptainExoKey(35), 32, 2, Hulls.Rockjaw, BackgroundMusic.Confusion, SkyMaterials.Midnight, false, 3),
                new SideQuestData(false, Exos.GetCaptainExoKey(36), 32, 3, Hulls.Eagle, BackgroundMusic.Bobby, SkyMaterials.Sunrise, false, 4),
                new SideQuestData(false, Exos.GetCaptainExoKey(37), 32, 4, Hulls.Hammerhead, BackgroundMusic.Sleeper, SkyMaterials.Midday, false, 5),
                new SideQuestData(false, Exos.GetCaptainExoKey(38), 32, 5, Hulls.Longbow, BackgroundMusic.Nothing, SkyMaterials.Morning, false, 6),
                new SideQuestData(false, Exos.GetCaptainExoKey(39), 32, 6, Hulls.Megalodon, BackgroundMusic.Juggernaut, SkyMaterials.Sunrise, false, 7),
                new SideQuestData(false, Exos.GetCaptainExoKey(40), 32, 7, Hulls.TasDevil, BackgroundMusic.Againagain, SkyMaterials.Midnight, false, 8),

                //Set 2: new SideQuests of BC v6.3
                new SideQuestData(false, Exos.GetCaptainExoKey(41), 8, -1, Hulls.Rickshaw, BackgroundMusic.Bobby, SkyMaterials.Purple, false, 9),
                new SideQuestData(false, Exos.GetCaptainExoKey(42), 11, -1, Hulls.BlackRig, BackgroundMusic.Confusion, SkyMaterials.Midnight, false, 10),
                new SideQuestData(false, Exos.GetCaptainExoKey(2), 16, -1, Hulls.Longbow, BackgroundMusic.Againagain, SkyMaterials.Cold, false, 11),
                new SideQuestData(false, Exos.GetCaptainExoKey(8), 19, -1, Hulls.Microlodon, BackgroundMusic.Nothing, SkyMaterials.Dusk, false, 12),
                new SideQuestData(false, Exos.GetCaptainExoKey(15), 0, 3, Hulls.Flea, BackgroundMusic.Sleeper, SkyMaterials.Sunrise, false, 13),
                new SideQuestData(false, Exos.GetCaptainExoKey(17), 23, -1, Hulls.Shepherd, BackgroundMusic.Experimental, SkyMaterials.Morning, false, 14),
                new SideQuestData(false, Exos.GetCaptainExoKey(50), 0, 9, Hulls.Shepherd, BackgroundMusic.Juggernaut, SkyMaterials.Midday, false, 15),
                new SideQuestData(false, Exos.GetCaptainExoKey(25), 0, 14 , Hulls.Flea, BackgroundMusic.Bobby, SkyMaterials.Dusk, false, 16),
                new SideQuestData(false, Exos.GetCaptainExoKey(29), 27, -1 , Hulls.Microlodon, BackgroundMusic.Confusion, SkyMaterials.Purple, false, 17),
                new SideQuestData(false, Exos.GetCaptainExoKey(1), 0, 17 , Hulls.Rickshaw, BackgroundMusic.Againagain, SkyMaterials.Midnight, false, 18),
                new SideQuestData(false, Exos.GetCaptainExoKey(8), 27, -1 , Hulls.TasDevil, BackgroundMusic.Nothing, SkyMaterials.Cold, false, 19),
                new SideQuestData(false, Exos.GetCaptainExoKey(46), 32, -1 , Hulls.TasDevil, BackgroundMusic.Sleeper, SkyMaterials.Sunrise, false, 20),
                new SideQuestData(false, Exos.GetCaptainExoKey(47), 32, 20 , Hulls.Megalodon, BackgroundMusic.Experimental, SkyMaterials.Morning, false, 21),
                new SideQuestData(false, Exos.GetCaptainExoKey(48), 32, 21 , Hulls.Yeti, BackgroundMusic.Juggernaut, SkyMaterials.Midday, false, 22),
                new SideQuestData(false, Exos.GetCaptainExoKey(49), 32, 22 , Hulls.FortressPrime, BackgroundMusic.Fortress, SkyMaterials.Midnight, false, 23),

                //Set 2: new SideQuests of BC v6.5
                new SideQuestData(false, Exos.GetCaptainExoKey(06), 6, 28, Hulls.Shepherd, BackgroundMusic.Experimental, SkyMaterials.Morning, false, 24),
                new SideQuestData(false, Exos.GetCaptainExoKey(4), 11, -1, Hulls.Megalith, BackgroundMusic.Bobby, SkyMaterials.Purple, false, 25),
                new SideQuestData(false, Exos.GetCaptainExoKey(13), 13, 10, Hulls.Pistol, BackgroundMusic.Confusion, SkyMaterials.Midnight, false, 26),
                new SideQuestData(false, Exos.GetCaptainExoKey(15), 15, 10, Hulls.Goatherd, BackgroundMusic.Againagain, SkyMaterials.Cold, false, 27),
                new SideQuestData(false, Exos.GetCaptainExoKey(44), 4, -1, Hulls.Goatherd, BackgroundMusic.Nothing, SkyMaterials.Dusk, false, 28),
                new SideQuestData(false, Exos.GetCaptainExoKey(45), 4, 27, Hulls.Megalith, BackgroundMusic.Sleeper, SkyMaterials.Sunrise, false, 29),
                new SideQuestData(false, Exos.GetCaptainExoKey(11), 11, 29, Hulls.Pistol, BackgroundMusic.Juggernaut, SkyMaterials.Midday, false, 30)
            };
        }


        private IDictionary<Map, IPvPLevel> CreatePvPLevels()
        {
            return new Dictionary<Map, IPvPLevel>()
            {
                // Practice Wreckyards
             {Map.PracticeWreckyards,  new PvPLevel(1, PvPStaticPrefabKeys.PvPHulls.PvPRaptor, PvPSoundKeys.Music.Background.Bobby, SkyMaterials.Morning)},
                // Oz Penitentiary
             {Map.OzPenitentiary,   new PvPLevel(2, PvPStaticPrefabKeys.PvPHulls.PvPBullshark, PvPSoundKeys.Music.Background.Juggernaut, SkyMaterials.Purple)},
                // San Francisco Fight Club
            {Map.SanFranciscoFightClub, new PvPLevel(3, PvPStaticPrefabKeys.PvPHulls.PvPRaptor, PvPSoundKeys.Music.Background.Experimental, SkyMaterials.Dusk)},
                // UAC Battle Night
            {Map.UACBattleNight, new PvPLevel(4, PvPStaticPrefabKeys.PvPHulls.PvPRockjaw, PvPSoundKeys.Music.Background.Nothing, SkyMaterials.Cold) },
                // Nuclear Dome
            {Map.NuclearDome,  new PvPLevel(5, PvPStaticPrefabKeys.PvPHulls.PvPBullshark, PvPSoundKeys.Music.Background.Confusion, SkyMaterials.Midday)},
                // UAC Arena
            {Map.UACArena, new PvPLevel(6, PvPStaticPrefabKeys.PvPHulls.PvPRaptor, PvPSoundKeys.Music.Background.Sleeper, SkyMaterials.Midnight)},
                // Rio Battlesport
            {Map.RioBattlesport, new PvPLevel(7, PvPStaticPrefabKeys.PvPHulls.PvPTasDevil, PvPSoundKeys.Music.Background.Bobby, SkyMaterials.Sunrise)},
                // UAC Ultimate
            {Map.UACUltimate,  new PvPLevel(8, PvPStaticPrefabKeys.PvPHulls.PvPHammerhead, PvPSoundKeys.Music.Background.Nothing, SkyMaterials.Cold)},
                // Mercenary One
            {Map.MercenaryOne,  new PvPLevel(9, PvPStaticPrefabKeys.PvPHulls.PvPEagle, PvPSoundKeys.Music.Background.Juggernaut, SkyMaterials.Morning)},
            };
        }
        private IDictionary<BuildingKey, int> AssignBuildingUnlockLevel()
        {
            return new Dictionary<BuildingKey, int>()
            {
                // Factories
                { Buildings.AirFactory, 1 },  //The number represents the first *main story* level you get this item, so it unlocks when you win the previous level.
                { Buildings.NavalFactory, 1 },
                { Buildings.DroneStation, 1 },
                { Buildings.DroneStation4, 27 },
                { Buildings.DroneStation6, 95 },
                { Buildings.DroneStation8, 31 },

                // Tactical
                { Buildings.ShieldGenerator, 2 },
                { Buildings.LocalBooster, 9 },
                { Buildings.StealthGenerator, 17 },
                { Buildings.SpySatelliteLauncher, 18 },
                { Buildings.ControlTower, 24 },
                { Buildings.GrapheneBarrier, 95 },

                // Defence
                { Buildings.AntiShipTurret, 1 },
                { Buildings.AntiAirTurret, 1 },
                { Buildings.Mortar, 3 },
                { Buildings.SamSite, 5 },
                { Buildings.TeslaCoil, 21 },
                { Buildings.Coastguard, 39 },
                { Buildings.FlakTurret, 95 },
                { Buildings.CIWS, 95 },

                // Offence
                { Buildings.Artillery, 1 },
                { Buildings.RocketLauncher, 20 },
                { Buildings.Railgun, 6 },
                { Buildings.MLRS, 29},
                { Buildings.GatlingMortar, 32},
                { Buildings.MissilePod, 36 },
                { Buildings.IonCannon, 38 },
                { Buildings.Cannon, 95 },

                // Ultras
                { Buildings.DeathstarLauncher, 7 },
                { Buildings.NukeLauncher, 10 },
                { Buildings.Ultralisk, 14 },
                { Buildings.KamikazeSignal, 22 },
                { Buildings.Broadsides, 25 },
                { Buildings.NovaArtillery, 33 },
                { Buildings.UltraCIWS, 95 },
                { Buildings.GlobeShield, 95 },
                { Buildings.Sledgehammer, 95 } //Set to 95: way past the highest main story level, so that the sidequest unlocks it instead.
            };
        }

        private IDictionary<UnitKey, int> AssignUnitUnlockLevel()
        {
            return new Dictionary<UnitKey, int>()
            {
                // Aircraft
                { Units.Bomber, 1 },
                { Units.Gunship, 5 },
                { Units.Fighter, 12 },
                { Units.SteamCopter, 28 },
                { Units.Broadsword, 41 },
                { Units.StratBomber, 95 },
                { Units.SpyPlane, 95 },
                { Units.MissileFighter, 95 },
                
                
                // Ships
                { Units.AttackBoat, 1 },
                { Units.Frigate, 3 },
                { Units.Destroyer, 13 },
                { Units.SiegeDestroyer, 95 },
                { Units.GlassCannoneer, 95 },
                { Units.GunBoat, 95 },
                { Units.ArchonBattleship, 16 },
                { Units.AttackRIB, 30 },
                { Units.RocketTurtle, 95 },
                { Units.FlakTurtle, 95 }
            };
        }

        private IDictionary<HullKey, int> AssignHullUnlockLevel()
        {
            return new Dictionary<HullKey, int>()
            {
                { Hulls.Trident, 1 },
                { Hulls.Raptor, 4 },
                { Hulls.Bullshark, 8 },
                { Hulls.Rockjaw, 11},
                { Hulls.Eagle, 15 },
                { Hulls.Hammerhead, 19 },
                { Hulls.Longbow, 23 },
                { Hulls.Megalodon, 26 },
                { Hulls.Megalith, 45 },
                { Hulls.Rickshaw, 34 },
                { Hulls.TasDevil, 35 },
                { Hulls.BlackRig, 37 },
                { Hulls.Yeti, 40 }
            };
        }

        private IDictionary<BuildingKey, int> AssignBuildingUnlockSideQuest()
        {
            //The number represents the side quest ID that unlocks the building
            //Right now, this only contains the loot from the secret levels

            return new Dictionary<BuildingKey, int>()
            {
                //Factories
                { Buildings.DroneStation6, 22 },

                // Defence
                { Buildings.MissilePod, 3 },
                { Buildings.Coastguard, 6 },
                { Buildings.CIWS, 10 },
                { Buildings.FlakTurret, 14 },

                //Tactical
                { Buildings.GrapheneBarrier, 11 },
                { Buildings.GlobeShield, 23 },

                // Offence
                { Buildings.IonCannon, 5 },
                { Buildings.Cannon, 28 },

                // Ultras
                { Buildings.NovaArtillery, 0 },
                { Buildings.UltraCIWS, 18 },
                { Buildings.Sledgehammer, 30 }
            };
        }

        private IDictionary<UnitKey, int> AssignUnitUnlockSideQuest()
        {
            //The number represents the side quest ID that unlocks the unit
            //Right now, this only contains the loot from the secret levels

            return new Dictionary<UnitKey, int>()
            {
                // Aircraft
                { Units.Broadsword, 8 },
                { Units.StratBomber, 9 },
                { Units.SpyPlane, 29 },
                
                // Ships
                { Units.GlassCannoneer, 16 },
                { Units.SiegeDestroyer, 17 },
                { Units.RocketTurtle, 19 },
                { Units.GunBoat, 20 },
                { Units.FlakTurtle, 21 }
            };
        }

        private IDictionary<HullKey, int> AssignHullUnlockSideQuest()
        {
            //The number represents the side quest ID that unlocks the hull
            //Right now, this only contains the loot from the secret levels

            return new Dictionary<HullKey, int>()
            {
                { Hulls.Rickshaw, 1 },
                { Hulls.TasDevil, 2 },
                { Hulls.BlackRig, 4 },
                { Hulls.Yeti, 7 },
                { Hulls.Microlodon, 12},
                { Hulls.Flea, 13 },
                { Hulls.Shepherd, 15},
                { Hulls.Pistol, 26},
                { Hulls.Goatherd, 27},
                { Hulls.Megalith, 25}
            };
        }

        /// <summary>
        /// Availability level number:  The first level that prefab is available.
        /// Loot level number (level completed):  The level that unlocks the prefab when you complete
        /// it successfully.
        /// 
        /// Availability level number = loot level number + 1
        /// </summary>

        public ILoot GetLevelLoot(int levelCompleted)
        {
            int availabilityLevelNum = levelCompleted + 1;

            Assert.IsTrue(availabilityLevelNum >= MIN_AVAILABILITY_LEVEL_NUM);
            Assert.IsTrue(availabilityLevelNum <= Levels.Count + 1);

            return
                new Loot(
                    hullKeys: GetHullsUnlockedInLevel(availabilityLevelNum),
                    unitKeys: GetUnitsUnlockedInLevel(availabilityLevelNum),
                    buildingKeys: GetBuildingsUnlockedInLevel(availabilityLevelNum));
        }

        public ILoot GetSideQuestLoot(int sideQuestID)
        {

            int availabilitySideQuestNum = sideQuestID;
            //hardcoded values while testing
            Assert.IsTrue(availabilitySideQuestNum >= 0);
            Assert.IsTrue(availabilitySideQuestNum <= NUM_OF_SIDEQUESTS);

            return
                new Loot(
                    hullKeys: GetHullsUnlockedInSideQuest(availabilitySideQuestNum),
                    unitKeys: GetUnitsUnlockedInSideQuest(availabilitySideQuestNum),
                    buildingKeys: GetBuildingsUnlockedInSideQuest(availabilitySideQuestNum));

        }

        private IList<UnitKey> GetUnitsUnlockedInLevel(int levelFirstAvailableIn)
        {
            return GetBuildablesUnlockedInLevel(_unitToUnlockedLevel, levelFirstAvailableIn);
        }

        private IList<BuildingKey> GetBuildingsUnlockedInLevel(int levelFirstAvailableIn)
        {
            return GetBuildablesUnlockedInLevel(_buildingToUnlockedLevel, levelFirstAvailableIn);
        }

        /// <summary>
        /// List should always have 0 or 1 entry, unless levelFirstAvailableIn is 1
        /// (ie, the starting level, where we have multiple buildables available).
        /// </summary>
        private IList<TKey> GetBuildablesUnlockedInLevel<TKey>(IDictionary<TKey, int> buildableToUnlockedLevel, int levelFirstAvailableIn)
            where TKey : IPrefabKey
        {
            return
                buildableToUnlockedLevel
                    .Where(buildableToLevel => buildableToLevel.Value == levelFirstAvailableIn)
                    .Select(buildableToLevel => buildableToLevel.Key)
                    .ToList();
        }

        private IList<HullKey> GetHullsUnlockedInLevel(int levelFirstAvailableIn)
        {
            return
                _hullToUnlockedLevel
                    .Where(hullToLevel => hullToLevel.Value == levelFirstAvailableIn)
                    .Select(hullToLevel => hullToLevel.Key)
                    .ToList();
        }

        public int UnitUnlockLevel(UnitKey unitKey)
        {
            Assert.IsTrue(_unitToUnlockedLevel.ContainsKey(unitKey));
            return _unitToUnlockedLevel[unitKey];
        }

        public int BuildingUnlockLevel(BuildingKey buildingKey)
        {
            //Assert.IsTrue(_buildingToUnlockedLevel.ContainsKey(buildingKey));
            return _buildingToUnlockedLevel[buildingKey];
        }

        private IList<UnitKey> GetUnitsUnlockedInSideQuest(int requiredSideQuestID)
        {
            return GetBuildablesUnlockedInSideQuest(_unitToCompletedSideQuest, requiredSideQuestID);
        }

        private IList<BuildingKey> GetBuildingsUnlockedInSideQuest(int requiredSideQuestID)
        {
            return GetBuildablesUnlockedInSideQuest(_buildingToCompletedSideQuest, requiredSideQuestID);
        }

        private IList<TKey> GetBuildablesUnlockedInSideQuest<TKey>(IDictionary<TKey, int> buildableToCompletedSideQuest, int requiredSideQuestID)
            where TKey : IPrefabKey
        {
            return
                buildableToCompletedSideQuest
                    .Where(buildableToSideQuest => buildableToSideQuest.Value == requiredSideQuestID)
                    .Select(buildableToSideQuest => buildableToSideQuest.Key)
                    .ToList();
        }

        private IList<HullKey> GetHullsUnlockedInSideQuest(int requiredSideQuestID)
        {
            return
                _hullToCompletedSideQuest
                    .Where(hullToSideQuest => hullToSideQuest.Value == requiredSideQuestID)
                    .Select(hullToSideQuest => hullToSideQuest.Key)
                    .ToList();
        }

        public int UnitUnlockSideQuest(UnitKey unitKey)
        {
            Assert.IsTrue(_unitToCompletedSideQuest.ContainsKey(unitKey));
            return _unitToCompletedSideQuest[unitKey];
        }

        public int BuildingSideQuest(BuildingKey buildingKey)
        {
            Assert.IsTrue(_buildingToCompletedSideQuest.ContainsKey(buildingKey));
            return _buildingToCompletedSideQuest[buildingKey];
        }
    }
}
