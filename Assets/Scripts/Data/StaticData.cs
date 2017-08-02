using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.Data
{
    public class StaticData : IStaticData
	{
		public GameModel InitialGameModel { get; private set; }
		public IList<ILevel> Levels { get; private set; }
		public ReadOnlyCollection<IPrefabKey> BuildingKeys { get; private set; }

		public StaticData()
		{
			InitialGameModel = CreateInitialGameModel();
			Levels = CreateLevels();

			IList<IPrefabKey> allBuildings =
				AllBuildingKeys()
				.Select(buildingKey => (IPrefabKey)buildingKey)
				.ToList();
			this.BuildingKeys = new ReadOnlyCollection<IPrefabKey>(allBuildings);
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
				StaticPrefabKeys.Buildings.NukeLauncher
			};
		}

		private List<UnitKey> AllUnitKeys()
		{
			return new List<UnitKey>()
			{
                // Aircraft
                StaticPrefabKeys.Units.Bomber,
				StaticPrefabKeys.Units.Fighter,
                
                // Ships
                StaticPrefabKeys.Units.AttackBoat
			};
		}

		// FELIX  For final game, don't add ALL the prefabs :D
		// NOTE:  Do NOT share key objects between Loadout and GameModel, otherwise
		// both will share the same object.  In that case if the Loadout deletes one of its
		// buildings the building will also be deleted from the GameModel.
		private GameModel CreateInitialGameModel()
		{
			Loadout playerLoadout = new Loadout(AllHullKeys()[4], AllBuildingKeys(), AllUnitKeys());

			int numOfLevelsUnlocked = 1;

			return new GameModel(
				numOfLevelsUnlocked,
				playerLoadout,
				null,
				AllHullKeys(),
				AllBuildingKeys(),
				AllUnitKeys());
		}

		// FELIX  All 21 levels :D
		private IList<ILevel> CreateLevels()
		{
			return new List<ILevel>()
			{
				new Level(1, "Sprawl Brawl", StaticPrefabKeys.Hulls.Raptor),
				new Level(2, "Fisticuffs", StaticPrefabKeys.Hulls.Bullshark),
				new Level(3, "Ambush at Dire Straits", StaticPrefabKeys.Hulls.Raptor),
				new Level(4, "Battle of Watercress", StaticPrefabKeys.Hulls.Rockjaw),
				new Level(5, "Little big elbow", StaticPrefabKeys.Hulls.Bullshark),
				new Level(6, "Dunspock", StaticPrefabKeys.Hulls.Rockjaw),
				new Level(7, "Gallient Flippery", StaticPrefabKeys.Hulls.Rockjaw)
			};
		}
	}
}
