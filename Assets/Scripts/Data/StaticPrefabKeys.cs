using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.Data
{
	public static class StaticPrefabKeys
	{
		public static class Buildings
		{
			// Factories
			public static BuildingKey AirFactory { get { return new BuildingKey(BuildingCategory.Factory, "AirFactory"); } }
			public static BuildingKey NavalFactory { get { return new BuildingKey(BuildingCategory.Factory, "NavalFactory"); } }
			public static BuildingKey DroneStation { get { return new BuildingKey(BuildingCategory.Factory, "EngineeringBay"); } }

			// Tactical
			public static BuildingKey ShieldGenerator { get { return new BuildingKey(BuildingCategory.Tactical, "ShieldGenerator"); } }

			// Defence
			public static BuildingKey AntiShipTurret { get { return new BuildingKey(BuildingCategory.Defence, "AntiShipTurret"); } }
			public static BuildingKey AntiAirTurret { get { return new BuildingKey(BuildingCategory.Defence, "AntiAirTurret"); } }
			public static BuildingKey Mortar { get { return new BuildingKey(BuildingCategory.Defence, "Mortar"); } }
			public static BuildingKey SamSite { get { return new BuildingKey(BuildingCategory.Defence, "SamSite"); } }
			public static BuildingKey TeslaCoil { get { return new BuildingKey(BuildingCategory.Defence, "TeslaCoil"); } }

			// Offence
			public static BuildingKey Artillery { get { return new BuildingKey(BuildingCategory.Offence, "Artillery"); } }
			public static BuildingKey RocketLauncher { get { return new BuildingKey(BuildingCategory.Offence, "RocketLauncher"); } }
			public static BuildingKey Railgun { get { return new BuildingKey(BuildingCategory.Offence, "Railgun"); } }

			// Ultras
			public static BuildingKey DeathstarLauncher { get { return new BuildingKey(BuildingCategory.Ultra, "DeathstarLauncher"); } }
			public static BuildingKey NukeLauncher { get { return new BuildingKey(BuildingCategory.Ultra, "NukeLauncher"); } }
		}

		public static class Units
		{
			// Aircraft
			public static UnitKey Bomber { get { return new UnitKey(UnitCategory.Aircraft, "Bomber"); } }
			public static UnitKey Fighter { get { return new UnitKey(UnitCategory.Aircraft, "Fighter"); } }

			// Ships
			public static UnitKey AttackBoat { get { return new UnitKey(UnitCategory.Naval, "AttackBoat"); } }
		}

		public static class Hulls
		{
			public static HullKey Bullshark { get { return new HullKey("Bullshark"); } }
			public static HullKey Eagle { get { return new HullKey("Eagle"); } }
			public static HullKey Hammerhead { get { return new HullKey("Hammerhead"); } }
			public static HullKey Longbow { get { return new HullKey("Longbow"); } }
			public static HullKey Megalodon { get { return new HullKey("Megalodon"); } }
			public static HullKey Raptor { get { return new HullKey("Raptor"); } }
			public static HullKey Rockjaw { get { return new HullKey("Rockjaw"); } }
			public static HullKey Trident { get { return new HullKey("Trident"); } }
		}
	}
}