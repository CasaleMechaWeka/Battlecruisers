using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Fetchers.PrefabKeys;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.DataModel
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
			IList<BuildingKey> factories = new List<BuildingKey>();
			factories.Add(new BuildingKey(BuildingCategory.Factory, "AirFactory"));
			factories.Add(new BuildingKey(BuildingCategory.Factory, "NavalFactory"));
			factories.Add(new BuildingKey(BuildingCategory.Factory, "EngineeringBay"));

			// Tactical
			IList<BuildingKey> tactical = new List<BuildingKey>();
			tactical.Add(new BuildingKey(BuildingCategory.Tactical, "ShieldGenerator"));

			// Defence
			IList<BuildingKey> defence = new List<BuildingKey>();
			defence.Add(new BuildingKey(BuildingCategory.Defence, "AntiShipTurret"));
			defence.Add(new BuildingKey(BuildingCategory.Defence, "AntiAirTurret"));

			// Offence
			IList<BuildingKey> offence = new List<BuildingKey>();
			offence.Add(new BuildingKey(BuildingCategory.Offence, "Artillery"));

			// Support
			IList<BuildingKey> support = new List<BuildingKey>();

			// Aircraft
			IList<UnitKey> aircraft = new List<UnitKey>();
			aircraft.Add(new UnitKey(UnitCategory.Aircraft, "Bomber"));
			aircraft.Add(new UnitKey(UnitCategory.Aircraft, "Fighter"));

			// Ships
			IList<UnitKey> ships = new List<UnitKey>();
			ships.Add(new UnitKey(UnitCategory.Naval, "AttackBoat"));
			ships.Add(new UnitKey(UnitCategory.Naval, "AttackBoat2"));

			return new Loadout(
				hull,
				factories,
				tactical,
				defence,
				offence,
				support,
				aircraft,
				ships);
		}
	}
}
