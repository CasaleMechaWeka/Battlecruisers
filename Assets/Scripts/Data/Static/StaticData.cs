using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static.LevelLoot;
using BattleCruisers.Data.Static.Strategies.Helper;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Data.Static
{
    public class StaticData : IStaticData
	{
        private readonly IDictionary<BuildingKey, int> _buildingToUnlockedLevel;
        private readonly IDictionary<UnitKey, int> _unitToUnlockedLevel;
        private readonly IDictionary<HullKey, int> _hullToUnlockedLevel;

        private readonly IList<BuildingKey> _allBuildings;
        private readonly IList<UnitKey> _allUnits;

        private const int MIN_AVAILABILITY_LEVEL_NUM = 2;
        public const int NUM_OF_LEVELS = 40;
        public const int NUM_OF_LEVELS_IN_DEMO = 7;

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
		public ReadOnlyCollection<HullKey> HullKeys { get; }
		public ReadOnlyCollection<UnitKey> UnitKeys { get; }
        public ReadOnlyCollection<BuildingKey> BuildingKeys { get; }
        public ReadOnlyCollection<BuildingKey> AIBannedUltrakeys { get; }
        public int LastLevelWithLoot => 31;
        public ILevelStrategies Strategies { get; }

        public StaticData()
		{
            HullKeys = AllHullKeys().AsReadOnly();

            _allBuildings = AllBuildingKeys();
            BuildingKeys = new ReadOnlyCollection<BuildingKey>(_allBuildings);

            _allUnits = AllUnitKeys();
            UnitKeys = new ReadOnlyCollection<UnitKey>(_allUnits);

            _buildingToUnlockedLevel = CreateBuildingAvailabilityMap();
            _unitToUnlockedLevel = CreateUnitAvailabilityMap();
            _hullToUnlockedLevel = CreateHullAvailabilityMap();

            Strategies = new LevelStrategies();

            AIBannedUltrakeys = new ReadOnlyCollection<BuildingKey>(CreateAIBannedUltraKeys());
			
            InitialGameModel = CreateInitialGameModel();
            Levels = new ReadOnlyCollection<ILevel>(CreateLevels());
		}

		private List<HullKey> AllHullKeys()
		{
            // In order they are available to the user.  Means the loadout
            // screen order is nice :)
			return new List<HullKey>()
			{
				StaticPrefabKeys.Hulls.Trident,
				StaticPrefabKeys.Hulls.Bullshark,
				StaticPrefabKeys.Hulls.Raptor,
				StaticPrefabKeys.Hulls.Rockjaw,
				StaticPrefabKeys.Hulls.Eagle,
				StaticPrefabKeys.Hulls.Hammerhead,
				StaticPrefabKeys.Hulls.Longbow,
				StaticPrefabKeys.Hulls.Megalodon,
                StaticPrefabKeys.Hulls.BlackRig,
                StaticPrefabKeys.Hulls.Rickshaw,
                StaticPrefabKeys.Hulls.TasDevil,
                StaticPrefabKeys.Hulls.Yeti,
                //Ultra Cruisers
                StaticPrefabKeys.Hulls.UltraTrident,
                StaticPrefabKeys.Hulls.UltraBullshark,
                StaticPrefabKeys.Hulls.UltraRaptor,
                StaticPrefabKeys.Hulls.UltraRockjaw,
                StaticPrefabKeys.Hulls.UltraEagle,
                StaticPrefabKeys.Hulls.UltraHammerhead,
                StaticPrefabKeys.Hulls.UltraLongbow,
                StaticPrefabKeys.Hulls.UltraMegalodon
            };
		}

		private List<BuildingKey> AllBuildingKeys()
		{
            // Buildings in a category (eg:  Factories) are in the order they 
            // become available to the user.  Means the loadout screen order is nice :)
            return new List<BuildingKey>()
			{
                // Factories
                StaticPrefabKeys.Buildings.AirFactory,
				StaticPrefabKeys.Buildings.NavalFactory,
				StaticPrefabKeys.Buildings.DroneStation,
                StaticPrefabKeys.Buildings.DroneStation4,
                StaticPrefabKeys.Buildings.DroneStation8,

                // Tactical
                StaticPrefabKeys.Buildings.ShieldGenerator,
				StaticPrefabKeys.Buildings.LocalBooster,
                StaticPrefabKeys.Buildings.ControlTower,
                StaticPrefabKeys.Buildings.StealthGenerator,
				StaticPrefabKeys.Buildings.SpySatelliteLauncher,

                // Defence
                StaticPrefabKeys.Buildings.AntiShipTurret,
				StaticPrefabKeys.Buildings.AntiAirTurret,
				StaticPrefabKeys.Buildings.Mortar,
				StaticPrefabKeys.Buildings.SamSite,
				StaticPrefabKeys.Buildings.TeslaCoil,
                StaticPrefabKeys.Buildings.Coastguard,//new

                // Offence
                StaticPrefabKeys.Buildings.Artillery,
				StaticPrefabKeys.Buildings.Railgun,
				StaticPrefabKeys.Buildings.RocketLauncher,
                StaticPrefabKeys.Buildings.MLRS,
                StaticPrefabKeys.Buildings.GatlingMortar,
                StaticPrefabKeys.Buildings.MissilePod,//new
                StaticPrefabKeys.Buildings.IonCannon,//new

                // Ultras
                StaticPrefabKeys.Buildings.DeathstarLauncher,
				StaticPrefabKeys.Buildings.NukeLauncher,
				StaticPrefabKeys.Buildings.Ultralisk,
				StaticPrefabKeys.Buildings.KamikazeSignal,
                StaticPrefabKeys.Buildings.Broadsides,
                StaticPrefabKeys.Buildings.NovaArtillery//new
			};
		}

        private IList<BuildingKey> CreateAIBannedUltraKeys()
        {
            return new List<BuildingKey>()
            {
                // Don't want AI to try and build a kamikaze signal as an ultra,
                // as it is only effective if there are a certain number of planes.
                // Simpler to make the AI only build ultras that are always effective.
                StaticPrefabKeys.Buildings.KamikazeSignal,

                // Don't want AI to try and build an ultralisk as an ultra,
                // because it is only super effective if building something else
                // afterwards (other offensives, ultras, or units).  As the AI's
                // strategy may be to win with a fast ultra (after which the AI
                // may do nothing), again only let the AI build ultras that
                // are always effective.
                StaticPrefabKeys.Buildings.Ultralisk
            };
        }

        private List<UnitKey> AllUnitKeys()
		{
            // Units in a category (eg:  Aircraft) are in the order they 
            // become available to the user.  Means the loadout screen order is nice :)
			return new List<UnitKey>()
			{
                // Aircraft
                StaticPrefabKeys.Units.Bomber,
                StaticPrefabKeys.Units.Gunship,
				StaticPrefabKeys.Units.Fighter,
                StaticPrefabKeys.Units.SteamCopter,

                // Ships
                StaticPrefabKeys.Units.AttackBoat,
				StaticPrefabKeys.Units.Frigate,
                StaticPrefabKeys.Units.Destroyer,
                StaticPrefabKeys.Units.ArchonBattleship,
                StaticPrefabKeys.Units.AttackRIB
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
            Loadout playerLoadout = new Loadout(initialHull, GetInitialBuildings(), GetInitialUnits(),GetInitialbuildingLimit(),GetInitialUnitLimit());
            //Loadout playerLoadout = new Loadout(initialHull, AllBuildingKeys(), AllUnitKeys());

            bool hasAttemptedTutorial = false;

            return new GameModel(
                hasAttemptedTutorial,
                0,
                0,
                playerLoadout,
                lastBattleResult: null,
                // TEMP  Do not unlock all hulls & buildables at the game start :P
                unlockedHulls: new List<HullKey>() { initialHull },
                unlockedBuildings: GetInitialBuildings(),
                unlockedUnits: GetInitialUnits());
                //unlockedHulls: AllHullKeys(),
                //unlockedBuildings: AllBuildingKeys(),
                //unlockedUnits: AllUnitKeys());
        }

        private Dictionary<BuildingCategory,List<BuildingKey>> GetInitialbuildingLimit()
        {
            List<BuildingKey> limit = GetInitialBuildings();
            List<BuildingKey> factories = new List<BuildingKey>();
            List<BuildingKey> defence = new List<BuildingKey>();
            List<BuildingKey> offense = new List<BuildingKey>();
            List<BuildingKey> tactical = new List<BuildingKey>();
            List<BuildingKey> Ultra = new List<BuildingKey>();
            foreach (BuildingKey key in limit)
            {
                switch(key.BuildingCategory)
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
            foreach(UnitKey unit in units)
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
            return StaticPrefabKeys.Hulls.Trident;
        }

        private List<BuildingKey> GetInitialBuildings()
        {
            return GetBuildingsFirstAvailableIn(levelFirstAvailableIn: 1).ToList();
        }

        private List<UnitKey> GetInitialUnits()
        {
            return GetUnitsFirstAvailableIn(levelFirstAvailableIn: 1).ToList();
        }

		private IList<ILevel> CreateLevels()
		{
			return new List<ILevel>()
			{
                // Set 1:  Raptor
                new Level(1, StaticPrefabKeys.Hulls.Raptor, SoundKeys.Music.Background.Bobby, SkyMaterials.Morning),
                new Level(2, StaticPrefabKeys.Hulls.Bullshark, SoundKeys.Music.Background.Juggernaut, SkyMaterials.Purple),
                new Level(3, StaticPrefabKeys.Hulls.Raptor, SoundKeys.Music.Background.Experimental, SkyMaterials.Dusk),
                
                // Set 2:  Bullshark
                new Level(4, StaticPrefabKeys.Hulls.Rockjaw, SoundKeys.Music.Background.Nothing, SkyMaterials.Cold),
                new Level(5, StaticPrefabKeys.Hulls.Bullshark, SoundKeys.Music.Background.Confusion, SkyMaterials.Midday),
                new Level(6, StaticPrefabKeys.Hulls.Raptor, SoundKeys.Music.Background.Sleeper, SkyMaterials.Midnight),
                new Level(7, StaticPrefabKeys.Hulls.Bullshark, SoundKeys.Music.Background.Bobby, SkyMaterials.Sunrise),

                // Set 3:  Rockjaw
                new Level(8, StaticPrefabKeys.Hulls.Hammerhead, SoundKeys.Music.Background.Nothing, SkyMaterials.Cold),
                new Level(9, StaticPrefabKeys.Hulls.Eagle, SoundKeys.Music.Background.Juggernaut, SkyMaterials.Morning),
                new Level(10, StaticPrefabKeys.Hulls.Rockjaw, SoundKeys.Music.Background.Againagain, SkyMaterials.Purple),

                // Set 4:  Eagle
                new Level(11, StaticPrefabKeys.Hulls.Longbow, SoundKeys.Music.Background.Sleeper, SkyMaterials.Midnight),
                new Level(12, StaticPrefabKeys.Hulls.Bullshark, SoundKeys.Music.Background.Nothing, SkyMaterials.Midday),
                new Level(13, StaticPrefabKeys.Hulls.Rockjaw, SoundKeys.Music.Background.Confusion, SkyMaterials.Dusk),
                new Level(14, StaticPrefabKeys.Hulls.Eagle, SoundKeys.Music.Background.Bobby, SkyMaterials.Sunrise),
                new Level(15, StaticPrefabKeys.Hulls.ManOfWarBoss, SoundKeys.Music.Background.Juggernaut, SkyMaterials.Midnight),

                // Set 5:  Hammerhead
                new Level(16, StaticPrefabKeys.Hulls.Longbow, SoundKeys.Music.Background.Experimental, SkyMaterials.Morning),
                new Level(17, StaticPrefabKeys.Hulls.Hammerhead, SoundKeys.Music.Background.Nothing, SkyMaterials.Midday),
                new Level(18, StaticPrefabKeys.Hulls.Rickshaw, SoundKeys.Music.Background.Juggernaut, SkyMaterials.Dusk),

                // Set 6:  Longbow
                new Level(19, StaticPrefabKeys.Hulls.Eagle, SoundKeys.Music.Background.Sleeper, SkyMaterials.Purple),
                new Level(20, StaticPrefabKeys.Hulls.Rockjaw, SoundKeys.Music.Background.Againagain, SkyMaterials.Midnight),
                new Level(21, StaticPrefabKeys.Hulls.Hammerhead, SoundKeys.Music.Background.Nothing, SkyMaterials.Cold),
                new Level(22, StaticPrefabKeys.Hulls.BlackRig, SoundKeys.Music.Background.Confusion, SkyMaterials.Sunrise),

                // Set 7:  Megolodon
                new Level(23, StaticPrefabKeys.Hulls.Bullshark, SoundKeys.Music.Background.Bobby, SkyMaterials.Dusk),
                new Level(24, StaticPrefabKeys.Hulls.Longbow, SoundKeys.Music.Background.Juggernaut, SkyMaterials.Midnight),
                new Level(25, StaticPrefabKeys.Hulls.Raptor, SoundKeys.Music.Background.Nothing, SkyMaterials.Morning),
                new Level(26, StaticPrefabKeys.Hulls.Megalodon, SoundKeys.Music.Background.Confusion, SkyMaterials.Midday),
				
			     // Set 8:  Huntress Prime
                new Level(27, StaticPrefabKeys.Hulls.TasDevil, SoundKeys.Music.Background.Experimental, SkyMaterials.Purple),
                new Level(28, StaticPrefabKeys.Hulls.BlackRig, SoundKeys.Music.Background.Juggernaut, SkyMaterials.Cold),
                new Level(29, StaticPrefabKeys.Hulls.Rickshaw, SoundKeys.Music.Background.Againagain, SkyMaterials.Dusk),
                new Level(30, StaticPrefabKeys.Hulls.Yeti, SoundKeys.Music.Background.Confusion, SkyMaterials.Midnight),
                new Level(31, StaticPrefabKeys.Hulls.HuntressBoss, SoundKeys.Music.Background.Bobby, SkyMaterials.Sunrise),

                 // Set 9:  Secret Levels
                new Level(32, StaticPrefabKeys.Hulls.Rickshaw, SoundKeys.Music.Background.Experimental, SkyMaterials.Purple),
                new Level(33, StaticPrefabKeys.Hulls.TasDevil, SoundKeys.Music.Background.Juggernaut, SkyMaterials.Cold),
                new Level(34, StaticPrefabKeys.Hulls.Longbow, SoundKeys.Music.Background.Againagain, SkyMaterials.Dusk),
                new Level(35, StaticPrefabKeys.Hulls.BlackRig, SoundKeys.Music.Background.Confusion, SkyMaterials.Midnight),
                new Level(36, StaticPrefabKeys.Hulls.Eagle, SoundKeys.Music.Background.Bobby, SkyMaterials.Sunrise),
                new Level(37, StaticPrefabKeys.Hulls.Longbow, SoundKeys.Music.Background.Sleeper, SkyMaterials.Midday),
                new Level(38, StaticPrefabKeys.Hulls.Rickshaw, SoundKeys.Music.Background.Nothing, SkyMaterials.Morning),
                new Level(39, StaticPrefabKeys.Hulls.Yeti, SoundKeys.Music.Background.Juggernaut, SkyMaterials.Sunrise),
                new Level(40, StaticPrefabKeys.Hulls.Yeti, SoundKeys.Music.Background.Juggernaut, SkyMaterials.Sunrise) //TODO: Change to new boss broadsword
            };
		}

        private IDictionary<BuildingKey, int> CreateBuildingAvailabilityMap()
        {
            return new Dictionary<BuildingKey, int>()
            {
                // Factories
                { StaticPrefabKeys.Buildings.AirFactory, 1 },
                { StaticPrefabKeys.Buildings.NavalFactory, 1 },
                { StaticPrefabKeys.Buildings.DroneStation, 1 },
                { StaticPrefabKeys.Buildings.DroneStation4, 27 },
                { StaticPrefabKeys.Buildings.DroneStation8, 31 },

                // Tactical
                { StaticPrefabKeys.Buildings.ShieldGenerator, 2 },
                { StaticPrefabKeys.Buildings.LocalBooster, 9 },
                { StaticPrefabKeys.Buildings.ControlTower, 24 },
                { StaticPrefabKeys.Buildings.StealthGenerator, 17 },
                { StaticPrefabKeys.Buildings.SpySatelliteLauncher, 18 },

                // Defence
                { StaticPrefabKeys.Buildings.AntiShipTurret, 1 },
                { StaticPrefabKeys.Buildings.AntiAirTurret, 1 },
                { StaticPrefabKeys.Buildings.Mortar, 3 },
                { StaticPrefabKeys.Buildings.SamSite, 5 },
                { StaticPrefabKeys.Buildings.TeslaCoil, 21 },
                { StaticPrefabKeys.Buildings.Coastguard, 33 },

                // Offence
                { StaticPrefabKeys.Buildings.Artillery, 1 },
                { StaticPrefabKeys.Buildings.RocketLauncher, 20 },
                { StaticPrefabKeys.Buildings.Railgun, 6 },
                { StaticPrefabKeys.Buildings.MLRS, 29},
                { StaticPrefabKeys.Buildings.GatlingMortar, 32},
                { StaticPrefabKeys.Buildings.MissilePod, 36 },
                { StaticPrefabKeys.Buildings.IonCannon, 37 },

                // Ultras
                { StaticPrefabKeys.Buildings.DeathstarLauncher, 7 },
                { StaticPrefabKeys.Buildings.NukeLauncher, 10 },
                { StaticPrefabKeys.Buildings.Ultralisk, 14 },
                { StaticPrefabKeys.Buildings.KamikazeSignal, 22 },
                { StaticPrefabKeys.Buildings.Broadsides, 25 },
                { StaticPrefabKeys.Buildings.NovaArtillery, 39 }
            };
        }

        private IDictionary<UnitKey, int> CreateUnitAvailabilityMap()
        {
            return new Dictionary<UnitKey, int>()
            {
                // Aircraft
                { StaticPrefabKeys.Units.Bomber, 1 },
                { StaticPrefabKeys.Units.Gunship, 5 },
                { StaticPrefabKeys.Units.Fighter, 12 },
                { StaticPrefabKeys.Units.SteamCopter, 28 },
                
                // Ships
                { StaticPrefabKeys.Units.AttackBoat, 1 },
                { StaticPrefabKeys.Units.Frigate, 3 },
                { StaticPrefabKeys.Units.Destroyer, 13 },
                { StaticPrefabKeys.Units.ArchonBattleship, 16 },
                { StaticPrefabKeys.Units.AttackRIB, 30 }
            };
        }

        private IDictionary<HullKey, int> CreateHullAvailabilityMap()
        {
            return new Dictionary<HullKey, int>()
            {
                { StaticPrefabKeys.Hulls.Trident, 1 },
                { StaticPrefabKeys.Hulls.Raptor, 4 },
                { StaticPrefabKeys.Hulls.Bullshark, 8 },
                { StaticPrefabKeys.Hulls.Rockjaw, 11},
                { StaticPrefabKeys.Hulls.Eagle, 15 },
                { StaticPrefabKeys.Hulls.Hammerhead, 19 },
                { StaticPrefabKeys.Hulls.Longbow, 23 },
                { StaticPrefabKeys.Hulls.Megalodon, 26 },
                { StaticPrefabKeys.Hulls.Rickshaw, 34 },
                { StaticPrefabKeys.Hulls.TasDevil, 35 },
                { StaticPrefabKeys.Hulls.BlackRig, 38 },
                { StaticPrefabKeys.Hulls.Yeti, 40 }
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
                    hullKeys: GetHullsFirstAvailableIn(availabilityLevelNum),
                    unitKeys: GetUnitsFirstAvailableIn(availabilityLevelNum),
                    buildingKeys: GetBuildingsFirstAvailableIn(availabilityLevelNum));
        }

        private IList<UnitKey> GetUnitsFirstAvailableIn(int levelFirstAvailableIn)
        {
            return GetBuildablesFirstAvailableIn(_unitToUnlockedLevel, levelFirstAvailableIn);
        }

        private IList<BuildingKey> GetBuildingsFirstAvailableIn(int levelFirstAvailableIn)
        {
            return GetBuildablesFirstAvailableIn(_buildingToUnlockedLevel, levelFirstAvailableIn);
        }

		/// <summary>
		/// List should always have 0 or 1 entry, unless levelFirstAvailableIn is 1
		/// (ie, the starting level, where we have multiple buildables available).
		/// </summary>
        private IList<TKey> GetBuildablesFirstAvailableIn<TKey>(IDictionary<TKey, int> buildableToUnlockedLevel, int levelFirstAvailableIn)
            where TKey : IPrefabKey
		{
			return
				buildableToUnlockedLevel
					.Where(buildableToLevel => buildableToLevel.Value == levelFirstAvailableIn)
					.Select(buildableToLevel => buildableToLevel.Key)
					.ToList();
		}

        private IList<HullKey> GetHullsFirstAvailableIn(int levelFirstAvailableIn)
        {
            return
                _hullToUnlockedLevel
                    .Where(hullToLevel => hullToLevel.Value == levelFirstAvailableIn)
                    .Select(hullToLevel => hullToLevel.Key)
                    .ToList();
        }

        public int LevelFirstAvailableIn(UnitKey unitKey)
        {
            Assert.IsTrue(_unitToUnlockedLevel.ContainsKey(unitKey));
            return _unitToUnlockedLevel[unitKey];
        }

        public int LevelFirstAvailableIn(BuildingKey buildingKey)
        {
            Assert.IsTrue(_buildingToUnlockedLevel.ContainsKey(buildingKey));
            return _buildingToUnlockedLevel[buildingKey];
        }
    }
}
