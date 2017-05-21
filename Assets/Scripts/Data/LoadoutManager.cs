using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Fetchers.PrefabKeys;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Data
{
	public interface ILoadoutManager
	{
		Loadout GetPlayerLoadout();
		Loadout GetAiLoadout(int levelNum);
	}

	public class LoadoutManager : ILoadoutManager
	{
		// FELIX  Don't hardcode.  Let player determine their loadout :P
		public Loadout GetPlayerLoadout()
		{
			return CreateLoadout();
		}

		// FELIX  Don't hardcode.  Determine based on level num :)
		public Loadout GetAiLoadout(int levelNum)
		{
			return CreateLoadout();
		}

		private Loadout CreateLoadout()
		{
			// Hull
			HullKey hull = new HullKey("Trident");

			// Factories
			List<BuildingKey> buildings = new List<BuildingKey>();
			buildings.Add(new BuildingKey(BuildingCategory.Factory, "AirFactory"));
			buildings.Add(new BuildingKey(BuildingCategory.Factory, "NavalFactory"));
			buildings.Add(new BuildingKey(BuildingCategory.Factory, "EngineeringBay"));

			// Tactical
			buildings.Add(new BuildingKey(BuildingCategory.Tactical, "ShieldGenerator"));

			// Defence
			buildings.Add(new BuildingKey(BuildingCategory.Defence, "AntiShipTurret"));
			buildings.Add(new BuildingKey(BuildingCategory.Defence, "AntiAirTurret"));

			// Offence
			buildings.Add(new BuildingKey(BuildingCategory.Offence, "Artillery"));

			// Aircraft
			List<UnitKey> units = new List<UnitKey>();
			units.Add(new UnitKey(UnitCategory.Aircraft, "Bomber"));
			units.Add(new UnitKey(UnitCategory.Aircraft, "Fighter"));

			// Ships
			units.Add(new UnitKey(UnitCategory.Naval, "AttackBoat"));
			units.Add(new UnitKey(UnitCategory.Naval, "AttackBoat2"));

			return new Loadout(
				hull,
				buildings,
				units);
		}
	}
}
