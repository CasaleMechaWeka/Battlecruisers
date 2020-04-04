using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static.LevelLoot;
using BattleCruisers.UI.BattleScene.Clouds;
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

		public GameModel InitialGameModel { get; }
        public ReadOnlyCollection<ILevel> Levels { get; }
		public ReadOnlyCollection<HullKey> HullKeys { get; }
		public ReadOnlyCollection<UnitKey> UnitKeys { get; }
        public ReadOnlyCollection<BuildingKey> BuildingKeys { get; }
        public ReadOnlyCollection<BuildingKey> AIBannedUltrakeys { get; }
        public int LastLevelWithLoot => 20;
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
				StaticPrefabKeys.Hulls.Megalodon
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

        // FELIX  Have cloud stats and sky be one class (as they will now be linked).
		private IList<ILevel> CreateLevels()
		{
			return new List<ILevel>()
			{
                // Set 1
                new Level(1, "Daisy", StaticPrefabKeys.Hulls.Raptor, SoundKeys.Music.Background.Sleeper, SkyMaterials.HDMidday, CreateCloudStats(CloudMovementSpeed.Slow)),
                new Level(2, "Rain", StaticPrefabKeys.Hulls.Bullshark, SoundKeys.Music.Background.Juggernaut, SkyMaterials.HDPurple, CreateCloudStats(CloudMovementSpeed.Fast)),
                new Level(3, "Jurassic Wolf", StaticPrefabKeys.Hulls.Raptor, SoundKeys.Music.Background.Bobby, SkyMaterials.HDDusk, CreateCloudStats(CloudMovementSpeed.Slow)),
                new Level(4, "Sky Turtles", StaticPrefabKeys.Hulls.Rockjaw, SoundKeys.Music.Background.Nothing, SkyMaterials.HDAfternoon, CreateCloudStats(CloudMovementSpeed.Fast)),
                new Level(5, "Pigheaded", StaticPrefabKeys.Hulls.Bullshark, SoundKeys.Music.Background.Confusion, SkyMaterials.HDPurpleClouds, CreateCloudStats(CloudMovementSpeed.Fast)),
                new Level(6, "Surprise", StaticPrefabKeys.Hulls.Rockjaw, SoundKeys.Music.Background.Bobby, SkyMaterials.HDMidnight, CreateCloudStats(CloudMovementSpeed.Slow)),
                new Level(7, "Rockstar", StaticPrefabKeys.Hulls.Rockjaw, SoundKeys.Music.Background.Experimental, SkyMaterials.HDSunset, CreateCloudStats(CloudMovementSpeed.Slow)),

                // Set 2
                new Level(8, "Boomtown", StaticPrefabKeys.Hulls.Eagle, SoundKeys.Music.Background.Nothing, SkyMaterials.HDAfternoon, CreateCloudStats(CloudMovementSpeed.Slow)),
                new Level(9, "Magic", StaticPrefabKeys.Hulls.Bullshark, SoundKeys.Music.Background.Bobby, SkyMaterials.HDPurpleClouds, CreateCloudStats(CloudMovementSpeed.Fast)),
                new Level(10, "Control Freak", StaticPrefabKeys.Hulls.Hammerhead, SoundKeys.Music.Background.Experimental, SkyMaterials.HDPurple, CreateCloudStats(CloudMovementSpeed.Fast)),
                new Level(11, "Bald Eagle", StaticPrefabKeys.Hulls.Eagle, SoundKeys.Music.Background.Juggernaut, SkyMaterials.HDMidnight, CreateCloudStats(CloudMovementSpeed.Slow)),
                new Level(12, "Fast Snow", StaticPrefabKeys.Hulls.Hammerhead, SoundKeys.Music.Background.Sleeper, SkyMaterials.HDMidday, CreateCloudStats(CloudMovementSpeed.Slow)),
                new Level(13, "Stealthrest", StaticPrefabKeys.Hulls.Eagle, SoundKeys.Music.Background.Confusion, SkyMaterials.HDDusk, CreateCloudStats(CloudMovementSpeed.Slow)),
                new Level(14, "Atomic Hammer", StaticPrefabKeys.Hulls.Hammerhead, SoundKeys.Music.Background.Juggernaut, SkyMaterials.HDSunset, CreateCloudStats(CloudMovementSpeed.Fast)),

                // Set 3
                new Level(15, "Drone Millionaire", StaticPrefabKeys.Hulls.Longbow, SoundKeys.Music.Background.Bobby, SkyMaterials.HDPurpleClouds, CreateCloudStats(CloudMovementSpeed.Slow)),
                new Level(16, "Japanese Heritage", StaticPrefabKeys.Hulls.Hammerhead, SoundKeys.Music.Background.Nothing, SkyMaterials.HDMidday, CreateCloudStats(CloudMovementSpeed.Slow)),
                new Level(17, "Pirate Power", StaticPrefabKeys.Hulls.Megalodon, SoundKeys.Music.Background.Juggernaut, SkyMaterials.HDDusk, CreateCloudStats(CloudMovementSpeed.Fast)),
                new Level(18, "British Domination", StaticPrefabKeys.Hulls.Longbow, SoundKeys.Music.Background.Sleeper, SkyMaterials.HDPurple, CreateCloudStats(CloudMovementSpeed.Fast)),
                new Level(19, "Athenian Topdog", StaticPrefabKeys.Hulls.Eagle, SoundKeys.Music.Background.Experimental, SkyMaterials.HDMidnight, CreateCloudStats(CloudMovementSpeed.Slow)),
                new Level(20, "King of the Depths", StaticPrefabKeys.Hulls.Megalodon, SoundKeys.Music.Background.Nothing, SkyMaterials.HDAfternoon, CreateCloudStats(CloudMovementSpeed.Slow)),
                new Level(21, "Let Down", StaticPrefabKeys.Hulls.Megalodon, SoundKeys.Music.Background.Confusion, SkyMaterials.HDSunset, CreateCloudStats(CloudMovementSpeed.Fast)),

                // Set 4
                new Level(22, "Big Pond", StaticPrefabKeys.Hulls.Eagle, SoundKeys.Music.Background.Experimental, SkyMaterials.HDPurple, CreateCloudStats(CloudMovementSpeed.Fast)),
                new Level(23, "Indecision", StaticPrefabKeys.Hulls.Longbow, SoundKeys.Music.Background.Juggernaut, SkyMaterials.HDMidnight, CreateCloudStats(CloudMovementSpeed.Slow)),
                new Level(24, "Premature", StaticPrefabKeys.Hulls.Rockjaw, SoundKeys.Music.Background.Nothing, SkyMaterials.HDAfternoon, CreateCloudStats(CloudMovementSpeed.Slow)),
                new Level(25, "Metrocity", StaticPrefabKeys.Hulls.Megalodon, SoundKeys.Music.Background.Confusion, SkyMaterials.HDPurpleClouds, CreateCloudStats(CloudMovementSpeed.Slow)),
                new Level(26, "Goldrush", StaticPrefabKeys.Hulls.Hammerhead, SoundKeys.Music.Background.Experimental, SkyMaterials.HDMidday, CreateCloudStats(CloudMovementSpeed.Fast)),
                new Level(27, "Dawn", StaticPrefabKeys.Hulls.Longbow, SoundKeys.Music.Background.Bobby, SkyMaterials.HDDusk, CreateCloudStats(CloudMovementSpeed.Fast)),
                new Level(28, "Mastery", StaticPrefabKeys.Hulls.Megalodon, SoundKeys.Music.Background.Sleeper, SkyMaterials.HDSunset, CreateCloudStats(CloudMovementSpeed.Slow))
            };
		}

        private ICloudStats CreateCloudStats(CloudMovementSpeed movementSpeed)
        {
            return new CloudStats(movementSpeed, frontCloudColour: Color.white, backCloudColour: Color.white);
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
                { StaticPrefabKeys.Buildings.LocalBooster, 10 },
                { StaticPrefabKeys.Buildings.ControlTower, 11 },
                { StaticPrefabKeys.Buildings.StealthGenerator, 14 },
                { StaticPrefabKeys.Buildings.SpySatelliteLauncher, 14 },

                // Defence
                { StaticPrefabKeys.Buildings.AntiShipTurret, 1 },
                { StaticPrefabKeys.Buildings.AntiAirTurret, 1 },
                { StaticPrefabKeys.Buildings.Mortar, 3 },
                { StaticPrefabKeys.Buildings.SamSite, 5 },
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

        private IDictionary<UnitKey, int> CreateUnitAvailabilityMap()
        {
            return new Dictionary<UnitKey, int>()
            {
                // Aircraft
                { StaticPrefabKeys.Units.Bomber, 1 },
                { StaticPrefabKeys.Units.Gunship, 5 },
                { StaticPrefabKeys.Units.Fighter, 9 },
                
                // Ships
                { StaticPrefabKeys.Units.AttackBoat, 1 },
                { StaticPrefabKeys.Units.Frigate, 3 },
                { StaticPrefabKeys.Units.Destroyer, 9 },
                { StaticPrefabKeys.Units.ArchonBattleship, 20 }
            };
        }

        private IDictionary<HullKey, int> CreateHullAvailabilityMap()
        {
            return new Dictionary<HullKey, int>()
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

        public IList<BuildingKey> GetAvailableBuildings(BuildingCategory category, int levelNum)
        {
            return 
                _allBuildings
                    .Where(buildingKey => buildingKey.BuildingCategory == category && IsBuildingAvailable(buildingKey, levelNum))
	                .ToList();
        }

        public IList<UnitKey> GetAvailableUnits(UnitCategory category, int levelNum)
        {
            return
                _allUnits
                    .Where(unitKey => unitKey.UnitCategory == category && IsUnitAvailable(unitKey, levelNum))
                    .ToList();
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
		
        public bool IsUnitAvailable(UnitKey unitKey, int levelNum)
        {
            Assert.IsTrue(_unitToUnlockedLevel.ContainsKey(unitKey));

            int firstLevelUnitIsAvailableIn = _unitToUnlockedLevel[unitKey];
            return levelNum >= firstLevelUnitIsAvailableIn;
        }

        public bool IsBuildingAvailable(BuildingKey buildingKey, int levelNum)
        {
            Assert.IsTrue(_buildingToUnlockedLevel.ContainsKey(buildingKey));

            int firstlevelBuildingIsAvailableIn = _buildingToUnlockedLevel[buildingKey];
            return levelNum >= firstlevelBuildingIsAvailableIn;
        }
    }
}
