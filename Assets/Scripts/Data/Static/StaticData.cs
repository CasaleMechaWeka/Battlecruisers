using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BattleCruisers.Data.Static.Strategies;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using UnityEngine.Assertions;

namespace BattleCruisers.Data.Static
{
    public class StaticData : IStaticData
	{
		private readonly IDictionary<IPrefabKey, int> _buildableToUnlockedLevel;
        private readonly IList<BuildingKey> _allBuildings;
        private readonly IList<UnitKey> _allUnits;
        private readonly ILevelStrategies _strategies;

		public GameModel InitialGameModel { get; private set; }
		public IList<ILevel> Levels { get; private set; }
		public ReadOnlyCollection<IPrefabKey> BuildingKeys { get; private set; }

		public StaticData()
		{
			InitialGameModel = CreateInitialGameModel();
			Levels = CreateLevels();

            _allBuildings = AllBuildingKeys();

			IList<IPrefabKey> allBuildings =
                _allBuildings
				.Select(buildingKey => (IPrefabKey)buildingKey)
				.ToList();
			BuildingKeys = new ReadOnlyCollection<IPrefabKey>(allBuildings);

            _allUnits = AllUnitKeys();

            _buildableToUnlockedLevel = CreateAvailabilityMap();

            _strategies = new LevelStrategies();
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
                // FELIX  Gunship

                // Ships
                StaticPrefabKeys.Units.AttackBoat,
				StaticPrefabKeys.Units.Frigate,
                StaticPrefabKeys.Units.Destroyer
			};
		}

		// FELIX  For final game, don't add ALL the prefabs :D
		// NOTE:  Do NOT share key objects between Loadout and GameModel, otherwise
		// both will share the same object.  In that case if the Loadout deletes one of its
		// buildings the building will also be deleted from the GameModel.
		private GameModel CreateInitialGameModel()
		{
			Loadout playerLoadout = new Loadout(AllHullKeys()[4], AllBuildingKeys(), AllUnitKeys());

			int numOfLevelsCompleted = 0;

			return new GameModel(
                numOfLevelsCompleted,
				playerLoadout,
				null,
				AllHullKeys(),
				AllBuildingKeys(),
				AllUnitKeys());
		}

        // FELIX  Create 4th set :)
		private IList<ILevel> CreateLevels()
		{
			return new List<ILevel>()
			{
                // Set 1
				new Level(1, "Sprawl Brawl", StaticPrefabKeys.Hulls.Raptor),
				new Level(2, "Fisticuffs", StaticPrefabKeys.Hulls.Bullshark),
				new Level(3, "Ambush at Dire Straits", StaticPrefabKeys.Hulls.Raptor),
				new Level(4, "Battle of Watercress", StaticPrefabKeys.Hulls.Rockjaw),
				new Level(5, "Little big elbow", StaticPrefabKeys.Hulls.Bullshark),
				new Level(6, "Dunspock", StaticPrefabKeys.Hulls.Rockjaw),
				new Level(7, "Gallient Flippery", StaticPrefabKeys.Hulls.Rockjaw),

                // Set 2
                new Level(8, "Sprawl Brawl", StaticPrefabKeys.Hulls.Eagle),
                new Level(9, "Fisticuffs", StaticPrefabKeys.Hulls.Bullshark),
                new Level(10, "Ambush at Dire Straits", StaticPrefabKeys.Hulls.Hammerhead),
                new Level(11, "Battle of Watercress", StaticPrefabKeys.Hulls.Eagle),
                new Level(12, "Little big elbow", StaticPrefabKeys.Hulls.Hammerhead),
                new Level(13, "Dunspock", StaticPrefabKeys.Hulls.Eagle),
                new Level(14, "Gallient Flippery", StaticPrefabKeys.Hulls.Hammerhead),

                // Set 3
                new Level(15, "Sprawl Brawl", StaticPrefabKeys.Hulls.Longbow),
                new Level(16, "Fisticuffs", StaticPrefabKeys.Hulls.Hammerhead),
                new Level(17, "Ambush at Dire Straits", StaticPrefabKeys.Hulls.Megalodon),
                new Level(18, "Battle of Watercress", StaticPrefabKeys.Hulls.Longbow),
                new Level(19, "Little big elbow", StaticPrefabKeys.Hulls.Eagle),
                new Level(20, "Dunspock", StaticPrefabKeys.Hulls.Megalodon),
                new Level(21, "Gallient Flippery", StaticPrefabKeys.Hulls.Megalodon),
			};
		}

		public bool IsBuildableAvailable(IPrefabKey buildableKey, int levelNum)
		{
			Assert.IsTrue(_buildableToUnlockedLevel.ContainsKey(buildableKey));

			int levelBuildableIsAvaible = _buildableToUnlockedLevel[buildableKey];
			return levelNum >= levelBuildableIsAvaible;
		}

		private IDictionary<IPrefabKey, int> CreateAvailabilityMap()
		{
			return new Dictionary<IPrefabKey, int>()
			{
                // === Buildings ===
                // Factories
                { StaticPrefabKeys.Buildings.AirFactory, 1 },
				{ StaticPrefabKeys.Buildings.NavalFactory, 1 },
				{ StaticPrefabKeys.Buildings.DroneStation, 5 },

                // Tactical
                { StaticPrefabKeys.Buildings.ShieldGenerator, 5 },
				{ StaticPrefabKeys.Buildings.Booster, 10 },
				{ StaticPrefabKeys.Buildings.ControlTower, 11 },
				{ StaticPrefabKeys.Buildings.StealthField, 14 },
                { StaticPrefabKeys.Buildings.SpySatellite, 14 },

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
				{ StaticPrefabKeys.Buildings.Broadsides, 18 },
                { StaticPrefabKeys.Buildings.ArchonBattleship, 20 },


                // === Units ===
                // Aircraft
                { StaticPrefabKeys.Units.Bomber, 1 },
				{ StaticPrefabKeys.Units.Gunship, 3 },
                { StaticPrefabKeys.Units.Fighter, 9 },
                
                // Ships
                { StaticPrefabKeys.Units.AttackBoat, 1 },
                { StaticPrefabKeys.Units.Frigate, 2 },
                { StaticPrefabKeys.Units.Destroyer, 9 }
			};
		}

        public IList<IPrefabKey> GetAvailableBuildings(BuildingCategory category, int levelNum)
        {
            return 
                _allBuildings
	                .Where(buildingKey => buildingKey.BuildingCategory == category && IsBuildableAvailable(buildingKey, levelNum))
	                .Select(buildingKey => (IPrefabKey)buildingKey)
	                .ToList();
        }

        public IList<IPrefabKey> GetAvailableUnits(UnitCategory category, int levelNum)
        {
            return
                _allUnits
                    .Where(unitKey => unitKey.UnitCategory == category && IsBuildableAvailable(unitKey, levelNum))
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
    }
}
