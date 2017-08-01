using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BattleCruisers.Buildables.Units;
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
				new HullKey("Bullshark"),
				new HullKey("Eagle"),
				new HullKey("Hammerhead"),
				new HullKey("Longbow"),
				new HullKey("Megalodon"),
				new HullKey("Raptor"),
				new HullKey("Rockjaw"),
				new HullKey("Trident")
			};
		}

		private List<BuildingKey> AllBuildingKeys()
		{
			List<BuildingKey> buildings = new List<BuildingKey>();

			// Factories
			buildings.Add(StaticPrefabKeys.Buildings.AirFactory);
			buildings.Add(StaticPrefabKeys.Buildings.NavalFactory);
			buildings.Add(StaticPrefabKeys.Buildings.DroneStation);

			// Tactical
			buildings.Add(StaticPrefabKeys.Buildings.ShieldGenerator);

			// Defence
			buildings.Add(StaticPrefabKeys.Buildings.AntiShipTurret);
			buildings.Add(StaticPrefabKeys.Buildings.AntiAirTurret);
            buildings.Add(StaticPrefabKeys.Buildings.Mortar);
			buildings.Add(StaticPrefabKeys.Buildings.SamSite);
			buildings.Add(StaticPrefabKeys.Buildings.TeslaCoil);

			// Offence
			buildings.Add(StaticPrefabKeys.Buildings.Artillery);
			buildings.Add(StaticPrefabKeys.Buildings.RocketLauncher);
			buildings.Add(StaticPrefabKeys.Buildings.Railgun);

            // Ultras
            buildings.Add(StaticPrefabKeys.Buildings.DeathstarLauncher);
            buildings.Add(StaticPrefabKeys.Buildings.NukeLauncher);

			return buildings;
		}

		private List<UnitKey> AllUnitKeys()
		{
			List<UnitKey> units = new List<UnitKey>();

			// Aircraft
			units.Add(new UnitKey(UnitCategory.Aircraft, "Bomber"));
			units.Add(new UnitKey(UnitCategory.Aircraft, "Fighter"));

			// Ships
			units.Add(new UnitKey(UnitCategory.Naval, "AttackBoat"));

			return units;
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

		// FELIX  more levels maybe?
		// FELIX  Don't give all loadouts to all levels :P
		private IList<ILevel> CreateLevels()
		{
			Loadout aiLoadout = new Loadout(AllHullKeys()[0], AllBuildingKeys(), AllUnitKeys());

			return new List<ILevel>()
			{
                new Level("Sprawl Brawl", aiLoadout, BuildOrders.Balanced),
				new Level("Fisticuffs", aiLoadout, BuildOrders.Balanced),
				new Level("Ambush at Dire Straits", aiLoadout, BuildOrders.Balanced),
				new Level("Battle of Watercress", aiLoadout, BuildOrders.Balanced)
			};
		}
	}
}
