using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static.LevelLoot;
using BattleCruisers.Data.Static.Strategies.Helper;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        public const int NUM_OF_LEVELS = 26;
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
        public int LastLevelWithLoot => 25;
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
                StaticPrefabKeys.Hulls.ManOfWarBoss,
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

                // Offence
                StaticPrefabKeys.Buildings.Artillery,
				StaticPrefabKeys.Buildings.Railgun,
				StaticPrefabKeys.Buildings.RocketLauncher,

                // Ultras
                StaticPrefabKeys.Buildings.DeathstarLauncher,
				StaticPrefabKeys.Buildings.NukeLauncher,
				StaticPrefabKeys.Buildings.Ultralisk,
				StaticPrefabKeys.Buildings.KamikazeSignal,
                StaticPrefabKeys.Buildings.Broadsides
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

                // Ships
                StaticPrefabKeys.Units.AttackBoat,
				StaticPrefabKeys.Units.Frigate,
                StaticPrefabKeys.Units.Destroyer,
                StaticPrefabKeys.Units.ArchonBattleship
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
            Loadout playerLoadout = new Loadout(initialHull, GetInitialBuildings(), GetInitialUnits());
            //Loadout playerLoadout = new Loadout(initialHull, AllBuildingKeys(), AllUnitKeys());

            bool hasAttemptedTutorial = false;

            return new GameModel(
                hasAttemptedTutorial,
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
                new Level(1, StaticPrefabKeys.Hulls.HuntressBoss, SoundKeys.Music.Background.Bobby, SkyMaterials.Morning),
                new Level(2, StaticPrefabKeys.Hulls.Bullshark, SoundKeys.Music.Background.Juggernaut, SkyMaterials.Purple),
                new Level(3, StaticPrefabKeys.Hulls.Raptor, SoundKeys.Music.Background.Bobby, SkyMaterials.Dusk),
                
                // Set 2:  Bullshark
                new Level(4, StaticPrefabKeys.Hulls.Rockjaw, SoundKeys.Music.Background.Nothing, SkyMaterials.Cold),
                new Level(5, StaticPrefabKeys.Hulls.Bullshark, SoundKeys.Music.Background.Confusion, SkyMaterials.Midday),
                new Level(6, StaticPrefabKeys.Hulls.Raptor, SoundKeys.Music.Background.Bobby, SkyMaterials.Midnight),
                new Level(7, StaticPrefabKeys.Hulls.Bullshark, SoundKeys.Music.Background.Sleeper, SkyMaterials.Sunrise),

                // Set 3:  Rockjaw
                new Level(8, StaticPrefabKeys.Hulls.Hammerhead, SoundKeys.Music.Background.Nothing, SkyMaterials.Cold),
                new Level(9, StaticPrefabKeys.Hulls.Eagle, SoundKeys.Music.Background.Bobby, SkyMaterials.Morning),
                new Level(10, StaticPrefabKeys.Hulls.Rockjaw, SoundKeys.Music.Background.Experimental, SkyMaterials.Purple),

                // Set 4:  Eagle
                new Level(11, StaticPrefabKeys.Hulls.Longbow, SoundKeys.Music.Background.Juggernaut, SkyMaterials.Midnight),
                new Level(12, StaticPrefabKeys.Hulls.Eagle, SoundKeys.Music.Background.Sleeper, SkyMaterials.Midday),
                new Level(13, StaticPrefabKeys.Hulls.Hammerhead, SoundKeys.Music.Background.Confusion, SkyMaterials.Dusk),
                new Level(14, StaticPrefabKeys.Hulls.Eagle, SoundKeys.Music.Background.Juggernaut, SkyMaterials.Sunrise),
                new Level(15, StaticPrefabKeys.Hulls.ManOfWarBoss, SoundKeys.Music.Background.Boss, SkyMaterials.Sunrise),

                // Set 5:  Hammerhead
                new Level(16, StaticPrefabKeys.Hulls.Longbow, SoundKeys.Music.Background.Bobby, SkyMaterials.Morning),
                new Level(17, StaticPrefabKeys.Hulls.Megalodon, SoundKeys.Music.Background.Nothing, SkyMaterials.Midday),
                new Level(18, StaticPrefabKeys.Hulls.Hammerhead, SoundKeys.Music.Background.Juggernaut, SkyMaterials.Dusk),

                // Set 6:  Longbow
                new Level(19, StaticPrefabKeys.Hulls.Megalodon, SoundKeys.Music.Background.Sleeper, SkyMaterials.Purple),
                new Level(20, StaticPrefabKeys.Hulls.Rockjaw, SoundKeys.Music.Background.Experimental, SkyMaterials.Midnight),
                new Level(21, StaticPrefabKeys.Hulls.Hammerhead, SoundKeys.Music.Background.Nothing, SkyMaterials.Cold),
                new Level(22, StaticPrefabKeys.Hulls.Longbow, SoundKeys.Music.Background.Confusion, SkyMaterials.Sunrise),

                // Set 7:  Megolodon
                new Level(23, StaticPrefabKeys.Hulls.Eagle, SoundKeys.Music.Background.Experimental, SkyMaterials.Purple),
                new Level(24, StaticPrefabKeys.Hulls.Longbow, SoundKeys.Music.Background.Juggernaut, SkyMaterials.Midnight),
                new Level(25, StaticPrefabKeys.Hulls.Megalodon, SoundKeys.Music.Background.Nothing, SkyMaterials.Morning),
                new Level(26, StaticPrefabKeys.Hulls.Megalodon, SoundKeys.Music.Background.Confusion, SkyMaterials.Cold)
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

                // Tactical
                { StaticPrefabKeys.Buildings.ShieldGenerator, 2 },
                { StaticPrefabKeys.Buildings.LocalBooster, 9 },
                { StaticPrefabKeys.Buildings.ControlTower, 23 },
                { StaticPrefabKeys.Buildings.StealthGenerator, 16 },
                { StaticPrefabKeys.Buildings.SpySatelliteLauncher, 16 },

                // Defence
                { StaticPrefabKeys.Buildings.AntiShipTurret, 1 },
                { StaticPrefabKeys.Buildings.AntiAirTurret, 1 },
                { StaticPrefabKeys.Buildings.Mortar, 3 },
                { StaticPrefabKeys.Buildings.SamSite, 5 },
                { StaticPrefabKeys.Buildings.TeslaCoil, 20 },

                // Offence
                { StaticPrefabKeys.Buildings.Artillery, 1 },
                { StaticPrefabKeys.Buildings.RocketLauncher, 19 },
                { StaticPrefabKeys.Buildings.Railgun, 6 },

                // Ultras
                { StaticPrefabKeys.Buildings.DeathstarLauncher, 7 },
                { StaticPrefabKeys.Buildings.NukeLauncher, 10 },
                { StaticPrefabKeys.Buildings.Ultralisk, 14 },
                { StaticPrefabKeys.Buildings.KamikazeSignal, 21 },
                { StaticPrefabKeys.Buildings.Broadsides, 24 }
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
                
                // Ships
                { StaticPrefabKeys.Units.AttackBoat, 1 },
                { StaticPrefabKeys.Units.Frigate, 3 },
                { StaticPrefabKeys.Units.Destroyer, 13 },
                { StaticPrefabKeys.Units.ArchonBattleship, 17 }
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
                { StaticPrefabKeys.Hulls.Hammerhead, 18 },
                { StaticPrefabKeys.Hulls.Longbow, 22 },
                { StaticPrefabKeys.Hulls.Megalodon, 25 }
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
