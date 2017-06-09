using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.PrefabKeys;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
		List<HullKey> HullKeys { get; }
		List<BuildingKey> BuildingKeys { get; }
		List<UnitKey> UnitKeys { get; }

		GameModel InitialGameModel { get; }

		IList<ILevel> Levels { get; }
	}

	// FELIX  Could scrape Assets folder and auto generate keys :P  Would make
	// updating assets easier, but might not be worth the implementation effort.
	public class StaticData : IStaticData
	{
		public List<HullKey> HullKeys { get; private set; }
		public List<BuildingKey> BuildingKeys { get; private set; }
		public List<UnitKey> UnitKeys { get; private set; }

		public GameModel InitialGameModel { get; private set; }

		public IList<ILevel> Levels { get; private set; }

		public StaticData()
		{
			HullKeys = AllHullKeys();
			BuildingKeys = AllBuildingKeys();
			UnitKeys = AllUnitKeys();

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
		private GameModel CreateInitialGameModel()
		{
			Loadout playerLoadout = new Loadout(HullKeys[3], BuildingKeys, UnitKeys);

			int numOfLevelsUnlocked = 1;

			return new GameModel(
				numOfLevelsUnlocked,
				playerLoadout,
				null,
				HullKeys,
				BuildingKeys,
				UnitKeys);
		}

		// FELIX  > 1 level maybe?
		// FELIX  Don't give all loadouts to all levels :P
		private IList<ILevel> CreateLevels()
		{
			Loadout aiLoadout = new Loadout(HullKeys[0], BuildingKeys, UnitKeys);

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
