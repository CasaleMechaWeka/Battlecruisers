using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static.LevelLoot;
using BattleCruisers.Data.Static.Strategies;
using UnityEngine.Assertions;

namespace BattleCruisers.Data.Static
{
    // FELIX  Create tests!
    public class StaticData : IStaticData
	{
        private readonly IDictionary<IPrefabKey, int> _buildingToUnlockedLevel;
        private readonly IDictionary<IPrefabKey, int> _unitToUnlockedLevel;
        private readonly IDictionary<IPrefabKey, int> _hullToUnlockedLevel;

        private readonly IList<BuildingKey> _allBuildings;
        private readonly IList<UnitKey> _allUnits;
        private readonly ILevelStrategies _strategies;

        private const int MIN_AVAILABILITY_LEVEL_NUM = 2;

		public GameModel InitialGameModel { get; private set; }
		public IList<ILevel> Levels { get; private set; }
		public ReadOnlyCollection<IPrefabKey> BuildingKeys { get; private set; }
        public ReadOnlyCollection<IPrefabKey> AIBannedUltrakeys { get; private set; }

        public StaticData()
		{
            _allBuildings = AllBuildingKeys();

            IList<IPrefabKey> allBuildings =
                _allBuildings
                .Select(buildingKey => (IPrefabKey)buildingKey)
                .ToList();
            BuildingKeys = new ReadOnlyCollection<IPrefabKey>(allBuildings);

            _allUnits = AllUnitKeys();

            _buildingToUnlockedLevel = CreateBuildingAvailabilityMap();
            _unitToUnlockedLevel = CreateUnitAvailabilityMap();
            _hullToUnlockedLevel = CreateHullAvailabilityMap();

            _strategies = new LevelStrategies();

            AIBannedUltrakeys = new ReadOnlyCollection<IPrefabKey>(CreateAIBannedUltraKeys());
			
            InitialGameModel = CreateInitialGameModel();
			Levels = CreateLevels();
		}

		private List<HullKey> AllHullKeys()
		{
			return new List<HullKey>()
			{
				StaticPrefabKeys.Hulls.Bullshark,
				StaticPrefabKeys.Hulls.Eagle,
				StaticPrefabKeys.Hulls.Hammerhead,
				StaticPrefabKeys.Hulls.Longbow,
				StaticPrefabKeys.Hulls.Megalodon,
				StaticPrefabKeys.Hulls.Raptor,
				StaticPrefabKeys.Hulls.Rockjaw,
				StaticPrefabKeys.Hulls.Trident
			};
		}

		private List<BuildingKey> AllBuildingKeys()
		{
			return new List<BuildingKey>()
			{
                // Factories
                StaticPrefabKeys.Buildings.AirFactory,
				StaticPrefabKeys.Buildings.NavalFactory,
				StaticPrefabKeys.Buildings.DroneStation,

                // Tactical
                StaticPrefabKeys.Buildings.ShieldGenerator,
                StaticPrefabKeys.Buildings.StealthGenerator,
				StaticPrefabKeys.Buildings.SpySatelliteLauncher,
				StaticPrefabKeys.Buildings.LocalBooster,
                StaticPrefabKeys.Buildings.ControlTower,

                // Defence
                StaticPrefabKeys.Buildings.AntiShipTurret,
				StaticPrefabKeys.Buildings.AntiAirTurret,
				StaticPrefabKeys.Buildings.Mortar,
				StaticPrefabKeys.Buildings.SamSite,
				StaticPrefabKeys.Buildings.TeslaCoil,

                // Offence
                StaticPrefabKeys.Buildings.Artillery,
				StaticPrefabKeys.Buildings.RocketLauncher,
				StaticPrefabKeys.Buildings.Railgun,

                // Ultras
                StaticPrefabKeys.Buildings.DeathstarLauncher,
				StaticPrefabKeys.Buildings.NukeLauncher,
				StaticPrefabKeys.Buildings.Ultralisk,
				StaticPrefabKeys.Buildings.KamikazeSignal,
                StaticPrefabKeys.Buildings.Broadsides
			};
		}

