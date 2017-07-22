using System.Collections.Generic;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.PrefabKeys;

namespace BattleCruisers.Data
{
    /// <summary>
    /// Provides data that does not change throughout the game.
    /// 
    /// This is in contrast to the GameModel, which changes as the player
    /// progresses and unlocks new prefabs.
    /// </summary>
    public interface IStaticData
	{
		GameModel InitialGameModel { get; }
		IList<ILevel> Levels { get; }
	}

	public class StaticData : IStaticData
	{
		public GameModel InitialGameModel { get; private set; }
		public IList<ILevel> Levels { get; private set; }

		public StaticData()
		{
			InitialGameModel = CreateInitialGameModel();
			Levels = CreateLevels();
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
			buildings.Add(new BuildingKey(BuildingCategory.Factory, "AirFactory"));
			buildings.Add(new BuildingKey(BuildingCategory.Factory, "NavalFactory"));
			buildings.Add(new BuildingKey(BuildingCategory.Factory, "EngineeringBay"));

			// Tactical
			buildings.Add(new BuildingKey(BuildingCategory.Tactical, "ShieldGenerator"));

			// Defence
			buildings.Add(new BuildingKey(BuildingCategory.Defence, "AntiShipTurret"));
			buildings.Add(new BuildingKey(BuildingCategory.Defence, "AntiAirTurret"));
			buildings.Add(new BuildingKey(BuildingCategory.Defence, "Mortar"));
			buildings.Add(new BuildingKey(BuildingCategory.Defence, "TeslaCoil"));

			// Offence
			buildings.Add(new BuildingKey(BuildingCategory.Offence, "Artillery"));
			buildings.Add(new BuildingKey(BuildingCategory.Offence, "RocketLauncher"));
			buildings.Add(new BuildingKey(BuildingCategory.Offence, "Railgun"));

			// Ultras
			buildings.Add(new BuildingKey(BuildingCategory.Ultra, "DeathstarLauncher"));

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
				new Level("Sprawl Brawl", aiLoadout),
				new Level("Fisticuffs", aiLoadout),
				new Level("Ambush at Dire Straits", aiLoadout),
				new Level("Battle of Watercress", aiLoadout)
			};
		}
	}
}