        private IList<IPrefabKey> CreateAIBannedUltraKeys()
        {
            return new List<IPrefabKey>()
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
			return new List<UnitKey>()
			{
                // Aircraft
                StaticPrefabKeys.Units.Bomber,
				StaticPrefabKeys.Units.Fighter,
                StaticPrefabKeys.Units.Gunship,

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
            // TEMP  For final game, don't add ALL the prefabs :D
            //Loadout playerLoadout = new Loadout(AllHullKeys().Last(), AllBuildingKeys(), AllUnitKeys());
            HullKey initialHull = GetInitialHull();
            Loadout playerLoadout = new Loadout(initialHull, GetInitialBuildings(), GetInitialUnits());

            // TEMP  For final game only unlock first level :P
			//int numOfLevelsCompleted = 20;
			int numOfLevelsCompleted = 0;

            return new GameModel(
                numOfLevelsCompleted,
                playerLoadout,
                lastBattleResult: null,
                unlockedHulls: new List<HullKey>() { initialHull },
                unlockedBuildings: GetInitialBuildings(),
                unlockedUnits: GetInitialUnits());
                // TEMP  Do not unlock all hulls & buildables at teh game start :P
				//AllHullKeys(),
				//AllBuildingKeys(),
				//AllUnitKeys());
		}

        private HullKey GetInitialHull()
        {
            return StaticPrefabKeys.Hulls.Trident;
        }

        private List<BuildingKey> GetInitialBuildings()
        {
            return
                GetBuildingsFirstAvailableIn(levelFirstAvailableIn: 1)
                    .Select(prefabKey => (BuildingKey)prefabKey)
                    .ToList();
        }

        private List<UnitKey> GetInitialUnits()
        {
            return
                GetUnitsFirstAvailableIn(levelFirstAvailableIn: 1)
                    .Select(prefabKey => (UnitKey)prefabKey)
                    .ToList();
        }

		private IList<ILevel> CreateLevels()
		{
			return new List<ILevel>()
			{
                // Set 1
                new Level(1, "Sprawl Brawl", StaticPrefabKeys.Hulls.Raptor, SkyMaterials.Blue),
                new Level(2, "Fisticuffs", StaticPrefabKeys.Hulls.Bullshark, SkyMaterials.BlueDeep),
                new Level(3, "Ambush at Dire Straits", StaticPrefabKeys.Hulls.Raptor, SkyMaterials.SunsetCloudy),
                new Level(4, "Battle of Watercress", StaticPrefabKeys.Hulls.Rockjaw, SkyMaterials.White),
                new Level(5, "Little big elbow", StaticPrefabKeys.Hulls.Bullshark, SkyMaterials.Sunset),
                new Level(6, "Dunspock", StaticPrefabKeys.Hulls.Rockjaw, SkyMaterials.BlueCloudy),
                new Level(7, "Gallient Flippery", StaticPrefabKeys.Hulls.Rockjaw, SkyMaterials.SunsetWeirdClouds),

                // Set 2
                new Level(8, "Sprawl Brawl", StaticPrefabKeys.Hulls.Eagle, SkyMaterials.SunsetCloudy),
                new Level(9, "Fisticuffs", StaticPrefabKeys.Hulls.Bullshark, SkyMaterials.White),
                new Level(10, "Ambush at Dire Straits", StaticPrefabKeys.Hulls.Hammerhead, SkyMaterials.BlueDeep),
                new Level(11, "Battle of Watercress", StaticPrefabKeys.Hulls.Eagle, SkyMaterials.SunsetWeirdClouds),
                new Level(12, "Little big elbow", StaticPrefabKeys.Hulls.Hammerhead, SkyMaterials.Blue),
                new Level(13, "Dunspock", StaticPrefabKeys.Hulls.Eagle, SkyMaterials.BlueCloudy),
                new Level(14, "Gallient Flippery", StaticPrefabKeys.Hulls.Hammerhead, SkyMaterials.Sunset),

                // Set 3
                new Level(15, "Sprawl Brawl", StaticPrefabKeys.Hulls.Longbow, SkyMaterials.BlueCloudy),
                new Level(16, "Fisticuffs", StaticPrefabKeys.Hulls.Hammerhead, SkyMaterials.SunsetWeirdClouds),
                new Level(17, "Ambush at Dire Straits", StaticPrefabKeys.Hulls.Megalodon, SkyMaterials.White),
                new Level(18, "Battle of Watercress", StaticPrefabKeys.Hulls.Longbow, SkyMaterials.Sunset),
                new Level(19, "Little big elbow", StaticPrefabKeys.Hulls.Eagle, SkyMaterials.Blue),
                new Level(20, "Dunspock", StaticPrefabKeys.Hulls.Megalodon, SkyMaterials.SunsetCloudy),
                new Level(21, "Gallient Flippery", StaticPrefabKeys.Hulls.Megalodon, SkyMaterials.BlueDeep)
			};
		}

        private IDictionary<IPrefabKey, int> CreateBuildingAvailabilityMap()
        {
            return new Dictionary<IPrefabKey, int>()
            {
                // Factories
                { StaticPrefabKeys.Buildings.AirFactory, 1 },
                { StaticPrefabKeys.Buildings.NavalFactory, 1 },
                { StaticPrefabKeys.Buildings.DroneStation, 1 },

                // Tactical
                { StaticPrefabKeys.Buildings.ShieldGenerator, 5 },
                { StaticPrefabKeys.Buildings.LocalBooster, 10 },
                { StaticPrefabKeys.Buildings.ControlTower, 11 },
                { StaticPrefabKeys.Buildings.StealthGenerator, 14 },
                { StaticPrefabKeys.Buildings.SpySatelliteLauncher, 14 },

                // Defence
                { StaticPrefabKeys.Buildings.AntiShipTurret, 1 },
                { StaticPrefabKeys.Buildings.AntiAirTurret, 1 },
                { StaticPrefabKeys.Buildings.Mortar, 2 },
                { StaticPrefabKeys.Buildings.SamSite, 3 },
                { StaticPrefabKeys.Buildings.TeslaCoil, 13 },

                // Offence
                { StaticPrefabKeys.Buildings.Artillery, 1 },
                { StaticPrefabKeys.Buildings.RocketLauncher, 13 },
                { StaticPrefabKeys.Buildings.Railgun, 7 },

                // Ultras
                { StaticPrefabKeys.Buildings.DeathstarLauncher, 8 },
                { StaticPrefabKeys.Buildings.NukeLauncher, 15 },
                { StaticPrefabKeys.Buildings.Ultralisk, 16 },
                { StaticPrefabKeys.Buildings.KamikazeSignal, 17 },
                { StaticPrefabKeys.Buildings.Broadsides, 18 }
            };
        }

        private IDictionary<IPrefabKey, int> CreateUnitAvailabilityMap()
        {
            return new Dictionary<IPrefabKey, int>()
            {
                // Aircraft
                { StaticPrefabKeys.Units.Bomber, 1 },
                { StaticPrefabKeys.Units.Gunship, 3 },
                { StaticPrefabKeys.Units.Fighter, 9 },
                
                // Ships
                { StaticPrefabKeys.Units.AttackBoat, 1 },
                { StaticPrefabKeys.Units.Frigate, 2 },
                { StaticPrefabKeys.Units.Destroyer, 9 },
                { StaticPrefabKeys.Units.ArchonBattleship, 20 }
            };
        }

        private IDictionary<IPrefabKey, int> CreateHullAvailabilityMap()
        {
            return new Dictionary<IPrefabKey, int>()
            {
                { StaticPrefabKeys.Hulls.Trident, 1 },
                { StaticPrefabKeys.Hulls.Raptor, 4 },
                { StaticPrefabKeys.Hulls.Bullshark, 6 },
                { StaticPrefabKeys.Hulls.Rockjaw, 8 },
                { StaticPrefabKeys.Hulls.Eagle, 12 },
                { StaticPrefabKeys.Hulls.Hammerhead, 15 },
                { StaticPrefabKeys.Hulls.Longbow, 19 },
                { StaticPrefabKeys.Hulls.Megalodon, 21 }
            };
        }

        public IList<IPrefabKey> GetAvailableBuildings(BuildingCategory category, int levelNum)
        {
            return 
                _allBuildings
                    .Where(buildingKey => buildingKey.BuildingCategory == category && IsBuildingAvailable(buildingKey, levelNum))
	                .Select(buildingKey => (IPrefabKey)buildingKey)
	                .ToList();
        }

        public IList<IPrefabKey> GetAvailableUnits(UnitCategory category, int levelNum)
        {
            return
                _allUnits
                    .Where(unitKey => unitKey.UnitCategory == category && IsUnitAvailable(unitKey, levelNum))
                    .Select(unitKey => (IPrefabKey)unitKey)
                    .ToList();
        }

        public IStrategy GetAdaptiveStrategy(int levelNum)
        {
			int levelIndex = levelNum - 1;
            Assert.IsTrue(levelIndex < _strategies.AdaptiveStrategies.Count);

            return _strategies.AdaptiveStrategies[levelIndex];
		}

        public IStrategy GetBasicStrategy(int levelNum)
        {
			int levelIndex = levelNum - 1;
            Assert.IsTrue(levelIndex < _strategies.BasicStrategies.Count);

            return _strategies.BasicStrategies[levelIndex];
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
                    buildingKey: GetBuildingsFirstAvailableIn(availabilityLevelNum).FirstOrDefault(),
                    unitKey: GetUnitsFirstAvailableIn(availabilityLevelNum).FirstOrDefault(),
                    hullKey: GetHullFirstAvailableIn(availabilityLevelNum));
        }

        private IList<IPrefabKey> GetUnitsFirstAvailableIn(int levelFirstAvailableIn)
        {
            return GetBuildablesFirstAvailableIn(_unitToUnlockedLevel, levelFirstAvailableIn);
        }

        private IList<IPrefabKey> GetBuildingsFirstAvailableIn(int levelFirstAvailableIn)
        {
            return GetBuildablesFirstAvailableIn(_buildingToUnlockedLevel, levelFirstAvailableIn);
        }

		/// <summary>
		/// List should always have 0 or 1 entry, unless levelFirstAvailableIn is 1
		/// (ie, the starting level, where we have multiple buildables available).
		/// </summary>
        private IList<IPrefabKey> GetBuildablesFirstAvailableIn(IDictionary<IPrefabKey, int> buildableToUnlockedLevel, int levelFirstAvailableIn)
		{
			return
				buildableToUnlockedLevel
					.Where(buildableToLevel => buildableToLevel.Value == levelFirstAvailableIn)
					.Select(buildableToLevel => buildableToLevel.Key)
					.ToList();
		}

        private IPrefabKey GetHullFirstAvailableIn(int levelFirstAvailableIn)
        {
            return
                _hullToUnlockedLevel
                    .FirstOrDefault(hullToLevel => hullToLevel.Value == levelFirstAvailableIn)
                    .Key;
        }
		
        public bool IsUnitAvailable(IPrefabKey unitKey, int levelNum)
        {
            Assert.IsTrue(_unitToUnlockedLevel.ContainsKey(unitKey));

            int firstLevelUnitIsAvailableIn = _unitToUnlockedLevel[unitKey];
            return levelNum >= firstLevelUnitIsAvailableIn;
        }

        public bool IsBuildingAvailable(IPrefabKey buildingKey, int levelNum)
        {
            Assert.IsTrue(_buildingToUnlockedLevel.ContainsKey(buildingKey));

            int firstlevelBuildingIsAvailableIn = _buildingToUnlockedLevel[buildingKey];
            return levelNum >= firstlevelBuildingIsAvailableIn;
        }
    }
}
